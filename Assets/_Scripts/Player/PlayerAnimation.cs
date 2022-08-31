using UnityEngine;

namespace Absentia.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private SpriteRenderer sprite;
        private PlayerStatus status;
        private PlayerInput input;

        private void Awake()
        {
            // Get all the components
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
            input = GetComponent<PlayerInput>();
            status = GetComponent<PlayerStatus>();
        }

        private void Update()
        {
            if (input.HorizontalInput != 0 && !status.IsDashing && status.CanMove)
            {
                sprite.flipX = status.IsMovementReversed ? input.HorizontalInput > 0 : input.HorizontalInput < 0;
            }
            status.IsLookingRight = status.IsMovementReversed ? sprite.flipX : !sprite.flipX;

            var state = GetState();

            if (state == _currentState) return;
            anim.CrossFade(state, 0, 0);
            _currentState = state; 
        }

        private int GetState()
        {
            if (status.IsGrounded && input.HorizontalInput != 0 && !status.IsDashing) return Run_Gun;
            if (status.IsJumping) return Jump_Gun;
            if (status.IsFalling) return Fall_Gun;
            if (status.IsDashing) return Dash_Gun;
            if (status.IsNearWall)
            {
                if (status.IsWallSliding) return WallSlide_Gun;
            }    
            return Idle;
        }

        #region Cached Properties

        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run_Gun = Animator.StringToHash("Run_Gun");
        private static readonly int Jump_Gun = Animator.StringToHash("Jump_Gun");
        private static readonly int Fall_Gun = Animator.StringToHash("Fall_Gun");
        private static readonly int Dash_Gun = Animator.StringToHash("Dash_Gun");
        private static readonly int WallSlide_Gun = Animator.StringToHash("WallSlide_Gun");
        #endregion Cached Properties
    }
}