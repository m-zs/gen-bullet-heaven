using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;

    private Rigidbody2D rb;
    private InputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (inputManager)
        {
            OnMove();
        }
    }

    private void OnMove()
    {
        rb.velocity = inputManager.movementInput * movementSpeed;
    }
}
