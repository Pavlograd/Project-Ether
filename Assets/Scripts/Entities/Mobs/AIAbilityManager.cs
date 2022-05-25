using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIAbilityManager : EntityAbilityManager
{
    private bool _isWaitingForCapacity = false;
    public ElementaryType elementaryType;

    protected virtual void Start()
    {
        _target = GameObject.Find("Player");
    }

    private void Update()
    {
        if (TargetIsClose() && _canAutoAttack && canAttack)
            TriggerAbility(_abilitiesHolder.defaultAttack, true);
        if (!_isWaitingForCapacity && _canAbilityAttack && canAttack)
            StartCoroutine(UseCapacity());
    }

    protected bool TargetIsClose()
    {
        return Vector2.Distance(transform.position, _target.transform.position) <= rangeAttack;
    }

    private IEnumerator UseCapacity()
    {
        Ability ability = _abilitiesHolder.abilities[Random.Range(0, _abilitiesHolder.abilities.Count)];
        _isWaitingForCapacity = true;
        yield return new WaitForSeconds(Random.Range(ability.cooldownTime, ability.cooldownTime * 2));
        _isWaitingForCapacity = false;
        if (TargetIsClose())
            TriggerAbility(ability, false);
    }
}

public enum ElementaryType
{
    NORMAL,
    ROCK,
    FIRE,
    WATER,
    ICE,
    PSYCHIC,
    POISON,
    ELECTRIC,
}