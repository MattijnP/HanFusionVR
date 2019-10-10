using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    private string _keyTag = string.Empty;

    [SerializeField]
    private Rigidbody _door;

    void Start()
    {
        _door.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag(_keyTag))
        {
            _door.isKinematic = false;
        }
    }
}
