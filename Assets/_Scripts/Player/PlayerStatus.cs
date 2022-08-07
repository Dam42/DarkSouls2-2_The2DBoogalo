using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool isGrounded;
    public bool isJumping;
    public bool isFalling;

    private Rigidbody2D player;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = GroundCheck();
        isJumping = player.velocity.y > 0;
        isFalling = player.velocity.y < 0;
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