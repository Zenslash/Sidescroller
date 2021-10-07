using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [SerializeField] private float      distance = 10f;
    [SerializeField] private float      angle = 30;
    [SerializeField] private float      height = 1.0f;
    [SerializeField] private Color      meshColor = Color.blue;
    [SerializeField] private int        scanFrequency = 30;
    [SerializeField] private LayerMask  scanLayers;
    [SerializeField] private LayerMask  occlusionLayers;
    
    
    private List<GameObject> objectsInSight;
    /**
     * Represents our sensor
     */
    private Mesh mesh;
    /**
     * Buffer to store scan objects
     */
    private Collider[] colliders = new Collider[50];
    /**
     * Objects in buffer
     */
    private int count;
    private float scanInterval;
    private float scanTimer;

    private void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        HandleScan();
    }

    private void HandleScan()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, scanLayers,
            QueryTriggerInteraction.Collide);
        
        objectsInSight.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject go = colliders[i].gameObject;

            if (IsInSight(go))
            {
                objectsInSight.Add(go);
            }
        }
    }

    private bool IsInSight(GameObject go)
    {
        Vector3 origin = transform.position;
        Vector3 dest = go.transform.position;
        Vector3 dir = dest - origin;
        
        //Y axis check
        if (dir.y < 0 || dir.y > height)
        {
            return false;
        }

        //Angle check
        dir.y = 0;
        float deltaAngle = Vector3.Angle(dir, Vector3.forward);
        if (deltaAngle > angle)
        {
            return false;
        }
        
        //Obstacle check
        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        
        return true;
    }
    private Mesh CreateWedgeMesh()
    {
        Mesh resultMesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = 3 * numTriangles;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];
        
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        //Current vertex index
        int vert = 0;
        
        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        
        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            
            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;
            
            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
        
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
        
            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
        
            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        resultMesh.vertices = vertices;
        resultMesh.triangles = triangles;
        resultMesh.RecalculateNormals();
        
        return resultMesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
        
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (GameObject go in objectsInSight)
        {
            Gizmos.DrawSphere(go.transform.position, 0.2f);
        }
    }

    public int Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;

        foreach (GameObject go in objectsInSight)
        {
            if (go.layer == layer)
            {
                buffer[count++] = go;
            }

            if (buffer.Length == count)
            {
                break;
            }
        }

        return count;
    }
}
