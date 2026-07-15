using System;
using UnityEngine;


public class EnemyHealthComponent : MonoBehaviour
{
    public IDamageble _damagebleEnemy;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _enemyDistance = .3f;

    private void Awake()
    {
        _damagebleEnemy = GetComponent<IDamageble>();
    }

    private void Start()
    {
        if (_damagebleEnemy is DamageSystem handler)
        {
            handler.OnDamageTaken += (damage, source) =>
            {
                Debug.Log($"Получен урон {damage}. Текущее здоровье {handler.CurrentHealth}");
            };

            handler.OnDamageTaken += (damage, source) =>
            {
                if (handler.IsDead())
                {
                    Debug.Log("Умер");
                    Destroy(this.gameObject);
                }
            };
        }
    }

    /*private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T) ||
             Input.GetKeyDown(KeyCode.Y)) && IsPlayer())
        {
            //IsEnemy();
            //_weapon.Attack(null);
            //Debug.Log(null);
            _damagebleEnemy?.TakeDamage(10, null);
        }
    }*/
    
    /*private bool IsPlayer()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(
            transform.position,
            Vector2.right,
            _enemyDistance,
            _enemyLayerMask);
        
        RaycastHit2D hitLeft = Physics2D.Raycast(
            transform.position,
            Vector2.left,
            _enemyDistance,
            _enemyLayerMask);
        //Debug.DrawLine(transform.position, hitLeft.point);
        if (hitRight.collider || hitLeft.collider)
        {
            //Destroy(hit.collider.gameObject, .7f);
            return true;
        }
        
        return false;
    }*/
}