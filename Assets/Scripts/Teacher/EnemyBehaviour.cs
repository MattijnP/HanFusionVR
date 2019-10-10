using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    // ----------- PUBLIC VARIABLES

    public AudioClip IllFindYou;
    public AudioClip Grunt;
    public AudioClip WhatWasThat;
    public AudioClip YourAreNowMine;
    public AudioClip Chase;

    public enum EnemyStates {
        None = 0,
        Wander,
        Chase,
        PointOfInterest,
        Inspect,
        Attack,
        Stun,
        Recover,
    };

    public EnemyStates InitialState;

    [Range(0f, 10f)]
    public float StunDuration;

    [Range(0f, 10f)]
    public float RecoverDuration;

    [Range(0f, 30f)]
    public float InspectDuration;

    [Range(0f, 50f)]
    public float WanderRadius;

    [Range(0f, 15f)]
    public float AwarenessRadius;

    [Range(0f, 3f)]
    public float AttackRadius;

    public float FieldOfViewAngle;

    // ----------- PRIVATE VARIABLES

    private EnemyStates _currentState;

    private float _timeInState;

    private NavMeshAgent _agent;

    private GameObject _player;

    private static Animator ANIMATOR;

    private string _currentAnimation;

    // ----------- START

    private void Start()
    {
        Setup();
    }

    // ----------- UPDATE

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyStates.Wander: UpdateWanderState(); break;
            case EnemyStates.Chase: UpdateChaseState(); break;
            case EnemyStates.PointOfInterest: UpdatePointOfInterestState(); break;
            case EnemyStates.Inspect: UpdateInspectState(); break;
            case EnemyStates.Attack: UpdateAttackState(); break;
            case EnemyStates.Stun: UpdateStunState(); break;
            case EnemyStates.Recover: UpdateRecoverState(); break;
        }
    }

    private void UpdateRecoverState()
    {
        UpdateTimeInState();
        SwitchAnimation("IsStandingUp");

        if (_timeInState <= 0f)
        {
            SwitchStates(EnemyStates.Wander);
        }
    }

    // ----------- STATES 

    private void UpdateWanderState()
    {
        SwitchAnimation("IsWalking");

        // If player in field of view, switch to Chase State
        if (IsPlayerInFieldOfView())
        {
            AudioManager.instance.ChangeSong(Chase);
            AudioManager.instance.PlaySfx(WhatWasThat);
            SwitchStates(EnemyStates.Chase);
        }

        // If teacher hears a sound, switch to Inspect State
        if (false)
        {
            SwitchStates(EnemyStates.Inspect);
        }

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            SetRandomDestinationForAgentNearTarget(this.transform, -WanderRadius, WanderRadius);
        }
    }

    private void UpdateChaseState()
    {
        

        SwitchAnimation("IsWalking");

        // If player is not in field of view, switch to Wander State
        if (!IsPlayerInFieldOfView())
        {
            AudioManager.instance.PlaySfx(IllFindYou);
            SwitchStates(EnemyStates.Wander);
        }

        // If teacher is close to player, switch to Attack State
        if (Vector3.Distance(this.transform.position, _player.transform.position) <=  AttackRadius)
        {
            AudioManager.instance.PlaySfx(YourAreNowMine);
            SwitchStates(EnemyStates.Attack);
        }

        _agent.SetDestination(_player.transform.position);
    }

    private void UpdatePointOfInterestState()
    {
        SwitchAnimation("IsWalking");

        // If point of interest reached, switch to Inspect State
        if (_timeInState == InspectDuration)
        {
            SwitchStates(EnemyStates.Wander);
        }

        // If player is in field of view, switch to Chase State
        if (IsPlayerInFieldOfView())
        {
            SwitchStates(EnemyStates.Chase);
        }
    }

    private void UpdateInspectState()
    {
        UpdateTimeInState();
        SwitchAnimation("IsStandingStill");

        // After <inspectDuration> seconds, switch to Wander State
        if (_timeInState == InspectDuration)
        {
            SwitchStates(EnemyStates.Wander);
        }

        // If player is in field of view, switch to Chase State
        if (IsPlayerInFieldOfView())
        {
            SwitchStates(EnemyStates.Chase);
        }
    }

    private void UpdateAttackState()
    {
        SwitchAnimation("IsAttacking");

        // If teacher is attacked by player, switch to Stun State
        if (Input.GetButtonDown("Fire1"))
        {
            _timeInState = StunDuration;
            SwitchStates(EnemyStates.Stun);
        }

        // If player is not in field of view, switch to Chase State
        if (!IsPlayerInFieldOfView())
        {
            AudioManager.instance.PlaySfx(IllFindYou);
            SwitchStates(EnemyStates.Wander);
        }

        // If player is in field of view but not close anymore, switch to Chase State
        if (IsPlayerInFieldOfView() && Vector3.Distance(_player.transform.position, this.transform.position) >= AttackRadius)
        {
            SwitchStates(EnemyStates.Chase);
        }
    }

    private void UpdateStunState()
    {
        UpdateTimeInState();
        SwitchAnimation("IsStunned");
        

        // After <stunDuration> seconds, switch to Chase State
        if (_timeInState <= 0f)
        {
            AudioManager.instance.PlaySfx(Grunt);
            _timeInState = RecoverDuration;
            SwitchStates(EnemyStates.Recover);
        }
    }

    // ----------- HELPER METHODS

    private void SwitchStates(EnemyStates state)
    {
        _currentState = state;
        Debug.Log("Switching " + transform.name + "state to: " + _currentState);
    }

    private void SwitchAnimation(string animationState)
    {
        if (_currentAnimation != null) {
            ANIMATOR.ResetTrigger(_currentAnimation);
        }
        _currentAnimation = animationState;
        ANIMATOR.SetTrigger(_currentAnimation);
    }

    private void Setup()
    {
        if (InitialState == EnemyStates.None)
        {
            _currentState = EnemyStates.Wander;
        }
        else
        {
            _currentState = InitialState;
        }

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Debug.LogError("No player has been spawned");
        }

        _agent = GetComponent<NavMeshAgent>();
        ANIMATOR = GetComponent<Animator>();
    }

    private void UpdateTimeInState()
    {
        _timeInState -= Time.deltaTime;
    }

    private void SetRandomDestinationForAgentNearTarget(Transform target, float minPos, float maxPos)
    {
        Vector3 newDestination = new Vector3(
            target.position.x + UnityEngine.Random.Range(minPos, maxPos),
            target.position.y,
            target.position.z + UnityEngine.Random.Range(minPos, maxPos)
            );
        _agent.SetDestination(newDestination);
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector3 rayDirection = _player.transform.position - transform.position;

        bool canPlayerBeSeen = IsPlayerVisible(rayDirection);
        bool isPlayerInFOV = (Vector3.Angle(rayDirection, transform.forward) <= FieldOfViewAngle);

        if (canPlayerBeSeen && isPlayerInFOV)
        {
            return true;
        }

        return false;
    }

    private bool IsPlayerVisible(Vector3 target)
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, target, out hit, AwarenessRadius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }

            return false;
        }

        return false;
    }
}
