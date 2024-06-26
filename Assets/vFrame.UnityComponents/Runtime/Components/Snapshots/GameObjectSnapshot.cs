﻿using UnityEngine;

namespace vFrame.UnityComponents
{
    public abstract class GameObjectSnapshot : MonoBehaviour
    {
        protected virtual void Awake() {
            hideFlags = HideFlags.HideInInspector;
        }

        public abstract bool Take();

        public abstract bool Restore();
    }
}