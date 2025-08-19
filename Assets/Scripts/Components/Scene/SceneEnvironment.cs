using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class SceneEnvironment: MonoBehaviour, ISceneEnvironment
    {
        [SerializeField] private Transform _dynamicObjectsParent;
        [SerializeField]
        private List<Transform> _spawnPoints = new List<Transform>();
        
        private Transform _playerSpawnPoint;

        public Transform DynamicObjectsParent => _dynamicObjectsParent;

        public Transform PlayerSpawnPoint => _playerSpawnPoint;

        public Vector3 GetRandomSpawnPoint()
        {
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                Debug.LogError("No Spawn Points assigned!");
                return Vector3.zero;
            }
            var randomIndex = Random.Range(0, _spawnPoints.Count);
            if (_spawnPoints[randomIndex] == null)
            {
                Debug.LogError("Spawn Point = null!");
                return Vector3.zero;
            }
            return _spawnPoints[randomIndex].position;
        }
    }
}