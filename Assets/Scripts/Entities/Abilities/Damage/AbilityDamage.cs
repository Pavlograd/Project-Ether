using UnityEngine;

public struct DamageData {
    public float damage;
    public States state;
    public int parentLayer;
}

public class AbilityDamage : MonoBehaviour
{
    [SerializeField] protected GameObject _hitEffect;
    [SerializeField] protected LayerMask _hitableMask;
    protected DamageData _damageData;
}