using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(PathNode))]
public class NodeEditor : Editor
{
    private PathNode _target;

    private void OnEnable()
    {
        _target = (PathNode)target;
    }

    private void DrawLines()
    {
        if (_target.Path.pathPositions.Count <= 0)
            return;;

        for (int i = 0; i < _target.Path.pathPositions.Count; i++)
        {
            if (_target.Path.pathPositions[i] == null)
                return;
        }
        
        for (int i = 0; i < _target.Path.pathPositions.Count; i++)
        {
            Vector3 nextPost = i == _target.Path.pathPositions.Count - 1 
                ? _target.Path.pathPositions[0].position 
                : _target.Path.pathPositions[i + 1].position;
            
            Handles.SphereHandleCap(i, _target.Path.pathPositions[i].position, Quaternion.identity, 0.1f, EventType.Repaint);

            Handles.DrawDottedLine(_target.Path.pathPositions[i].position, nextPost, 10);
        }
    }
    
    private void OnSceneGUI()
    {
        DrawLines();
        
        Handles.BeginGUI();
        DrawControlPanel();
        Handles.EndGUI();
    }

    private void DrawControlPanel()
    {
        GUILayout.BeginArea(new Rect(0, 0, 200, 400));

        var rec = EditorGUILayout.BeginVertical();

        GUI.color = Color.white;
        GUI.Box(rec, GUIContent.none);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Node Manager");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Adjacent", GUILayout.Height(30)))
            _target.AddNode(true);
        if(GUILayout.Button("Add Preceding ", GUILayout.Height(30)))
            _target.AddNode(false);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Delete Node", GUILayout.Height(20)))
            _target.DeleteNode();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();


        GUILayout.EndArea();
    }
}
