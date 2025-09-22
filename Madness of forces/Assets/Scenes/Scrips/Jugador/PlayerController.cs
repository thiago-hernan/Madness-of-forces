using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movimiento y Salto
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float doubleJumpForce = 5f;

    // Dash
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isDashing;
    private float dashTime;
    public Vector2 lastMoveDirection = Vector2.right;

    // Cámara
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 0, -10f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (!isDashing)
        {
            float moveInput = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
            }

            // Si el jugador se está moviendo, actualiza la última dirección
            if (moveInput != 0)
            {
                lastMoveDirection = new Vector2(moveInput, 0);
            }

            // Movimiento
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Voltear el personaje basado en el movimiento
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            // Mantiene el último movimiento si no hay input para el dash
            else
            {
                transform.localScale = new Vector3(lastMoveDirection.x, 1, 1);
            }

            // Salto con W
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    isGrounded = false;
                    jumpCount = 1;
                }
                else if (jumpCount < 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                    jumpCount++;
                }
            }

            // Dash con LeftShift o RightShift
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                isDashing = true;
                dashTime = dashDuration;

                rb.velocity = lastMoveDirection * dashSpeed;
            }
        }
        else
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        // Sigue al personaje con la cámara
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }
    }

    // Detectar si el personaje está en el suelo
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }
}