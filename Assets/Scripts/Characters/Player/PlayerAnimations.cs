// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;

// public class PlayerAnimations : MonoBehaviour
// {
//     [SerializeField] private Animator _playerAnimator;
//     [SerializeField] private Animator _staffAnimator;
//     private bool _facing = true; // false == LEFT, true == RIGHT

//     public void Run(float animationSpeed)
//     {
//         if (animationSpeed > 0) {
//             SetAnimationSpeed(animationSpeed);
//             _playerAnimator.SetBool("Is_running", true);
//             _staffAnimator.SetBool("Is_running", true);
//         } else {
//             SetAnimationSpeed();
//             _playerAnimator.SetBool("Is_running", false);
//             _staffAnimator.SetBool("Is_running", false);
//         }
//     }

//     public void Fly(float animationSpeed, bool value)
//     {
//         if (value)
//             SetAnimationSpeed(animationSpeed);
//         if (value && !_playerAnimator.GetBool("Is_flying")) {
//             _playerAnimator.SetBool("Is_flying", value);
//         } else if (!value && _playerAnimator.GetBool("Is_flying")) {
//             SetAnimationSpeed();
//             _playerAnimator.SetBool("Is_flying", value);
//         }
//         if (value && !_staffAnimator.GetBool("Is_flying")) {
//             _staffAnimator.SetBool("Is_flying", value);
//         } else if (!value && _staffAnimator.GetBool("Is_flying")) {
//             _staffAnimator.SetBool("Is_flying", value);
//         }
//     }

//     public void Ability()
//     {
//         _playerAnimator.SetTrigger("Ability");
//         _staffAnimator.SetTrigger("Ability");
//     }

//     public void AutoAttack()
//     {
//         _playerAnimator.SetTrigger("Auto_attack");
//         _staffAnimator.SetTrigger("Auto_attack");
//     }

//     public void TakeDamage()
//     {
//         _playerAnimator.SetTrigger("Take_damage");
//         _staffAnimator.SetTrigger("Take_damage");
//     }

//     public void Dead()
//     {
//         _playerAnimator.SetTrigger("Dead");
//         _staffAnimator.SetTrigger("Dead");
//     }

//     public void FlipSprites(bool state)
//     {
//         _facing = state;
//         transform.localScale = state ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
//     }

//     private void SetAnimationSpeed(float speed = 1f)
//     {
//         if (speed < 0f)
//             speed *= -1;
//         _playerAnimator.speed = speed;
//     }

//     // V1 SCRIPT VERSION

//     // [SerializeField] private Animator _playerAnimator;
//     // [SerializeField] private Animator _staffAnimator;
//     // private CharacterAnimationState _playerState = CharacterAnimationState.IDLE_GROUND;
//     // private CharacterAnimationState _staffState = CharacterAnimationState.IDLE_GROUND;
//     // private bool _facing = true; // false == LEFT, true == RIGHT

//     // public void Run(float animationSpeed)
//     // {
//     //     SetAnimationSpeed(animationSpeed);
//     //     if (_playerState != CharacterAnimationState.RUN)
//     //     {
//     //         _playerAnimator.SetTrigger("Run");
//     //         _playerState = CharacterAnimationState.RUN;
//     //     }
//     //     if (_staffState != CharacterAnimationState.RUN)
//     //     {
//     //         _staffAnimator.SetTrigger("Run");
//     //         _staffState = CharacterAnimationState.RUN;
//     //     }
//     // }

//     // public void Idle()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.IDLE_GROUND)
//     //     {
//     //         _playerAnimator.SetTrigger("Idle_ground");
//     //         _playerState = CharacterAnimationState.IDLE_GROUND;
//     //     }

//     //     if (_staffState != CharacterAnimationState.IDLE_GROUND)
//     //     {
//     //         _staffAnimator.SetTrigger("Idle_ground");
//     //         _staffState = CharacterAnimationState.IDLE_GROUND;
//     //     }
//     // }

//     // public void IdleAir()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.IDLE_AIR)
//     //     {
//     //         _playerAnimator.SetTrigger("Idle_air");
//     //         _playerState = CharacterAnimationState.IDLE_AIR;
//     //     }

//     //     if (_staffState != CharacterAnimationState.IDLE_AIR)
//     //     {
//     //         _staffAnimator.SetTrigger("Idle_air");
//     //         _staffState = CharacterAnimationState.IDLE_AIR;
//     //     }
//     // }

