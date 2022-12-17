using System.Collections.Generic;
using UnityEngine;

public class ElementsIconsPool : MonoBehaviour
{
    public Dictionary<States, StateUIManager> _iconsObj = new Dictionary<States, StateUIManager>();

    private void Start()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++) {
            StateUIManager childScript = transform.GetChild(i).GetComponent<StateUIManager>();
            if (childScript != null) {
                _iconsObj.Add(childScript.state, childScript);
            }
        }
    }

    public void ActiveState(States state, Transform trfm, float duration)
    {
        StateUIManager stateManager;

        if (_iconsObj.TryGetValue(state, out stateManager)) {
            stateManager.gameObject.transform.SetParent(trfm, true);
            stateManager.gameObject.SetActive(true);
            stateManager.StartTimer(duration);
        }
    }

    public void ActiveState(States state, Transform trfm, float duration, float elaspedTime)
    {
        StateUIManager stateManager;

        if (_iconsObj.TryGetValue(state, out stateManager)) {
            stateManager.gameObject.transform.SetParent(trfm, true);
            stateManager.gameObject.SetActive(true);
            stateManager.StartTimer(duration);
            stateManager.StartTimer(duration, elaspedTime);
        }
    }

    public void RemoveState(States state)
    {
        StateUIManager stateManager;

        if (_iconsObj.TryGetValue(state, out stateManager)) {
            if (!stateManager) {
                return;
            }
            stateManager.gameObject.transform.SetParent(this.transform, true);
            stateManager.gameObject.SetActive(false);
        }
    }

    public void ResetState(States state)
    {
        StateUIManager stateManager;

        if (_iconsObj.TryGetValue(state, out stateManager)) {
            stateManager.ResetFillAmount();
        }
    }
}
