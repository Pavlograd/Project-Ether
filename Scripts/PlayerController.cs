using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public VariableJoystick variableJoystick;


    private bool _canMove = false;
    private Vector3 _forward, _right;
    private Animator _animator;
    private static readonly int IsIdle = Animator.StringToHash("isIdle");

    private void Start()
    {
        if (Camera.main != null) {
            _forward = Camera.main.transform.forward;
            Camera.main.gameObject.SetActive(false);
        }
        _forward.y = 0;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
        _animator = transform.Find("Model").GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_canMove)
            return;
        float horizontal = Math.Abs(variableJoystick.Horizontal) > 0.1f ? variableJoystick.Horizontal : Input.GetAxis("Horizontal");
        float vertical = Math.Abs(variableJoystick.Vertical) > 0.1f ? variableJoystick.Vertical : Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
            Move(horizontal, vertical);
        _animator.SetBool(IsIdle, Math.Abs(Math.Abs(horizontal) + Math.Abs(vertical)) > 0.05f);
    }

    private void Move(float horizontal, float vertical)
    {
        Vector3 rightMovement = horizontal * moveSpeed * Time.deltaTime * _right;
        Vector3 upMovement = vertical * moveSpeed * Time.deltaTime * _forward;
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);


        transform.position += rightMovement;
        transform.position += upMovement;
        if (heading != Vector3.zero)
            transform.forward = heading;
    }

    public void ToggleMove()
    {
        _canMove = !_canMove;
    }
}
