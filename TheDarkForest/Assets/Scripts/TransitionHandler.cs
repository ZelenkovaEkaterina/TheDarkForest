using System;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Transform _transition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = _transition.position;
        }
    }
}