//     // public void Fly()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.FLY)
//     //     {
//     //         _playerAnimator.SetTrigger("Fly");
//     //         _playerState = CharacterAnimationState.FLY;
//     //     }

//     //     if (_staffState != CharacterAnimationState.FLY)
//     //     {
//     //         _staffAnimator.SetTrigger("Fly");
//     //         _staffState = CharacterAnimationState.FLY;
//     //     }
//     // }

//     // public void AttackIdleGround()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.ATTACK_GROUND && _staffState != CharacterAnimationState.ATTACK_GROUND)
//     //     {
//     //         _playerAnimator.SetTrigger("Attack_ground");
//     //         _staffAnimator.SetTrigger("Attack_ground");
//     //         _playerState = CharacterAnimationState.ATTACK_GROUND;
//     //         _staffState = CharacterAnimationState.ATTACK_GROUND;
//     //     }
//     // }

//     // public void AttackIdleGroundInverted()
//     // {
//     //     if (_playerState != CharacterAnimationState.ATTACK_GROUND && _staffState != CharacterAnimationState.ATTACK_GROUND)
//     //     {
//     //         _playerAnimator.SetTrigger("Attack_ground");
//     //         _staffAnimator.SetTrigger("Attack_ground_inverted");
//     //         _playerState = CharacterAnimationState.ATTACK_GROUND;
//     //         _staffState = CharacterAnimationState.ATTACK_GROUND;
//     //     }
//     // }

//     // public void AttackRunGround()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.RUN)
//     //     {
//     //         _playerAnimator.SetTrigger("Run");
//     //         _playerState = CharacterAnimationState.RUN;
//     //     }

//     //     if (_staffState != CharacterAnimationState.ATTACK_GROUND)
//     //     {
//     //         _staffAnimator.SetTrigger("Attack_ground");
//     //         _staffState = CharacterAnimationState.ATTACK_GROUND;
//     //     }
//     // }

//     // public void AttackAir()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.ATTACK_AIR && _staffState != CharacterAnimationState.ATTACK_AIR)
//     //     {
//     //         _playerAnimator.SetTrigger("Attack_air");
//     //         _staffAnimator.SetTrigger("Attack_air");
//     //         _playerState = CharacterAnimationState.ATTACK_AIR;
//     //         _staffState = CharacterAnimationState.ATTACK_AIR;
//     //     }
//     // }

//     // public void AttackFlyAir()
//     // {
//     //     SetAnimationSpeed();
//     //     if (_playerState != CharacterAnimationState.FLY)
//     //     {
//     //         _playerAnimator.SetTrigger("Fly");
//     //         _playerState = CharacterAnimationState.FLY;
//     //     }

//     //     if (_staffState != CharacterAnimationState.ATTACK_AIR)
//     //     {
//     //         _staffAnimator.SetTrigger("Attack_air");
//     //         _staffState = CharacterAnimationState.ATTACK_AIR;
//     //     }
//     // }

//     // public void TakeDamage()
//     // {
//     //     if (_playerState != CharacterAnimationState.TAKE_DAMAGE && _staffState != CharacterAnimationState.TAKE_DAMAGE)
//     //     {
//     //         SetAnimationSpeed();
//     //         _playerAnimator.SetTrigger("Take_damage");
//     //         _staffAnimator.SetTrigger("Take_damage");
//     //         _playerState = CharacterAnimationState.TAKE_DAMAGE;
//     //         _staffState = CharacterAnimationState.TAKE_DAMAGE;
//     //     }
//     // }

//     // public void Dead()
//     // {
//     //     if (_playerState != CharacterAnimationState.DEAD && _staffState != CharacterAnimationState.DEAD)
//     //     {
//     //         SetAnimationSpeed();
//     //         _playerAnimator.SetTrigger("Dead");
//     //         _staffAnimator.SetTrigger("Dead");
//     //         _playerState = CharacterAnimationState.DEAD;
//     //         _staffState = CharacterAnimationState.DEAD;
//     //     }
//     // }

//     // public void FlipSprites(bool state)
//     // {
//     //     _facing = state;
//     //     transform.localScale = state ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
//     // }

//     // private void SetAnimationSpeed(float speed = 1f)
//     // {
//     //     if (speed < 0f)
//     //         speed *= -1;
//     //     _playerAnimator.speed = speed;
//     //     // _staffAnimator.speed = speed;
//     // }
// }