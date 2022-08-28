using UnityEngine;

namespace Absentia.Player 
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector] public float HorizontalInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector] public bool IsStillHoldingJump;
        [HideInInspector] public bool DashInput;

        private void Update()
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            JumpInput = Input.GetKeyDown(KeyCode.Z);
            IsStillHoldingJump = Input.GetKey(KeyCode.Z);
            DashInput = Input.GetKeyDown(KeyCode.C);
        }
    }
}

