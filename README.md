[English](README.md) | [简体中文](README.zh-CN.md)

# vFrame UnityComponents

`vFrame.UnityComponents` provides a set of `MonoBehaviour` components that can be attached directly to `GameObject`s, extending Unity's built-in animation, audio, culling, timed invocation, and state snapshot capabilities.

## Features

- Provides named playback over Unity's legacy `Animation` component through `AnimationPlayer`.
- Provides `AnimatorEx` and `TrailRendererEx` with time-scale and pause/resume control.
- Provides `AudioPlayer` to unify `AudioSource` playback, pause, mute, and completion callbacks.
- Provides `MethodInvoker` for delayed invocation and looping invocation through coroutines.
- Provides `CullingBehaviour` and `ParticleCulling` for frustum visibility control based on `CullingGroup`.
- Provides `GameObjectSnapshotBehaviour` and `GameObjectSnapshotRecursiveBehaviour` to save and restore object state.
- Includes built-in snapshot types: `TransformSnapshot`, `RendererEnableStateSnapshot`, `BehaviourEnableStateSnapshot`, and `ParticleSystemEnableStateSnapshot`.
- Includes Editor inspectors and `ShowOnlyAttribute` to make component inspection and setup easier in the Inspector.

## Requirements

- Unity `2018.4` or newer, as declared in `package.json`
- Verified and maintained in the current workspace with Unity `2022.3.62f3`
- Runtime assembly `vFrame.UnityComponents` depends on `vFrame.Core` and `vFrame.Core.Unity`
- Namespace: `vFrame.UnityComponents`

## Installation

### Install via UPM Git URL

Add the dependency to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.vyronlee.vframe.unity-components": "https://github.com/VyronLee/vFrame.UnityComponents.git#1.0.1"
  }
}
```

You can also add the repository URL in Unity through `Window > Package Manager > Add package from git URL...`.

### Install from source

If you use this library inside the vFrame workspace, make sure the following are available together:

- `Assets/vFrame.UnityComponents/`
- `vFrame.Core`
- `vFrame.Core.Unity`

## Component List

| Component | Description |
| --- | --- |
| `AnimationPlayer` | Maps custom string names to `AnimationClip` assets and supports playback, cross-fades, and waiting for completion |
| `AnimatorEx` | Controls `Animator.speed` by combining `TimeScale` and `Speed` |
| `AudioPlayer` | Wraps common `AudioSource` playback control and playback-finished callbacks |
| `TrailRendererEx` | Adds pause, resume, clear, and time-scale control to `TrailRenderer` |
| `MethodInvoker` | Provides coroutine scheduling APIs: `Invoke`, `DelayInvoke`, and `LoopInvoke` |
| `CullingBehaviour` | Inheritable culling base class using `CullingGroup` to detect visibility changes |
| `ParticleCulling` | Automatically starts and stops particle systems and renderers based on visibility |
| `GameObjectSnapshot` | Base class for snapshot implementations, defining `Take()` and `Restore()` |
| `GameObjectSnapshotBehaviour` | Captures and restores snapshots for a single `GameObject` according to configuration |
| `GameObjectSnapshotRecursiveBehaviour` | Recursively processes snapshots for the current object and all its children |
| `GameObjectSnapshotSettings` | `ScriptableObject` configuration base class that declares `SnapshotTypes` |
| `TransformSnapshot` | Stores active state, layer, tag, and local position/scale/rotation |
| `RendererEnableStateSnapshot` | Stores `Renderer.enabled` |
| `BehaviourEnableStateSnapshot` | Stores `Behaviour.enabled` state for all child behaviours |
| `ParticleSystemEnableStateSnapshot` | Stores `ParticleSystem.emission.enabled` |
| `ShowOnlyAttribute` | Inspector helper attribute for read-only field display |

## Quick Start

The example below shows how to play an audio clip and execute a callback two seconds later:

```csharp
using UnityEngine;
using vFrame.UnityComponents;

public class AudioPlayerExample : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private void Start()
    {
        var player = gameObject.AddComponent<AudioPlayer>();
        var invoker = gameObject.AddComponent<MethodInvoker>();

        player.SetClip(_clip);
        player.Play(false, 1f, () => Debug.Log("Audio finished"));

        invoker.DelayInvoke(2f, () => Debug.Log("2 seconds passed"));
    }
}
```

## Usage

### Scenario 1: Play legacy `Animation` clips by name

`AnimationPlayer` depends on Unity's legacy `Animation` component and uses an `AnimationSet` list to map names to clips.

```csharp
using System.Collections;
using UnityEngine;
using vFrame.UnityComponents;

public class AnimationPlayerExample : MonoBehaviour
{
    [SerializeField] private AnimationPlayer _player;

