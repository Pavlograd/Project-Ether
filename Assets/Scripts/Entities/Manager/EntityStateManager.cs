using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    NEUTRAL,
    FIRE,
    ICE,
    ELECTRIC,
    EARTH,
    WIND,
    FIRE_ICE,
    FIRE_ELECTRIC,
    FIRE_EARTH,
    FIRE_WIND,
    ICE_ELECTRIC,
    ICE_EARTH,
    ICE_WIND,
    WIND_EARTH,
    WIND_ELECTRIC,
    EARTH_ELECTRIC,
    STRENGTH,
    ELEMENTAL_STRENGTH,
    MOVE_SPEED,
    ATTACH_SPEED,
    STUN,
    SLOW,
    DOT,
    STUN_SUCCESSION,
}

public struct DataBuff
{
    public float increasedAmount;
    public States type;
    public string name;
    public float duration;
    public bool isPourcentage;
    public Coroutine coroutine;
    public float? initialValue;
    public GameObject gameObject;
}

public class StateData
{
    public Action func;
    public Coroutine stateCoroutine;
    public float effectDuration = 5f;
    public float stateElapsedTime = 0f;
    public float value = 0f;
    public States sideState = States.NEUTRAL;
    public bool isCombo = true;
    public bool hasStateIcon = true;
}

public class EntityStateManager : MonoBehaviour
{
    [HideInInspector] public bool isTargeted;

    private float _stateDuration = 5;
    private EntityData _entityData;
    private Dictionary<States, StateData> _statesData = new Dictionary<States, StateData>();
    private Dictionary<States, Action<DataBuff>> _buffsFunctions = new Dictionary<States, Action<DataBuff>>();
    [HideInInspector] public List<DataBuff> currentBuffs = new List<DataBuff>();
    [HideInInspector] public List<States> currentElementalStates = new List<States>();
    [SerializeField] private StatesIconHandler _statesIconHandler;
    [SerializeField] private GameObject _fireEarthComboPrefab;

    [Header("Target")]
    [SerializeField] private GameObject _targetContainer;
    [SerializeField] private Sprite _iconTarget;

    private void Awake()
    {
        _entityData = GetComponent<EntityData>();
        FillStatesData();
        FillBuffsFunctionsDict();
    }

    public void SetStateIconHandler(StatesIconHandler statesIconHandler)
    {
        _statesIconHandler = statesIconHandler;
    }

    private void FillStatesData()
    {
        _statesData.Add(States.FIRE, new StateData() {
            func = FireState,
            isCombo = false,
            sideState = States.DOT
        });
        _statesData.Add(States.ICE, new StateData() {
            func = IceState,
            value = 1.25f,
            effectDuration = 4f,
            isCombo = false,
            sideState = States.SLOW
        });
        _statesData.Add(States.ELECTRIC, new StateData() {
            func = ElectricState,
            sideState = States.STUN,
            isCombo = false,
        });
        _statesData.Add(States.EARTH, new StateData() {
            func = EarthState,
            effectDuration = 2f,
            sideState = States.STUN,
            isCombo = false,
        });
        _statesData.Add(States.WIND, new StateData() {
            func = WindState,
            isCombo = false,
        });
        _statesData.Add(States.FIRE_EARTH, new StateData() {
            func = FireEarthCombo,
            hasStateIcon = false,
        });
        _statesData.Add(States.FIRE_WIND, new StateData() {
            func = FireWindCombo,
            sideState = States.DOT,
            hasStateIcon = false,
            effectDuration = 12f,
        });
        _statesData.Add(States.FIRE_ICE, new StateData() {
            func = FireIceCombo,
            effectDuration = 4f,
            value = .75f,
            hasStateIcon = false,
        });
        _statesData.Add(States.FIRE_ELECTRIC, new StateData() {
            func = FireElectricCombo,
            hasStateIcon = false,
        });
        _statesData.Add(States.WIND_EARTH, new StateData() {
            func = EarthWindCombo,
            sideState = States.STUN,
            effectDuration = 3f,
            hasStateIcon = false,
        });
        _statesData.Add(States.ICE_EARTH, new StateData() {
            func = EarthIceCombo,
            value = 1f,
            hasStateIcon = false,
        });
        _statesData.Add(States.EARTH_ELECTRIC, new StateData() {
            func = EarthElectricCombo,
            effectDuration = 4f,
            sideState = States.STUN,
            hasStateIcon = false,
        });
        _statesData.Add(States.WIND_ELECTRIC, new StateData() {
            func = WindElectricCombo,
            sideState = States.STUN,
            hasStateIcon = false,
        });
        _statesData.Add(States.ICE_WIND, new StateData() {
            func = WindIceCombo,
            hasStateIcon = false,
        });
        _statesData.Add(States.ICE_ELECTRIC, new StateData() {
            func = IceElectricCombo,
            sideState = States.STUN,
        });
        _statesData.Add(States.STUN, new StateData() {
            isCombo = false,
        });
        _statesData.Add(States.DOT, new StateData() {
            isCombo = false,
        });
        _statesData.Add(States.SLOW, new StateData()  {
            isCombo = false,
        });
    }

