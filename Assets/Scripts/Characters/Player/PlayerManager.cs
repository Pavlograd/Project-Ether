// using UnityEngine;
// using UnityEngine.AI;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class PlayerManager : EntityManager
// {
//     [SerializeField] private HealthBar _healthBar;
//     [SerializeField] private GameObject _abilityCanvas;
//     private PlayerAnimations _playerAnimations;
//     public bool ableToTeleport = true;

//     void Start()
//     {
//         _playerAnimations = transform.Find("Sprites").GetComponent<PlayerAnimations>();
//         if (_healthBar)
//             _healthBar.SetMaxHealth(_maxhealth);
//         SetupAbilityCanvas();
//     }

//     void Update()
//     {
//         if (isDead())
//         {
//             //todo call canvasmanager.showEndGameUI()
//         } else {
//             _target = FindClosestEnemy(transform.position, _range);
//             if (_canAutoAttack && _canAbilityAttack) {
//                 DefaultAttack();
//             }
//         }
//     }

//     private void SetupAbilityCanvas()
//     {
//         List<Ability> abilities = _abilitiesHolder.GetOriginalAbilities();

//         for (int index = 0; index < abilities.Count; index++) {
//             Image imgComponent = _abilityCanvas.transform.GetChild(index).GetComponent<Image>();

//             if (imgComponent != null) {
//                 imgComponent.sprite = abilities[index].thumbnail;
//             }
//         }
//     }

//     private static GameObject FindClosestEnemy(Vector3 pos, float range)
//     {
//         float closest = 0;
//         GameObject closestEnemy = null;

//         foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
//         {
//             float distance = Vector3.Distance(pos, enemy.transform.localPosition);

//             if (closestEnemy && (distance > closest || distance > range))
//                 continue;
//             closestEnemy = enemy;
//             closest = distance;
//         }
//         return closest > range ? null : closestEnemy;
//     }

//     IEnumerator AbilityRegenerator(float delaySeconds, int slot)
//     {
//         float animationTime = delaySeconds;
//         Transform maskUiCanvas = _abilityCanvas.transform.GetChild(slot - 1).GetChild(0);

//         for (; animationTime > 0; animationTime -= Time.deltaTime)
//         {
//             maskUiCanvas.GetComponent<Image>().fillAmount = animationTime / delaySeconds;
//             yield return new WaitForEndOfFrame();
//         }
//         maskUiCanvas.GetComponent<Image>().fillAmount = 0;
//     }

//     private void TriggerAbility(Ability ability, bool isAutoAttack, int index = 0)
//     {
//         if (ability.IsOnCooldown() || ability.abilityType == AbilityType.NONE) {
//             return;
//         }
//         if (isAutoAttack) {
//             _canAutoAttack = false;
//             _playerAnimations.AutoAttack();
//         } else {
//             _canAbilityAttack = false;
//             _playerAnimations.Ability();
//         }
//         _abilityOnUse = ability;
//         StartCoroutine(ability.ActiveCooldown());
//         if (index > 0) {
//             StartCoroutine(AbilityRegenerator(ability.cooldownTime, index));
//         }
//     }

//     public void LaunchAttack(int index)
//     {
//         try {
//             if (!_canAbilityAttack || index < 1 && index > 4 || !_target)
//                 return;
//             TriggerAbility(_abilitiesHolder.abilities[index - 1], false, index);
//         } catch (System.Exception err) {
//             Debug.LogError(err);
//         }
//     }

//     private void DefaultAttack()
//     {
//         try {
//             if (!_target) {
//                 return;
//             }
//             TriggerAbility(_abilitiesHolder.defaultAttack, true);
//         } catch (System.Exception err) {
//             Debug.LogError(err);
//         }
//     }

//     //TODO REARRANGE TRAPS/CHEST MANAGER AND ADD REFERENCE HERE (split in other file if possible)

//     void TriggerStepTuto(Collider other)
//     {
//         _ableToWalk = false;
//         int index = other.transform.GetSiblingIndex();
//         GameObject nextBrotherNode = other.transform.parent.GetChild(index + 1).gameObject;

//         if (nextBrotherNode != null)
//         {
//             nextBrotherNode.SetActive(true);
//         }

//         if (other.transform.parent.gameObject.name == "Tuto_trigger_step_Trap")
//         {
//             other.transform.parent.GetChild(index + 2).GetChild(1).gameObject.SetActive(true);
//         }
//         else if (other.transform.parent.gameObject.name == "Tuto_trigger_step_Gard")
//         {
//             //Here for the camera movmnent
//         }
//     }

//     void TriggerLootChest(Collider other)
//     {
//         ChestManager chest_object = other.transform.parent.gameObject.GetComponent<ChestManager>();
//         chest_object.ReabledAnimation();
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.tag == "TutoInvisibleWall")
//         {
//             TriggerStepTuto(other);
//         }
//         else if (other.gameObject.tag == "LootChest")
//         {
//             TriggerLootChest(other);
//         }
//     }

//     public void TakeDamageFromTrap(float damage)
//     {
//         // Here in case there is a skill with a buff

//         TakeDamage(damage);
//     }

//     public void TakeDamage(float damage)
//     {
//         int a;
//         _health -= damage;
//         a = (int)_health;
//         if (_healthBar)
//             _healthBar.SetHealth(a);
//     }
// }