public class AIHealthManager : EntityHealthManager
{
    private AILootPool _lootpool;

    protected override void Awake() {
        base.Awake();
        AILootPool lootpool = GetComponent<AILootPool>();
        if (lootpool) {
            _lootpool = lootpool;
        }
    }

    protected override void Start()
    {
        base.Start();
        _healthBar.gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _healthBar.gameObject.SetActive(true);
    }

    protected override void Die()
    {
        if (_lootpool) {
            _lootpool.ActiveLoot();
        }
        base.Die();
        Destroy(gameObject, _entityData.entityAnimationManager.bodyAnimator.GetCurrentAnimatorStateInfo(0).length);
    }
}
