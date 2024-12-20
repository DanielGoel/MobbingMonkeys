using System.Collections;
using UnityEngine;


public class Target : MonoBehaviour
{
    public GameObject zombie;
    public ParticleSystem deathEffect;
    public float health = 100;
    public bool isDead = false;
    GameManagement game;

    private void Awake()
    {
        game = FindObjectOfType<GameManagement>();
        int val = GameManagement.getRound();
        while(val > 1)
        {
            health *= 1.2f;
            val--;
        }
    }


    public void TakeDamage(float amount){
        if (isDead) {
            Debug.Log($"{gameObject.name} is already dead.");
            return; // Prevent taking damage if already dead
        }

        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {health}");

        if(health <= 0){
            isDead = true; // Set as dead immediately
            Debug.Log($"{gameObject.name} died.");
            StartCoroutine(Die());
        }
        else
        {
            GameManagement.currentPoints += 10;
        }
    }

    public float getHealth()
    {
        return health;
    }

    IEnumerator Die(){
        zombie.gameObject.SetActive(false);
        deathEffect.gameObject.SetActive(true);
        deathEffect.Play();
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
        isDead = true;
        GameManagement.registerKill();
        
    }
}
