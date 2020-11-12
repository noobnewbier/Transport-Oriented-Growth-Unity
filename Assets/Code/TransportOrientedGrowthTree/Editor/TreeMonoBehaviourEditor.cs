using TransportOrientedGrowthTree.Core;
using UnityEditor;
using UnityEngine;
// ReSharper disable InvertIf

namespace Code.TransportOrientedGrowthTree.Editor
{
    [CustomEditor(typeof(TreeMonoBehaviour)), CanEditMultipleObjects]
    public class TreeMonoBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawGrowButton();
            DrawDrawButton();
            DrawGrowAndDrawButton();
        }

        private void DrawGrowButton()
        {
            if (GUILayout.Button(nameof(TreeMonoBehaviour.Grow)))
            {
                foreach (var t in targets)
                {
                    ((TreeMonoBehaviour) t).Grow();
                }
            }
        }
        
        private void DrawDrawButton()
        {
            if (GUILayout.Button(nameof(TreeMonoBehaviour.Draw)))
            {
                foreach (var t in targets)
                {
                    ((TreeMonoBehaviour) t).Draw();
                }
            }
        }
        
        private void DrawGrowAndDrawButton()
        {
            if (GUILayout.Button($"{nameof(TreeMonoBehaviour.Grow)} and {nameof(TreeMonoBehaviour.Draw)}"))
            {
                foreach (var t in targets)
                {
                    ((TreeMonoBehaviour) t).Grow();
                    ((TreeMonoBehaviour) t).Draw();
                }
            }
        }
    }
}