using System.Collections;
using UnityEngine;

public class Ability : ScriptableObject
{
    protected bool _isOnUse = false;

    [Header("Global")]
    public new string name;
    public SFX sfx = SFX.DEFAULT;
    public float cooldownTime;
    public Sprite thumbnail;
    public AbilityType abilityType;
    public float damages;
    public int lvl;
    public int id;
    public int parentId;
    public int geared;
    [SerializeField] protected GameObject _particles;
    public States state = States.NEUTRAL;

    public virtual void Activate(int parentLayer, Transform firePoint, Transform targetTransform, float additionalDamages, bool doesIgnoreObstacle = false) {}
    public virtual void Activate(Transform transform, float additionalDamages) {}
    public virtual void Activate(Transform transform) {}
    public virtual void Activate(Transform parent, int parentLayer, float additionalDamages) {}

    public IEnumerator ActiveCooldown()
    {
        this._isOnUse = true;
        yield return new WaitForSeconds(this.cooldownTime);
        this._isOnUse = false;
    }

    public bool IsOnCooldown()
    {
        return this._isOnUse;
    }

    public void ResetCooldown()
    {
        this._isOnUse = false;
    }
}
