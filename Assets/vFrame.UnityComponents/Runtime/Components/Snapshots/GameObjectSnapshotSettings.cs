using System;
using System.Collections.Generic;
using UnityEngine;

namespace vFrame.UnityComponents
{
    public abstract class GameObjectSnapshotSettings : ScriptableObject
    {
        public abstract List<Type> SnapshotTypes { get; }
    }
}