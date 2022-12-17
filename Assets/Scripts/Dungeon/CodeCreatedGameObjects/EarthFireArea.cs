using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthFireArea : MonoBehaviour
{
    Dictionary<GameObject, EntityStateManager> _entityInZone = new Dictionary<GameObject, EntityStateManager>();
    Coroutine _applyEffectLoop;

    private void Start()
    {
        Invoke("DestroyObject", 10f);
        _applyEffectLoop = StartCoroutine(ApplyEffect());
    }

    private IEnumerator ApplyEffect()
    {
        while(true) {
            foreach (KeyValuePair<GameObject, EntityStateManager> item in _entityInZone) {
                item.Value.ForceActiveHarmfullEffects(States.DOT);
                item.Value.ForceActiveHarmfullEffects(States.STUN);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_entityInZone.ContainsKey(other.gameObject) && other.TryGetComponent<EntityStateManager>(out var component)) {
            _entityInZone.Add(other.gameObject, component);
            component.ForceActiveHarmfullEffects(States.DOT);
            component.ForceActiveHarmfullEffects(States.STUN);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_entityInZone.ContainsKey(other.gameObject)) {
            _entityInZone.Remove(other.gameObject);
        }
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     Debug.Log("in");
    //     // Create of component to avoid GetComponent on each frame
    //     // if (other.TryGetComponent<EntityStateManager>(out var component)) {
    //     //     component.ActiveHarmfullEffects(States.FIRE);
    //     // }
    // }
}
