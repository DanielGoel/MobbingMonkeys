using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent nav;
    private Transform player;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float timeToAttack = 0.5f;

    void Start()
    {
        // Initialize NavMeshAgent and find the player
        nav = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player GameObject is tagged as 'Player'.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Update NavMeshAgent destination only when necessary
            if (!isAttacking)
            {
                nav.SetDestination(player.position);
            }

            // Attack logic
            if (isAttacking)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= timeToAttack)
                {
                    Attack();
                    attackTimer = 0f;
                }
            }
        }
    }

    void Attack()
    {
        // Deal damage to the player
        GameManagement.currentHealth -= 25;
        Debug.Log("Player attacked! Current Health: " + GameManagement.currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = true;
            nav.isStopped = true; // Stop the NavMeshAgent when attacking
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            attackTimer = 0f;
            nav.isStopped = false; // Resume the NavMeshAgent
        }
    }
}
