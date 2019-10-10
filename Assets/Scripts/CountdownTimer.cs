using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [Tooltip("Time in seconds")]
    [SerializeField]
    private int countdownTime = 120;

    [SerializeField]
    private Text countDownText;

    void Start()
    {
        UpdateText();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while(countdownTime > 0)
        {
            yield return new WaitForSeconds (1);
            countdownTime--;
            UpdateText();
        }
    }

    void UpdateText()
    {
        TimeSpan time = TimeSpan.FromSeconds(countdownTime);

        string formatedString = time .ToString(@"hh\:mm\:ss");
        countDownText.text = formatedString;
    }
}
