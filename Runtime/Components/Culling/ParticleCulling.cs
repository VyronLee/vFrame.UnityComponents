//------------------------------------------------------------
//        File:  ParticleCulling.cs
//       Brief:  粒子剔除
//
//      Author:  VyronLee, lwz_jz@hotmail.com
//
//     Modified:  2019-05-10 17:27
//   Copyright:  Copyright (c) 2019, VyronLee
//============================================================

using System.Collections.Generic;
using UnityEngine;

namespace vFrame.UnityComponents
{
    public class ParticleCulling : CullingBehaviour
    {
        private readonly List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
        private readonly List<Renderer> _renderer = new List<Renderer>();

        protected override void Awake() {
            base.Awake();

            GetComponentsInChildren(_particleSystems);
            GetComponentsInChildren(_renderer);
        }

        protected override void OnBecameInvisible() {
            _particleSystems.ForEach(ps => ps.Stop());
            _renderer.ForEach(rd => rd.enabled = false);
        }

        protected override void OnBecameVisible() {
            _particleSystems.ForEach(ps => ps.Play());
            _renderer.ForEach(rd => rd.enabled = true);
        }
    }
}