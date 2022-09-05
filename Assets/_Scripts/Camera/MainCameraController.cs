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
        private float screenYup = .55f;
        private float screenYdown = .15f;

        private void Awake()
        {
            transposer = GetComponentInChildren<CinemachineFramingTransposer>();
        }

        private void FixedUpdate()
        {
            transposer.m_TrackedObjectOffset = new Vector3(status.IsLookingRight ? .5f : -.5f, 1, 0);

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