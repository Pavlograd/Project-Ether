using UnityEngine;

[CreateAssetMenu(fileName = "Abitlity", menuName = "Ability/OnTargetAbility")]
public class OnTargetAbility : Ability
{
    public override void Activate(Transform target, float additionalDamages)
    {
        Transform ground = target.Find("Ground");
        GameObject projectile = Instantiate(this._particles, ground == null ? target : ground);
        EntityData entityData = target.gameObject.GetComponent<EntityData>();

        if (entityData) {
            entityData.entityHealthManager.TakeDamage(this.damages + additionalDamages);
            if (state != States.NEUTRAL) {
                entityData.entityStateManager.ActiveHarmfullEffects(state);
            }
        }
        Destroy(projectile, .5f);
    }
}
