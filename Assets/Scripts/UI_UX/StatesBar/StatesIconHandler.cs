using System.Collections.Generic;
using UnityEngine;

public class StatesIconHandler : MonoBehaviour
{
    private List<States> _activeIcons = new List<States>();
    [SerializeField] private ElementsIconsPool _elementsObjPool;

    public void AddNewState(States state, float duration = 5f)
    {
        if (_activeIcons.Contains(state)) {
            _elementsObjPool.ResetState(state);
        } else {
            _activeIcons.Add(state);
            _elementsObjPool.ActiveState(state, transform, duration);
        }
    }

    public void ResetState(States state)
    {
        _elementsObjPool.ResetState(state);
    }

    public void RemoveState(States state)
    {
        if (!_activeIcons.Contains(state)) {
            return;
        }
        _elementsObjPool.RemoveState(state);
        _activeIcons.Remove(state);
    }
}
