using UnityEngine;

public class AgroZone : MonoBehaviour
{
    [SerializeField] private MobGroup group;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            group.ActivateGroup();
        }
    }
}
