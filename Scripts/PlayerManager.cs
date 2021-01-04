using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float health = 100.0f;
    public float attackRange = 5.0f;
    public float attackDamage = 50f;
    public float attackDelay = 1.0f;

    private float _lastAttackTime;
    private ParticleSystem _getHitEffect;
    private ParticleSystem _attackEffect;
    private Animator _animator;
    private static readonly int AttackAnimation = Animator.StringToHash("attack");
    private static readonly int HitAnimation = Animator.StringToHash("hit");

    private void Start()
    {
        GameObject healthText = GameManager.Instance.healthText;
        _getHitEffect = GameObject.Find("HitDamage").GetComponent<ParticleSystem>();
        _attackEffect = GameObject.Find("AttackEffect").GetComponent<ParticleSystem>();
        _lastAttackTime = Time.time;
        _animator = transform.Find("Model").GetComponent<Animator>();

        if (healthText.activeSelf)
            healthText.GetComponent<Text>().text = health.ToString(CultureInfo.CurrentCulture) + "❤";
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            Attack();
    }

    public void Hit(float damage)
    {
        GameObject healthText = GameManager.Instance.healthText;
        health -= damage;
        _getHitEffect.Play();
        _animator.SetTrigger(HitAnimation);

        if (health <= 0)
            GameManager.Instance.KillPlayer(gameObject);
        if (healthText.activeSelf)
            healthText.GetComponent<Text>().text = health.ToString(CultureInfo.CurrentCulture) + "❤";
    }

    private GameObject GetNearestEnemy()
    {
        GameObject found = null;
        float lastDist = -1;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist > attackRange || (dist > lastDist && found))
                continue;
            else if (enemy.GetComponent<Enemy>().IsDead())
                continue;
            found = enemy;
            lastDist = dist;
        }

        return (found);
    }

    public void Attack()
    {
        if (Time.time - _lastAttackTime < attackDelay)
            return;
        _attackEffect.Play();
        _lastAttackTime = Time.time;
        GameObject enemy = GetNearestEnemy();
        if (!enemy)
            return;
        enemy.GetComponent<Enemy>().Hit(attackDamage);
        _animator.SetTrigger(AttackAnimation);
    }
}
