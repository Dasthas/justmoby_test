using System;
using System.Collections.Generic;
using Components.Characters;
using Components.Scene;
using DG.Tweening;
using Services.Base;
using Services.Player;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Services.Enemy
{
    [Serializable]
    public class EnemyService : Service, IEnemyService
    {
        [SerializeField] private float _deathAnimationDuration = 0.2f;
        [SerializeField] private float _enemiesSpeed = 2f;
        [SerializeField] private Vector2 _hpRandomRange = new Vector2(10, 100);
        [SerializeField] private Vector2 _enemySpawnTimeRandomRange = new Vector2(5, 10);
        [SerializeField] private uint _maxEnemiesCount = 7;

        [SerializeField] [AssetsOnly] private CharacterProxy _prefab;

        [Inject] private ISceneEnvironment _sceneEnvironment;

        [Inject] private IObjectResolver _objectResolver;
        [Inject] private IPlayerService _playerService;

        private float _spawnTimer;
        private List<CharacterProxy> _spawnedEnemies = new List<CharacterProxy>();
        private CompositeDisposable _disposables;

        private ReactiveCommand<DeathData> _onAnyEnemyDeath = new ReactiveCommand<DeathData>();
        public IObservable<DeathData> OnAnyEnemyDeath => _onAnyEnemyDeath;

        private void SpawnEnemy()
        {
            var instance = Object.Instantiate(_prefab, _sceneEnvironment.DynamicObjectsParent);
            _objectResolver.Inject(instance.gameObject);
            instance.transform.position = _sceneEnvironment.GetRandomSpawnPoint();
            _spawnedEnemies.Add(instance);
            var hp = Random.Range(_hpRandomRange.x, _hpRandomRange.y);
            instance.Initialize(hp);

            instance.OnDead
                .Subscribe(data => OnEnemyDeath(data, instance))
                .AddTo(instance)
                .AddTo(_disposables);
        }

        private void OnEnemyDeath(DeathData deathData, CharacterProxy character)
        {
            _spawnedEnemies.Remove(character);
            _onAnyEnemyDeath.Execute(deathData);
            character.transform.DOScale(0, _deathAnimationDuration)
                .OnComplete(() => Object.Destroy(character.gameObject))
                .SetEase(Ease.InOutBounce)
                .SetTarget(character.transform);
        }

        private void ProcessTimer()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer > 0 || _spawnedEnemies.Count >= _maxEnemiesCount)
            {
                return;
            }

            if (_spawnTimer <= 0)
            {
                SpawnEnemy();
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            _spawnTimer = Random.Range(_enemySpawnTimeRandomRange.x, _enemySpawnTimeRandomRange.y);
        }

        #region Service

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as EnemyService;
            builder.RegisterInstance<IEnemyService>(instance)
                .As<IInitializable>()
                .As<ITickable>()
                .As<IDisposable>();
            return instance;
        }

        protected override void OnInitialize()
        {
            _spawnedEnemies.Clear();
            _disposables = new CompositeDisposable();
            ResetTimer();
        }

        protected override void OnDispose()
        {
            _disposables.Dispose();
            _spawnedEnemies.Clear();
        }

        protected override void OnTick()
        {
            ProcessTimer();
            var playerPos = _playerService.PlayerProxy.transform.position;

            foreach (var enemy in _spawnedEnemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                var enemyPos = enemy.transform.position;
                var dirToPlayer = (playerPos - enemyPos).normalized;
                dirToPlayer.y = 0;
                enemy.LookToDirectionSmooth(dirToPlayer, 1);
                enemy.Move(dirToPlayer * _enemiesSpeed * Time.deltaTime);
            }
        }

        #endregion
    }
}