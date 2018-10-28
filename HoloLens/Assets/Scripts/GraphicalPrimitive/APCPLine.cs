using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public abstract class APCPLine : AGraphicalPrimitive
    {
        private LineRenderer lr;
        public LineRenderer LR
        {
            get
            {
                if(lr == null)
                {
                    lr = GetNewProperLineRenderer();
                    lr.transform.parent = this.transform;
                }
                return lr;
            }
        }

        public abstract LineRenderer GetNewProperLineRenderer();

        public Vector3[] points;

        public void Init(Vector3[] points)
        {
            SetPoints(points);
            GenerateCollider();
        }

        public override void ApplyColor(Color color)
        {
            if(LR != null)
            {
                LR.startColor = color;
                LR.endColor = color;
            }
        }

        public void SetWidth(float width)
        {
            LR.startWidth = width;
            LR.endWidth = width;
        }

        private void SetPoints(Vector3[] points)
        {
            this.points = points;
            LR.positionCount = points.Length;
            LR.SetPositions(points);
        }

        private void GenerateCollider()
        {
            if(points == null)
            {
                Debug.LogWarning("Tried to generate PCPLine2D-Colliders, but line is empty.");
                return;
            }

            int[] triangles = new int[(points.Length - 1) * 6 * 4 + 12]; // two triangles for every line segment makes 6 vertices for every segment

            var collider = pivot.AddComponent<MeshCollider>();

            // Generate path
            Vector3[] path1 = new Vector3[points.Length * 2];
            Vector3[] path2 = new Vector3[points.Length * 2];
            Vector3[] path3 = new Vector3[points.Length * 2];
            Vector3[] path4 = new Vector3[points.Length * 2];

            for(int i = 0; i < points.Length; i++)
            {
                path1[i] = new Vector3(points[i].x, points[i].y + .01f, points[i].z + .01f);
                path2[i] = new Vector3(points[i].x, points[i].y + .01f, points[i].z - .01f);
                path3[i] = new Vector3(points[i].x, points[i].y - .01f, points[i].z - .01f);
                path4[i] = new Vector3(points[i].x, points[i].y - .01f, points[i].z + .01f);
            }

            var path = new Vector3[points.Length * 4];
            for(int i = 0; i < points.Length; i++)
            {
                path[i + points.Length * 0] = path1[i];
                path[i + points.Length * 1] = path2[i];
                path[i + points.Length * 2] = path3[i];
                path[i + points.Length * 3] = path4[i];
            }

            var mesh = new Mesh();

            mesh.vertices = path;

            int length = points.Length;
            int trianglesPerSide = (length - 1) * 2;
            int start;


            int triangleID = 0;


            for(int i = 0; i < points.Length - 1; i++)
            {
                // STRIP 1
                // Triangle 1
                start = triangleID * 3;
                triangles[start] = i;
                triangles[start + 1] = i + 1;
                triangles[start + 2] = length + i;

                // Triangle 2
                start = (triangleID + 1) * 3;
                triangles[start] = length + i;
                triangles[start + 1] = i + 1;
                triangles[start + 2] = length + i + 1;

                // STRIP 2
                start = trianglesPerSide * 3 * 1 + triangleID * 3;
                triangles[start] = length + i;
                triangles[start + 1] = length + i + 1;
                triangles[start + 2] = length * 2 + i;

                start = trianglesPerSide * 3 * 1 + (triangleID + 1) * 3;
                triangles[start] = length * 2 + i;
                triangles[start + 1] = length + i + 1;
                triangles[start + 2] = length * 2 + i + 1;

                // STRIP 3
                start = trianglesPerSide * 3 * 2 + triangleID * 3;
                triangles[start] = length * 2 + i;
                triangles[start + 1] = length * 2 + i + 1;
                triangles[start + 2] = length * 3 + i;

                start = trianglesPerSide * 3 * 2 + (triangleID + 1) * 3;
                triangles[start] = length * 3 + i;
                triangles[start + 1] = length * 2 + i + 1;
                triangles[start + 2] = length * 3 + i + 1;

                // STRIP 4
                start = trianglesPerSide * 3 * 3 + triangleID * 3;
                triangles[start] = length * 3 + i;
                triangles[start + 1] = length * 3 + i + 1;
                triangles[start + 2] = i;

                start = trianglesPerSide * 3 * 3 + (triangleID + 1) * 3;
                triangles[start] = i;
                triangles[start + 1] = length * 3 + i + 1;
                triangles[start + 2] = i + 1;

                triangleID += 2;
            }

            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            collider.sharedMesh = mesh;
        }
    }
}
