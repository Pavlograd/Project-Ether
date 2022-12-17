using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StateBehavior {
    Add,
    Reset,
    Remove
}

public class TargetUi : MonoBehaviour
{
    // Why using 2 statesIconHandler:
    // When changing target we have to reset all active states and show the ones of the new target.
    // To avoid conflict of timing between the removing and the adding,
    // we use 2 UI to clear the oldest while showing the new one
    public StatesIconHandler _statesIconHandler;
    public StatesIconHandler _statesIconHandlerBis;
    [SerializeField] private GameObject container;
    [SerializeField] private Image _iconComponent;
    [SerializeField] private Slider _healthBar;

    private void OnEnable() {
        if (PlayerAbilityManager.changeEvent == null)
            PlayerAbilityManager.changeEvent = new ChangeTargetEvent();
        PlayerAbilityManager.changeEvent.AddListener(TargetChanged);
        if (AIHealthManager.onTargetHurt == null)
            AIHealthManager.onTargetHurt = new OnTargetHurt();
        AIHealthManager.onTargetHurt.AddListener(OnTargetHurt);
    }

    private void OnDisable() {
        PlayerAbilityManager.changeEvent.RemoveListener(TargetChanged);
        AIHealthManager.onTargetHurt.RemoveListener(OnTargetHurt);
    }

    private void OnTargetHurt(float healthRatio)
    {
        _healthBar.value = healthRatio;
    }

    private void TargetChanged(GameObject newTarget, GameObject currentTarget) {
        if (newTarget) {
            container.SetActive(true);
            EntityData targetEntityData = newTarget.GetComponent<EntityData>();
            targetEntityData.entityStateManager.IsTargeted(true, this);
            _healthBar.value = targetEntityData.entityHealthManager.GetHealthRatio();
        } else if (!newTarget && currentTarget) {
            container.SetActive(false);
            currentTarget.GetComponent<EntityStateManager>()?.IsTargeted(false, this);
        } else if (!newTarget && !currentTarget) {
            container.SetActive(false);
        }
    }

    public void AddStateDuringTargeting(StateBehavior behavior, States state, float? duration = null)
    {
        switch (behavior) {
            case StateBehavior.Add:
                if (_statesIconHandler.gameObject.activeSelf) {
                    _statesIconHandler.AddNewState(state, (float)duration);
                } else {
                    _statesIconHandlerBis.AddNewState(state, (float)duration);
                }
                break;
            case StateBehavior.Reset:
                if (_statesIconHandler.gameObject.activeSelf) {
                    _statesIconHandler.ResetState(state);
                } else {
                    _statesIconHandlerBis.ResetState(state);
                }
                break;
            case StateBehavior.Remove:
                if (_statesIconHandler.gameObject.activeSelf) {
                    _statesIconHandler.RemoveState(state);
                } else {
                    _statesIconHandlerBis.RemoveState(state);
                }
                break;
            default:
                break;
        }
    }

    private void ActiveStateIconHandler()
    {
        if (_statesIconHandler.gameObject.activeSelf) {
            _statesIconHandler.ResetAll(true);
            _statesIconHandlerBis.gameObject.SetActive(true);
        } else if (!_statesIconHandler.gameObject.activeSelf && !_statesIconHandlerBis.gameObject.activeSelf) {
            _statesIconHandler.gameObject.SetActive(true);
        } else {
            _statesIconHandlerBis.ResetAll(true);
            _statesIconHandler.gameObject.SetActive(true);
        }
    }

    public void SetCurrentStateData(List<States> currentStates, Dictionary<States, StateData> data, Sprite icon)
    {
        ActiveStateIconHandler();
        _iconComponent.sprite = icon;
        if (_statesIconHandler.gameObject.activeSelf) {
            foreach (var item in currentStates) {
                _statesIconHandler.AddAlreadyStartedState(item, data[item].effectDuration, data[item].stateElapsedTime);
            }
        } else {
            foreach (var item in currentStates) {
                _statesIconHandlerBis.AddAlreadyStartedState(item, data[item].effectDuration, data[item].stateElapsedTime);
            }
        }
    }
}
