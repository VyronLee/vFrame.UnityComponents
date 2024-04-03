# vFrame Unity组件库

![vFrame](https://img.shields.io/badge/vFrame-UnityComponents-blue) [![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com) [![License](https://img.shields.io/badge/License-Apache%202.0-brightgreen.svg)](#License)

该组件库主要提供一些继承于 Unity MonoBehaviour 的组件，用作于官方组件的补充。

[English Version (Power by ChatGPT)](./README_en.md)

## 目录

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

### 功能介绍
AnimationPlayer是一个用于播放动画的组件。它可以根据**指定的动画名称**获取对应的动画片段，并提供了多种播放动画的方法，包括直接播放、播放到末尾、淡入淡出等。跟Animation最大的区别是，它可以使用自定义动画名称跟AnimationClip绑定，无需使用AnimationClip名称来操作动画。

### 接口说明

1. 获取指定名称的动画片段
```csharp
public AnimationClip GetAnimation(string animationName)
```

2. 根据指定名称播放动画片段
```csharp
public bool Play(string animationName)
```

3. 根据指定名称播放动画片段，直到动画播放完成
```csharp
public IEnumerator PlayUntilFinished(string animationName)
```

4. 播放指定名称动画片段到末尾
```csharp
public void ForwardToEnd(string animationName)
```

5. 根据指定名称进行动画的交叉淡入
```csharp
public void CrossFade(string animationName)
```

6. 根据指定名称进行动画的交叉淡入，直到动画播放完成
```csharp
public IEnumerator CrossFadeUntilFinished(string animationName)
```

## AnimatorEx

### 功能介绍
`AnimatorEx` 类是一个Unity组件，旨在扩展标准 `Animator` 组件的功能，使其支持动态调整动画播放速度和时间缩放。

### 接口说明

1. 时间缩放属性
```csharp
public float TimeScale { get; set; }
```
允许获取或设置时间缩放值。该值会影响动画的播放速度，与Speed属性共同决定最终的动画播放速度。

2. 速度属性
```csharp
public float Speed { get; set; }
```
允许获取或设置动画的播放速度。该值会与TimeScale属性的值相乘，共同决定最终的动画播放速度。

## AudioPlayer

### 功能介绍
`AudioPlayer` 类是一个Unity组件，用于简化音频播放的过程。它提供了一系列方法来控制音频的播放、暂停、停止、静音以及设置音频剪辑和音量等。此类还支持播放完成后触发回调函数，使得在音频播放完毕后执行特定操作变得简单直接。

### 接口说明

1. 获取是否暂停状态
```csharp
public bool IsPause { get; }
```

2. 获取和设置音量
```csharp
public float Volume { get; set; }
```

3. 检查是否正在播放音频
```csharp
public bool IsPlaying { get; }
```

4. 获取音频源组件
```csharp
public AudioSource Source { get; }
```

5. 重置音频源状态
```csharp
public void ResetAudioSource()
```

6. 暂停音频播放
```csharp
public void Pause()
```

7. 继续音频播放
```csharp
public void UnPause()
```

8. 静音
```csharp
public void Mute()
```

9. 取消静音
```csharp
public void UnMute()
```

10. 设置播放的音频剪辑
```csharp
public void SetClip(AudioClip clip)
```

11. 停止音频播放
```csharp
public void Stop()
```

12. 播放音频
```csharp
public void Play()
```

13. 播放音频（带参数）
```csharp
public void Play(bool loop, float volume, Action onPlayFinished = null)
```

14. 播放音频并在完成时可选择销毁
```csharp
public void Play(bool loop, float volume, bool destroyWhenFinished)
```

15. 播放指定的音频剪辑
```csharp
public void Play(AudioClip clip, bool loop, float volume, Action onPlayFinished = null)
```

## TrailRendererEx

### 功能介绍
`TrailRendererEx` 类是一个Unity组件，用于增强标准 `TrailRenderer` 组件的功能。它提供了暂停和恢复拖尾效果的能力，同时允许动态调整拖尾的时间缩放。

### 接口说明

1. 时间缩放属性
```csharp
public float TimeScale { get; set; }
```
允许获取或设置时间缩放值。该值会影响拖尾效果的持续时间，同时在设置时会自动暂停并恢复拖尾，以应用新的时间缩放值。

2. 暂停拖尾效果
```csharp
public void Pause()
```
暂停拖尾效果，使其在游戏暂停或需要停止拖尾时不再更新。

3. 恢复拖尾效果
```csharp
public void UnPause()
```
恢复暂停的拖尾效果，继续根据时间缩放值更新拖尾。

4. 检查是否暂停
```csharp
public bool IsPaused()
```
返回一个布尔值，表示拖尾效果是否处于暂停状态。

5. 清除拖尾效果
```csharp
public void Clear()
```
立即清除当前的拖尾效果，并在下一帧重置拖尾的时间，以便重新开始拖尾效果的绘制。

## MethodInvoker

### 功能介绍
`MethodInvoker` 类是一个Unity组件，专门用于在运行时调用方法、执行延迟调用、循环调用等操作。它利用协程（Coroutine）来实现这些功能，并提供了一种方便的方式来处理带有不同参数的方法调用。通过使用 `MethodInvoker`，开发者可以轻松地在游戏中实现复杂的时间控制和方法调度，如定时执行任务、重复执行任务直到某条件满足等。

### 接口说明

1. 时间缩放属性
```csharp
public float TimeScale { get; set; }
```
允许获取或设置时间缩放值，该值会影响延迟调用和循环调用的时间间隔。

2. 停止所有协程
```csharp
public void Stop()
```
停止 `MethodInvoker` 组件上所有正在执行的协程。

3. 调用方法
```csharp
public Coroutine Invoke(Func<IEnumerator> action)
```
启动一个协程来调用指定的方法。

4. 延迟调用方法
```csharp
public Coroutine DelayInvoke(float time, Action action)
```
在指定的延迟时间后调用方法。

5. 循环调用方法
```csharp
public Coroutine LoopInvoke(float interval, Func<bool> action, bool invokeImmediately = true)
```
以指定的时间间隔重复调用方法。当方法返回 `true` 或协程被停止时，循环调用结束。

`MethodInvoker` 还提供了多个重载版本的 `Invoke`、`DelayInvoke` 和 `LoopInvoke` 方法，以支持不同数量和类型的参数。这些重载方法允许开发者根据需要传递参数给被调用的方法，从而增加了调用的灵活性。

## CullingBehaviour

### 功能介绍
`CullingBehaviour` 类是一个Unity抽象组件，用于实现基于视图剔除的功能。通过使用`CullingGroup`和`BoundingSphere`，它可以根据相机视角和设置的剔除半径来决定对象是否应该被渲染。这个类旨在被继承，其中的`OnBecameVisible`和`OnBecameInvisible`方法可以被重写以实现当对象变为可见或不可见时的自定义行为。此组件适用于优化场景渲染性能，特别是在处理大量对象的可见性时。

### 接口说明

1. 设置剔除使用的相机
```csharp
public Camera TargetCamera { set; get; }
```
允许获取或设置用于剔除判断的目标相机。如果未指定，将默认使用主相机。

2. 自动更新包围盒位置
```csharp
public bool AutoUpdate { set; get; }
```
开启或关闭自动更新剔除区域的功能。当开启时，每帧都会根据对象的当前位置更新剔除区域。

3. 重置
```csharp
public void Reset()
```
重置并释放`CullingGroup`资源，清除所有相关的剔除设置。

4. 更新剔除区域
```csharp
public void UpdateCullingGroup()
```
手动更新剔除区域，通常在对象位置变化或需要强制更新剔除设置时调用。

5. 更新剔除状态
```csharp
public void UpdateCullingState()
```
根据当前的剔除区域和目标相机的位置，更新对象的可见性状态。

6. 可见性变更事件
```csharp
public event Action<bool> onCullingStateChanged;
```
当对象的可见性状态发生变化时触发的事件。参数为`true`表示对象变为不可见，`false`表示变为可见。

此类还包含了一系列受保护的虚方法，如`OnBecameInvisible`和`OnBecameVisible`，允许在派生类中重写以实现自定义的可见性变更行为。此外，还提供了`IsVisible`方法来检查当前对象是否可见。

## ParticleCulling

功能介绍
`ParticleCulling` 类继承自 `CullingBehaviour`，专门用于粒子系统的视图剔除。它通过覆盖 `OnBecameVisible` 和 `OnBecameInvisible` 方法，实现了当粒子系统进入或离开相机视野时自动开始或停止播放粒子效果。此外，它也控制相关渲染器的启用状态，从而优化了场景中粒子系统的性能表现，特别是在处理大量粒子系统时，可以显著减少不必要的渲染开销。

## GameObjectSnapshot

### GameObjectSnapshotBehaviour

#### 功能介绍
`GameObjectSnapshotBehaviour` 类是Unity组件，用于对游戏对象的状态进行快照和恢复。它允许开发者在运行时捕捉游戏对象的当前状态，并在之后的任何时间点将其恢复到这个状态。这个功能特别适用于需要临时修改游戏对象状态进行测试或特定操作，而后又希望恢复到原始状态的场景。

#### 接口说明

1. 设置和获取快照设置
```csharp
public GameObjectSnapshotSettings SnapshotSettings { get; set; }
```
允许获取或设置快照的配置，包括决定哪些组件类型需要被捕捉状态的信息。开发者可以通过继承`GameObjectSnapshot`来自定义快照捕捉功能。

2. 捕捉当前状态
```csharp
public void Take()
```
对游戏对象按照快照设置中定义的规则捕捉当前状态，并存储这些状态。如果之前已有快照，则先清除旧的快照。

3. 恢复到快照状态
```csharp
public void Restore()
```
将游戏对象恢复到最近一次捕捉的状态。如果没有可用的快照，则此操作无效。

4. 清除所有快照
```csharp
public void Clear()
```
清除所有存储的快照信息，释放相关资源。在捕捉新的快照前自动调用，也可以手动调用以确保不保留任何旧的快照数据。

`GameObjectSnapshotBehaviour` 通过内部方法 `TakeInternal` 和 `RestoreInternal` 实现了快照的捕捉和恢复逻辑。这些方法通过反射创建和操作继承自 `GameObjectSnapshot` 的组件，以实现对特定组件状态的捕捉和恢复。此组件的设计使得开发者可以灵活地对游戏对象的状态进行管理，以适应各种运行时状态变化的需求。

### GameObjectSnapshotRecursiveBehaviour

#### 功能介绍
`GameObjectSnapshotRecursiveBehaviour` 类是一个Unity组件，用于对**游戏对象及其所有子对象**的状态进行递归快照和恢复。它扩展了 `GameObjectSnapshotBehaviour` 的功能，使得不仅可以对单个游戏对象进行快照，还能够递归地对其所有子对象进行快照，从而实现整个游戏对象树的状态捕捉和恢复。这对于需要同时管理多个游戏对象状态的场景非常有用，例如，在一次性恢复整个场景的状态到特定时刻。

#### 接口说明

1. 设置和获取快照设置
```csharp
public GameObjectSnapshotSettings SnapshotSettings { get; set; }
```
允许获取或设置快照的配置，包括决定哪些组件类型需要被捕捉状态的信息。

2. 捕捉当前状态
```csharp
public void Take()
```
对游戏对象及其所有子对象按照快照设置中定义的规则捕捉当前状态，并存储这些状态。如果之前已有快照，则先清除旧的快照。

3. 恢复到快照状态
```csharp
public void Restore()
```
将游戏对象及其所有子对象恢复到最近一次捕捉的状态。如果没有可用的快照，则此操作无效。

4. 清除所有快照
```csharp
public void Clear()
```
清除所有存储的快照信息，释放相关资源。在捕捉新的快照前自动调用，也可以手动调用以确保不保留任何旧的快照数据。

`GameObjectSnapshotRecursiveBehaviour` 通过内部方法 `TakeInternal` 实现了对游戏对象树中每个对象的快照捕捉逻辑。此方法会为每个游戏对象添加一个 `GameObjectSnapshotBehaviour` 组件，并调用其 `Take` 方法来捕捉状态。这种递归捕捉方式确保了整个游戏对象树的状态可以被完整地保存和恢复，为开发者提供了强大的状态管理能力。

## License

[Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0)