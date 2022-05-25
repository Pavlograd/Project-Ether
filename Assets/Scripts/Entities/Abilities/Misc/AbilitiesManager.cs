using UnityEngine;

public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] private EntityAbilityManager _entityAbilityManager;

    public void ActivateAbility()
    {
        _entityAbilityManager.ActivateAbility();
    }

    public void EndAbility()
    {
        _entityAbilityManager.ReableAbilityAttack();
    }

    public void EndAttack()
    {
        _entityAbilityManager.ReableAutoAttack();
    }
}
