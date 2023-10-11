using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject[] pickups;
    public GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 oldPlayerPosition;
    private Vector3 playerVelocity;
    private enum DebugMode { Normal, Distance, Vision }
    private DebugMode currentMode = DebugMode.Normal;

    void Start() {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        oldPlayerPosition = player.transform.position;
    }

    void Update() {
        playerVelocity = (player.transform.position - oldPlayerPosition) / Time.deltaTime;
        oldPlayerPosition = player.transform.position;

        if (Input.GetKeyDown(KeyCode.Space)) {
            switch (currentMode) {
                case DebugMode.Normal:
                    currentMode = DebugMode.Distance;
                    break;
                case DebugMode.Distance:
                    currentMode = DebugMode.Vision;
                    break;
                case DebugMode.Vision:
                    currentMode = DebugMode.Normal;
                    break;
            }
        }

        switch (currentMode) {
            case DebugMode.Normal:
                ResetPickups();
                lineRenderer.enabled = false;
                break;
            case DebugMode.Distance:
                DisplayDistanceMode();
                break;
            case DebugMode.Vision:
                DisplayVisionMode();
                break;
        }
    }

    void ResetPickups() {
        foreach (var pickup in pickups) {
            if (pickup.activeSelf) {
                pickup.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void DisplayDistanceMode() {
    float shortestDistance = Mathf.Infinity; // Start with a very high value
    GameObject closestPickup = null;

    foreach (var pickup in pickups) {
        if (pickup.activeSelf) {
            float distance = Vector3.Distance(player.transform.position, pickup.transform.position);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                closestPickup = pickup;
            }
            pickup.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    if (closestPickup) {
        closestPickup.GetComponent<Renderer>().material.color = Color.blue;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, closestPickup.transform.position);
    } else {
        lineRenderer.enabled = false; // If there's no closest active pickup, don't render the line.
    }
}


    void DisplayVisionMode() {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, player.transform.position + playerVelocity);

        GameObject directPickup = null;
        float maxDot = -Mathf.Infinity;

        foreach (var pickup in pickups) {
            if (pickup.activeSelf) {
                Vector3 directionToPickup = (pickup.transform.position - player.transform.position).normalized;
                float dotProduct = Vector3.Dot(playerVelocity.normalized, directionToPickup);

                if (dotProduct > maxDot) {
                    maxDot = dotProduct;
                    directPickup = pickup;
                }

                pickup.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        if (directPickup) {
            directPickup.GetComponent<Renderer>().material.color = Color.green;
            directPickup.transform.LookAt(player.transform);
        }
    }
}
