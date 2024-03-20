using UnityEditor;
using UnityEngine;

namespace vFrame.UnityComponents
{
    [CustomEditor(typeof(GameObjectSnapshotBehaviour))]
    public class GameObjectSnapshotBehaviourInspector : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            serializedObject.Update();

            if (GUILayout.Button("Take")) {
                var snapshot = target as GameObjectSnapshotBehaviour;
                if (null != snapshot) snapshot.Take();
            }

            if (GUILayout.Button("Restore")) {
                var snapshot = target as GameObjectSnapshotBehaviour;
                if (null != snapshot) snapshot.Restore();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}