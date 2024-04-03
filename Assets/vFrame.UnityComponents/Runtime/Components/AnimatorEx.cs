using UnityEngine;

namespace vFrame.UnityComponents
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class AnimatorEx : MonoBehaviour
    {
        [SerializeField]
        private float _timeScale = 1f;

        [SerializeField]
        private float _speed = 1f;

        private Animator _animator;

        public float TimeScale {
            get => _timeScale;
            set {
                _timeScale = value;
                ApplyState();
            }
        }

        public float Speed {
            get => _speed;
            set {
                _speed = value;
                ApplyState();
            }
        }

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void ApplyState() {
            if (!_animator) {
                return;
            }
            _animator.speed = _speed * _timeScale;
        }
    }
}