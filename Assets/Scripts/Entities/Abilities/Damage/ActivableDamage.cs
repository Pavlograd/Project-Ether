using System.Collections;
using UnityEngine;

public class ActivableDamage : AbilityDamage
{
    private bool _isIn = false;
    private float _occurrence;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(this._hitableMask == (this._hitableMask | (1 << other.gameObject.layer)))
            || other.gameObject.layer == this._damageData.parentLayer) {
            return;
        }
        Transform ground = other.gameObject.transform.Find("Ground");
        EntityData entityData = other.gameObject.GetComponent<EntityData>();
        if (entityData) {
            if (entityData.entityHealthManager.isAlive) {
                this._isIn = true;
                StartCoroutine(DealDamage(entityData));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((this._hitableMask == (this._hitableMask | (1 << other.gameObject.layer)))) {
            this._isIn = false;
        }
    }

    public void Setup(float occurrence, DamageData damageData)
    {
        this._occurrence = occurrence;
        this._damageData = damageData;
    }

    private IEnumerator DealDamage(EntityData entityData)
    {
        while (this._isIn) {
            if (entityData != null && entityData.entityHealthManager.isAlive) {
                entityData.entityAnimationManager.TakeDamage();
                entityData.entityHealthManager.TakeDamage(this._damageData.damage);
                entityData.entityStateManager.ActiveHarmfullEffects(this._damageData.state);
                yield return new WaitForSeconds(this._occurrence);
            } else {
                this._isIn = false;
            }
        }
    }
}