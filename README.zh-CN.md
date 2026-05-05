# vFrame UnityComponents

[English](README.md) | [简体中文](README.zh-CN.md)

`vFrame.UnityComponents` 提供一组可直接挂载到 `GameObject` 的 `MonoBehaviour` 组件，用于补充 Unity 内置的动画、音频、剔除、定时调用与状态快照能力。

## 特性

- 提供对 `Animation` 的命名封装，使用 `AnimationPlayer` 按业务名称播放动画片段
- 提供 `AnimatorEx` 和 `TrailRendererEx`，支持时间缩放与暂停恢复控制
- 提供 `AudioPlayer`，统一处理 `AudioSource` 的播放、暂停、静音与完成回调
- 提供 `MethodInvoker`，通过协程实现延迟调用与循环调用
- 提供 `CullingBehaviour` 与 `ParticleCulling`，基于 `CullingGroup` 做视锥可见性控制
- 提供 `GameObjectSnapshotBehaviour` 与 `GameObjectSnapshotRecursiveBehaviour`，保存并恢复对象状态
- 内置 `TransformSnapshot`、`RendererEnableStateSnapshot`、`BehaviourEnableStateSnapshot`、`ParticleSystemEnableStateSnapshot`
- 包含 Editor 检视器与 `ShowOnlyAttribute`，方便在 Inspector 中查看与配置组件

## 环境要求

- Unity `2018.4` 及以上（`package.json` 声明）
- 当前工作区使用 Unity `2022.3.62f3` 验证与维护
- 运行时程序集 `vFrame.UnityComponents` 依赖：`vFrame.Core`、`vFrame.Core.Unity`
- 命名空间：`vFrame.UnityComponents`

## 安装

### 通过 UPM Git URL 安装

在项目的 `Packages/manifest.json` 中添加依赖：

```json
{
  "dependencies": {
    "com.vyronlee.vframe.unity-components": "https://github.com/VyronLee/vFrame.UnityComponents.git#1.0.1"
  }
}
```

也可以在 Unity 中通过 `Window > Package Manager > Add package from git URL...` 添加仓库地址。

### 源码方式安装

如果你在 vFrame 工作区中使用该库，请确保以下内容一并可用：

- `Assets/vFrame.UnityComponents/`
- `vFrame.Core`
- `vFrame.Core.Unity`

## 组件列表

| 组件 | 说明 |
| --- | --- |
| `AnimationPlayer` | 将自定义字符串名称映射到 `AnimationClip`，并支持播放、淡入与播放到结束 |
| `AnimatorEx` | 通过 `TimeScale` 和 `Speed` 联合控制 `Animator.speed` |
| `AudioPlayer` | 封装 `AudioSource` 的常用播放控制与播放完成回调 |
| `TrailRendererEx` | 为 `TrailRenderer` 提供暂停、恢复、清空与时间缩放 |
| `MethodInvoker` | 提供 `Invoke`、`DelayInvoke`、`LoopInvoke` 协程调度接口 |
| `CullingBehaviour` | 可继承的剔除基类，使用 `CullingGroup` 检测可见性变化 |
| `ParticleCulling` | 在可见/不可见时自动启停粒子系统与渲染器 |
| `GameObjectSnapshot` | 快照实现基类，定义 `Take()` 与 `Restore()` |
| `GameObjectSnapshotBehaviour` | 对单个 `GameObject` 按配置采集并恢复快照 |
| `GameObjectSnapshotRecursiveBehaviour` | 递归处理当前对象及其所有子对象的快照 |
| `GameObjectSnapshotSettings` | `ScriptableObject` 配置基类，声明 `SnapshotTypes` |
| `TransformSnapshot` | 保存激活状态、层级、标签以及本地位移/缩放/旋转 |
| `RendererEnableStateSnapshot` | 保存 `Renderer.enabled` |
| `BehaviourEnableStateSnapshot` | 保存子层级全部 `Behaviour.enabled` 状态 |
| `ParticleSystemEnableStateSnapshot` | 保存 `ParticleSystem.emission.enabled` |
| `ShowOnlyAttribute` | Inspector 辅助特性，用于只读显示字段 |

## 快速开始

下面的示例演示如何播放音效，并在 2 秒后执行一个回调：

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

## 用法

### 场景一：按名称播放旧版 `Animation`

`AnimationPlayer` 依赖 Unity 旧版 `Animation` 组件，并通过 `AnimationSet` 列表建立名称与片段的映射。

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

常用成员：

| 成员 | 说明 |
| --- | --- |
| `GetAnimation(string animationName)` | 获取映射后的 `AnimationClip` |
| `Play(string animationName)` | 播放指定动画，找不到返回 `false` |
| `PlayUntilFinished(string animationName)` | 播放并等待结束 |
| `CrossFade(string animationName)` | 对指定动画执行淡入切换 |
| `CrossFadeUntilFinished(string animationName)` | 淡入切换并等待结束 |
| `ForwardToEnd(string animationName)` | 直接采样到动画末尾 |

### 场景二：控制 `Animator`、拖尾和音频节奏

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

补充说明：

- `AnimatorEx.Speed` 与 `AnimatorEx.TimeScale` 相乘后写入 `Animator.speed`
- `TrailRendererEx.Clear()` 会清空当前拖尾，并在下一帧恢复时间参数
- `AudioPlayer.Play()` 与 `AudioPlayer.Play(bool loop, float volume, Action onPlayFinished = null)` 都要求 `AudioSource.clip` 已设置

### 场景三：延迟调用与循环轮询

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

当 `LoopInvoke` 的回调返回 `true` 时，循环停止；组件 `OnDisable()` 与 `OnDestroy()` 时也会自动 `Stop()`。

### 场景四：基于视锥控制粒子系统

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

`ParticleCulling` 继承自 `CullingBehaviour`，当对象进入视野时会调用 `ParticleSystem.Play()` 并启用相关 `Renderer`，离开视野时则调用 `ParticleSystem.Stop()` 并禁用 `Renderer`。

### 场景五：保存并恢复对象状态

先创建一个派生自 `GameObjectSnapshotSettings` 的配置资源，返回要采集的快照类型列表：

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

然后在组件中调用：

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

如果需要连同所有子对象一起处理，使用 `GameObjectSnapshotRecursiveBehaviour` 的 `Take()`、`Restore()` 与 `Clear()`。

## 说明与注意事项

- `AnimationPlayer` 使用的是 Unity 旧版 `Animation`，不是 `Animator`
- `AudioPlayer` 带回调的播放流程依赖 `Update()` 轮询播放结束状态
- `CullingBehaviour` 标记了 `[ExecuteInEditMode]`，但运行时剔除逻辑以 `Application.isPlaying` 为主
- `GameObjectSnapshotBehaviour` 与 `GameObjectSnapshotRecursiveBehaviour` 会在运行时动态添加快照组件，并将其 `hideFlags` 设为 `HideInInspector`
- 快照系统的配置入口是 `GameObjectSnapshotSettings.SnapshotTypes`
- 该包同时包含 Editor 程序集，提供 Inspector 与 Drawer 支持；运行时代码位于 `Assets/vFrame.UnityComponents/Runtime/`

## License

本项目基于 [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0) 许可协议发布。
