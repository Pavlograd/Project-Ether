using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : EntityAnimationManager
{
    [SerializeField] private Animator _staffAnimator;

    public override void Run(float animationSpeed)
    {
        if (!_entityData.entityHealthManager.isAlive) return;
        base.Run(animationSpeed);
        if (animationSpeed > 0) {
            _staffAnimator.SetBool("Is_running", true);
        } else {
            _staffAnimator.SetBool("Is_running", false);
        }
    }

    public override void Fly(float animationSpeed, bool value)
    {
        if (!_entityData.entityHealthManager.isAlive) return;
        base.Fly(animationSpeed, value);
        if (value && !bodyAnimator.GetBool("Is_flying")) {
            _staffAnimator.SetBool("Is_running", value);
        } else if (!value && bodyAnimator.GetBool("Is_flying")) {
            SetAnimationSpeed();

            _staffAnimator.SetBool("Is_running", value);
        }
    }

    public override void Ability()
    {
        if (!_entityData.entityHealthManager.isAlive) return;
        base.Ability();
        _staffAnimator.SetTrigger("Ability");
    }

    public override void AutoAttack()
    {
        if (!_entityData.entityHealthManager.isAlive) return;
        base.AutoAttack();
        _staffAnimator.SetTrigger("Auto_attack");
    }

    public override void Dead()
    {
        base.Dead();
        _staffAnimator.SetTrigger("Dead");
    }
}