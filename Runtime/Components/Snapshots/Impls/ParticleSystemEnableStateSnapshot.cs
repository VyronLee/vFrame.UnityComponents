using UnityEngine;

namespace vFrame.UnityComponents
{
    [DisallowMultipleComponent]
    public class ParticleSystemEnableStateSnapshot : GameObjectSnapshot
    {
        [SerializeField]
        private bool _enabled;

        public override bool Take() {
            var ps = transform.GetComponent<ParticleSystem>();
            if (!ps) {
                return false;
            }
            var emission = ps.emission;
            _enabled = emission.enabled;
            return true;
        }

        public override bool Restore() {
            var ps = transform.GetComponent<ParticleSystem>();
            if (!ps) {
                return false;
            }
            var emission = ps.emission;
            emission.enabled = _enabled;
            return true;
        }
    }
}