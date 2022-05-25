using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerElementsStates : MonoBehaviour
{
    public States state;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<EntityStateManager>().ActiveHarmfullEffects(state);
        }
    }
}
