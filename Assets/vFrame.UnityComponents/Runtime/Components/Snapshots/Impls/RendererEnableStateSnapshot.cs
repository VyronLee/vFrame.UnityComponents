using UnityEngine;

namespace vFrame.UnityComponents
{
    [DisallowMultipleComponent]
    public class RendererEnableStateSnapshot : GameObjectSnapshot
    {
        [SerializeField]
        private bool _enabled;

        public override bool Take() {
            var renderer_ = GetComponent<Renderer>();
            if (!renderer_) {
                return false;
            }
            _enabled = renderer_.enabled;
            return true;
        }

        public override bool Restore() {
            var renderer_ = GetComponent<Renderer>();
            if (!renderer_) {
                return false;
            }
            renderer_.enabled = _enabled;
            return true;
        }
    }
}