using UnityEngine;

public class EntityAbilityManager : MonoBehaviour
{
    protected bool _canAbilityAttack = true;
    protected bool _canAutoAttack = true;
    protected Ability _abilityOnUse;
    protected GameObject _target;
    protected EntityData _entityData;
    public bool canAttack = true;
    public float rangeAttack = 5.0f;
    public AbilitiesHolder _abilitiesHolder;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected AbilitiesManager _abilitiesManager;

    private void Awake()
    {
        _entityData = GetComponent<EntityData>();
    }

    private void FaceTargetDirection()
    {
        if (_target == null) return;
        Vector2 enemyPos = _target.transform.position;
        Vector2 myPos = transform.position;
        if (enemyPos.x > myPos.x) {
            _entityData.entityMovementManager.RotateEntity(1);
        } else {
            _entityData.entityMovementManager.RotateEntity(-1);
        }
    }

    protected void TriggerAbility(Ability ability, bool isAutoAttack)
    {
        if (ability.IsOnCooldown() || ability.abilityType == AbilityType.NONE || (ability.abilityType == AbilityType.PROJECTILE && !TargetIsReachable())) {
            return;
        }
        FaceTargetDirection();
        if (isAutoAttack) {
            _canAutoAttack = false;
            _entityData.entityAnimationManager.AutoAttack();
        } else {
            _canAbilityAttack = false;
            _entityData.entityAnimationManager.Ability();
        }
        _abilityOnUse = ability;
        StartCoroutine(ability.ActiveCooldown());
    }

    public void ActivateAbility()
    {
        if (_abilityOnUse == null) {
            return;
        }
        Ability ability = !_canAutoAttack && !_canAbilityAttack ? _abilitiesHolder.defaultAttack : _abilityOnUse;
        AbilityType type = ability.abilityType;
        FaceTargetDirection();
        switch (type) {
            case AbilityType.TARGET:
                if (_target != null) {
                    ability.Activate(_target.transform, ComputeAdditionalDamages(ability.damages, ability.lvl));
                }
                break;
            case AbilityType.PROJECTILE:
                if (_target != null) {
                    ability.Activate(gameObject.layer, _firePoint, _target.transform, ComputeAdditionalDamages(ability.damages, ability.lvl));
                }
                break;
            case AbilityType.BUFF:
                ability.Activate(gameObject.transform);
                break;
            case AbilityType.ACTIVABLE:
                ability.Activate(gameObject.transform, gameObject.layer, ComputeAdditionalDamages(ability.damages, ability.lvl));
                break;
            default:
                break;
        }
        if (type != AbilityType.NONE && AudioManager.instance) {
            AudioManager.instance.PlaySoundEffect(ability.sfx);
        }
    }

    public bool TargetIsReachable()
    {
        return !(Physics2D.Linecast(transform.position, _target.transform.position, 1 << LayerMask.NameToLayer("Obstacle"))).collider;
    }

    public void ReableAutoAttack()
    {
        _canAutoAttack = true;
    }

    public void ReableAbilityAttack()
    {
        _canAbilityAttack = true;
    }

    private float ComputeAdditionalDamages(float initialDamage, float abiliyLevel)
    {
        float additionalDamages = 0;

        foreach (DataBuff data in _entityData.entityStateManager.currentBuffs) {
            if (data.type == States.STRENGTH || data.type == States.ELEMENTAL_STRENGTH) {
                if (data.isPourcentage) {
                    additionalDamages += (initialDamage / 100) * data.increasedAmount;
                } else {
                    additionalDamages += data.increasedAmount;
                }
            }
        }
        if (abiliyLevel > 1) {
            additionalDamages += 5 * abiliyLevel - 1;
        }
        return additionalDamages;
    }
}