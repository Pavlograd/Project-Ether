using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float detectionRange = 1.0f;
    public float attackDelay = 1.0f;
    public float attackDamage = 25.0f;
    public float health = 75.0f;

    private Vector3 _movement;
    private Vector3 _basePosition;
    private Rigidbody _rb;
    private Animator _animator;
    private ParticleSystem _attackEffect;
    private float _lastAttackTime;
    private bool _isDead = false;
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int AttackAnimation = Animator.StringToHash("attack");
    private static readonly int HitAnimation = Animator.StringToHash("hit");
    private static readonly int DeadAnimation = Animator.StringToHash("dead");

    private void Start()
    {
        _basePosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _attackEffect = GetComponentInChildren<ParticleSystem>();
        _lastAttackTime = Time.time;
    }

    private void Update()
    {
        if (_isDead)
            return;
        Vector3 playerPos = player.transform.position;
        float dist = Vector3.Distance(playerPos, transform.position);

        GetComponent<NavMeshAgent>().SetDestination(
            dist < detectionRange
            ? player.transform.position
            : _basePosition);
        if (dist < 5f)
            Attack();
        _animator.SetBool(IsIdle, Math.Abs(_rb.velocity.magnitude) < 0.05f);
    }

    private void Attack()
    {
        if (Time.time - _lastAttackTime < attackDelay)
            return;
        _attackEffect.Play();
        _lastAttackTime = Time.time;
        _animator.SetTrigger(AttackAnimation);
        player.GetComponent<PlayerManager>().Hit(attackDamage);
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0) {
            _animator.SetTrigger(DeadAnimation);
            _isDead = true;
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            GetComponent<NavMeshAgent>().SetDestination(transform.position);
            Destroy(gameObject, 5f);
        } else
            _animator.SetTrigger(HitAnimation);
    }
}
