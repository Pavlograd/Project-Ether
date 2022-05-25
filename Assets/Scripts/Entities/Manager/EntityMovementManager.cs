using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementManager : MonoBehaviour
{
    public float speed = 3f;
    public bool canMove = true;
    [SerializeField] protected Rigidbody2D _rigidbody;
    protected EntityData _entityData;
    private Transform _spriteTransform;

    protected virtual void Awake()
    {
        _entityData = GetComponent<EntityData>();
        _spriteTransform = transform.Find("Sprite");
    }

    protected void SetVelocity(Vector2 velocity)
    {
        if (_entityData.entityHealthManager.isAlive) {
            _rigidbody.velocity = velocity;
        } else {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    protected float GetSpeedWithVelocity()
    {
        return Vector2.Distance(_rigidbody.velocity, Vector2.zero) / 4;
    }

    public void RotateEntity(float horizontalAxis)
    {
        if (horizontalAxis > 0f) {
            if (_spriteTransform != null) {
                _spriteTransform.transform.localScale = new Vector2(1f, 1f);
            } else {
                transform.localScale = new Vector2(1f, 1f);
            }
        } else if (horizontalAxis < 0f) {
            if (_spriteTransform != null) {
                _spriteTransform.transform.localScale = new Vector2(-1f, 1f);
            } else {
                transform.localScale = new Vector2(-1f, 1f);
            }
        }
    }

    public void SetVelocityToZero()
    {
        _rigidbody.velocity = Vector3.zero;
    }
}
