using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [HideInInspector]
    public AIPath Path;

    public void AddNode(bool isAdjacent)
    {
        if (Path == null|| Path.pathPositions == null)
        {
            Debug.LogWarning("Node has no path: " + gameObject.name);
            return;
        }
        
        int currentIndex = Path.pathPositions.FindIndex(a => a == gameObject.transform);
        GameObject newNode = Path.CreateNode(true);

        if (isAdjacent)
        {
            if (currentIndex == Path.pathPositions.Count - 1)
                Path.pathPositions.Add(newNode.transform);
            else
                Path.pathPositions.Insert(currentIndex + 1, newNode.transform);
        }
        else
        {
            if (currentIndex == 0)
                Path.pathPositions.Insert(0, newNode.transform);
            else
                Path.pathPositions.Insert(currentIndex - 1, newNode.transform);   
        }
    }

    public void DeleteNode()
    {
        int currentIndex = Path.pathPositions.FindIndex(a => a == gameObject.transform);
        
        Path.pathPositions.RemoveAt(currentIndex);
        
        DestroyImmediate(gameObject);
    }
}
