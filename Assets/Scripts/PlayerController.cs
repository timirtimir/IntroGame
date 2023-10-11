using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Added to use the Text class
using TMPro; // Added to use TextMeshProUGUI class
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Vector2 moveValue;
    private int count;
    private int numPickups = 8; // Add the number of pickups you have
    public TextMeshProUGUI scoreText; // For displaying score
    public TextMeshProUGUI winText; // For displaying win message

    void Start() {
        count = 0;
        winText.text = ""; // Initialize the winText to an empty string
        SetCountText(); // Set the initial count text
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            count++; // Increment the count by 1 when a pickup is collected
            SetCountText(); // Update the score text whenever a pickup is collected
        }
    }

    private void SetCountText() {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups) {
            winText.text = "You win!";
        }
    }
}
