using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagement : MonoBehaviour
{
    public static int round = 1;
    public static int zombiesKilledInRound = 0;
    public static int points = 0;
    [SerializeField] public int zombiesInRound = 10;
    int zombiesSpawnedInRound = 0;
    float zombiesSpawnTimer = 0;
    public Transform[] zombiesSpawnPoints;

    // Monkey Prefabs
    public GameObject zombieEnemy; // Default monkey
    public GameObject greyMonkey; // Grey monkey prefab
    public GameObject blackMonkey; // Black monkey prefab

    public float maxHealth = 100;

    public static float currentHealth = 100;
    public static int currentPoints = 0;

    public TMP_Text currentHealthText;
    public TMP_Text currentPointsText;
    public GameObject deadText;
    private bool isDead = false;
    
    public TMP_Text waveInfoText; // UI element to show wave and zombies info
    public TMP_Text waveTimerText; // UI element to show the time until the next wave

    private bool waveInProgress = true; // Tracks if a wave is in progress
    private float nextWaveTimer = 10f; // Time before the next wave starts

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            currentHealth += (5 * Time.deltaTime);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            isDead = true;
            deadText.gameObject.SetActive(true);
            currentHealth = 0;
        }

        currentHealthText.text = ((int)currentHealth).ToString();
        currentPointsText.text = currentPoints.ToString();

        // Update wave information UI
        waveInfoText.text = $"Wave {round}\nZombies Remaining: {zombiesInRound - zombiesKilledInRound}";

        if (waveInProgress)
        {
            // Basic zombie spawner system
            if (zombiesSpawnedInRound < zombiesInRound)
            {
                if (zombiesSpawnTimer > 2)
                {
                    SpawnZombie();
                    zombiesSpawnTimer = 0;
                }
                else
                {
                    zombiesSpawnTimer += Time.deltaTime;
                }
            }
            else if (zombiesKilledInRound == zombiesInRound)
            {
                waveInProgress = false;
                nextWaveTimer = 1f; // Reset the timer for the next wave
            }
        }
        else
        {
            // Handle countdown for next wave
            nextWaveTimer -= Time.deltaTime;
            waveTimerText.text = $"Next Wave In: {Mathf.CeilToInt(nextWaveTimer)}s";

            if (nextWaveTimer <= 0)
            {
                NextRound();
                waveInProgress = true;
            }
        }
    }


    void SpawnZombie()
    {
        // Determine which type of monkey to spawn
        GameObject monkeyToSpawn;

        // Generate a random number between 0 and 100
        int randomChance = Random.Range(0, 100);

        if (round >= 5)
        {
            // 10% chance for black monkey, 30% chance for grey monkey, else default monkey
            if (randomChance < 10)
            {
                monkeyToSpawn = blackMonkey; // 10% chance
            }
            else if (randomChance < 40) // 10% + 30% = 40%
            {
                monkeyToSpawn = greyMonkey; // 30% chance
            }
            else
            {
                monkeyToSpawn = zombieEnemy; // 60% chance
            }
        }
        else if (round >= 3)
        {
            // 20% chance for grey monkey, else default monkey
            if (randomChance < 20)
            {
                monkeyToSpawn = greyMonkey; // 20% chance
            }
            else
            {
                monkeyToSpawn = zombieEnemy; // 80% chance
            }
        }
        else
        {
            // Default monkey for earlier rounds
            monkeyToSpawn = zombieEnemy;
        }

        // Spawn the selected monkey
        Vector3 randomSpawnPoint = zombiesSpawnPoints[Random.Range(0, zombiesSpawnPoints.Length)].position;
        Instantiate(monkeyToSpawn, randomSpawnPoint, Quaternion.identity);

        zombiesSpawnedInRound++;
    }

    public static void registerKill()
    {
        zombiesKilledInRound++;
        currentPoints += 100;
    }

    public void NextRound()
    {
        round++;
        zombiesInRound += (int)(zombiesInRound * 0.25); // Increase wave size
        zombiesSpawnedInRound = 0;
        zombiesSpawnTimer = 0;
        zombiesKilledInRound = 0;
    
        waveTimerText.text = ""; // Clear the timer text
    }

    public static int getRound()
    {
        return round;
    }

    public static void damageHealth(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0; // Prevent health from going below zero
        }
    }

    public static void setPoints(int points)
    {
        currentPoints = points;
    }
}



