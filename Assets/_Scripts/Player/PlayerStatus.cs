using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        isGrounded = GroundCheck();
    }

    #region Ground Check
    public bool isGrounded;
    [SerializeField][HideInInspector] private LayerMask groundLayer;

    private bool GroundCheck()
    {
        float extraHeightTest = .3f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, extraHeightTest, groundLayer);
        return raycastHit.collider != null;
    }
    #endregion
}
