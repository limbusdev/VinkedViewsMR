using ETV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class PCPLineGlobal : AGraphicalPrimitive
    {
        [SerializeField]
        public LineRenderer lineRenderer;

        public override void ApplyColor(Color color)
        {
            if(lineRenderer != null)
            {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }

        public void GenerateCollider()
        {
            Vector3[] positions = new Vector3[lineRenderer.positionCount];

            lineRenderer.GetPositions(positions);
            int[] triangles = new int[(positions.Length-1)*6*4+12]; // two triangles for every line segment makes 6 vertices for every segment

            var collider = pivot.AddComponent<MeshCollider>();

            // Generate path
            Vector3[] path1 = new Vector3[positions.Length * 2];
            Vector3[] path2 = new Vector3[positions.Length * 2];
            Vector3[] path3 = new Vector3[positions.Length * 2];
            Vector3[] path4 = new Vector3[positions.Length * 2];

            for(int i=0; i<positions.Length; i++)
            {
                path1[i] = new Vector3(positions[i].x, positions[i].y + .01f, positions[i].z + .01f);
                path2[i] = new Vector3(positions[i].x, positions[i].y + .01f, positions[i].z - .01f);
                path3[i] = new Vector3(positions[i].x, positions[i].y - .01f, positions[i].z - .01f);
                path4[i] = new Vector3(positions[i].x, positions[i].y - .01f, positions[i].z + .01f);
            }

            var path = new Vector3[positions.Length*4];
            for(int i = 0; i<positions.Length; i++)
            {
                path[i + positions.Length * 0] = path1[i];
                path[i + positions.Length * 1] = path2[i];
                path[i + positions.Length * 2] = path3[i];
                path[i + positions.Length * 3] = path4[i];
            }
            
            var mesh = new Mesh();

            mesh.vertices = path;

            int length = positions.Length;
            int trianglesPerSide = (length - 1) * 2;
            int start;
            // Start Cap
            //int start = trianglesPerSide * 3 * 4;
            //triangles[start + 2] = length * 2;
            //triangles[start + 1] = length * 1;
            //triangles[start] = 0;

            //triangles[start + 3] = length * 2;
            //triangles[start + 5] = length * 3;
            //triangles[start + 4] = length * 1;

            // End Cap
            //start = trianglesPerSide * 3 * 4;
            //triangles[start] = 0;
            //triangles[start + 2] = length * 2;
            //triangles[start + 1] = length * 1;

            //triangles[start + 3] = length * 2;
            //triangles[start + 5] = length * 3;
            //triangles[start + 4] = length * 1;


            int triangleID = 0;
            

            for(int i=0; i<positions.Length-1; i++)
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
                start = trianglesPerSide*3 * 1 + triangleID * 3;
                triangles[start] = length + i;
                triangles[start + 1] = length + i + 1;
                triangles[start + 2] = length * 2 + i;

                start = trianglesPerSide*3 * 1 + (triangleID + 1) * 3;
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

        //public static int[] GenerateTriangleStrip(Vector3[] positions, float yAlteration, float zAlteration, out Vector3[] stripPositions)
        //{
        //    int[] indizes = new int[(positions.Length-1)*2*4+12];
        //    stripPositions = new Vector3[positions.Length];

        //    int triangleIndexCounter = 0;
        //    int indexCounter = 0;

        //    // Strip 1
        //    for(int i=0; i<positions.Length; i++)
        //    {
        //        stripPositions[indexCounter] = new Vector3(positions[i].x, positions[i].y + yAlteration, positions[i].z + zAlteration);
        //        indizes[triangleIndexCounter] = indexCounter;
        //        indexCounter++;
        //        triangleIndexCounter++;

        //        stripPositions[indexCounter] = new Vector3(positions[i+1].x, positions[i+1].y + yAlteration, positions[i+1].z + zAlteration);
        //        indizes[triangleIndexCounter] = indexCounter;
        //        indexCounter++;
        //        triangleIndexCounter++;

        //        stripPositions[indexCounter] = new Vector3(positions[i].x, positions[i].y + yAlteration, positions[i].z - zAlteration);
        //        indizes[triangleIndexCounter] = indexCounter;
        //        indexCounter++;
        //        triangleIndexCounter++;

        //        indizes[triangleIndexCounter] = indexCounter-1;
        //        triangleIndexCounter++;

        //        indizes[triangleIndexCounter] = indexCounter-2;
        //        triangleIndexCounter++;

        //        stripPositions[indexCounter] = new Vector3(positions[i+1].x, positions[i+1].y + yAlteration, positions[i+1].z - zAlteration);
        //        indizes[triangleIndexCounter] = indexCounter;
        //        indexCounter++;
        //        triangleIndexCounter++;
        //    }
        //    return indizes;
        //}
    }
}