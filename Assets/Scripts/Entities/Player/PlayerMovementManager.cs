using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : EntityMovementManager
{
    private float _horizontalAxis;
    private float _verticalAxis;
    public bool ableToTeleport = true;
    [SerializeField] private VariableJoystick _variableJoystick;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_variableJoystick && _entityData.entityHealthManager.isAlive) {
            _horizontalAxis = Mathf.Abs(_variableJoystick.Horizontal) > 0.1f ? _variableJoystick.Horizontal : Input.GetAxis("Horizontal");
            _verticalAxis = Mathf.Abs(_variableJoystick.Vertical) > 0.1f ? _variableJoystick.Vertical : Input.GetAxis("Vertical");

            if (!canMove) {
                DisablePlayerAnimation();
            } else {
                if (_horizontalAxis == 0f && _verticalAxis == 0f) {
                    if (_rigidbody.velocity != Vector2.zero) {
                        DisablePlayerAnimation();
                    }
                } else {
                    Run();
                    RotateEntity(_horizontalAxis);
                    SetVelocity(new Vector2(_horizontalAxis, _verticalAxis) * speed);
                }
            }
        }
    }

    private void Run()
    {
        _entityData.entityAnimationManager.Run(GetSpeedWithVelocity());
    }

    private void DisablePlayerAnimation()
    {
        SetVelocity(Vector2.zero);
        _entityData.entityAnimationManager.Run(GetSpeedWithVelocity());
    }

    public void SetJoystick(VariableJoystick variableJoystick)
    {
        _variableJoystick = variableJoystick;
    }
}