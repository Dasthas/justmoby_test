using System;
using System.Collections.Generic;
using Components;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Services
{
    [Serializable]
    public class EnemyService : Service, IEnemyService
    {
        [SerializeField] private Vector2 _enemySpawnRandomRange = new Vector2(5, 10);
        [SerializeField] private uint _maxEnemiesCount = 7;

        [SerializeField] [AssetsOnly] private CharacterProxy _prefab;

        [Inject] private ISceneEnvironment _sceneEnvironment;

        [Inject] private IObjectResolver _objectResolver;

        private float _spawnTimer;
        private List<CharacterProxy> _spawnedEnemies = new List<CharacterProxy>();
        private CompositeDisposable _disposables;

        private ReactiveCommand<DeathData> _everyEnemyDeath = new ReactiveCommand<DeathData>();
        public IObservable<DeathData> EveryEnemyDeath => _everyEnemyDeath;

        private void SpawnEnemy()
        {
            var instance = Object.Instantiate(_prefab, _sceneEnvironment.DynamicObjectsParent);
            instance.transform.position = _sceneEnvironment.GetRandomSpawnPoint();
            _spawnedEnemies.Add(instance);
            instance.OnDead
                .Subscribe(data => OnEnemyDeath(data, instance))
                .AddTo(instance)
                .AddTo(_disposables);
        }

        private void OnEnemyDeath(DeathData deathData, CharacterProxy character)
        {
            _spawnedEnemies.Remove(character);
            _everyEnemyDeath.Execute(deathData);
        }

        private void ProcessTimer()
        {
            if (_spawnTimer > 0 || _spawnedEnemies.Count >= _maxEnemiesCount)
            {
                return;
            }

            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                SpawnEnemy();
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            _spawnTimer = Random.Range(_enemySpawnRandomRange.x, _enemySpawnRandomRange.y);
        }

        #region Service

        public override void RegisterSelf(IContainerBuilder builder)
        {
            builder.RegisterInstance<IEnemyService>(this)
                .As<IInitializable>()
                .As<IDisposable>();
        }

        protected override void OnInitialize()
        {
            _disposables = new CompositeDisposable();
            Debug.Log("OnInitialize EnemyService");
        }

        protected override void OnDispose()
        {
            _disposables.Dispose();
            _spawnedEnemies.Clear();
            Debug.Log("OnDispose EnemyService");
        }

        protected override void OnTick()
        {
            ProcessTimer();
        }

        #endregion
    }
}