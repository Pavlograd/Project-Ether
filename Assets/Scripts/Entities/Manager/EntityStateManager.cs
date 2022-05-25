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

public class EntityStateManager : MonoBehaviour
{
    private class StateData
    {
        public Action func;
        public Coroutine stateCoroutine;
        public Coroutine sideStateCoroutine;
        public float effectDuration = 5f;
        public float effectElapsedTime = 0f;
        public float stateElapsedTime = 0f;
        public float value = 0f;
        public States sideState = States.NEUTRAL;
        public bool isCombo = true;
    }

    private float _stateDuration = 5;
    private EntityData _entityData;
    private Dictionary<States, StateData> _statesData = new Dictionary<States, StateData>();
    private Dictionary<States, Action<DataBuff>> _buffsFunctions = new Dictionary<States, Action<DataBuff>>();
    [HideInInspector] public List<DataBuff> currentBuffs = new List<DataBuff>();
    [HideInInspector] public List<States> currentElementalStates = new List<States>();
    [SerializeField] private StatesIconHandler _statesIconHandler;

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
        // _statesData.Add(States.FIRE_EARTH, new StateData() {
        //     func = FireEarthCombo,
        // });
        _statesData.Add(States.FIRE_WIND, new StateData() {
            func = FireWindCombo,
            sideState = States.DOT,
        });
        // _statesData.Add(States.FIRE_ICE, new StateData() {
        //     func = FireIceCombo,
        // });
        // _statesData.Add(States.FIRE_ELECTRIC, new StateData() {
        //     func = FireElectricCombo,
        // });
        // _statesData.Add(States.WIND_EARTH, new StateData() {
        //     func = EarthWindCombo,
        // });
        _statesData.Add(States.ICE_EARTH, new StateData() {
            func = EarthIceCombo,
            value = 1f,
        });
        _statesData.Add(States.EARTH_ELECTRIC, new StateData() {
            func = EarthElectricCombo,
            effectDuration = 4f,
            sideState = States.STUN
        });
        // _statesData.Add(States.WIND_ELECTRIC, new StateData() {
        //     func = WindElectricCombo,
        // });
        // _statesData.Add(States.ICE_WIND, new StateData() {
        //     func = WindIceCombo,
        // });
        _statesData.Add(States.ICE_ELECTRIC, new StateData() {
            func = IceElectricCombo,
            sideState = States.STUN
        });
    }

    private void FillBuffsFunctionsDict()
    {
        _buffsFunctions.Add(States.STRENGTH, AddStrengthBuff);
        _buffsFunctions.Add(States.MOVE_SPEED, AddSpeedBuff);
        _buffsFunctions.Add(States.ELEMENTAL_STRENGTH, AddStrengthBuff);
    }

    private States GetElementalComboEnum (States state1, States state2)
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

    private void ResetEffectByState(States state, bool removeMainState)
    {
        switch (state) {
            case States.STUN:
                _entityData.entityAbilityManager.canAttack = true;
                _entityData.entityMovementManager.canMove = true;
                if (_statesIconHandler != null) {
                    _statesIconHandler.ResetState(state);
                    _statesIconHandler.RemoveState(state);
                }
                currentElementalStates.Remove(States.STUN);
                break;
            case States.ICE:
                _entityData.entityMovementManager.speed += _statesData[state].value;
                if (_statesIconHandler != null) {
                    _statesIconHandler.ResetState(state);
                    _statesIconHandler.RemoveState(state);
                }
                currentElementalStates.Remove(States.SLOW);
                break;
            case States.ICE_EARTH:
                _entityData.entityMovementManager.speed += _statesData[state].value;
                currentElementalStates.Remove(States.SLOW);
                break;
            case States.DOT:
                currentElementalStates.Remove(States.DOT);
                break;
            default:
                break;
        }
        if (removeMainState && currentElementalStates.Contains(state)) {
            if (_statesIconHandler != null) {
                _statesIconHandler.ResetState(state);
                _statesIconHandler.RemoveState(state);
            }
            currentElementalStates.Remove(state);
        }
    }

    private IEnumerator Dot(States state, float duration, float damage, float occcurrence)
    {
        States effectState = States.DOT;
        currentElementalStates.Add(effectState);
        if (_statesIconHandler != null) {
            _statesIconHandler.AddNewState(effectState, duration);
        }
        for (; _statesData[state].effectElapsedTime < duration; _statesData[state].effectElapsedTime += occcurrence) {
            yield return new WaitForSeconds(occcurrence);
            _entityData.entityHealthManager.TakeDamage(damage);
        }
        ResetEffectByState(effectState, false);
    }

    private IEnumerator Slow(States state, float duration)
    {
        States effectState = States.SLOW;
        currentElementalStates.Add(effectState);
        if (_statesIconHandler != null) {
            _statesIconHandler.AddNewState(effectState, duration);
        }
        for (; _statesData[state].effectElapsedTime < duration; _statesData[state].effectElapsedTime += Time.deltaTime) {
            yield return null;
        }
        ResetEffectByState(effectState, true);
    }

    private IEnumerator Stun(States state, float duration)
    {
        float time = Time.time;
        States effectState = States.STUN;

        currentElementalStates.Add(effectState);
        if (_statesIconHandler != null) {
            _statesIconHandler.AddNewState(effectState, duration);
        }
        while (_statesData[state].effectElapsedTime <= duration) {
            _entityData.entityMovementManager.canMove = false;
            _entityData.entityAbilityManager.canAttack = false;
            _statesData[state].effectElapsedTime += Time.deltaTime;
            yield return null;
        }
        ResetEffectByState(effectState, false);
    }

    private IEnumerator StunsSuccessions(States state, float duration, float stunDuration, float ocurrence)
    {
        float time = Time.time;
        States effectState = States.STUN;

        currentElementalStates.Add(effectState);
        for (; _statesData[state].effectElapsedTime < duration ; _statesData[state].effectElapsedTime += 1f) {
            _entityData.entityMovementManager.canMove = true;
            yield return new WaitForSeconds(ocurrence);
            _entityData.entityMovementManager.canMove = false;
            if (_statesIconHandler != null) {
                _statesIconHandler.AddNewState(effectState, stunDuration);
            }
            _entityData.entityAnimationManager.TakeDamage();
            yield return new WaitForSeconds(stunDuration);
            if (_statesIconHandler != null) {
                _statesIconHandler.RemoveState(effectState);
            }
        }
        ResetEffectByState(effectState, true);
    }

    private IEnumerator StateTimer(States state)
    {
        for (; _statesData[state].stateElapsedTime < _stateDuration; _statesData[state].stateElapsedTime += Time.deltaTime) {
            yield return null;
        }
        ResetEffectByState(state, true);
    }

    private IEnumerator StateTimer(States state, float duration)
    {
        for (; _statesData[state].stateElapsedTime < duration; _statesData[state].stateElapsedTime += Time.deltaTime) {
            yield return null;
        }
        ResetEffectByState(state, true);
    }

    private bool CanApplySideState(States sideState, float duration)
    {
        if (!currentElementalStates.Contains(sideState)) {
            return true;
        }
        foreach (States item in currentElementalStates) {
            if (_statesData.ContainsKey(item) && _statesData[item].sideState == sideState) {
                StateData itemData =_statesData[item];
                if (itemData.sideStateCoroutine != null) {
                    if (itemData.effectDuration - itemData.effectElapsedTime < duration) {
                        StopCoroutine(itemData.sideStateCoroutine);
                        ResetEffectByState(sideState, true);
                        if (itemData.isCombo) {
                            StopCoroutine(itemData.stateCoroutine);
                            ResetEffectByState(item, true);
                        }
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void FireEarthCombo() { }

    private void FireWindCombo()
    {
        States state = States.FIRE_WIND;
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
            _statesData[state].sideStateCoroutine = StartCoroutine(Dot(state, _statesData[state].effectDuration, 4f, 1f));
        }
    }

    private void FireIceCombo() { }

    private void FireElectricCombo() { }

    private void EarthWindCombo() { }

    private void EarthIceCombo()
    {
        States state = States.ICE_EARTH;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
        ActiveHarmfullEffects(States.FIRE);
        ActiveHarmfullEffects(States.ICE);
    }

    private void EarthElectricCombo()
    {
        States state = States.EARTH_ELECTRIC;
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
            _statesData[state].sideStateCoroutine = StartCoroutine(Stun(state, _statesData[state].effectDuration));
        }
    }

    private void WindElectricCombo() { }

    private void WindIceCombo() { }

    private void IceElectricCombo()
    {
        States state = States.ICE_ELECTRIC;
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state, _statesData[state].effectDuration));
            _statesData[state].sideStateCoroutine = StartCoroutine(StunsSuccessions(state, _statesData[state].effectDuration, .75f, .25f));
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
        _entityData.entityMovementManager.speed -= _statesData[state].value;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].sideStateCoroutine = StartCoroutine(Slow(state, _statesData[state].effectDuration));
        }
    }

    private void EarthState()
    {
        States state = States.EARTH;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].sideStateCoroutine = StartCoroutine(Stun(state, _statesData[state].effectDuration));
        }
    }

    private void ElectricState()
    {
        States state = States.ELECTRIC;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].sideStateCoroutine = StartCoroutine(StunsSuccessions(state, _statesData[state].effectDuration, .5f, .5f));
        }
    }

    private void FireState()
    {
        States state = States.FIRE;
        _statesData[state].stateCoroutine = StartCoroutine(StateTimer(state));
        if (CanApplySideState(_statesData[state].sideState, _statesData[state].effectDuration)) {
            _statesData[state].sideStateCoroutine = StartCoroutine(Dot(state, _statesData[state].effectDuration, 2f, 1f));
        }
    }

    private void AddState(States state)
    {
        if (_statesIconHandler != null) {
            if (_statesData[state].isCombo) {
                _statesIconHandler.AddNewState(state, _statesData[state].effectDuration);
            } else {
                _statesIconHandler.AddNewState(state);
            }
        }
        currentElementalStates.Add(state);
        if (_statesData.ContainsKey(state)) {
            _statesData[state].effectElapsedTime = 0;
            _statesData[state].stateElapsedTime = 0;
            _statesData[state].func();
        }
    }

    private void ActiveCombo(States comboState, States firstState)
    {
        StopCoroutine(_statesData[firstState].stateCoroutine);
        ResetEffectByState(firstState, true);
        if (_statesData[firstState].sideStateCoroutine != null) {
            StopCoroutine(_statesData[firstState].sideStateCoroutine);
            _statesData[firstState].sideStateCoroutine = null;
            ResetEffectByState(_statesData[firstState].sideState, false);
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
        if (_statesIconHandler != null) {
            _statesIconHandler.AddNewState(state);
        }
        _statesData[state].stateElapsedTime = 0;
        _statesData[state].effectElapsedTime = 0;
        if (_statesData[state].sideState != States.NEUTRAL) {
            if (_statesIconHandler != null) {
                _statesIconHandler.AddNewState(_statesData[state].sideState, _statesData[state].effectDuration);
            }
        }
    }

    public void ActiveHarmfullEffects(States state)
    {
        if (state == States.NEUTRAL) return;
        // Are there any states already applied?
        if (currentElementalStates.Count > 0) {
            // Is the state already applied?
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
}