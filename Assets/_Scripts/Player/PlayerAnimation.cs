using UnityEngine;

namespace Absentia.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody2D player;
        private SpriteRenderer sprite;
        private PlayerInput input;

        private void Awake()
        {
            // Get all the components
            anim = GetComponent<Animator>();
            player = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            input = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (input.HorizontalInput != 0) sprite.flipX = input.HorizontalInput < 0;

            var state = GetState();

            if (state == _currentState) return;
            anim.CrossFade(state, 0, 0);
            _currentState = state;
        }

        private int GetState()
        {
            if (player.velocity.x != 0 && player.velocity.y == 0) return Run_Gun;
            if (player.velocity.y > 0) return Jump_Gun;
            if (player.velocity.y < 0) return Fall_Gun;       
            return Idle;
        }

        #region Cached Properties

        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run_Gun = Animator.StringToHash("Run_Gun");
        private static readonly int Jump_Gun = Animator.StringToHash("Jump_Gun");
        private static readonly int Fall_Gun = Animator.StringToHash("Fall_Gun");

        #endregion Cached Properties
    }
}