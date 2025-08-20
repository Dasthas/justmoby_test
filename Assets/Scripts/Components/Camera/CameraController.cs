using Cinemachine;
using UnityEngine;

namespace Components
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform _lookTarget;

        public Camera Camera => _camera;
        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        public Transform LookTarget => _lookTarget;

        public void SetFollowTarget(Transform target)
        {
            VirtualCamera.Follow = target;
        }
    }
}