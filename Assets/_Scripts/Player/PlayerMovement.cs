using System;
using UnityEngine;

namespace Absentia.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Setting")]
        [SerializeField] private int movementSpeed;
        private Vector2 newVelocityVector = new Vector2(0, 0);

        [Header("Jump Setting")]
        [SerializeField] private float JumpPower;
        private Vector2 newJumpingVector = new Vector2(0, 0);
        private bool hasDoubleJump;
        private float fallingMultiplier;
        private float lowJumpMultiplier;

        [Header("Dash Setting")]
        [SerializeField] private float dashPower;
        [SerializeField] private float dashCooldown;
        private float currentDashCooldown;

        // Components
        private Rigidbody2D playerRB;
        private PlayerStatus status;
        private PlayerInput input;

        private void Awake()
        {
            // Get all the needed components
            playerRB = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerInput>();
            status = GetComponent<PlayerStatus>();

            // Set gravity multipliers for falling and for low jump
            fallingMultiplier = Physics2D.gravity.y * (2 - 1);
            lowJumpMultiplier = Physics2D.gravity.y * (10 - 1);
        }

        private void Update()
        {
            HandleInput();
            HandleLowJumpAndFasterFalling();
            if (status.IsGrounded) hasDoubleJump = true;
            currentDashCooldown += Time.deltaTime;
        }

        private void HandleInput()
        {
            if (input.HorizontalInput != 0 && !status.IsDashing) PlayerMove();
            if(input.JumpInput && !status.IsDashing)
            {
                if (status.IsGrounded) PlayerNormalJump();
                else if (!status.IsGrounded && hasDoubleJump) PlayerDoubleJump();
            }
            if (input.DashInput && currentDashCooldown >= dashCooldown && input.HorizontalInput != 0) PlayerDash();
        }

        private void PlayerMove()
        {
            newVelocityVector.x = input.HorizontalInput * movementSpeed;
            newVelocityVector.y = playerRB.velocity.y;
            playerRB.velocity = newVelocityVector;
        }

        #region ----- Jumping ----- 
        private void PlayerNormalJump()
        {
            newJumpingVector.x = playerRB.velocity.x;
            newJumpingVector.y = JumpPower;
            playerRB.velocity = newJumpingVector;
        }
        private void PlayerDoubleJump()
        {
            newJumpingVector.x = playerRB.velocity.x;
            newJumpingVector.y = JumpPower;
            playerRB.velocity = newJumpingVector;
            hasDoubleJump = false;
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
        #endregion

        private void PlayerDash()
        {
            currentDashCooldown = 0f;
            status.IsDashing = true;
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            playerRB.velocity = new Vector2(dashPower * Mathf.Sign(input.HorizontalInput), 0);
            Invoke("PlayerDashEnded", .3f);
        }
        private void PlayerDashEnded()
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            status.IsDashing = false;
            playerRB.velocity = new Vector2(0, 0);
        }
    }
}