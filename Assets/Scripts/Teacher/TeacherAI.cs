using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeacherAI : MonoBehaviour
{
    // ----------- PUBLIC VARIABLES

    public enum TeacherStates {
        None = 0,
        Wander,
        Chase,
        PointOfInterest,
        Inspect,
        Attack,
        Stun,
    };

    public TeacherStates initialState;

    public float stunDuration;

    public float inspectDuration;

    public float WanderRadius;

    public float FieldOfViewAngle;

    // ----------- PRIVATE VARIABLES

    private TeacherStates _currentState;

    private float _timeInState;

    private NavMeshAgent _agent;

    private Transform _playerTransform;

    private static Animator ANIMATOR;

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
            case TeacherStates.Wander: UpdateWanderState(); break;
            case TeacherStates.Chase: UpdateChaseState(); break;
            case TeacherStates.PointOfInterest: UpdatePointOfInterestState(); break;
            case TeacherStates.Inspect: UpdateInspectState(); break;
            case TeacherStates.Attack: UpdateAttackState(); break;
            case TeacherStates.Stun: UpdateStunState(); break;
        }
    }

    // ----------- STATES 

    private void UpdateWanderState()
    {
        SwitchAnimation("IsWalking");

        // If player in field of view, switch to Chase State
        if (IsPlayerInFieldOfView())
        {
            SwitchStates(TeacherStates.Chase);
        }

        // If teacher hears a sound, switch to Inspect State
        if (true)
        {
            SwitchStates(TeacherStates.Inspect);
        }

        SetRandomDestinationForAgent(-WanderRadius, WanderRadius);
    }

    private void UpdateChaseState()
    {
        SwitchAnimation("IsWalking");

        // If player is not in field of view, switch to Wander State
        if (!IsPlayerInFieldOfView())
        {
            SwitchStates(TeacherStates.Wander);
        }

        // If teacher is close to player, switch to Attack State
        if (true)
        {
            SwitchStates(TeacherStates.Attack);
        }
    }

    private void UpdatePointOfInterestState()
    {
        SwitchAnimation("IsWalking");

        // If point of interest reached, switch to Inspect State
        if (_timeInState == inspectDuration)
        {
            SwitchStates(TeacherStates.Wander);
        }

        // If player is in field of view, switch to Chase State
        if (IsPlayerInFieldOfView())
        {
            SwitchStates(TeacherStates.Chase);
        }
    }

    private void UpdateInspectState()
    {
        UpdateTimeInState();
        SwitchAnimation("IsStandingStill");

        // After <inspectDuration> seconds, switch to Wander State
        if (_timeInState == inspectDuration)
        {
            SwitchStates(TeacherStates.Wander);
        }

        // If player is in field of view, switch to Chase State
        if (IsPlayerInFieldOfView())
        {
            SwitchStates(TeacherStates.Chase);
        }
    }

    private void UpdateAttackState()
    {
        SwitchAnimation("IsAttacking");

        // If teacher is attacked by player, switch to Stun State
        if (true)
        {
            SwitchStates(TeacherStates.Stun);
        }

        // If player is in field of view but not close anymore, switch to Chase State
        if (true)
        {
            SwitchStates(TeacherStates.Chase);
        }

    }

    private void UpdateStunState()
    {
        UpdateTimeInState();
        SwitchAnimation("IsStunned");

        // After <stunDuration> seconds, switch to Chase State
        if (_timeInState == stunDuration)
        {
            SwitchStates(TeacherStates.Chase);
        }
    }

    // ----------- HELPER METHODS

    private void SwitchStates(TeacherStates state)
    {
        _currentState = state;
        _timeInState = 0f;
        Debug.Log("Switching " + transform.name + "state to: " + _currentState);
    }

    private void SwitchAnimation(string animationState)
    {
        ANIMATOR.SetTrigger(animationState);
    }

    private void Setup()
    {
        if (initialState == TeacherStates.None)
        {
            _currentState = TeacherStates.Wander;
        }
        else
        {
            _currentState = initialState;
        }

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (!_playerTransform)
        {
            Debug.LogError("No player has been spawned");
        }
    }

    private void UpdateTimeInState()
    {
        _timeInState = Time.deltaTime;
    }

    private void SetRandomDestinationForAgent(float minPos, float maxPos)
    {
        Vector3 currentAgentPosition = transform.position;
        Vector3 newDestination = new Vector3(
            currentAgentPosition.x + Random.Range(minPos, maxPos), 
            currentAgentPosition.y, 
            currentAgentPosition.z + Random.Range(minPos, maxPos)
            );
        _agent.SetDestination(newDestination);
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector3 rayDirection = _playerTransform.position - transform.position;
        return (Vector3.Angle(rayDirection, transform.forward) <= FieldOfViewAngle);
    }
}
