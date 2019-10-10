using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeacherBehaviour : MonoBehaviour
{
    public enum TeacherStates
    {
        None = 0,
        Lost,
        Moving,
        Save,
    };

    public TeacherStates InitialState;

    public Transform SaveZoneLocation;

    private TeacherStates _currentState;

    private NavMeshAgent _agent;

    private static Animator ANIMATOR;

    private string _currentAnimation;

    private bool _isFreed;

    // Start is called before the first frame update
    private void Start()
    {
        Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_currentState)
        {
            case TeacherStates.Lost: UpdateLostState(); break;
            case TeacherStates.Moving: UpdateMovingState(); break;
            case TeacherStates.Save: UpdateSaveState(); break;
        }
    }

    private void UpdateLostState()
    {
        SwitchAnimation("IsStandingStill");

        if (_isFreed)
        {
            SwitchStates(TeacherStates.Moving);
        }
    }

    private void UpdateMovingState()
    {
        SwitchAnimation("IsWalking");
        _agent.SetDestination(SaveZoneLocation.position);

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            SwitchStates(TeacherStates.Save);
        }
    }

    private void UpdateSaveState()
    {
        SwitchAnimation("IsStandingStill");
    }

    private void Setup()
    {
        if (InitialState == TeacherStates.None)
        {
            _currentState = TeacherStates.Lost;
        }
        else
        {
            _currentState = InitialState;
        }

        _agent = GetComponent<NavMeshAgent>();
        ANIMATOR = GetComponent<Animator>();
    }

    private void SwitchStates(TeacherStates state)
    {
        _currentState = state;
        Debug.Log("Switching " + transform.name + "state to: " + _currentState);
    }

    private void SwitchAnimation(string animationState)
    {
        if (_currentAnimation != null)
        {
            ANIMATOR.ResetTrigger(_currentAnimation);
        }
        _currentAnimation = animationState;
        ANIMATOR.SetTrigger(_currentAnimation);
    }
}
