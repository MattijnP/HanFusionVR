using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [Tooltip("Time in seconds")]
    [SerializeField]
    [Range(0, 86_400f)]
    private int _countdownTime = 120;

    [SerializeField]
    private Text _countDownText;

    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        UpdateText();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (_countdownTime > 0)
        {
            yield return new WaitForSeconds(1);
            _countdownTime--;
            UpdateText();
            PlaySound();
        }
    }

    void PlaySound()
    {
        _audioSource.Play();
    }

    void UpdateText()
    {
        TimeSpan time = TimeSpan.FromSeconds(_countdownTime);

        string formatedString = time.ToString(@"hh\:mm\:ss");
        _countDownText.text = formatedString;
    }
}
