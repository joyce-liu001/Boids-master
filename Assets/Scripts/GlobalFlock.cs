using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    [Header("Fish Settings")]
    public GameObject fishPrefab; // Prefab representing a single fish
    public int numberOfFish = 100; // Total number of fish in the simulation
    public static GameObject[] allFish; // Array to hold all fish instances

    [Header("Tank Settings")]
    public static int tankBounds = 2; // The size of the tank (half-length of the cube)
    public static Vector3 globalGoalPosition = Vector3.zero; // Shared goal position for all fish

    void Start()
    {
        // Initialize the fish array
        allFish = new GameObject[numberOfFish];
        Vector3 tankCenter = Vector3.zero; // You can change this to any Vector3 as the desired center.

        // Spawn each fish at a random position within the tank bounds
        for (int i = 0; i < numberOfFish; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-tankBounds, tankBounds),
                Random.Range(-tankBounds, tankBounds),
                Random.Range(-tankBounds, tankBounds)
            )+ tankCenter;

            // Instantiate a fish prefab at the random position
            allFish[i] = Instantiate(fishPrefab, randomPosition, Quaternion.identity);
        }
    }

    void Update()
    {
        // Occasionally update the global goal position
        if (Random.Range(0, 10000) < 50)
        {
            globalGoalPosition = new Vector3(
                Random.Range(-tankBounds, tankBounds),
                Random.Range(-tankBounds, tankBounds),
                Random.Range(-tankBounds, tankBounds)
            );
        }
    }
}
