using UnityEngine;

public class EnableAbilityOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AbilitiesManager abilitiesManager = animator.gameObject.GetComponent<AbilitiesManager>();
        if (abilitiesManager != null) {
            abilitiesManager.EndAbility();
        }
    }
}
