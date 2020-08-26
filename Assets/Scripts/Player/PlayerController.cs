using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public Text collectedText;
    public static int collectedAmount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Player movement:
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        // GetAxisRaw = more snappy movement while GetAxis = more floaty movement

        // UI: 
        collectedText.text = "Items Collected " + collectedAmount; 
    }

    // speedChange can be negative to slow down player
    public void ChangeSpeed(float speedChange)
    {
        speed += speedChange;
    }

    
}
