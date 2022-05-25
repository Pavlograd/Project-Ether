using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
//    [SerializeField] private PlayerManager _player;
//    [SerializeField] public float speed = 6.0f;
//    [SerializeField] private VariableJoystick _variableJoystick;
//
//    private Rigidbody2D _rb;
//    private PlayerAnimations _playerAnimations;
//
//    private bool _onceDead;
//    private float _horizontalAxis;
//    private float _verticalAxis;
//
//    void Awake()
//    {
//        _playerAnimations = transform.Find("Sprites").GetComponent<PlayerAnimations>();
//        _player = GetComponent<PlayerManager>();
//        _rb = GetComponent<Rigidbody2D>();
//        _onceDead = false;
//    }
//
//    void Update()
//    {
//        if (!_player.isDead())
//        {
//            _horizontalAxis = Mathf.Abs(_variableJoystick.Horizontal) > 0.1f ? _variableJoystick.Horizontal : Input.GetAxis("Horizontal");
//            _verticalAxis = Mathf.Abs(_variableJoystick.Vertical) > 0.1f ? _variableJoystick.Vertical : Input.GetAxis("Vertical");
//            if (!_player.CanWalk())
//                DisablePlayerAnimation();
//            else
//            {
//                if (_horizontalAxis == 0f && _verticalAxis == 0f)
//                    DisablePlayerAnimation();
//                else
//                {
//                    Run();
//                    if (_horizontalAxis < 0f)
//                        _playerAnimations.FlipSprites(true);
//                    else
//                        _playerAnimations.FlipSprites(false);
//                    PlayerMove();
//                }
//            }
//        }
//        else if (_player.isDead() && _onceDead == false)
//            PlayerDie();
//    }
//
//    private void Run()
//    {
//        _playerAnimations.Run(CalculateSpeed(), true);
//    }
//
//    private float CalculateSpeed()
//    {
//        Vector3 position = new Vector3(_horizontalAxis, _verticalAxis, 0);
//        return Vector3.Distance(position, Vector3.zero);
//    }
//
//    private void DisablePlayerAnimation()
//    {
//        _playerAnimations.Run(CalculateSpeed(), false);
//    }
//
//    private void PlayerMove()
//    {
//        Vector3 movement = new Vector3(speed * _horizontalAxis, speed * _verticalAxis, 0);
//        movement *= Time.deltaTime;
//        transform.position += movement;
//    }
//
//    private void PlayerDie()
//    {
//        _playerAnimations.Dead();
//        _onceDead = true;
//    }
}