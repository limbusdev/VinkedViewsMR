/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
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
                if(lr == null && gameObject != null && transform != null)
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
            transform.hasChanged = true;
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

                if(collider != null)
                {
                    var primCollider = collider.GetComponent<BoxCollider>();
                    var segmentLength = Vector3.Magnitude(segmentEnd - segmentStart);

                    primCollider.size = new Vector3(.02f, .02f, segmentLength);

                    // Move Collider to correct position
                    collider.transform.localPosition = (segmentStart + segmentEnd) / 2f;

                    // Rotate collider correctly
                    collider.transform.LookAt(segmentEnd);
                }
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
