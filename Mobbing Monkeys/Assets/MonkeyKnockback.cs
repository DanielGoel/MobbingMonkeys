using UnityEngine;

public class MonkeyKnockback : MonoBehaviour
{
    public float knockbackForce = 100f; // Adjust force for strong knockback

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate knockback direction
            Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;

            // Apply knockback
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();
            if (player != null)
            {
                player.ApplyKnockback(knockbackDirection, knockbackForce);

                // Deal damage to the player
                GameManagement.damageHealth(30); // Player takes 10 damage
                Debug.Log("Player took 10 damage from knockback.");
            }
        }
    }
}
