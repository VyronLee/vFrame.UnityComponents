using UnityEditor;
using UnityEngine;

namespace vFrame.UnityComponents
{
    [CustomEditor(typeof(AnimationPlayer))]
    public class AnimationPlayerInspector : Editor
    {
        private SerializedProperty _animations;
        private bool _listVisibility = true;

        private void OnEnable() {
            _animations = serializedObject.FindProperty("_animations");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            var player = target as AnimationPlayer;
            if (!player) {
                return;
            }

            var countInput = EditorGUILayout.TextField("Animation Count", _animations.arraySize.ToString());
            _animations.arraySize = int.Parse(countInput);

            _listVisibility = EditorGUILayout.Foldout(_listVisibility, "Animations");
            if (_listVisibility) {
                EditorGUI.indentLevel++;
                for (var i = 0; i < _animations.arraySize; i++) {
                    var elementProperty = _animations.GetArrayElementAtIndex(i);

                    var name = elementProperty.FindPropertyRelative("name");
                    var clip = elementProperty.FindPropertyRelative("clip");

                    var prevLabelWidth = EditorGUIUtility.labelWidth;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 60f;
                    EditorGUILayout.PropertyField(name);
                    EditorGUIUtility.labelWidth = 40f;
                    EditorGUILayout.PropertyField(clip);
                    EditorGUIUtility.labelWidth = prevLabelWidth;

                    var prevEnabled = GUI.enabled;
                    GUI.enabled = Application.isPlaying;
                    if (GUILayout.Button("Play", GUILayout.Width(50f))) {
                        player.Play(name.stringValue);
                    }
                    GUI.enabled = prevEnabled;

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}