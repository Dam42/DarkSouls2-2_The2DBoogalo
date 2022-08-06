using System;
using UnityEngine;

namespace Absentia.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private int movementSpeed;
        private float fallingMultiplier;
        private float lowJumpMultiplier;

        private Rigidbody2D playerRB;
        private Vector2 newVelocityVector = new Vector2(0, 0);
        private Vector2 newJumpingVector = new Vector2(0, 0);
        [SerializeField] private float JumpPower;
        private PlayerInput input;

        private void Awake()
        {
            // Get all the needed components
            playerRB = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerInput>();

            //Set gravity multipliers for falling and for low jump
            fallingMultiplier = Physics2D.gravity.y * (2 - 1);
            lowJumpMultiplier = Physics2D.gravity.y * (10 - 1);
        }

        private void Update()
        {
            HandleInput();
            HandleLowJumpAndFasterFalling();
        }

        private void HandleInput()
        {
            if (input.HorizontalInput != 0) PlayerMove();
            if (input.Jump) PlayerJump();
        }

        private void PlayerMove()
        {
            newVelocityVector.x = input.HorizontalInput * movementSpeed;
            newVelocityVector.y = playerRB.velocity.y;
            playerRB.velocity = newVelocityVector;
        }

        private void PlayerJump()
        {
            newJumpingVector.x = playerRB.velocity.x;
            newJumpingVector.y = JumpPower;
            playerRB.velocity = newJumpingVector;
        }

        private void HandleLowJumpAndFasterFalling()
        {
            if (playerRB.velocity.y < 0)
            {
                playerRB.velocity += Vector2.up * fallingMultiplier * Time.deltaTime;
            }
            else if (playerRB.velocity.y > 0 && !input.IsStillHoldingJump)
            {
                playerRB.velocity += Vector2.up * lowJumpMultiplier * Time.deltaTime;
            }
        }
    }
}