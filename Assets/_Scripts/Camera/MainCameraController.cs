using Absentia.Player;
using Cinemachine;
using UnityEngine;

namespace Absentia.Camera
{
    public class MainCameraController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus status;
        [SerializeField] private Rigidbody2D player;
        private CinemachineFramingTransposer transposer;
        private Vector3 trackedObjectOffset = new Vector3(0, 1, 0);
        private float screenYup = .6f;
        private float screenYdown = .15f;

        private void Awake()
        {
            transposer = GetComponentInChildren<CinemachineFramingTransposer>();
        }

        private void FixedUpdate()
        {
            trackedObjectOffset.x = status.IsLookingRight ? .5f : -.5f;
            transposer.m_TrackedObjectOffset = trackedObjectOffset;

            if (player.velocity.y < -10f && transposer.m_ScreenY != screenYdown)
            {
                transposer.m_ScreenY = Mathf.Lerp(transposer.m_ScreenY, screenYdown, Time.deltaTime * 5);
            }
            else if (transposer.m_ScreenY != screenYup)
            {
                transposer.m_ScreenY = Mathf.Lerp(transposer.m_ScreenY, screenYup, Time.deltaTime * 5);
            }
        }
    }
}