using System.Collections;
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
            HandleTimers();
        }

        private void FixedUpdate()
        {
            if (status.IsGrounded) hasDoubleJump = true;
            if (status.IsGrounded && playerRB.velocity.x != 0 && input.HorizontalInput == 0 && !status.IsDashing) PreventPlayerFromSliding();
        }

        private void HandleInput()
        {
            if (input.HorizontalInput != 0 && !status.IsDashing && status.CanMove) PlayerMove();
            if (input.JumpInput && !status.IsDashing)
            {
                if (status.IsGrounded) PlayerNormalJump();
                else if (!status.IsGrounded && !status.IsNearWall && hasDoubleJump) PlayerDoubleJump();
                else if (status.IsWallSliding) WallJump();
            }
            if (input.DashInput && currentDashCooldown >= dashCooldown) PlayerDash();

            if (status.IsNearWall && status.CanMove)
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
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        }

        private void ExitWallSilde()
        {
            status.IsWallSliding = false;
        }

        private void WallJump()
        {
            status.IsWallSliding = false;
            StartCoroutine("DoBlockMovementAfterWallJump");
            playerRB.velocity = new Vector2(status.IsLookingRight ? -4 : 4, 7);
        }

        private IEnumerator DoBlockMovementAfterWallJump()
        {
            status.CanMove = false;
            yield return new WaitForSecondsRealtime(.2f);
            status.CanMove = true;
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
        }

        #endregion ----- Jumping -----

        #region ----- Dashing -----

        private void PlayerDash()
        {
            currentDashCooldown = 0f;
            status.IsDashing = true;
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            var dashDirection = status.IsWallSliding ? (status.IsLookingRight ? -1 : 1) : (status.IsLookingRight ? 1 : -1);
            playerRB.velocity = new Vector2(dashPower * dashDirection, 0);
            Invoke("PlayerDashEnded", .3f);
        }

        private void PlayerDashEnded()
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            status.IsDashing = false;
            playerRB.velocity = new Vector2(0, 0);
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
            playerRB.velocity = new Vector2(0, 0);
        }

        private void HandleTimers()
        {
            currentDashCooldown += Time.deltaTime;
        }
    }
}