using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityAnimationManager : MonoBehaviour
{
    [SerializeField] public Animator bodyAnimator;
    protected EntityData _entityData;

    private void Awake()
    {
        _entityData = GetComponent<EntityData>();
    }

    virtual public void Start()
    {
        if (gameObject.name == "BossNotPlayer")
        {
            //TODO update boss skin to defender skin
        }
    }
    
    virtual public void Run(float animationSpeed)
    {
        if (!_entityData.entityHealthManager.isAlive) return;
        if (animationSpeed > 0) {
            SetAnimationSpeed(animationSpeed);
            bodyAnimator.SetBool("Is_running", true);
        } else {
            SetAnimationSpeed();
            bodyAnimator.SetBool("Is_running", false);
        }
    }

    virtual public void Fly(float animationSpeed, bool value)
    {
        if (!_entityData.entityHealthManager.isAlive) return;
        if (value)
            SetAnimationSpeed(animationSpeed);
        if (value && !bodyAnimator.GetBool("Is_flying")) {
            bodyAnimator.SetBool("Is_flying", value);
        } else if (!value && bodyAnimator.GetBool("Is_flying")) {
            SetAnimationSpeed();
            bodyAnimator.SetBool("Is_flying", value);
        }
    }

    virtual public void Ability()
    {
        if (_entityData.entityHealthManager.isAlive)
            bodyAnimator.SetTrigger("Ability");
    }

    virtual public void AutoAttack()
    {
        if (_entityData.entityHealthManager.isAlive)
            bodyAnimator.SetTrigger("Auto_attack");
    }

    public void TakeDamage()
    {
        // TO CHANGE
        if (_entityData.entityHealthManager.isAlive && !bodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ability"))
            bodyAnimator.SetTrigger("Take_damage");
    }

    virtual public void Dead()
    {
        SetAnimationSpeed();
        bodyAnimator.SetTrigger("Dead");
    }

    protected void SetAnimationSpeed(float speed = 1f)
    {
        if (!_entityData.entityHealthManager.isAlive) {
            bodyAnimator.speed = 1f;
            return;
        }
        if (speed < 0f)
            speed *= -1;
        bodyAnimator.speed = speed;
    }
}