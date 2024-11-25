using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
    [Header("Movement Settings")]
    public float movementSpeed = 0.5f; // The current speed of the fish.
    private float rotationSpeed = 3.0f; // How fast the fish rotates towards its target direction.
    private float neighborDetectionRange = 3.0f; // The range within which the fish detects other fish as neighbors.
    private bool isTurning = false; // Indicates if the fish is turning to avoid an obstacle.

    [Header("Flocking Behavior Weights")]
    public float separationWeight = 1.5f; // Weight factor for separation behavior (avoiding crowding).
    public float alignmentWeight = 1.0f; // Weight factor for alignment behavior (matching the group's direction).
    public float cohesionWeight = 1.0f; // Weight factor for cohesion behavior (staying close to the group).
    public float wallAvoidanceWeight = 2.0f; // Weight factor for avoiding walls and obstacles.

    [Header("Wall Avoidance Settings")]
    public float wallDetectionDistance = 1.0f; // The distance within which the fish detects walls or obstacles.

    [Header("Goal Position")]
    public Vector3 goalPosition; // Goal position that fishes will move towards

    void Start() {
        // Initialize the fish with a random speed within a defined range.
        movementSpeed = Random.Range(0.5f, 5.0f);
    }

    void Update() {
        SetGoalPosition();
        
        // Attempt to avoid walls or obstacles dynamically.
        Vector3 wallAvoidance = AvoidWalls();

        if (wallAvoidance != Vector3.zero) {
            // If the fish detects a wall or obstacle, it starts turning away from it.
            isTurning = true;
            AdjustDirection(wallAvoidance);
        }
        else
        {
            // If not turning to avoid obstacles, it resumes flocking behavior.
            isTurning = false;
            // Periodically apply flocking rules for natural movement.
            if (Random.Range(0, 5) < 1) {
                ApplyFlockingRules();
            }
        }

        // Move the fish forward in its current direction.
        transform.Translate(0, 0, Time.deltaTime * movementSpeed);
    }

    // Function to handle setting the goal position via user input (mouse click)
    void SetGoalPosition() {
        // Raycast from the camera to detect user clicks on the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Check if the ray hits an object (for example, a plane or ground)
        if (Physics.Raycast(ray, out hit))
        {
            goalPosition = hit.point; // Set the goal position to the point where the ray hits
        }
    }
    Vector3 AvoidWalls() {
        RaycastHit hit;
        Vector3 avoidanceVector = Vector3.zero;

        // Cast rays in six directions to detect nearby walls or obstacles.
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance)) {
            avoidanceVector += hit.normal; // Add the normal vector of the hit surface.
        }
        if (Physics.Raycast(transform.position, transform.right, out hit, wallDetectionDistance)) {
            avoidanceVector += hit.normal;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, wallDetectionDistance)) {
            avoidanceVector += hit.normal;
        }
        if (Physics.Raycast(transform.position, transform.up, out hit, wallDetectionDistance)) {
            avoidanceVector += hit.normal;
        }
        if (Physics.Raycast(transform.position, -transform.up, out hit, wallDetectionDistance)) {
            avoidanceVector += hit.normal;
        }
        if (Physics.Raycast(transform.position, -transform.forward, out hit, wallDetectionDistance)) {
            avoidanceVector += hit.normal;
        }

        return avoidanceVector.normalized; // Return the combined direction normalized.
    }

    void ApplyFlockingRules() {
        Vector3 separation = Separate(); // Avoid crowding.
        Vector3 alignment = Align(); // Match group's heading.
        Vector3 cohesion = Cohesion(); // Move towards the center of the group.

        // Combine the behaviors with their respective weights.
        Vector3 combinedDirection = separation * separationWeight +
                                    alignment * alignmentWeight +
                                    cohesion * cohesionWeight;

        // Adjust the fish's direction based on the combined behaviors.
        if (combinedDirection != Vector3.zero) {
            AdjustDirection(combinedDirection);
        }
        // Move the fish towards the goal position
        if (goalPosition != Vector3.zero) {
            Vector3 directionToGoal = (goalPosition - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(directionToGoal),
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void AdjustDirection(Vector3 direction) {
        // Smoothly adjust the fish's direction towards the specified target direction.
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime
        );
    }

    Vector3 Separate() {
        // Ensure the fish maintains a comfortable distance from nearby fish.
        Vector3 avoidanceVector = Vector3.zero;
        GameObject[] allFish = GlobalFlock.allFish;

        foreach (GameObject fish in allFish)
        {
            if (fish != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, fish.transform.position);

                // Add a repelling force if the fish is too close.
                if (distance < 1.0f)
                {
                    avoidanceVector += (transform.position - fish.transform.position);
                }
            }
        }

        return avoidanceVector.normalized;
    }

    Vector3 Align() {
        // Align the fish's heading with the average direction of nearby fish.
        Vector3 averageHeading = Vector3.zero;
        int groupSize = 0;
        GameObject[] allFish = GlobalFlock.allFish;

        foreach (GameObject fish in allFish)
        {
            if (fish != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, fish.transform.position);

                if (distance <= neighborDetectionRange)
                {
                    averageHeading += fish.transform.forward;
                    groupSize++;
                }
            }
        }

        if (groupSize > 0)
        {
            averageHeading /= groupSize; // Compute the average heading of the group.
        }

        return averageHeading.normalized;
    }

    Vector3 Cohesion() {   
        //Steer the fish towards the center of the nearby group.
        Vector3 centerOfGroup = Vector3.zero;
        int groupSize = 0;
        GameObject[] allFish = GlobalFlock.allFish;

        foreach (GameObject fish in allFish)
        {
            if (fish != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, fish.transform.position);

                if (distance <= neighborDetectionRange)
                {
                    centerOfGroup += fish.transform.position;
                    groupSize++;
                }
            }
        }

        if (groupSize > 0)
        {
            centerOfGroup /= groupSize; // Calculate the center position of the group.
            centerOfGroup = centerOfGroup - transform.position; // Calculate the direction to the center.
        }

        return centerOfGroup.normalized;
    }
}
