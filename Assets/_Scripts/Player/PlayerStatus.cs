using UnityEngine;

namespace Absentia.Player
{
    public class PlayerStatus : MonoBehaviour
    {
        // Statuses
        public bool IsGrounded;
        public bool CanGrabWall;
        public bool IsJumping;
        public bool IsFalling;
        public bool IsDashing;
        public bool IsWallSliding;
        public bool IsLookingRight = true;  
        public bool CanMove;

        // Stuff we don't need to see in the inspector
        [HideInInspector] public bool IsMovementReversed;
        [HideInInspector] public bool IsTouchingWallWithFeet;
        [HideInInspector] public bool IsTouchingWallWithHead;
        [HideInInspector] public bool IsTouchingWallWithMiddle;
        // Other
        [SerializeField] [HideInInspector] private LayerMask groundLayer;
        private Rigidbody2D player;
        private BoxCollider2D boxCollider;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            player = GetComponent<Rigidbody2D>();
            CanMove = true;
        }

        private void FixedUpdate()
        {
            IsGrounded = GroundCheck();
            IsTouchingWallWithFeet = IsTouchingWallBottom();
            IsTouchingWallWithHead = IsTouchingWallTop();
            IsTouchingWallWithMiddle = IsTouchingWallMiddle();
            CanGrabWall = (IsTouchingWallWithFeet && IsTouchingWallWithMiddle && IsTouchingWallWithHead) && !IsGrounded;
            IsJumping = player.velocity.y > 0 && !IsGrounded && !IsWallSliding;
            IsFalling = player.velocity.y < 0 && !IsGrounded && !IsWallSliding;
            IsMovementReversed = IsWallSliding;
        }

        #region Ground Check

        float extraHeightTest = .2f;

        private bool GroundCheck()
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightTest, groundLayer);
            return raycastHit.collider != null;
        }

        #endregion Ground Check

        #region Wall Check

        private float extraWidthTest = .4f;

        private bool IsTouchingWallTop()
        {
            var colliderTop = boxCollider.bounds.center + new Vector3(0, boxCollider.bounds.extents.y, 0);
            RaycastHit2D raycastHit = Physics2D.Raycast(colliderTop, IsLookingRight ? Vector2.right : Vector2.left, extraWidthTest, groundLayer);
            return raycastHit.collider != null;
        }

        private bool IsTouchingWallMiddle()
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider.bounds.center, IsLookingRight ? Vector2.right : Vector2.left, extraWidthTest, groundLayer);
            return raycastHit.collider != null;
        }

        private bool IsTouchingWallBottom()
        {
            var colliderBottom = boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y, 0);
            RaycastHit2D raycastHit = Physics2D.Raycast(colliderBottom, IsLookingRight ? Vector2.right : Vector2.left, extraWidthTest, groundLayer);
            return raycastHit.collider != null;
        }

        #endregion Wall Check
    }
}