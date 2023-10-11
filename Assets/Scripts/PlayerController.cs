using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Vector2 moveValue;
    private int count;
    private int numPickups = 3; 
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI positionText; // For displaying the player's position
    public TextMeshProUGUI velocityText; // For displaying the player's velocity
    public TextMeshProUGUI distanceText; // For displaying distance to closest pickup

    private Vector3 lastPosition; // Store the last position for velocity calculation
    private Vector3 velocity; // Store the calculated velocity

    void Start() {
        count = 0;
        winText.text = "";
        SetCountText();
        lastPosition = transform.position; // Initialize last position at the start
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    void Update() {
        CalculateVelocity();
        DisplayDebugInfo();
        // You might have a function in the GameController to get the distance 
        // to the nearest pickup. That function can update distanceText.
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            count++; 
            SetCountText(); 
        }
    }

    private void SetCountText() {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups) {
            winText.text = "You win!";
        }
    }

    private void CalculateVelocity() {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void DisplayDebugInfo() {
        positionText.text = "Position: " + transform.position.ToString("0.00");
        velocityText.text = "Velocity: " + velocity.ToString("0.00") + " | Speed: " + velocity.magnitude.ToString("0.00");
    }
}
