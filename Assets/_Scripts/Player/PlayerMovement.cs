using System.Collections;
using UnityEngine;

namespace Absentia.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Setting")]
        [SerializeField] private float movementSpeed;
        private Vector2 newVelocityVector = new Vector2(0, 0);

        [Header("Jump Setting")]
        [SerializeField] private float JumpPower;
        [SerializeField] private Vector2 wallJumpPower;
        private Vector2 newJumpingVector = new Vector2(0, 0);
        private bool hasDoubleJump;
        private float fallingMultiplier;
        private float lowJumpMultiplier;

        [Header("Dash Setting")]
        [SerializeField] private float dashPower;
        [SerializeField] private float dashCooldown;
        private float currentDashCooldown;

        [Header("Other Setting")]
        [SerializeField] private float maxFallingSpeed;

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
            HandleTimers();
        }

        private void FixedUpdate()
        {
            HandleLowJumpAndFasterFalling();
            if (status.IsGrounded) hasDoubleJump = true;
            if (status.IsGrounded && playerRB.velocity.x != 0 && input.HorizontalInput == 0 && !status.IsDashing) PreventPlayerFromSliding();
            if (status.IsTouchingWallWithFeet && !status.IsTouchingWallWithMiddle && !status.IsTouchingWallWithHead && !input.IsStillHoldingJump) BumpPlayerUp();
            else if (status.IsTouchingWallWithHead && !status.IsTouchingWallWithMiddle && !status.IsTouchingWallWithFeet) BumpPlayerDown();
        }

        private void HandleInput()
        {
            if (input.HorizontalInput != 0 && !status.IsDashing && status.CanMove) PlayerMove();
            if (status.IsGrounded && input.HasJumpBuffered) PlayerNormalJump();
            else if (input.JumpInput && !status.IsDashing)
            {
                if (status.IsGrounded || status.hasCoyoteTime) PlayerNormalJump();
                else if (!status.IsGrounded && !status.IsWallSliding && hasDoubleJump) PlayerDoubleJump();
                else if (status.IsWallSliding) WallJump();
            }
            if ((input.DashInput || input.HasDashBuffered) && currentDashCooldown >= dashCooldown) PlayerDash();

            if (status.CanGrabWall && status.CanMove)
            {
                if (!status.IsWallSliding && status.IsLookingRight ? input.HorizontalInput > 0 : input.HorizontalInput < 0) EnterWallSlide();
                if (status.IsWallSliding) WallSlide();
            }
            else ExitWallSilde();
        }
        
        #region ----- Wall Interactions -----

        private void EnterWallSlide()
        {
            status.IsWallSliding = true;
        }
        private void WallSlide()
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, -1f);
        }

        private void ExitWallSilde()
        {
            status.IsWallSliding = false;
        }

        private void WallJump()
        {
            StartCoroutine("DoBlockMovementAfterWallJump");
            playerRB.velocity = new Vector2(wallJumpPower.x * (status.IsLookingRight ? -1 : 1), wallJumpPower.y);
        }

        private IEnumerator DoBlockMovementAfterWallJump()
        {
            status.CanMove = false;
            yield return new WaitForSecondsRealtime(.2f);
            status.CanMove = true;
        }

        private void BumpPlayerUp()
        {
            playerRB.AddForce(new Vector2 (status.IsLookingRight ? 5 : -5, 3), ForceMode2D.Impulse);
        }

        private void BumpPlayerDown()
        {
            playerRB.AddForce(new Vector2(status.IsLookingRight ? .5f : -.5f, -.5f), ForceMode2D.Impulse);
        }


        #endregion ----- Wall Interactions -----

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
            if (playerRB.velocity.y < -maxFallingSpeed) playerRB.velocity = new Vector2(playerRB.velocity.x, -maxFallingSpeed);
        }

        #endregion ----- Jumping -----

        #region ----- Dashing -----

        private void PlayerDash()
        {
            currentDashCooldown = 0f;
            status.IsDashing = true;
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            var dashDirection = status.IsMovementReversed ? (status.IsLookingRight ? -1 : 1) : (status.IsLookingRight ? 1 : -1);
            playerRB.velocity = new Vector2(dashPower * dashDirection, 0);
            Invoke("PlayerDashEnded", .3f);
        }

        private void PlayerDashEnded()
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            status.IsDashing = false;
            if (!status.IsGrounded) playerRB.velocity = new Vector2(status.IsLookingRight ? 4 : -4, 0);
        }

        #endregion ----- Dashing -----

        private void PlayerMove()
        {
            newVelocityVector.x = input.HorizontalInput * movementSpeed;
            newVelocityVector.y = playerRB.velocity.y;
            playerRB.velocity = newVelocityVector;
        }

        private void PreventPlayerFromSliding()
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }

        private void HandleTimers()
        {
            currentDashCooldown += Time.deltaTime;
        }
    }
}