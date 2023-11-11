using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public float fovSetting = 90f;
    public int rayCountSetting = 50;
    public float viewDistanceSetting = 5f;

    public LayerMask layerMask;

    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        float fov = fovSetting;
        int rayCount = rayCountSetting;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = viewDistanceSetting;

        Vector3[] verticies = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[rayCount * 3];

        verticies[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            //Making light/mesh
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if(raycastHit2D.collider == null)
            {
                //no hit, light go max distance
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //hit object, light stop at object
                vertex = raycastHit2D.point;   
            }

            verticies[vertexIndex] = vertex;

            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex -1;
                triangles[triangleIndex + 2] = vertexIndex;
            
                triangleIndex += 3; 
            }

            vertexIndex++;
            angle -= angleIncrease; 
        }

        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }

    private static Vector3 GetVectorFromAngle(float angle)
    {
        //angle [0, 360] Interval notation
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection (Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fovSetting / 2f;
    }

    public void SetFoV(float fov)
    {
        fovSetting = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        viewDistanceSetting = viewDistance;
    }
}
