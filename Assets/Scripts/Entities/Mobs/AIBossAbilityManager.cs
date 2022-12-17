using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBossAbilityManager : AIAbilityManager
{
    private List<Ability> _buffAbilities = new List<Ability>(4);
    private List<Ability> _attackAbilities = new List<Ability>(4);
    private float timerAttack = 0f;
    [SerializeField] private RuntimeAnimatorController _playerController;//TEMPORAIRE

    protected override void Start()
    {
        _target = GameObject.Find("Player");

        _entityData.entityAnimationManager.bodyAnimator.runtimeAnimatorController = _playerController;

        foreach (Ability ability in _abilitiesHolder.abilities) {
            if (ability.abilityType == AbilityType.BUFF || ability.abilityType == AbilityType.ACTIVABLE)
                _buffAbilities.Add(ability);
            else
                _attackAbilities.Add(ability);
        }
    }

    private void Update()
    {
        if (!TargetIsClose())
            return;
        if (timerAttack <= 0f) {
            if (_canAbilityAttack) {
                foreach (Ability buffAbility in _abilitiesHolder.abilities) {
                    if (!buffAbility.IsOnCooldown()) {
                        TriggerAbility(buffAbility, false);
                        timerAttack = 3f;
                        return;
                    }
                }
                foreach (Ability attackAbilities in _attackAbilities)
                    if (!attackAbilities.IsOnCooldown()) {
                        TriggerAbility(attackAbilities, false);
                        timerAttack = 3f;
                        return;
                    }
            }
        } else
            timerAttack -= Time.deltaTime;
        if (_canAbilityAttack && _canAutoAttack) {
            TriggerAbility(_abilitiesHolder.defaultAttack, true);
        }
    }
}