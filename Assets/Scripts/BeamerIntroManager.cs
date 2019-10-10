using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class BeamerIntroManager : MonoBehaviour
{
    [SerializeField]
    private float _playVideoAfterSeconds;
    private VideoPlayer _videoSource;

    // Start is called before the first frame update
    void Start()
    {
        _videoSource = GetComponent<VideoPlayer>();

        StartCoroutine(PlayVideoDelay());
    }

    IEnumerator PlayVideoDelay()
    {
        yield return new WaitForSeconds(_playVideoAfterSeconds);

        _videoSource.Play();
    }
}
