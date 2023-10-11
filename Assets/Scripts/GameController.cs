using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject[] pickups; // Assuming all pickups are stored in this array
    public GameObject player;
    private LineRenderer lineRenderer;
    
    void Start() {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2; // Setting two positions for start and end of the line
    }

    void Update() {
        float closestDistance = float.MaxValue; // Start with a very high value
        GameObject closestPickup = null;

        // Reset all pickups color to white
        foreach (var pickup in pickups) {
            if (pickup.activeSelf) { // Ensure the pickup is active (hasn't been collected yet)
                pickup.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        // Find the closest active pickup
        foreach (var pickup in pickups) {
            if (pickup.activeSelf) { // Ensure the pickup is active (hasn't been collected yet)
                float distance = Vector3.Distance(player.transform.position, pickup.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestPickup = pickup;
                }
            }
        }

        // If we found an active pickup, highlight it and draw a line to it
        if (closestPickup != null) {
            closestPickup.GetComponent<Renderer>().material.color = Color.blue;
            DrawLineToPickup(closestPickup.transform.position);
        }
    }

    void DrawLineToPickup(Vector3 pickupPosition) {
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, pickupPosition);
    }
}
