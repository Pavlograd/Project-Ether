using UnityEngine;
using UnityEngine.UI;

public class EntityHealthManager : MonoBehaviour
{
    [HideInInspector] public bool isAlive = true;
    [SerializeField] protected float _health = 100;
    protected float _maxHealth = 100;
    [SerializeField] protected Slider _healthBar;
    protected EntityData _entityData;


    protected virtual void Awake()
    {
        _entityData = GetComponent<EntityData>();
        _maxHealth = _health;
    }

    protected virtual void Start()
    {
        if (_healthBar) {
            _healthBar.maxValue = _health;
            _healthBar.value = _health;
            _healthBar.minValue = 0;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive)
            return;
        _health -= damage;
        if (_health <= 0)
            Die();
        if (_healthBar) {
            _healthBar.value = _health;
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        _entityData.entityAnimationManager.Dead();
        gameObject.tag = "Dead";
    }

    public void SetHealthBar(Slider healthBar)
    {
        _healthBar = healthBar;
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
    }

    public float GetHealthRatio()
    {
        return _health / _maxHealth;
    }
}