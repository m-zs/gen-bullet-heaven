using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float movementSpeed)
    {
        rb.velocity = inputManager.movementInput * movementSpeed;
    }
}
