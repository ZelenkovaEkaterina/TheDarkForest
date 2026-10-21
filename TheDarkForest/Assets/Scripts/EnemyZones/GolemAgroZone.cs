using System;
using UnityEngine;

public class GolemAgroZone : MonoBehaviour
{
    public event Action OnEnterPlayer;
    public event Action OnExitPlayer;
    
    [SerializeField] private GameObject _golem;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _golem.GetComponent<GolemAI>().GolemSetTarget(other.transform);
        OnEnterPlayer?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _golem.GetComponent<GolemAI>().GolemClearTarget(other.transform);
        OnExitPlayer?.Invoke();
    }
}
