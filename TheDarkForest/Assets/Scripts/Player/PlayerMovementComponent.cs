using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            foreach (RaycastHit hit in hits)
            {
                // Опционально: проверка тега или слоя, чтобы не кликать по стенам/врагам
                if (hit.collider.CompareTag("Ground"))
                {
                    agent.SetDestination(hit.point);
                    Debug.Log(hit.collider.name);
                }
            }
                
        }
    }
}
