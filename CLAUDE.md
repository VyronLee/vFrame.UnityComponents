# vFrame.UnityComponents

Supplementary MonoBehaviour components for Unity — animation playback, audio control, culling, timed invocation, and state snapshots.

**For workspace conventions** (Unity version, multi-package structure, coding standards), see workspace-root `CLAUDE.md` and `.claude/rules/*.md`.

---

## Assemblies & Dependencies

| Assembly | References | Platform |
|----------|-------------|----------|
| `vFrame.UnityComponents` | `vFrame.Core`, `vFrame.Core.Unity` | All |
| `vFrame.Core.Unity.Editor` | `vFrame.UnityComponents` | Editor only |

**Note**: The Editor assembly name is `vFrame.Core.Unity.Editor` (not `vFrame.UnityComponents.Editor`).

**Namespace**: `vFrame.UnityComponents`

---

## Component Catalog

| Component | Purpose |
|-----------|---------|
| `AnimationPlayer` | Legacy `Animation` wrapper — name-based clip playback, cross-fade, wait-until-finished |
| `AnimatorEx` | `Animator` time-scale control (`TimeScale * Speed → Animator.speed`) |
| `AudioPlayer` | `AudioSource` wrapper — play, pause, mute, volume, completion callbacks (polls in `Update()`) |
| `MethodInvoker` | Coroutine-based delayed/looping invocation with `TimeScale` support |
| `TrailRendererEx` | `TrailRenderer` pause/resume/time-scale control |
| `CullingBehaviour` | Abstract `CullingGroup` visibility base — override `OnBecameVisible/Invisible()` |
| `ParticleCulling` | Auto `ParticleSystem.Play()`/`Stop()` + renderer toggle by visibility |
| `GameObjectSnapshotBehaviour` | Single-object state snapshot via `Take()`/`Restore()` |
| `GameObjectSnapshotRecursiveBehaviour` | Recursive snapshot over child hierarchy |
| `GameObjectSnapshotSettings` | `ScriptableObject` config — declare `SnapshotTypes` list |
| `TransformSnapshot` | Saves active state, layer, tag, local position/scale/rotation |
| `RendererEnableStateSnapshot` | Saves `Renderer.enabled` |
| `BehaviourEnableStateSnapshot` | Saves all child `Behaviour.enabled` states |
| `ParticleSystemEnableStateSnapshot` | Saves `ParticleSystem.emission.enabled` |
| `ShowOnlyAttribute` | Inspector read-only field display |

---

## Build & Test

This package has **no test assemblies**. Verify compilation after C# changes:

```powershell
# Locate .csproj under vFrame.UnityComponents/ (or build the .sln)
dotnet build "D:/Workspace/vFrame/vFrame.UnityComponents/vFrame.UnityComponents.csproj" --no-restore 2>&1
```

For Unity verification (runtime/domain reload), use the workspace Unity Skills API or open the Editor.

---

## Gotchas

- **`AnimationPlayer`** uses Unity's **legacy `Animation` component**, not `Animator`
- **`AudioPlayer`** completion callbacks rely on `Update()` polling — not event-driven
- **`CullingBehaviour`** has `[ExecuteInEditMode]`, but runtime culling mainly checks `Application.isPlaying`
- **`GameObjectSnapshotBehaviour`** and **`GameObjectSnapshotRecursiveBehaviour`** dynamically add snapshot components at runtime with `hideFlags = HideInInspector`
- **Snapshot configuration** requires a `GameObjectSnapshotSettings` asset that declares `SnapshotTypes`
- **Editor asmdef name** is `vFrame.Core.Unity.Editor` (historical naming)

---

## Code Organization

```
Assets/vFrame.UnityComponents/
├── Runtime/
│   ├── Attributes/
│   │   └── ShowOnlyAttribute.cs
│   └── Components/
│       ├── AnimationPlayer.cs
│       ├── AnimatorEx.cs
│       ├── AudioPlayer.cs
│       ├── MethodInvoker.cs
│       ├── TrailRendererEx.cs
│       ├── Culling/
│       │   ├── CullingBehaviour.cs
│       │   └── ParticleCulling.cs
│       └── Snapshots/
│           ├── GameObjectSnapshot.cs
│           ├── GameObjectSnapshotBehaviour.cs
│           ├── GameObjectSnapshotRecursiveBehaviour.cs
│           ├── GameObjectSnapshotSettings.cs
│           └── Impls/
│               ├── TransformSnapshot.cs
│               ├── RendererEnableStateSnapshot.cs
│               ├── BehaviourEnableStateSnapshot.cs
│               └── ParticleSystemEnableStateSnapshot.cs
└── Editor/
    ├── Drawers/
    │   └── ShowOnlyDrawer.cs
    └── Inspectors/
        ├── AnimationPlayerInspector.cs
        └── Snapshots/
```
