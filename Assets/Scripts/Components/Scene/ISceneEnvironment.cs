using UnityEngine;

namespace Components
{
    public interface ISceneEnvironment
    {
        Vector3 GetRandomSpawnPoint();
        Transform DynamicObjectsParent { get; }
        Transform PlayerSpawnPoint { get; }
    }
}