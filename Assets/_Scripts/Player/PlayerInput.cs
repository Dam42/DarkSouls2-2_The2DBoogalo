using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Absentia.Player 
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector] public float HorizontalInput;
        [HideInInspector] public bool Jump;
        [HideInInspector] public bool IsStillHoldingJump;

        private void Update()
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            Jump = Input.GetKeyDown(KeyCode.X);
            IsStillHoldingJump = Input.GetKey(KeyCode.X);
        }
    }
}

