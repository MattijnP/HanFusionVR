using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeacherBehaviour : MonoBehaviour
{

    public AudioClip Help;
    public AudioClip HelpHelp;
    public AudioClip IAmStuckInHere;

    public enum TeacherStates
    {
        None = 0,
        Lost,
        Moving,
        Save,
    };

    public TeacherStates InitialState;

    public Transform SaveZoneLocation;

    public float TimeBetweenSpeeches;

    private TeacherStates _currentState;

    private float _timeSinceLastSpoken;

    private NavMeshAgent _agent;

    private static Animator ANIMATOR;

    private string _currentAnimation;

    private bool _isFreed;

    private List<AudioClip> HelpAudio = new List<AudioClip>();

    // Start is called before the first frame update
    private void Start()
    {
        Setup();

        HelpAudio.Add(Help);
        HelpAudio.Add(HelpHelp);
        HelpAudio.Add(IAmStuckInHere);
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
        UpdateTimeInState();

        if (_timeSinceLastSpoken <= 0f)
        {
            int value = Random.Range(1, 2);
            AudioClip Play = HelpAudio[value];
            Debug.Log(Play.name);
            AudioManager.instance.PlaySfx(Play);
            _timeSinceLastSpoken = TimeBetweenSpeeches;
        }

        if (_isFreed)
        {
            SwitchStates(TeacherStates.Moving);
        }
    }

    private void UpdateMovingState()
    {
        transform.position = SaveZoneLocation.position;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            SwitchStates(TeacherStates.Save);
        }
    }

    private void UpdateSaveState()
    {
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

        if (TimeBetweenSpeeches == 0)
        {
            TimeBetweenSpeeches = 5f;
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

    private void UpdateTimeInState()
    {
        _timeSinceLastSpoken -= Time.deltaTime;
    }
}
