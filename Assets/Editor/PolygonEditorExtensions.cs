using UnityEngine;
using System.Collections;
using UnityEditor;
    
[CustomEditor(typeof(PolygonCollider2D))]
public class PolygonEditorExtensions : Editor
{
    private Vector2[][] _paths;
    private bool[] _pathFoldouts = new bool[0];
    private bool _precisionPathEdit;
    
    public override void OnInspectorGUI()
    {
        PolygonCollider2D collider = (PolygonCollider2D)target;
    
        DrawDefaultInspector();
    
        _precisionPathEdit = EditorGUILayout.Foldout(_precisionPathEdit, "Precision Path Editing");
    
        if (_precisionPathEdit)
        {
            _paths = new Vector2[collider.pathCount][];
            for (int i = 0; i < collider.pathCount; i++)
            {
                var path = collider.GetPath(i);
                _paths[i] = path;
            }
    
            int size = EditorGUILayout.IntField("Size", _paths.Length);
            if (size != _paths.Length)
            {
                Vector2[][] newPaths = new Vector2[size][];
                for (int i = 0; i < size; i++)
                {
                    if (_paths.Length > i)
                    {
                        newPaths[i] = _paths[i];
                    }
                    else
                    {
                        newPaths[i] = new Vector2[0];
                    }
                }
                _paths = newPaths;
            }
    
            if (_pathFoldouts.Length != _paths.Length)
            {
                var newFoldouts = new bool[_paths.Length];
    
                for (int i = 0; i < newFoldouts.Length; i++)
                {
                    if (_pathFoldouts.Length > i)
                        newFoldouts[i] = _pathFoldouts[i];
                    else
                        newFoldouts[i] = false;
                }
    
                _pathFoldouts = newFoldouts;
            }
    
            EditorGUI.indentLevel++;
            for (int i = 0; i < size; i++)
            {
                _pathFoldouts[i] = EditorGUILayout.Foldout(_pathFoldouts[i], string.Concat("Path ", i));
    
                if (_pathFoldouts[i])
                {
                    var path = _paths[i];
                    int pathSize = EditorGUILayout.IntField("Size", path.Length);
                    if (pathSize != path.Length)
                    {
                        Vector2[] newPath = new Vector2[pathSize];
                        for (int j = 0; j < pathSize; j++)
                        {
                            if (path.Length > j)
                            {
                                newPath[j] = path[j];
                            }
                            else
                            {
                                newPath[j] = new Vector2();
                            }
                        }
                        _paths[i] = newPath;
                    }
    
                    for (int j = 0; j < pathSize; j++)
                    {
                        _paths[i][j] = EditorGUILayout.Vector2Field(string.Concat("Point ", j), _paths[i][j]);
                    }
                }
            }
            EditorGUI.indentLevel--;
    
            collider.pathCount = _paths.Length;
            for (int i = 0; i < _paths.Length; i++)
            {
                collider.SetPath(i, _paths[i]);
            }
        }
    }
}