    private IEnumerator Start()
    {
        if (_player.Play("Idle"))
        {
            yield return _player.CrossFadeUntilFinished("Attack");
            _player.ForwardToEnd("Attack");
        }
    }
}
```

Common members:

| Member | Description |
| --- | --- |
| `GetAnimation(string animationName)` | Gets the mapped `AnimationClip` |
| `Play(string animationName)` | Plays the specified animation and returns `false` if the name is not found |
| `PlayUntilFinished(string animationName)` | Plays and waits until completion |
| `CrossFade(string animationName)` | Cross-fades to the specified animation |
| `CrossFadeUntilFinished(string animationName)` | Cross-fades and waits until completion |
| `ForwardToEnd(string animationName)` | Samples directly to the end of the animation |

### Scenario 2: Control animator, trail, and audio timing

```csharp
using UnityEngine;
using vFrame.UnityComponents;

public class TimeScaleExample : MonoBehaviour
{
    [SerializeField] private AnimatorEx _animatorEx;
    [SerializeField] private TrailRendererEx _trailRendererEx;
    [SerializeField] private AudioPlayer _audioPlayer;

    public void PauseEffects()
    {
        _animatorEx.TimeScale = 0f;
        _trailRendererEx.Pause();
        _audioPlayer.Pause();
    }

    public void ResumeEffects()
    {
        _animatorEx.TimeScale = 1f;
        _trailRendererEx.UnPause();
        _audioPlayer.UnPause();
    }
}
```

Additional notes:

- `AnimatorEx.Speed` and `AnimatorEx.TimeScale` are multiplied together before being written to `Animator.speed`
- `TrailRendererEx.Clear()` clears the current trail and restores timing parameters on the next frame
- Both `AudioPlayer.Play()` and `AudioPlayer.Play(bool loop, float volume, Action onPlayFinished = null)` require `AudioSource.clip` to be assigned first

### Scenario 3: Delayed invocation and repeated polling

```csharp
using UnityEngine;
using vFrame.UnityComponents;

public class MethodInvokerExample : MonoBehaviour
{
    [SerializeField] private MethodInvoker _invoker;
    private int _count;

    private void Start()
    {
        _invoker.TimeScale = 1f;
        _invoker.DelayInvoke(1f, () => Debug.Log("Delayed once"));
        _invoker.LoopInvoke(0.5f, Tick, true);
    }

    private bool Tick()
    {
        _count++;
        Debug.Log($"Tick {_count}");
        return _count >= 5;
    }
}
```

When the callback passed to `LoopInvoke` returns `true`, the loop stops. The component also calls `Stop()` automatically in `OnDisable()` and `OnDestroy()`.

### Scenario 4: Control particle systems by camera visibility

```csharp
using UnityEngine;
using vFrame.UnityComponents;

public class ParticleCullingExample : MonoBehaviour
{
    [SerializeField] private ParticleCulling _particleCulling;

    private void Awake()
    {
        _particleCulling.TargetCamera = Camera.main;
        _particleCulling.AutoUpdate = true;
        _particleCulling.onCullingStateChanged += isInvisible =>
            Debug.Log($"Particle invisible: {isInvisible}");
    }
}
```

`ParticleCulling` derives from `CullingBehaviour`. When the object enters the view, it calls `ParticleSystem.Play()` and enables related `Renderer`s; when it leaves the view, it calls `ParticleSystem.Stop()` and disables those renderers.

### Scenario 5: Save and restore object state

First, create a configuration asset derived from `GameObjectSnapshotSettings` and return the snapshot types to capture:

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using vFrame.UnityComponents;

[CreateAssetMenu(menuName = "vFrame/Snapshot Settings")]
public class ExampleSnapshotSettings : GameObjectSnapshotSettings
{
    public override List<Type> SnapshotTypes => new List<Type>
    {
        typeof(TransformSnapshot),
        typeof(RendererEnableStateSnapshot),
        typeof(ParticleSystemEnableStateSnapshot)
    };
}
```

Then call it from a component:

```csharp
using UnityEngine;
using vFrame.UnityComponents;

public class SnapshotExample : MonoBehaviour
{
    [SerializeField] private GameObjectSnapshotBehaviour _snapshot;

    public void SaveState()
    {
        _snapshot.Take();
    }

    public void RestoreState()
    {
        _snapshot.Restore();
    }
}
```

If you need to process the entire child hierarchy as well, use `Take()`, `Restore()`, and `Clear()` on `GameObjectSnapshotRecursiveBehaviour`.

## Notes

- `AnimationPlayer` uses Unity's legacy `Animation`, not `Animator`
- `AudioPlayer` playback with completion callbacks relies on `Update()` polling for the playback-finished state
- `CullingBehaviour` is marked with `[ExecuteInEditMode]`, but its runtime culling logic mainly targets `Application.isPlaying`
- `GameObjectSnapshotBehaviour` and `GameObjectSnapshotRecursiveBehaviour` dynamically add snapshot components at runtime and set their `hideFlags` to `HideInInspector`
- The configuration entry point for the snapshot system is `GameObjectSnapshotSettings.SnapshotTypes`
- The package also includes an Editor assembly for Inspector and drawer support; runtime code lives under `Assets/vFrame.UnityComponents/Runtime/`

## License

This project is licensed under the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).
