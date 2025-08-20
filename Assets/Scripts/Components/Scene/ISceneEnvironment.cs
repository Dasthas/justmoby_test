using UnityEngine;

namespace Components.Scene
{
    public interface ISceneEnvironment
    {
        Vector3 GetRandomSpawnPoint();
        Transform DynamicObjectsParent { get; }
        Transform PlayerSpawnPoint { get; }
    }
}