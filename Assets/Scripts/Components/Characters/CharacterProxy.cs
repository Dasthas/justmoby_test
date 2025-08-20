using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Components.Characters
{
    public class CharacterProxy : MonoBehaviour, ICharacter
    {
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private HealthController _healthController;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private ParticleSystem _onShootParticleSystem;
        [SerializeField] private Transform _weaponTransform;

        private Sequence _onShootSequence;
        public Transform WeaponTransform => _weaponTransform;

        private ReactiveCommand<DeathData> _onDead = new ReactiveCommand<DeathData>();
        private ReactiveCommand<TakeDamageData> _onTakeDamage = new ReactiveCommand<TakeDamageData>();
        public IObservable<DeathData> OnDead => _onDead;
        public IObservable<TakeDamageData> OnTakeDamage => _onTakeDamage;

        public void Initialize(float maxHp)
        {
            _healthController.Initialize(maxHp, _onDead, _onTakeDamage);
        }

        public void LookToDirectionSmooth(Vector3 direction, float speed)
        {
            _lookTransform.forward = Vector3.Lerp(_lookTransform.forward, direction, speed);
        }

        public void Move(Vector3 velocity)
        {
            transform.position += velocity;
            _bodyTransform.forward = velocity.normalized;
        }

        public void ShowShootVfx()
        {
            _onShootParticleSystem.Play();
            if (_onShootSequence == null)
            {
                _onShootSequence = DOTween.Sequence(_weaponTransform);
                _weaponTransform.localPosition = Vector3.zero;
                _onShootSequence.Append(_weaponTransform.DOLocalMoveZ(-1, 0.1f))
                    .Append(_weaponTransform.DOLocalMoveZ(0, 0.1f))
                    .SetAutoKill(false);
            }

            _onShootSequence.Restart();
        }
    }
}