using Cinemachine;
using UnityEngine;

namespace Components
{
    public interface ICameraController
    {
        UnityEngine.Camera Camera { get; }
        CinemachineVirtualCamera VirtualCamera { get; }
        Transform LookTarget { get; }
        void SetFollowTarget(Transform target);
    }
}