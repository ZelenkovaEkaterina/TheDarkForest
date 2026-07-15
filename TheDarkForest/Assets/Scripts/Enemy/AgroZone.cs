using UnityEngine;

public class AgroZone : MonoBehaviour
{
    [SerializeField] private MobGroup group;
    [SerializeField] private SpawnGroup spawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            group.ActivateGroup(other.transform);
            //spawn.SpawnObjects();
        }
    }
}
