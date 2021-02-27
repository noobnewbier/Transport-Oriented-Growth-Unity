using TransportOrientedGrowthTree.Ui;
using UnityEditor;
using UnityEngine;

// ReSharper disable InvertIf

namespace Code.TransportOrientedGrowthTree.Editor
{
    [CustomEditor(typeof(TogTreeDrawer))]
    [CanEditMultipleObjects]
    public class TreeMeshDrawerEditor : UnityEditor.Editor
    {
        private static int _targetTrianglesCount;
        private static int _cellsCount;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawGrowButton();
            DrawDrawButton();
            DrawGrowAndDrawButton();
            DrawSimplifyDrawButton();
        }

        private void DrawGrowButton()
        {
            if (GUILayout.Button(nameof(TogTreeDrawer.GrowTree)))
                foreach (var t in targets)
                    ((TogTreeDrawer) t).GrowTree();
        }

        private void DrawDrawButton()
        {
            if (GUILayout.Button(nameof(TogTreeDrawer.Draw)))
                foreach (var t in targets)
                    ((TogTreeDrawer) t).Draw();
        }

        private void DrawGrowAndDrawButton()
        {
            if (GUILayout.Button($"{nameof(TogTreeDrawer.GrowTree)} and {nameof(TogTreeDrawer.Draw)}"))
                foreach (var t in targets)
                {
                    ((TogTreeDrawer) t).GrowTree();
                    ((TogTreeDrawer) t).Draw();
                }
        }
        
        private void DrawSimplifyDrawButton()
        {
            if (GUILayout.Button($"{nameof(TogTreeDrawer.SimplifyMesh)}"))
                foreach (var t in targets)
                {
                    ((TogTreeDrawer) t).SimplifyMesh(_cellsCount, _targetTrianglesCount);
                }

            _targetTrianglesCount = EditorGUILayout.IntField("target vertices", _targetTrianglesCount);
            _cellsCount = EditorGUILayout.IntField("Cells Num", _cellsCount);
        }
    }
}