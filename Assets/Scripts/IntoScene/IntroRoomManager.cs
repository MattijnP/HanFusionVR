using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRoomManager : MonoBehaviour
{
    [SerializeField]
    private CountdownTimer _countdownTimer;

    public void EnableCountdownTimer()
    {
        _countdownTimer.enabled = true;
    }
}