    private void FillBuffsFunctionsDict()
    {
        _buffsFunctions.Add(States.STRENGTH, AddStrengthBuff);
        _buffsFunctions.Add(States.MOVE_SPEED, AddSpeedBuff);
        _buffsFunctions.Add(States.ELEMENTAL_STRENGTH, AddStrengthBuff);
    }

    private States GetElementalComboEnum(States state1, States state2)
    {
        if (((int)state1 < 1 || (int)state1 > 5) || ((int)state2 < 1 || (int)state2 > 5)) {
            return States.NEUTRAL;
        }
        string combinedState = state1.ToString() + "_" + state2.ToString();
        switch (combinedState) {
            case "FIRE_ICE":
            case "ICE_FIRE":
                return States.FIRE_ICE;
            case "FIRE_ELECTRIC":
            case "ELECTRIC_FIRE":
                return States.FIRE_ELECTRIC;
            case "FIRE_EARTH":
            case "EARTH_FIRE":
                return States.FIRE_EARTH;
            case "FIRE_WIND":
            case "WIND_FIRE":
                return States.FIRE_WIND;
            case "ICE_ELECTRIC":
            case "ELECTRIC_ICE":
                return States.ICE_ELECTRIC;
            case "ICE_EARTH":
            case "EARTH_ICE":
                return States.ICE_EARTH;
            case "ICE_WIND":
            case "WIND_ICE":
                return States.ICE_WIND;
            case "WIND_EARTH":
            case "EARTH_WIND":
                return States.WIND_EARTH;
            case "WIND_ELECTRIC":
            case "ELECTRIC_WIND":
                return States.WIND_ELECTRIC;
            case "EARTH_ELECTRIC":
            case "ELECTRIC_EARTH":
                return States.EARTH_ELECTRIC;
            default: break;
        }
        return States.NEUTRAL;
    }

    // <--- Harmfull effects --->

    private void ResetEffectByState(States state)
    {
        void RemoveStateIcon(States state) {
            _statesIconHandler?.ResetState(state);
            _statesIconHandler?.RemoveState(state);
            currentElementalStates.Remove(state);
            _statesData[state].stateElapsedTime = 0;
        }
        switch (state) {
            case States.STUN:
                _entityData.entityAbilityManager.canAttack = true;
                _entityData.entityMovementManager.canMove = true;
                break;
            case States.SLOW:
                _entityData.entityMovementManager.speed = _entityData.entityMovementManager.initialSpeed;
                break;
            case States.DOT:
                break;
            default:
                break;
        }
        if (currentElementalStates.Contains(state)) {
            RemoveStateIcon(state);
        }
    }

