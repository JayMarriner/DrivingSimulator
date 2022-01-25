using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Nodes : MonoBehaviour
{
    public Transform[] nodes;
    [SerializeField] Color gizmoColor;

    // Update is called once per frame
    void Update()
    {
        nodes = GetComponentsInChildren<Transform>();
        int counter = 0;
        foreach (Transform i in nodes)
        {
            if (i == gameObject.transform)
                i.name = "Nodes";
            else
                i.name = "Node" + counter;
            counter++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        int counter = 0;
        foreach (Transform i in nodes)
        {
            if(i != gameObject.transform)
            {
                if(counter == 1)
                    Gizmos.DrawSphere(i.position, 1f);
                else
                    Gizmos.DrawSphere(i.position, 0.5f);
            }
            counter++;
        }

        //Draw line between
        for(int i =1; i < nodes.Length; i++)
        {
            if (i == nodes.Length - 1 && nodes.Length > 1)
                Gizmos.DrawLine(nodes[i].position, nodes[1].position);
            else
                Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        }

        /*for(int i = 0; i < nodes.Length - 1; i++)
        {
            if(nodes.Length > 2)
            {
                if(i+2 < nodes.Length)
                    Handles.DrawBezier(nodes[i].position, nodes[i + 2].position, nodes[i + 1].position, nodes[i + 1].position, Color.red, null, 2f);
                else if(i == nodes.Length - 2)
                    Handles.DrawBezier(nodes[i].position, nodes[0].position, nodes[i + 1].position, nodes[i + 1].position, Color.red, null, 2f);
                else if(i == nodes.Length - 1)
                    Handles.DrawBezier(nodes[i].position, nodes[1].position, nodes[1].position, nodes[1].position, Color.red, null, 2f);
            }
        }
        */
    }

    /*private Vector3 BezierCurve(float t, Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * pos1;
        p += 2 * u * t * pos2;
        p += tt * pos3;
        return p;
    }

    void DrawQuadraticCurve()
    {
        for(int i = 0; i < nodes.Length - 1; i++)
        {
            float t = i+1/nodes.Length;
            positions[i] = BezierCurve(t, nodes[i].position, nodes[i + 1].position, nodes[i + 2].position);
        }
    }*/
}
