using UnityEngine;

public class EnemyWeapon : MonoBehaviour, IDamageDealer
{
    [SerializeField] private int _damageAmount;

    [SerializeField] private GameController _weapon;
    public int DamageAmount => _damageAmount;

    public void DealDamage(IDamageable target, int damage)
    {
        if (target != null && !target.IsDead())
        {
            target.TakeDamage(damage, gameObject);
        }
    }
}
