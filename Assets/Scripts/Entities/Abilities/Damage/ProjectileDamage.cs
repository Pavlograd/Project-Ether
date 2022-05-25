using UnityEngine;

public class ProjectileDamage : AbilityDamage
{
    [SerializeField] protected Transform _hitPoint;
    private bool _doesIgnoreObstacle = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(this._hitableMask == (this._hitableMask | (1 << other.gameObject.layer)))
            || other.gameObject.layer == this._damageData.parentLayer) {
            return;
        }
        if (this._doesIgnoreObstacle && LayerMask.LayerToName(other.gameObject.layer) == "Obstacle") {
            return;
        }
        Transform ground = other.gameObject.transform.Find("Ground");
        EntityData entityData = other.gameObject.GetComponent<EntityData>();
        Destroy(gameObject);
        if (entityData) {
            if (entityData.entityHealthManager.isAlive) {
                entityData.entityAnimationManager.TakeDamage();
                entityData.entityHealthManager.TakeDamage(this._damageData.damage);
                entityData.entityStateManager.ActiveHarmfullEffects(this._damageData.state);
            }
        }
        if (this._hitEffect) {
            Vector3 hitEffectPos = ground == null ? this._hitPoint.position : ground.position;
            Instantiate(this._hitEffect, hitEffectPos, Quaternion.identity);
        }
    }

    public void Setup(bool doesIgnoreObstacle, DamageData damageData)
    {
        this._doesIgnoreObstacle = doesIgnoreObstacle;
        this._damageData = damageData;
    }
}
