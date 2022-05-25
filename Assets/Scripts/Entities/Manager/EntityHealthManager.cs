using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealthManager : MonoBehaviour
{
    [HideInInspector] public bool isAlive = true;
    [SerializeField] private float _health = 100;
    [SerializeField] protected Slider _healthBar;
    protected EntityData _entityData;

    protected virtual void Awake()
    {
        _entityData = GetComponent<EntityData>();
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
        if (gameObject.name == "Player");
            print(_healthBar.value);
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
}