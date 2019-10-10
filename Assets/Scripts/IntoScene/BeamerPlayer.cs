using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.Events;

public class BeamerPlayer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _videoFinished;

    [SerializeField]
    private float _playVideoAfterSeconds;
    private VideoPlayer _videoSource;

    void Start()
    {
        _videoSource = GetComponent<VideoPlayer>();

        StartCoroutine(PlayVideoDelay());
    }

    IEnumerator PlayVideoDelay()
    {
        yield return new WaitForSeconds(_playVideoAfterSeconds);

        _videoSource.Play();

        _videoSource.loopPointReached += VideoFinsihed;
    }

    void VideoFinsihed(VideoPlayer videoPlayer)
    {
        _videoFinished.Invoke();
    }
}
