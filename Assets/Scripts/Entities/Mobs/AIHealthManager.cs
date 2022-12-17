using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnTargetHurt : UnityEvent<float> { }

public class AIHealthManager : EntityHealthManager
{
    public static OnTargetHurt onTargetHurt;
    private AILootPool _lootpool;

    protected override void Start()
    {
        base.Start();
        if (!TryGetComponent<AILootPool>(out _lootpool))
        {
            Debug.LogError("Lootpool script is missing");
        }
        else
        {
            _lootpool.SetAbilityPool(this._entityData.entityAbilityManager._abilitiesHolder.abilities);
        }
        _healthBar.gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (this._entityData.entityStateManager.isTargeted)
        {
            onTargetHurt?.Invoke(_health / _maxHealth);
        }
        _healthBar.gameObject.SetActive(true);
    }

    protected override void Die()
    {
        _lootpool?.ActiveLoot();
        base.Die();

        // Modify room in consequence
        Room room = transform.parent.parent.gameObject.GetComponent<Room>();

        room.MobKilled(gameObject);

        Destroy(gameObject, _entityData.entityAnimationManager.bodyAnimator.GetCurrentAnimatorStateInfo(0).length);
    }
}
