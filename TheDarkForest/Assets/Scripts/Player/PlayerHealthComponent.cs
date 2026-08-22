using UnityEngine;

public class PlayerHealthComponent : HealthSystem
{
    protected override void Die()
    {
        Debug.Log("ты сдох");
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(5, other.gameObject);
            Debug.Log(currentHealth);
        }
    }
}
