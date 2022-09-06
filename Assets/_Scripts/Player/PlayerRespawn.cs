using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform startingCheckpoint;
    private Vector3 currentCheckpoint;

    private void Awake()
    {
        currentCheckpoint = startingCheckpoint.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.gameObject.transform.position;
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.position = currentCheckpoint;
        }
    }
}
