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
                    lr.transform.parent = transform;
                }
                return lr;
            }
        }

        public abstract LineRenderer GetNewProperLineRenderer();

        public Vector3[] points;
        public IDictionary<string,float> Values;
        private GameObject[] colliders;

        public void Init(Vector3[] points, IDictionary<string, float> values)
        {
            Values = values;
            SetPoints(points);
            GenerateCollider();
        }

        public void UpdatePoints(Vector3[] points)
        {
            SetPoints(points);
            UpdateCollider();
        }

        protected override void ApplyColor(Color color)
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

        private void UpdateCollider()
        {
            for(int i = 0; i < points.Length - 1; i++)
            {
                // Better performance with primitive colliders
                var segmentStart = points[i];
                var segmentEnd = points[i + 1];

                // Create Collider
                var collider = colliders[i];
                var primCollider = collider.GetComponent<BoxCollider>();
                var segmentLength = Vector3.Magnitude(segmentEnd - segmentStart);

                primCollider.size = new Vector3(.02f, .02f, segmentLength);

                // Move Collider to correct position
                collider.transform.localPosition = (segmentStart + segmentEnd) / 2f;

                // Rotate collider correctly
                collider.transform.LookAt(segmentEnd);
            }
        }

        private void GenerateCollider()
        {
            // Return, when line is empty
            if(points == null)
            {
                Debug.LogWarning("Tried to generate PCPLine2D-Colliders, but line is empty.");
                return;
            }

            // create new collider, if there is none
            if(pivot == null)
            {
                Debug.Log("Stop here");
            }
            
            colliders = new GameObject[points.Length -1];
            

            for(int i = 0; i < points.Length - 1; i++)
            {
                // Create Collider
                var collider = new GameObject("LineCollider");
                colliders[i] = collider;
                collider.transform.parent = pivot.transform;
                collider.AddComponent<BoxCollider>();

                UpdateCollider();
            }
        }
    }
}
