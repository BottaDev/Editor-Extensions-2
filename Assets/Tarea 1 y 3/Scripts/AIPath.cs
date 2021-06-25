using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AIPath : MonoBehaviour
{
    public List<Transform> pathPositions;

    public GameObject CreateNode(bool fromNode = false)
    {
        GameObject newNode = new GameObject("P" + (pathPositions.Count + 1));
        newNode.transform.position = transform.position + transform.forward;
        PathNode pn = newNode.AddComponent<PathNode>();

        pn.Path = this;
        
        if (!fromNode)
            pathPositions.Add(newNode.transform);

        return newNode;
    }

    public void DestroyNodes()
    {
        if (pathPositions == null)
            return;
        
        if (Application.isEditor)
        {
            foreach (Transform obj in pathPositions)
            {
                DestroyImmediate(obj.gameObject);
            }
        }
        
        pathPositions.Clear();
    }
}
