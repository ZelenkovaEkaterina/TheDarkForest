using System;
using UnityEngine;

public class GolemAgroZone : MonoBehaviour
{
    [SerializeField] private GameObject _golem;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _golem.GetComponent<GolemAI>().GolemSetTarget(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _golem.GetComponent<GolemAI>().GolemClearTarget(other.transform);
    }
}
