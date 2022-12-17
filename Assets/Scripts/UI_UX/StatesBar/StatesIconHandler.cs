using System.Collections.Generic;
using UnityEngine;

public class StatesIconHandler : MonoBehaviour
{
    private List<States> _activeIcons = new List<States>();
    private TargetUi _targetUi = null;
    // if _isTargeted is true, update the UI of the targeted enemy
    private bool _isTargeted = false;
    [SerializeField] private ElementsIconsPool _elementsObjPool;
    [SerializeField] private Transform _arrow_target;

    public void AddNewState(States state, float duration = 5f)
    {
        if (!_elementsObjPool) {
            return;
        }
        if (_activeIcons.Contains(state)) {
            _elementsObjPool.ResetState(state);
        } else {
            _activeIcons.Add(state);
            _elementsObjPool.ActiveState(state, transform, duration);
            if (_arrow_target != null) {
                int row = _activeIcons.Count % 3;
                _arrow_target.position = new Vector3(_arrow_target.position.x, _arrow_target.position.y + (.2f * row), 0);
            }
        }
        if (_isTargeted) {
            _targetUi.AddStateDuringTargeting(StateBehavior.Add, state, duration);
        }
    }

    public void AddAlreadyStartedState(States state, float duration, float elaspedTime)
    {
        if (_activeIcons.Contains(state)) {
            _elementsObjPool.ResetState(state);
        } else {
            _activeIcons.Add(state);
            _elementsObjPool.ActiveState(state, transform, duration, elaspedTime);
        }
    }

    public void ResetState(States state)
    {
        if (_activeIcons.Contains(state)) {
            _elementsObjPool.ResetState(state);
        }
        if (_isTargeted) {
            _targetUi.AddStateDuringTargeting(StateBehavior.Reset, state);
        }
    }

    public void RemoveState(States state)
    {
        if (!_activeIcons.Contains(state)) {
            return;
        }
        _elementsObjPool.RemoveState(state);
        _activeIcons.Remove(state);
        if (_arrow_target != null) {
            int row = _activeIcons.Count % 3;
            _arrow_target.position = new Vector3(_arrow_target.position.x, _arrow_target.position.y - (.3f * row), 0);
        }
        if (_isTargeted) {
            _targetUi.AddStateDuringTargeting(StateBehavior.Remove, state);
        }
    }

    public void ResetAll(bool hideComponent = false)
    {
        try {
            List<States> copy = new List<States>(_activeIcons);
            foreach (var item in copy) {
                RemoveState(item);
            }
        } catch (System.Exception e) {
            Debug.Log(e);
        }
        if (hideComponent) {
            gameObject.SetActive(false);
        }
    }

    // Target UI handling

    public void SetTargetUi(TargetUi targetUi)
    {
        _targetUi = targetUi;
        if (targetUi != null) {
            _isTargeted = true;
        } else {
            _isTargeted = false;
        }
    }
}