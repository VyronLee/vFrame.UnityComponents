//------------------------------------------------------------
//        File:  AudioPlayer.cs
//       Brief:  音乐播放器简单封装
//
//      Author:  VyronLee, lwz_jz@hotmail.com
//
//    Modified:  2019-11-07 20:19
//   Copyright:  Copyright (c) 2019, VyronLee
//============================================================

using System;
using UnityEngine;

namespace vFrame.UnityComponents
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private bool _isDirty;
        private Action _onPlayFinished;

        public bool IsPause { get; private set; }

        public float Volume {
            get {
                if (Source) {
                    return Source.volume;
                }
                return 0f;
            }
            set {
                if (Source) {
                    Source.volume = value;
                }
            }
        }

        public bool IsPlaying {
            get {
                if (Source) {
                    return Source.isPlaying;
                }
                return false;
            }
        }

        public AudioSource Source { get; private set; }

        private void Awake() {
            Source = GetComponent<AudioSource>();
        }

        private void Update() {
            if (!_isDirty) {
                return;
            }

            if (IsPause || !Source || Source.isPlaying) {
                return;
            }

            if (null != _onPlayFinished) {
                _onPlayFinished.Invoke();
                _onPlayFinished = null;
            }

            _isDirty = false;
        }

        public void ResetAudioSource() {
            if (!Source) {
                return;
            }
            Source.time = 0f;
            Source.pitch = 1f;
        }

        public void Pause() {
            IsPause = true;
            if (Source) {
                Source.Pause();
            }
        }

        public void UnPause() {
            IsPause = false;
            if (Source) {
                Source.UnPause();
            }
        }

        public void Mute() {
            if (Source) {
                Source.mute = true;
            }
        }

        public void UnMute() {
            if (Source) {
                Source.mute = false;
            }
        }

        public void SetClip(AudioClip clip) {
            if (Source) {
                Source.clip = clip;
            }
        }

        public void Stop() {
            if (Source) {
                Source.Stop();
            }
            _isDirty = false;
            IsPause = false;
            _onPlayFinished = null;
        }

        public void Play() {
            if (!Source || !Source.clip) {
                Debug.LogError("AudioClip not set.");
                return;
            }

            if (IsPlaying) {
                Stop();
            }
            Source.Play();
        }

        public void Play(bool loop, float volume, Action onPlayFinished = null) {
            if (!Source || !Source.clip) {
                Debug.LogError("AudioClip not set.");
                return;
            }

            if (IsPlaying) {
                Stop();
            }

            _onPlayFinished = onPlayFinished;
            Source.loop = loop;
            Source.volume = volume;
            Source.Play();
            _isDirty = true;
        }

        public void Play(bool loop, float volume, bool destroyWhenFinished) {
            if (!Source || !Source.clip) {
                Debug.LogError("AudioClip not set.");
                return;
            }

            if (IsPlaying) {
                Stop();
            }

            if (destroyWhenFinished) {
                _onPlayFinished = () => { Destroy(gameObject); };
            }
            Source.loop = loop;
            Source.volume = volume;
            Source.Play();
            _isDirty = true;
        }

        public void Play(AudioClip clip, bool loop, float volume, Action onPlayFinished = null) {
            if (!Source) {
                return;
            }

            if (IsPlaying) {
                Stop();
            }

            _onPlayFinished = onPlayFinished;
            Source.clip = clip;
            Source.loop = loop;
            Source.volume = volume;
            Source.Play();
            _isDirty = true;
        }
    }
}