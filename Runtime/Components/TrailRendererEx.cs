using System.Collections;
using UnityEngine;

namespace vFrame.UnityComponents
{
    [RequireComponent(typeof(TrailRenderer))]
    [DisallowMultipleComponent]
    public class TrailRendererEx : MonoBehaviour
    {
        [SerializeField]
        private float _timeScale = 1f;

        private bool _paused;
        private float _pauseTime;
        private TrailRenderer _trailRender;
        private float _trailTime;

        public float TimeScale {
            get => _timeScale;
            set {
                _timeScale = value;

                Pause();
                UnPause();
            }
        }

        private void Awake() {
            _trailRender = GetComponent<TrailRenderer>();
            _trailTime = _trailRender.time;
        }

        public void Pause() {
            _pauseTime = Time.time;
            _trailRender.time = Mathf.Infinity;
            _paused = true;
        }

        public void UnPause() {
            var resumeTime = Time.time;
            _trailRender.time = resumeTime - _pauseTime + _trailTime / _timeScale;
            _paused = false;

            Invoke(nameof(DelayResetFromPause), _trailTime / _timeScale);
        }

        public bool IsPaused() {
            return _paused;
        }

        public void Clear() {
            _trailRender.time = 0;
            StartCoroutine(ResetTrail());
        }

        private void DelayResetFromPause() {
            if (_paused) {
                return;
            }
            _trailRender.time = _trailTime / _timeScale;
        }

        private IEnumerator ResetTrail() {
            yield return null;
            _trailRender.time = _trailTime / _timeScale;
        }
    }
}