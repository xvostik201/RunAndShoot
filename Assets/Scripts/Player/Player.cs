using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _mouseSensitivity = 35f;

    private Transform[] _wayPoints;
    private bool _isShootPathActivated;
    private int _agentMoveIndex = 0;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Weapon _currentPlayerWeapon;
    private Level _level;

    private void Awake()
    {
        _level = FindObjectOfType<Level>();
        _wayPoints = _level.WayPoints();

        _currentPlayerWeapon = GetComponentInChildren<Weapon>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _agent.speed = _movementSpeed;
    }

    void Start()
    {
        if (_wayPoints != null && _wayPoints.Length > 0 && _wayPoints[_agentMoveIndex] != null)
        {
            _agent.SetDestination(_wayPoints[_agentMoveIndex].position);
            _level.HideSliderByWayPoint(_wayPoints[_agentMoveIndex]);
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            return;
        }

        UpdateAnimator();
        CheckWaypointReached();

        if (_isShootPathActivated)
        {
            RotateTowardsMouse();

            if (Input.GetMouseButton(0))
            {
                Shooting();
            }
        }
       
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("isRunning", _agent.velocity.magnitude > 0.1f);
    }

    private void CheckWaypointReached()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                if (_agentMoveIndex != 0)
                {
                    ShootPath();
                }
                
                else
                {
                    _agentMoveIndex = (_agentMoveIndex + 1) % _wayPoints.Length;
                    _agent.SetDestination(_wayPoints[_agentMoveIndex].position);
                    _level.HideSliderByWayPoint(_wayPoints[_agentMoveIndex]);
                }
            }
        }
    }

    private void ShootPath()
    {
        if (!_isShootPathActivated)
        {
            _isShootPathActivated = true;
            _agent.isStopped = true;
            _animator.SetBool("isRunning", false);
        }
    }

    public void RunToNextPoint()
    {
            _isShootPathActivated = false;
            _agent.isStopped = false;
            _agentMoveIndex = (_agentMoveIndex + 1) % _wayPoints.Length;
            _agent.SetDestination(_wayPoints[_agentMoveIndex].position);
            _level.HideSliderByWayPoint(_wayPoints[_agentMoveIndex]);
    }

    private void RotateTowardsMouse()
    {
        Vector3 tapPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(tapPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;
            Vector3 direction = (hitPoint - transform.position).normalized;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _mouseSensitivity * Time.deltaTime);
        }
    }

    private void Shooting()
    {
        _currentPlayerWeapon.Shoot();
    }
}
