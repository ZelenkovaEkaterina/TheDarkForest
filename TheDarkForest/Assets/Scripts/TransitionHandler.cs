using System;
using UnityEngine;
using UnityEngine.AI;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Transform _transition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<NavMeshAgent>().ResetPath();
            other.GetComponent<NavMeshAgent>().Warp(_transition.position);
        }
    }
}
