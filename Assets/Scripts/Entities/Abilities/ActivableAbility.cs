using UnityEngine;

[CreateAssetMenu(fileName = "Abitlity", menuName = "Ability/ActivableAbility")]
public class ActivableAbility : Ability
{
    [Header("Effects")]
    [SerializeField] private float _duration;
    [SerializeField] private float _occurrence;

    public override void Activate(Transform parent, int parentLayer, float additionalDamages)
    {
        GameObject obj = Instantiate(this._particles, parent.position, new Quaternion(0, 0, 0, 1));

        ActivableDamage objDamage = obj.GetComponent<ActivableDamage>();
        if (objDamage) {
            DamageData damageData = new DamageData {
                damage = this.damages + additionalDamages,
                parentLayer = parentLayer,
                state = this.state,
            };
            objDamage.Setup(this._occurrence, damageData);
        }
        obj.transform.parent = parent;
        Destroy(obj, this._duration);
    }
}