    private IEnumerator Dot(float duration, float damage, float occcurrence)
    {
        int lastSecond = 0;
        States effectState = States.DOT;
        currentElementalStates.Add(effectState);
        _statesIconHandler?.AddNewState(effectState, duration);
        _statesData[effectState].effectDuration = duration;
        for (; _statesData[effectState].stateElapsedTime < duration; _statesData[effectState].stateElapsedTime += Time.deltaTime) {
            int currentSecond = (int)Math.Round(_statesData[effectState].stateElapsedTime, MidpointRounding.ToEven);
            if (lastSecond < currentSecond) {
                lastSecond = currentSecond;
                _entityData.entityHealthManager.TakeDamage(damage);
            }
            yield return null;
        }
        _statesData[effectState].stateElapsedTime = 0;
        ResetEffectByState(effectState);
    }

    private IEnumerator Slow(float duration)
    {
        States effectState = States.SLOW;
        currentElementalStates.Add(effectState);
        _statesIconHandler?.AddNewState(effectState, duration);
        _statesData[effectState].effectDuration = duration;
        for (; _statesData[effectState].stateElapsedTime < duration; _statesData[effectState].stateElapsedTime += Time.deltaTime) {
            yield return null;
        }
        _statesData[effectState].stateElapsedTime = 0;
        ResetEffectByState(effectState);
    }

    private IEnumerator Stun(float duration)
    {
        float time = Time.time;
        States effectState = States.STUN;
        currentElementalStates.Add(effectState);
        _statesIconHandler?.AddNewState(effectState, duration);
        _statesData[effectState].effectDuration = duration;
        while (_statesData[effectState].stateElapsedTime <= duration) {
            _entityData.entityMovementManager.canMove = false;
            _entityData.entityAbilityManager.canAttack = false;
            _statesData[effectState].stateElapsedTime += Time.deltaTime;
            yield return null;
        }
        _statesData[effectState].stateElapsedTime = 0;
        ResetEffectByState(effectState);
    }

    private IEnumerator StunsSuccessions(float duration, float stunDuration, float ocurrence)
    {
        float time = Time.time;
        States effectState = States.STUN;
        currentElementalStates.Add(effectState);
        _statesData[effectState].effectDuration = duration;
        for (; _statesData[effectState].stateElapsedTime < duration ; _statesData[effectState].stateElapsedTime += 1f) {
            _entityData.entityMovementManager.canMove = true;
            yield return new WaitForSeconds(ocurrence);
            _entityData.entityMovementManager.canMove = false;
            _statesIconHandler?.AddNewState(effectState, stunDuration);
            _entityData.entityAnimationManager.TakeDamage();
            yield return new WaitForSeconds(stunDuration);
            _statesIconHandler?.RemoveState(effectState);
        }
        _statesData[effectState].stateElapsedTime = 0;
        ResetEffectByState(effectState);
    }

    private IEnumerator StateTimer(States state)
    {
        for (; _statesData[state].stateElapsedTime < _stateDuration; _statesData[state].stateElapsedTime += Time.deltaTime) {
            yield return null;
        }
        ResetEffectByState(state);
    }

    private IEnumerator StateTimer(States state, float duration)
    {
        for (; _statesData[state].stateElapsedTime < duration; _statesData[state].stateElapsedTime += Time.deltaTime) {
            yield return null;
        }
        ResetEffectByState(state);
    }

