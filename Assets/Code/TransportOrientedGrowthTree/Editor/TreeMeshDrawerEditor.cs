using TransportOrientedGrowthTree.Ui;
using UnityEditor;
using UnityEngine;

// ReSharper disable InvertIf

namespace Code.TransportOrientedGrowthTree.Editor
{
    [CustomEditor(typeof(TreeMeshDrawer))]
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
            if (GUILayout.Button(nameof(TreeMeshDrawer.GrowTree)))
                foreach (var t in targets)
                    ((TreeMeshDrawer) t).GrowTree();
        }

        private void DrawDrawButton()
        {
            if (GUILayout.Button(nameof(TreeMeshDrawer.Draw)))
                foreach (var t in targets)
                    ((TreeMeshDrawer) t).Draw();
        }

        private void DrawGrowAndDrawButton()
        {
            if (GUILayout.Button($"{nameof(TreeMeshDrawer.GrowTree)} and {nameof(TreeMeshDrawer.Draw)}"))
                foreach (var t in targets)
                {
                    ((TreeMeshDrawer) t).GrowTree();
                    ((TreeMeshDrawer) t).Draw();
                }
        }
        
        private void DrawSimplifyDrawButton()
        {
            if (GUILayout.Button($"{nameof(TreeMeshDrawer.SimplifyMesh)}"))
                foreach (var t in targets)
                {
                    ((TreeMeshDrawer) t).SimplifyMesh(_cellsCount, _targetTrianglesCount);
                }

            _targetTrianglesCount = EditorGUILayout.IntField("target vertices", _targetTrianglesCount);
            _cellsCount = EditorGUILayout.IntField("Cells Num", _cellsCount);
        }
    }
}