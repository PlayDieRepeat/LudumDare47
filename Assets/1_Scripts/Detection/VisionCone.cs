using System.Collections.Generic;
using UnityEngine;

namespace LD47.Detection
{
    public static class MaterialLoader
    {
        public static void AddResource(this ICollection<Material> materials, string resource)
        {
            Material material = Resources.Load(resource, typeof(Material)) as Material;
            if (material)
                materials.Add(material);
            else
                Debug.LogWarning("Material Resource '" + resource + "' could not be loaded.");
        }
    }

    public class VisionCone : MonoBehaviour
    {
        Vector3[] vertices = new Vector3[8];
        Vector3[] frustrumWorldPoints = new Vector3[8];
        Vector2[] UV = new Vector2[8];
        int[] triangles = new int[36];
        [SerializeField] [Range(-1f, 1f)] float forwardPosOffset = 0.4f;
        [SerializeField] [Range(-1f, 1f)] float bottomPosOffset = 0.6f;        
        [SerializeField] [Range(0.5f, 5f)] float viewLength = 1.75f;
        [SerializeField] [Range(0.1f, 2f)] float viewBackHeight = 0.15f;
        [SerializeField] [Range(0.1f, 2f)] float viewBackWidth = 0.15f;
        [SerializeField] [Range(0.1f, 2f)] float viewFrontHeight = 0.18f;
        [SerializeField] [Range(0.1f, 2f)] float viewFrontWidth = 5f;
        Vector3 center = Vector3.zero;
        GameObject Cone;

        void Start()
        {
            // creat "cone" of vision and set transform to the Guardog
            Cone = new GameObject("Cone");
            Cone.transform.SetParent(gameObject.transform, false);
            // add all mesh components needed
            Mesh mesh = new Mesh();
            mesh.name = "Cone";
            Cone.AddComponent<MeshFilter>();
            Cone.GetComponent<MeshFilter>().mesh = mesh;
            Cone.AddComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            List<Material> materialList = new List<Material>();
            //materialList.AddResource("emission_yellow");
            materialList.AddResource("trans_red");
            Cone.GetComponent<MeshRenderer>().materials = materialList.ToArray();
            CreateFrustrumVertices();
            CalculateTriangles();
            mesh.SetVertices(vertices);
            mesh.uv = UV;
            mesh.triangles = triangles;
            Cone.AddComponent<MeshCollider>();
            MeshCollider coneCollider = Cone.GetComponent<MeshCollider>();
            coneCollider.sharedMesh = mesh;
            coneCollider.enabled = true;
            coneCollider.convex = true;
            coneCollider.isTrigger = true;
        }

        // calculates the vertices
        void CreateFrustrumVertices()
        {
            center = transform.position;
            float halfBX = viewBackWidth / 2;
            float halfBY = viewBackHeight / 2;
            float halfFX = viewFrontWidth / 2;
            float halfFY = viewFrontHeight / 2;
            frustrumWorldPoints[0] = new Vector3(center.x + halfBX, center.y + halfBY, center.z + forwardPosOffset);
            frustrumWorldPoints[1] = new Vector3(center.x + halfBX, center.y - halfBY, center.z + forwardPosOffset);
            frustrumWorldPoints[2] = new Vector3(center.x - halfBX, center.y - halfBY, center.z + forwardPosOffset);
            frustrumWorldPoints[3] = new Vector3(center.x - halfBX, center.y + halfBY, center.z + forwardPosOffset);
            frustrumWorldPoints[4] = new Vector3(center.x - halfFX, center.y + halfFY, center.z + viewLength);
            frustrumWorldPoints[5] = new Vector3(center.x - halfFX, center.y - halfFY - bottomPosOffset, center.z + viewLength);
            frustrumWorldPoints[6] = new Vector3(center.x + halfFX, center.y - halfFY - bottomPosOffset, center.z + viewLength);
            frustrumWorldPoints[7] = new Vector3(center.x + halfFX, center.y + halfFY, center.z + viewLength);
            // v = q * (v - center) + center;
            //Quaternion rotation = Quaternion.LookRotation(gameObject.transform.forward, Vector3.up);
            for (int i = 0; i < 8; i++)
            {
                //vertices[i] = rotation * (frustrumWorldPoints[i] - transform.position) + transform.position;
                vertices[i] = Cone.transform.InverseTransformPoint(frustrumWorldPoints[i]);
            }
        }
        void CalculateTriangles()
        {
            // backplane face
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            //top
            triangles[6] = 7;
            triangles[7] = 0;
            triangles[8] = 3;
            triangles[9] = 7;
            triangles[10] = 3;
            triangles[11] = 4;
            //right
            triangles[12] = 6;
            triangles[13] = 1;
            triangles[14] = 0;
            triangles[15] = 6;
            triangles[16] = 0;
            triangles[17] = 7;
            //bottom
            triangles[18] = 5;
            triangles[19] = 2;
            triangles[20] = 1;
            triangles[21] = 5;
            triangles[22] = 1;
            triangles[23] = 6;
            //left
            triangles[24] = 4;
            triangles[25] = 3;
            triangles[26] = 2;
            triangles[27] = 4;
            triangles[28] = 2;
            triangles[29] = 5;
            //front plane            
            triangles[30] = 4;
            triangles[31] = 5;
            triangles[32] = 6;
            triangles[33] = 4;
            triangles[34] = 6;
            triangles[35] = 7;
        }
        Vector3 ConvertAngleToVector(float angle)
        {
            float radian = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }
}