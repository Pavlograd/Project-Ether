using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIMovementManager : EntityMovementManager
{
    private Transform _player;
    private Vector2 _target;
    private Vector2 _startingPos;
    private Vector2 _moveVector;
    private Path _path;
    private float _nextWaypointDistance = 0.5f;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    private bool _targetIsPlayer = false;
    [SerializeField] private Seeker _seeker;
    [SerializeField] private float _distanceMovement = 5f;
    [SerializeField] private float _distanceDetection = 5f;

    private void Start()
    {
        _player = GameObject.Find("Player").transform;
        _startingPos = _rigidbody.position;
        _target = _startingPos;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(_rigidbody.position, _target, OnPathComplete);
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error) {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    private void Update()
    {
        float distanceWithPlayer = Vector2.Distance(_rigidbody.position, _player.position);
        float distanceWithCenter = Vector2.Distance(_rigidbody.position, _startingPos);
        float distancePlayerCenter = Vector2.Distance(_player.position, _startingPos);

        if (!canMove) {
            _moveVector = Vector2.zero;
            _entityData.entityAnimationManager.Run(0);
            return;
        }
        if (distancePlayerCenter <= _distanceMovement + _entityData.entityAbilityManager.rangeAttack && distanceWithPlayer <= _distanceDetection) {
            _target = _player.position;
            if (distanceWithPlayer > _entityData.entityAbilityManager.rangeAttack || !_entityData.entityAbilityManager.TargetIsReachable()) {
                if (!_targetIsPlayer)
                    UpdatePath();
                SetMovement();
                _targetIsPlayer = true;
            } else {
                _moveVector = Vector2.zero;
                _entityData.entityAnimationManager.Run(0);
            }
        } else {
            _target = _startingPos;
            if (distanceWithCenter > 0.5f) {
                if (_targetIsPlayer)
                    UpdatePath();
                SetMovement();
                _targetIsPlayer = false;
            } else {
                _moveVector = Vector2.zero;
                _entityData.entityAnimationManager.Run(0);
            }
        }
    }

    private void FixedUpdate()
    {
        SetVelocity(_moveVector * speed * Time.fixedDeltaTime);
    }

    private void SetMovement()
    {
        if (_path != null) {
            _reachedEndOfPath = _currentWaypoint >= _path.vectorPath.Count;
            if (!_reachedEndOfPath) {
                _moveVector = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody.position).normalized;
                _entityData.entityAnimationManager.Run(GetSpeedWithVelocity());
                RotateEntity(_rigidbody.velocity.x);
                float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWaypoint]);
                if (distance < _nextWaypointDistance)
                    _currentWaypoint++;
            }
        }
    }
}
