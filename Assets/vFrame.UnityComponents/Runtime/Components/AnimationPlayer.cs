﻿//------------------------------------------------------------
//        File:  AnimationList.cs
//       Brief:  动画列表，用于设定一系列动画名称以及其对应的动画片段
//               方便代码中根据名称直接播放对应动画
//
//      Author:  VyronLee, lwz_jz@hotmail.com
//
//     Modified:  2019-03-22 14:44
//   Copyright:  Copyright (c) 2019, VyronLee
//============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vFrame.Core.Unity.Extensions;

namespace vFrame.UnityComponents
{
    [RequireComponent(typeof(Animation))]
    [DisallowMultipleComponent]
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField]
        private List<AnimationSet> _animations;

        private Animation _animation;

        private void Awake() {
            _animation = GetComponent<Animation>();
        }

        /// <summary>
        ///     根据指定名称获取动画片段
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public AnimationClip GetAnimation(string animationName) {
            AnimationClip clip = null;
            foreach (var animationSet in _animations) {
                if (animationSet.name != animationName) {
                    continue;
                }
                clip = animationSet.clip;
                break;
            }

            return clip;
        }

        /// <summary>
        ///     根据指定名称播放动画片段
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public bool Play(string animationName) {
            var clip = GetAnimation(animationName);
            if (!clip) {
                return false;
            }
            _animation.clip = clip;
            _animation.Reset();
            return _animation.Play(clip.name);
        }

        /// <summary>
        ///     根据指定名称播放动画片段，直到动画播放完成
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public IEnumerator PlayUntilFinished(string animationName) {
            var clip = GetAnimation(animationName);
            if (clip) {
                yield return _animation.PlayUntilFinished(clip.name);
            }
        }

        /// <summary>
        /// 播放指定名称动画片段到末尾
        /// </summary>
        /// <param name="animationName"></param>
        public void ForwardToEnd(string animationName) {
            var clip = GetAnimation(animationName);
            if (clip) {
                _animation.clip = clip;
                _animation.Play();
                _animation[clip.name].normalizedTime = 1f;
                _animation.Sample();
                _animation.Stop();
            }
        }

        /// <summary>
        ///     根据指定名称播放动画片段
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public void CrossFade(string animationName) {
            var clip = GetAnimation(animationName);
            if (!clip) {
                return;
            }
            _animation.clip = clip;
            _animation.CrossFade(clip.name);
        }

        /// <summary>
        ///     根据指定名称播放动画片段，直到动画播放完成
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public IEnumerator CrossFadeUntilFinished(string animationName) {
            var clip = GetAnimation(animationName);
            if (clip) {
                yield return _animation.CrossFadeUntilFinished(clip.name);
            }
        }

        [Serializable]
        public class AnimationSet
        {
            public string name;
            public AnimationClip clip;
        }
    }
}