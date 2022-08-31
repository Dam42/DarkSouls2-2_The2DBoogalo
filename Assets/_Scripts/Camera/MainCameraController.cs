using Absentia.Player;
using UnityEngine;
using Cinemachine;

namespace Absentia.Camera
{
    public class MainCameraController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus status;
        private CinemachineFramingTransposer transposer;

        private void Awake()
        {
            transposer = GetComponentInChildren<CinemachineFramingTransposer>();
        }

        private void FixedUpdate()
        {
            transposer.m_TrackedObjectOffset = new Vector3(status.IsLookingRight ? .5f : -.5f, 1, 0);
        }
    }
}
