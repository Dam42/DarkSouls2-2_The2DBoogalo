using UnityEngine;

namespace Absentia.Player
{
    public class PlayerStatus : MonoBehaviour
    {
        public bool IsGrounded;
        public bool IsNearWall;
        public bool IsJumping;
        public bool IsFalling;
        public bool IsDashing;
        public bool IsWallSliding;
        public bool IsLookingRight = true;
        public bool CanMove;

        private Rigidbody2D player;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            player = GetComponent<Rigidbody2D>();
            CanMove = true;
        }

        private void Update()
        {
            IsGrounded = GroundCheck();
            IsNearWall = WallCheck() && !IsGrounded;
            IsJumping = player.velocity.y > 0 && !IsWallSliding;
            IsFalling = player.velocity.y < 0 && !IsGrounded && !IsWallSliding;
        }

        [SerializeField] [HideInInspector] private LayerMask groundLayer;
        private BoxCollider2D boxCollider;

        #region Ground Check      
        private bool GroundCheck()
        {
            float extraHeightTest = .2f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightTest, groundLayer);
            //RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, extraHeightTest, groundLayer);
            return raycastHit.collider != null;
        }

        #endregion Ground Check

        #region Wall Check

        private bool WallCheck()
        {
            float extraWidthTest = .2f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, 
                IsLookingRight ? Vector2.right : Vector2.left, extraWidthTest, groundLayer);
            return raycastHit.collider != null;
        }

        #endregion
    }
}
