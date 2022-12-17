using UnityEngine;

public class PlayerMovementManager : EntityMovementManager
{
    private float _horizontalAxis;
    private float _verticalAxis;
    private Vector2 _moveVector;
    public bool ableToTeleport = true;
    [SerializeField] private VariableJoystick _variableJoystick;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
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
                    _moveVector = new Vector2(_horizontalAxis, _verticalAxis);
                    // SetVelocity(new Vector2(_horizontalAxis, _verticalAxis) * speed);
                }
            }
        }
    }

    void FixedUpdate()
    {
        SetVelocity(_moveVector * speed * Time.fixedDeltaTime);
    }

    private void Run()
    {
        _entityData.entityAnimationManager.Run(GetSpeedWithVelocity());
    }

    private void DisablePlayerAnimation()
    {
        _moveVector = Vector2.zero;
        SetVelocity(Vector2.zero);
        _entityData.entityAnimationManager.Run(GetSpeedWithVelocity());
    }

    public void SetJoystick(VariableJoystick variableJoystick)
    {
        _variableJoystick = variableJoystick;
    }
}