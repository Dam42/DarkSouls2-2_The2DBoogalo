using UnityEngine;

namespace Absentia.Player
{
    public class PlayerStatus : MonoBehaviour
    {
        public bool IsGrounded;
        public bool IsJumping;
        public bool IsFalling;
        public bool IsDashing;
        public bool CanMove;

        private Rigidbody2D player;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            player = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            IsGrounded = GroundCheck();
            IsJumping = player.velocity.y > 0;
            IsFalling = player.velocity.y < 0;
        }

        #region Ground Check

        [SerializeField] [HideInInspector] private LayerMask groundLayer;
        private BoxCollider2D boxCollider;

        private bool GroundCheck()
        {
            float extraHeightTest = .3f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, extraHeightTest, groundLayer);
            return raycastHit.collider != null;
        }

        #endregion Ground Check
    }
}