    private bool CanApplySideState(States sideState, float duration, bool reset = true)
    {
        if (!currentElementalStates.Contains(sideState)) {
            return true;
        }
        foreach (States item in currentElementalStates) {
            if (item == sideState && _statesData.ContainsKey(item)) {
                StateData itemData =_statesData[item];
                if (itemData.effectDuration - itemData.stateElapsedTime < duration) {
                    if (reset) {
                        StopCoroutine(itemData.stateCoroutine);
                        ResetEffectByState(sideState);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    private void FireEarthCombo()
    {
        Instantiate(_fireEarthComboPrefab, transform.position, Quaternion.identity);
    }

    private void FireWindCombo()
    {
        States state = States.FIRE_WIND;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration, true)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(Dot(_statesData[state].effectDuration, 4f, 1f));
        }
    }

    private void FireIceCombo()
    {
        States state = States.FIRE_ICE;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        string entitiesTag = gameObject.tag == "Enemy" ? "Enemy" : "Player";
        float range = 3f;
        ForceActiveHarmfullEffects(States.SLOW);
        foreach (GameObject entity in GameObject.FindGameObjectsWithTag(entitiesTag)) {
            float distance = Vector3.Distance(transform.position, entity.transform.localPosition);
            if (distance < range) {
                if (entity.TryGetComponent<EntityStateManager>(out var entityStateManager)) {
                    entityStateManager.ForceActiveHarmfullEffects(States.SLOW);
                }
            }
        }
    }

    private void FireElectricCombo()
    {
        States state = States.FIRE_ELECTRIC;
        string entitiesTag = gameObject.tag == "Enemy" ? "Enemy" : "Player";
        float range = 3f;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, 0f));
        _entityData.entityHealthManager.TakeDamage(10f);
        foreach (GameObject entity in GameObject.FindGameObjectsWithTag(entitiesTag)){
            float distance = Vector3.Distance(transform.position, entity.transform.localPosition);
            if (distance < range) {
                if (entity.TryGetComponent<EntityHealthManager>(out var healthManager)) {
                    healthManager.TakeDamage(7.5f);
                }
            }
        }
    }

    private void EarthWindCombo()
    {
        States state = States.WIND_EARTH;
        States sideState = _statesData[state].sideState;
        _entityData.entityHealthManager.TakeDamage(15f);
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration, true)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(Stun(_statesData[state].effectDuration));
        }
    }

    private void EarthIceCombo()
    {
        States state = States.ICE_EARTH;
        States firstEffect = States.DOT;
        States scndEffect = States.SLOW;
        int dotDuration = 5;
        int slowDuration = 5;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (CanApplySideState(firstEffect, dotDuration, true)) {
            _statesData[firstEffect].stateCoroutine = StartCoroutine(Dot(dotDuration, 4f, 1f));
        }
        if (CanApplySideState(scndEffect, slowDuration, true)) {
            _entityData.entityMovementManager.speed -= _statesData[States.ICE].value;
            _statesData[scndEffect].stateCoroutine = StartCoroutine(Slow(slowDuration));
        }
    }

    private void EarthElectricCombo()
    {
        States state = States.EARTH_ELECTRIC;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(Stun(_statesData[state].effectDuration));
        }
    }

    private void WindElectricCombo()
    {
        States state = States.ICE_WIND;
        States sideState = _statesData[state].sideState;
        _entityData.entityHealthManager.TakeDamage(10f);
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (CanApplySideState(sideState, _statesData[state].effectDuration, true)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(StunsSuccessions(_statesData[state].effectDuration, .5f, .5f));
        }
    }

    private void WindIceCombo()
    {
        States state = States.ICE_WIND;
        States effect = States.SLOW;
        _entityData.entityHealthManager.TakeDamage(6f);
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (CanApplySideState(effect, _statesData[state].effectDuration, true)) {
            _entityData.entityMovementManager.speed -= _statesData[States.ICE].value;
            _statesData[effect].stateCoroutine = StartCoroutine(Slow(_statesData[state].effectDuration));
        }
    }

    private void IceElectricCombo()
    {
        States state = States.ICE_ELECTRIC;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        if (sideState != States.NEUTRAL && CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(StunsSuccessions(_statesData[state].effectDuration, .75f, .25f));
        }
    }

    private void WindState()
    {
        States state = States.WIND;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
    }

    private void IceState()
    {
        States state = States.ICE;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration)) {
            _entityData.entityMovementManager.speed -= _statesData[state].value;
            _statesData[sideState].stateCoroutine = StartCoroutine(Slow(_statesData[state].effectDuration));
        }
    }

    private void EarthState()
    {
        States state = States.EARTH;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(Stun(_statesData[state].effectDuration));
        }
    }

    private void ElectricState()
    {
        States state = States.ELECTRIC;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(StunsSuccessions(_statesData[state].effectDuration, .5f, .5f));
        }
    }

    private void FireState()
    {
        States state = States.FIRE;
        States sideState = _statesData[state].sideState;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration)) {
            _statesData[sideState].stateCoroutine = StartCoroutine(Dot(_statesData[state].effectDuration, 2f, 1f));
        }
    }

    private void AddState(States state)
    {
        if (_statesData[state].hasStateIcon) {
            if (_statesData[state].isCombo) {
                _statesIconHandler?.AddNewState(state, _statesData[state].effectDuration);
            } else {
                _statesIconHandler?.AddNewState(state);
            }
        }
        currentElementalStates.Add(state);
        if (_statesData.ContainsKey(state)) {
            _statesData[state].stateElapsedTime = 0;
            _statesData[state].func();
        }
    }

    private void ActiveCombo(States comboState, States firstState)
    {
        StopCoroutine(_statesData[firstState].stateCoroutine);
        ResetEffectByState(firstState);
        if (_statesData[firstState].sideState != States.NEUTRAL) {
            States sideState = _statesData[firstState].sideState;
            if (_statesData[sideState].stateElapsedTime > 0 && _statesData[sideState].stateCoroutine != null) {
                StopCoroutine(_statesData[sideState].stateCoroutine);
                ResetEffectByState(sideState);
            }
        }
        if (currentElementalStates.Contains(comboState)) {
            OverrideCurrentState(comboState);
        } else {
            AddState(comboState);
        }
    }

    private bool CheckIfCombo(States state)
    {
        States firstState = States.NEUTRAL;

        foreach (States item in currentElementalStates) {
            if ((int)item >= 1 && (int)item <= 5) {
                firstState = item;
                break;
            }
        }
        if (firstState != States.NEUTRAL) {
            States comboState = GetElementalComboEnum(state, firstState);
            if (_statesData.ContainsKey(comboState)) {
                ActiveCombo(comboState, firstState);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    private void OverrideCurrentState(States state)
    {
        if (_statesData[state].hasStateIcon) {
            _statesIconHandler?.AddNewState(state);
        }
        States sideState = _statesData[state].sideState;
        if (sideState != States.NEUTRAL && CanApplySideState(sideState, _statesData[state].effectDuration, false)) {
            _statesIconHandler?.AddNewState(sideState, _statesData[state].effectDuration);
            _statesData[sideState].stateElapsedTime = 0;
        }
        _statesData[state].stateElapsedTime = 0;
    }

    public void ActiveHarmfullEffects(States state)
    {
        if (state == States.NEUTRAL) return;
        // Are there any states already applied?
        if (currentElementalStates.Count > 0) {
            // Is the same state already applied?
            if (currentElementalStates.Contains(state)) {
                // Override state
                OverrideCurrentState(state);
            } else {
                // Is it a combo? if not apply new state
                if (!CheckIfCombo(state)) {
                    AddState(state);
                }
            }
        } else {
            // apply new state
            AddState(state);
        }
    }

    public void ForceActiveHarmfullEffects(States state)
    {
        if (state == States.NEUTRAL) return;
        switch (state) {
            case States.STUN:
                if (CanApplySideState(state, _statesData[States.EARTH].effectDuration, true)) {
                   _statesData[state].stateCoroutine = StartCoroutine(Stun(_statesData[States.EARTH].effectDuration));
                }
                break;
            case States.SLOW:
                if (CanApplySideState(state, _statesData[States.ICE].effectDuration, true)) {
                    _entityData.entityMovementManager.speed -= _statesData[States.ICE].value;
                    _statesData[state].stateCoroutine = StartCoroutine(Slow(_statesData[States.ICE].effectDuration));
                }
                break;
            case States.DOT:
                if (CanApplySideState(state, _statesData[States.FIRE].effectDuration, true)) {
                    _statesData[state].stateCoroutine = StartCoroutine(Dot(_statesData[States.FIRE].effectDuration, 2f, 1f));
                }
                break;
            case States.STUN_SUCCESSION:
                if (CanApplySideState(state, _statesData[States.ELECTRIC].effectDuration, true)) {
                    _statesData[state].stateCoroutine = StartCoroutine(StunsSuccessions(_statesData[States.ELECTRIC].effectDuration, .5f, .5f));
                }
                break;
            default:
                break;
        }
    }

    // <--- Buffs --->

    private void RemoveBuffEffect(States type, string name, float? initialValue = null)
    {
        switch (type) {
            case States.MOVE_SPEED:
                if (initialValue != null) {
                    _entityData.entityMovementManager.speed = (float)initialValue;
                }
                break;
            default:
                break;
        }
        if (_statesIconHandler != null) {
            _statesIconHandler.ResetState(type);
            _statesIconHandler.RemoveState(type);
        }
        currentBuffs.Remove(currentBuffs.Find((elem) => elem.name == name));
    }

    private IEnumerator BuffTimerFunction(States type, float duration, string name, float? initialValue = null)
    {
        yield return new WaitForSeconds(duration);
        RemoveBuffEffect(type, name, initialValue);
    }

    private void AddSpeedBuff(DataBuff dataBuff)
    {
        float entitySpeed = _entityData.entityMovementManager.speed;
        if (dataBuff.isPourcentage) {
            _entityData.entityMovementManager.speed = entitySpeed + ((entitySpeed / 100) * dataBuff.increasedAmount);
        } else {
            _entityData.entityMovementManager.speed += dataBuff.increasedAmount;
        }
        dataBuff.initialValue = entitySpeed;
        dataBuff.coroutine = StartCoroutine(BuffTimerFunction(dataBuff.type, dataBuff.duration, dataBuff.name, entitySpeed));
        if (_statesIconHandler != null) {
            _statesIconHandler.AddNewState(States.MOVE_SPEED, dataBuff.duration);
        }
        currentBuffs.Add(dataBuff);
    }

    private void AddStrengthBuff(DataBuff dataBuff)
    {
        if (_statesIconHandler != null) {
            _statesIconHandler.AddNewState(States.STRENGTH, dataBuff.duration);
        }
        dataBuff.coroutine = StartCoroutine(BuffTimerFunction(dataBuff.type, dataBuff.duration, dataBuff.name));
        currentBuffs.Add(dataBuff);
    }

    private void CheckIfBuffAlreadyApplied(DataBuff dataBuff)
    {
        foreach (DataBuff data in currentBuffs) {
            if (data.name == dataBuff.name && data.type == dataBuff.type) {
                StopCoroutine(data.coroutine);
                Destroy(data.gameObject);
                RemoveBuffEffect(data.type, data.name, data.initialValue);
                break;
            }
        }
    }

    public void ActiveBuffState(DataBuff dataBuff)
    {
        if (_buffsFunctions.ContainsKey(dataBuff.type)) {
            CheckIfBuffAlreadyApplied(dataBuff);
            _buffsFunctions[dataBuff.type](dataBuff);
        }
    }

    // <-- Target Management -->

    public void IsTargeted(bool value, TargetUi targetUi)
    {
        // Red arrow above target
        _targetContainer.SetActive(value);
        isTargeted = value;
        if (value) {
            targetUi.SetCurrentStateData(currentElementalStates, _statesData, _iconTarget);
            _statesIconHandler.SetTargetUi(targetUi);
        } else {
            _statesIconHandler.SetTargetUi(null);
        }
    }
}