using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FallOver : MonoBehaviour
{
    public Transform treeParent; // Parent of all trees
    public float rotationSpeed = 30f; // Speed of rotation (degrees per second)
    public float interactDistance = 5f; // Interaction radius for each tree
    public TMP_Text interactText; // TextMeshPro for interaction prompt
    public Transform player; // Reference to the player
    public int cost = 1000; // Cost to knock down trees

    private bool isFalling = false; // Prevent multiple triggers

    void Update()
    {
        // Check if the player is near any tree and has enough credits
        bool isPlayerNearAnyTree = false;

        foreach (Transform tree in treeParent)
        {
            float distanceToTree = Vector3.Distance(player.position, tree.position);
            if (distanceToTree <= interactDistance && GameManagement.currentPoints >= cost) // Check distance and credits
            {
                isPlayerNearAnyTree = true;
                interactText.text = $"Press E to knock down trees (Cost: {cost})";
                interactText.gameObject.SetActive(true);

                // Check for player input
                if (Input.GetKeyDown(KeyCode.E) && !isFalling)
                {
                    // Deduct cost and proceed
                    GameManagement.setPoints(GameManagement.currentPoints - cost);
                    interactText.gameObject.SetActive(false); // Hide the prompt
                    StartTreeFall();
                }

                break; // Exit the loop if one tree is in range
            }
        }

        // If no tree is near or player doesn't have enough credits, hide the interaction prompt
        if (!isPlayerNearAnyTree)
        {
            interactText.gameObject.SetActive(false);
        }
    }
    private void StartTreeFall()
    {
        isFalling = true; // Prevent re-triggering
        StartCoroutine(RotateAllTreesSimultaneously());
    }

    private IEnumerator RotateAllTreesSimultaneously()
    {
        List<IEnumerator> treeRotations = new List<IEnumerator>();

        // Start rotating all trees at the same time
        foreach (Transform tree in treeParent)
        {
            treeRotations.Add(RotateTree(tree));
        }

        // Run all rotations in parallel
        yield return StartCoroutine(RunAllCoroutines(treeRotations));
    }

    private IEnumerator RotateTree(Transform tree)
    {
        float currentRotation = 0f;
        Collider treeCollider = tree.GetComponent<Collider>();

        while (currentRotation < 180f)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (currentRotation + rotationStep > 180f)
            {
                rotationStep = 180f - currentRotation; // Cap rotation at 180 degrees
            }

            tree.RotateAround(tree.position, Vector3.right, rotationStep); // Rotate around base
            currentRotation += rotationStep;

            yield return null; // Wait for the next frame
        }

        // Wait briefly to emphasize the fall
        yield return new WaitForSeconds(1f);

        // Disable collider after the tree falls
        if (treeCollider != null)
        {
            treeCollider.enabled = false;
        }

        // Destroy the tree
        Destroy(tree.gameObject);
    }

    private IEnumerator RunAllCoroutines(List<IEnumerator> coroutines)
    {
        List<Coroutine> runningCoroutines = new List<Coroutine>();

        foreach (IEnumerator coroutine in coroutines)
        {
            runningCoroutines.Add(StartCoroutine(coroutine));
        }

        // Wait for all coroutines to complete
        foreach (Coroutine coroutine in runningCoroutines)
        {
            yield return coroutine;
        }
    }
}
