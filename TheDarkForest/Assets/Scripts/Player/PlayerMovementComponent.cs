using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        private NavMeshAgent agent;
        [SerializeField] private Camera mainCamera;
        
        private PlayerDamageComponent playerDamageComponent;

        [SerializeField] private LayerMask groundLayer;
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            playerDamageComponent = GetComponent<PlayerDamageComponent>();
        }

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
                RaycastHit hit;
                //RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // Проверяем, не нажали ли на врага
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        // Если нажали на врага - атакуем его
                        Debug.Log("enemy");
                        playerDamageComponent.AttackEnemy(hit.collider.gameObject);
                        return;
                    }
                
                    // Иначе двигаемся в точку
                    agent.SetDestination(hit.point);
                }
                /*foreach (RaycastHit hit in hits)
                {
                    // Опционально: проверка тега или слоя, чтобы не кликать по стенам/врагам
                    if (hits[0].collider.CompareTag("Ground") ||  hits[0].collider.CompareTag("Agro"))
                    {
                        //statePlayer = PlayerState.Idle;
                        agent.isStopped = false;
                        agent.SetDestination(hit.point);
                    }

                    if (hits[0].collider.CompareTag("Enemy"))
                    {
                        //statePlayer = PlayerState.Attack;
                        agent.isStopped = true;
                        playerDamageComponent.AttackEnemy(hit.collider.gameObject);
                    }
                    Debug.Log(hits[0].collider.name);
                }*/
                
            }
        }
    }
}

