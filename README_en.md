# vFrame Unity Component Library

![vFrame](https://img.shields.io/badge/vFrame-UnityComponents-blue) [![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com) [![License](https://img.shields.io/badge/License-Apache%202.0-brightgreen.svg)](#License)

This library mainly provides a series of components inherited from Unity MonoBehaviour, serving as supplements to the official components.

[English Version (Powered by ChatGPT)](./README_en.md)

## Table of Contents

* [AnimationPlayer](#animationplayer)
* [AnimatorEx](#AnimatorEx)
* [AudioPlayer](#audioplayer)
* [TrailRendererEx](#trailrendererex)
* [MethodInvoker](#methodinvoker)
* [CullingBehaviour](#cullingbehaviour)
* [ParticleCulling](#particleculling)
* [GameObjectSnapshot](#gameobjectsnapshot)
    + [GameObjectSnapshotBehaviour](#gameobjectsnapshotbehaviour)
    + [GameObjectSnapshotRecursiveBehaviour](#gameobjectsnapshotrecursivebehaviour)
* [License](#license)

## AnimationPlayer

### Feature Introduction
AnimationPlayer is a component for playing animations. It can retrieve animation clips based on **specified animation names** and provides various methods for playing animations, including direct play, play until the end, fade in and out, etc. The biggest difference from Animation is that it can use custom animation names to bind with AnimationClip, without using the AnimationClip names to control animations.

### Interface Description

1. Retrieve the animation clip with the specified name
```csharp
public AnimationClip GetAnimation(string animationName)
```

2. Play the animation clip with the specified name
```csharp
public bool Play(string animationName)
```

3. Play the animation clip with the specified name until the animation is finished
```csharp
public IEnumerator PlayUntilFinished(string animationName)
```

4. Play the specified animation clip to the end
```csharp
public void ForwardToEnd(string animationName)
```

5. Crossfade animation with the specified name
```csharp
public void CrossFade(string animationName)
```

6. Crossfade animation with the specified name until the animation is finished
```csharp
public IEnumerator CrossFadeUntilFinished(string animationName)
```

## AnimatorEx

### Feature Introduction
`AnimatorEx` is a Unity component designed to extend the functionality of the standard `Animator` component, supporting dynamic adjustment of animation playback speed and time scaling.

### Interface Description

1. Time scaling property
```csharp
public float TimeScale { get; set; }
```
Allows getting or setting the time scale value. This value affects the playback speed of the animation and determines the final animation playback speed in conjunction with the Speed property.

2. Speed property
```csharp
public float Speed { get; set; }
```
Allows getting or setting the playback speed of the animation. This value multiplies with the TimeScale property value to determine the final animation playback speed.

## AudioPlayer

### Feature Introduction
`AudioPlayer` is a Unity component designed to simplify the process of audio playback. It provides a series of methods for controlling audio playback, pause, stop, mute, and setting audio clips and volume. This class also supports triggering a callback function after audio playback is complete, making it simple and straightforward to perform specific operations once the audio has finished playing.

### Interface Description

1. Get the pause status
```csharp
public bool IsPause { get; }
```

2. Get and set volume
```csharp
public float Volume { get; set; }
```

3. Check if audio is playing
```csharp
public bool IsPlaying { get; }
```

4. Get the audio source component
```csharp
public AudioSource Source { get; }
```

5. Reset the audio source state
```csharp
public void ResetAudioSource()
```

6. Pause audio playback
```csharp
public void Pause()
```

7. Continue audio playback
```csharp
public void UnPause()
```

8. Mute audio
```csharp
public void Mute()
```

9. Unmute audio
```csharp
public void UnMute()
```

10. Set the audio clip to play
```csharp
public void SetClip(AudioClip clip)
```

11. Stop audio playback
```csharp
public void Stop()
```

12. Play audio
```csharp
public void Play()
```

13. Play audio (with parameters)
```csharp
public void Play(bool loop, float volume, Action onPlayFinished = null)
```

14. Play audio and optionally destroy on completion
```csharp
public void Play(bool loop, float volume, bool destroyWhenFinished)
```

15. Play a specified audio clip
```csharp
public void Play(AudioClip clip, bool loop, float volume, Action onPlayFinished = null)
```

## TrailRendererEx

### Feature Introduction
`TrailRendererEx` is a Unity component that enhances the functionality of the standard `TrailRenderer` component. It provides the ability to pause and resume trail effects and allows dynamic adjustment of the trail's time scale.

### Interface Description

1. Time scale property
```csharp
public float TimeScale { get; set; }
```
Allows getting or setting the time scale value. This value affects the duration of the trail effect, and the trail is automatically paused and resumed when the time scale is set to apply the new value.

2. Pause trail effect
```csharp
public void Pause()
```
Pauses the trail effect, so it no longer updates when the game is paused or when it's necessary to stop the trail.

3. Resume trail effect
```csharp
public void UnPause()
```
Resumes the paused trail effect and continues updating the trail according to the time scale value.

4. Check if paused
```csharp
public bool IsPaused()
```
Returns a boolean value indicating whether the trail effect is paused.

5. Clear trail effect
```csharp
public void Clear()
```
Immediately clears the current trail effect and resets the trail's time in the next frame to begin drawing the trail effect anew.

## MethodInvoker

### Feature Introduction
`MethodInvoker` is a Unity component specifically used for invoking methods at runtime, performing delayed calls, looping calls, etc. It utilizes coroutines to implement these features and provides a convenient way to handle method calls with different parameters. By using `MethodInvoker`, developers can easily implement complex timing control and method scheduling in their games, such as executing tasks at set times or repeating tasks until a certain condition is met.

### Interface Description

1. Time scale property
```csharp
public float TimeScale { get; set; }
```
Allows getting or setting the time scale value, which affects the timing intervals of delayed and looping calls.

2. Stop all coroutines
```csharp
public void Stop()
```
Stops all coroutines currently executing on the `MethodInvoker` component.

3. Invoke a method
```csharp
public Coroutine Invoke(Func<IEnumerator> action)
```
Starts a coroutine to invoke the specified method.

4. Delayed method invocation
```csharp
public Coroutine DelayInvoke(float time, Action action)
```
Invokes a method after a specified delay time.

5. Looping method invocation
```csharp
public Coroutine LoopInvoke(float interval, Func<bool> action, bool invokeImmediately = true)
```
Repeats the invocation of a method at specified intervals. The looping stops when the method returns `true` or the coroutine is stopped.

`MethodInvoker` also provides several overloaded versions of `Invoke`, `DelayInvoke`, and `LoopInvoke` methods to support different numbers and types of parameters. These overloaded methods allow developers to pass parameters to the invoked methods as needed, adding flexibility to the invocations.

## CullingBehaviour

### Feature Introduction
`CullingBehaviour` is an abstract Unity component for implementing view-based culling functionality. Using `CullingGroup` and `BoundingSphere`, it can determine whether objects should be rendered based on the camera's perspective and the set culling radius. This class is meant to be inherited, and its `OnBecameVisible` and `OnBecameInvisible` methods can be overridden to implement custom behaviors when objects become visible or invisible. This component is suitable for optimizing scene rendering performance, especially when dealing with a large number of objects' visibility.

### Interface Description

1. Set the culling camera
```csharp
public Camera TargetCamera { set; get; }
```
Allows getting or setting the target camera for culling decisions. If not specified, the main camera will be used by default.

2. Auto-update bounding box position
```csharp
public bool AutoUpdate { set; get; }
```
Enables or disables the auto-update feature for the culling area. When enabled, the culling area is updated every frame based on the object's current position.

3. Reset
```csharp
public void Reset()
```
Resets and releases `CullingGroup` resources, clearing all related culling settings.

4. Update culling group
```csharp
public void UpdateCullingGroup()
```
Manually updates the culling area, typically called when the object's position changes or when a forced update of culling settings is needed.

5. Update culling state
```csharp
public void UpdateCullingState()
```
Updates the visibility state of the object based on the current culling area and the position of the target camera.

6. Visibility change event
```csharp
public event Action<bool> onCullingStateChanged;
```
An event triggered when the visibility state of the object changes. The parameter is `true` for the object becoming invisible and `false` for becoming visible.

This class also contains a series of protected virtual methods like `OnBecameInvisible` and `OnBecameVisible`, allowing for custom visibility change behaviors to be implemented in derived classes. Additionally, it provides an `IsVisible` method for checking the current visibility of the object.

## ParticleCulling

### Feature Introduction
`ParticleCulling` class inherits from `CullingBehaviour`, specifically for view culling of particle systems. It overrides the `OnBecameVisible` and `OnBecameInvisible` methods to automatically start or stop playing particle effects when the particle system enters or leaves the camera view. It also controls the enabled state of the related renderer, optimizing the performance of particle systems in the scene, especially when dealing with a large number of particle systems, significantly reducing unnecessary rendering overhead.

## GameObjectSnapshot

### GameObjectSnapshotBehaviour

#### Feature Introduction
`GameObjectSnapshotBehaviour` is a Unity component used for snapshotting and restoring the state of game objects. It enables developers to capture the current state of a game object at runtime and restore it to that state at any later point in time. This feature is particularly useful in scenarios where game object states need to be temporarily modified for testing or specific operations, and then later restored to their original state.

#### Interface Description

1. Set and get snapshot settings
```csharp
public GameObjectSnapshotSettings SnapshotSettings { get; set; }
```
Allows getting or setting the configuration for snapshots, including information on which component types need to have their states captured. Developers can customize the snapshot capturing functionality by inheriting from `GameObjectSnapshot`.

2. Capture the current state
```csharp
public void Take()
```
Captures the current state of the game object according to the rules defined in the snapshot settings and stores these states. If a snapshot already exists, it clears the old snapshot first.

3. Restore to snapshot state
```csharp
public void Restore()
```
Restores the game object to the state of the most recent snapshot taken. If no snapshot is available, this operation is ineffective.

4. Clear all snapshots
```csharp
public void Clear()
```
Clears all stored snapshot information and frees related resources. It is automatically called before taking a new snapshot and can also be manually called to ensure no old snapshot data is retained.

`GameObjectSnapshotBehaviour` implements the logic of capturing and restoring snapshots through internal methods `TakeInternal` and `RestoreInternal`. These methods use reflection to create and manipulate components inherited from `GameObjectSnapshot` to capture and restore the state of specific components. The design of this component allows developers to flexibly manage the state of game objects to accommodate various runtime state changes.

### GameObjectSnapshotRecursiveBehaviour

#### Feature Introduction
`GameObjectSnapshotRecursiveBehaviour` is a Unity component that recursively snapshots and restores the state of **game objects and all their children**. It extends the functionality of `GameObjectSnapshotBehaviour`, not only allowing snapshots of individual game objects but also recursively capturing the state of all their children, thus implementing the state capture and restoration of an entire game object tree. This is highly useful for scenarios where the states of multiple game objects need to be managed simultaneously, such as restoring the entire state of a scene to a specific moment at once.

#### Interface Description

1. Set and get snapshot settings
```csharp
public GameObjectSnapshotSettings SnapshotSettings { get; set; }
```
Allows getting or setting the configuration for snapshots, including information on which component types need to have their states captured.

2. Capture the current state
```csharp
public void Take()
```
Captures the current state of the game object and all its children according to the rules defined in the snapshot settings and stores these states. If a snapshot already exists, it clears the old snapshot first.

3. Restore to snapshot state
```csharp
public void Restore()
```
Restores the game object and all its children to the state of the most recent snapshot taken. If no snapshot is available, this operation is ineffective.

4. Clear all snapshots
```csharp
public void Clear()
```
Clears all stored snapshot information and frees related resources. It is automatically called before taking a new snapshot and can also be manually called to ensure no old snapshot data is retained.

`GameObjectSnapshotRecursiveBehaviour` implements the logic for capturing snapshots of every object in the game object tree through the internal method `TakeInternal`. This method adds a `GameObjectSnapshotBehaviour` component to each game object and calls its `Take` method to capture the state. This recursive capturing approach ensures that the state of the entire game object tree can be comprehensively saved and restored, providing developers with powerful state management capabilities.

## License

[Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0)