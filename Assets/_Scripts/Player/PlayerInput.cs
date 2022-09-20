using UnityEngine;

namespace Absentia.Player 
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector] public float HorizontalInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector]public bool HasJumpBuffered;
        [HideInInspector] public bool IsStillHoldingJump;
        [HideInInspector] public bool DashInput;
        [HideInInspector] public bool HasDashBuffered;
        private float jumpBufferTimer;
        private float dashBufferTimer;

        private void Update()
        {
            handleTimers();

            HorizontalInput = Input.GetAxis("Horizontal");
            JumpInput = Input.GetKeyDown(KeyCode.Z);
            if (JumpInput)
            {
                jumpBufferTimer = 0;
                HasJumpBuffered = true;
            }
            if (jumpBufferTimer >= .2f) HasJumpBuffered = false;
            IsStillHoldingJump = Input.GetKey(KeyCode.Z);
            DashInput = Input.GetKeyDown(KeyCode.C);
            if (DashInput)
            {
                dashBufferTimer = 0;
                HasDashBuffered = true;
            }
            if (dashBufferTimer >= .2f) HasDashBuffered = false;
        }

        private void handleTimers()
        {
            jumpBufferTimer += Time.deltaTime;
            dashBufferTimer += Time.deltaTime;
        }
    }
}

