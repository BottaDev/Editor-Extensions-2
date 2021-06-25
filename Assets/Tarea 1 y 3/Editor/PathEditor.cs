using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIPath))]
public class PathEditor : Editor
{
    private AIPath _target;
    
    private void OnEnable()
    {
        _target = (AIPath)target;
    }

    public override void OnInspectorGUI()
    {
        var serializedObject = new SerializedObject(target);
        var property = serializedObject.FindProperty("pathPositions");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Create Node", GUILayout.Height(30)))
            _target.CreateNode();
        
        if (GUILayout.Button("Destroy all Nodes", GUILayout.Height(20)))
            _target.DestroyNodes();
    }

    private void OnSceneGUI()
    {
        DrawLines();
    }
    
    private void DrawLines()
    {
        if (_target.pathPositions.Count <= 0)
            return;;

        for (int i = 0; i < _target.pathPositions.Count; i++)
        {
            if (_target.pathPositions[i] == null)
                return;
        }
        
        for (int i = 0; i < _target.pathPositions.Count; i++)
        {
            if (i == 0)
                DrawPathText("Start", _target.pathPositions[i]);
            else if (i == _target.pathPositions.Count - 1)
                DrawPathText("End", _target.pathPositions[i]);
            
            _target.pathPositions[i].position = Handles.PositionHandle(_target.pathPositions[i].position, _target.pathPositions[i].rotation);
            
            Vector3 nextPost = i == _target.pathPositions.Count - 1 
                ? _target.pathPositions[0].position 
                : _target.pathPositions[i + 1].position;
            
            Handles.SphereHandleCap(i, _target.pathPositions[i].position, Quaternion.identity, 0.1f, EventType.Repaint);

            Handles.DrawDottedLine(_target.pathPositions[i].position, nextPost, 10);

            DrawPosText(_target.pathPositions[i]);
        }
    }

    private void DrawPathText(string text, Transform node)
    {
        Handles.BeginGUI();
        var p = Camera.current.WorldToScreenPoint(node.position);
        var r = new Rect(p.x - 30, Screen.height - p.y, 100, 50);
        
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        
        GUI.Label(r, text, style);
        Handles.EndGUI();
    }
    
    private void DrawPosText(Transform node)
    {
        Handles.BeginGUI();
        var p = Camera.current.WorldToScreenPoint(node.position);
        var rx = new Rect(p.x + 30, Screen.height - p.y - 20, 100, 50);
        var ry = new Rect(p.x + 80, Screen.height - p.y - 20, 100, 50);
        var rz = new Rect(p.x + 130, Screen.height - p.y - 20, 100, 50);

        GUIStyle xStyle = new GUIStyle();
        GUIStyle yStyle = new GUIStyle();
        GUIStyle zStyle = new GUIStyle();
        
        xStyle.normal.textColor = Color.red;
        xStyle.fontSize = 10;
        xStyle.fontStyle = FontStyle.Bold;
        
        yStyle.normal.textColor = Color.green;
        yStyle.fontSize = 10;
        yStyle.fontStyle = FontStyle.Bold;
        
        zStyle.normal.textColor = Color.blue;
        zStyle.fontSize = 10;
        zStyle.fontStyle = FontStyle.Bold;
        
        GUI.Label(rx, node.position.x.ToString(".000"), xStyle);
        GUI.Label(ry, node.position.y.ToString(".000"), yStyle);
        GUI.Label(rz, node.position.z.ToString(".000"), zStyle);
        Handles.EndGUI();
        //Handles.Label(transform.position + Vector3.right, transform.position.x.ToString(".000"), xStyle);
        //Handles.Label(pos, transform.position.x.ToString(".000"), xStyle);
        //Handles.Label(transform.position + Vector3.up, transform.position.y.ToString(".000"), yStyle);
        //Handles.Label(transform.position + Vector3.forward, transform.position.z.ToString(".000"), zStyle);
    }
}
