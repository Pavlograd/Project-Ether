// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EntityManager : MonoBehaviour
// {
//     protected bool _canAbilityAttack = true;
//     protected bool _canAutoAttack = true;
//     protected Ability _abilityOnUse;
//     protected GameObject _target;
//     [SerializeField] protected int _maxhealth = 100;
//     [SerializeField] protected float _health = 100.0f;
//     [SerializeField] protected float _damage = 10f;
//     [SerializeField] protected bool _ableToWalk = true;
//     [SerializeField] protected float _range = 5.0f;
//     [SerializeField] protected Transform _firePoint;
//     [SerializeField] protected AbilitiesManager _abilitiesManager;
//     [SerializeField] protected AbilitiesHolder _abilitiesHolder;
//     [SerializeField] private AudioSource _staffAudioSource;

//     public bool CanWalk()
//     {
//         return _ableToWalk;
//     }

//     public bool isDead()
//     {
//         return (_health <= 0);
//     }

//     public void ActivateAbility()
//     {
//         if (_abilityOnUse == null) {
//             return;
//         }
//         Ability ability = !_canAutoAttack && !_canAbilityAttack ?  _abilitiesHolder.defaultAttack : _abilityOnUse;
//         AbilityType type = ability.abilityType;
//         if (type == AbilityType.TARGET && _target != null) {
//             ability.Activate(_target.transform);
//             PlaySoundEffect(ability.soundFx);
//         } else if (type == AbilityType.PROJECTILE && _target != null) {
//             ability.Activate(gameObject.layer, _firePoint, _target.transform);
//             PlaySoundEffect(ability.soundFx);
//         } else if (type == AbilityType.BUFF || type == AbilityType.ACTIVABLE) {
//             ability.Activate(gameObject.transform);
//             PlaySoundEffect(ability.soundFx);
//         }
//     }

//     public void ReableAutoAttack()
//     {
//         _canAutoAttack = true;
//     }

//     public void ReableAbilityAttack()
//     {
//         _canAbilityAttack = true;
//     }

//     private void PlaySoundEffect(AudioClip clip)
//     {
//         _staffAudioSource.clip = clip;
//         _staffAudioSource.Play();
//     }
// }
