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
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace VisBridges
{
    public class VisBridgeBranch : MonoBehaviour, IPrimitiveObserver, IObservable<VisBridgeBranch>
    {
        [SerializeField]
        public Material material;

        public AGraphicalPrimitive origin { get; private set; }
        public AGraphicalPrimitive target { get; private set; }

        private bool initialized = false;
        private bool paused = false;
        public Color color { get; private set; } = Color.green;
        public InfoObject ID { get; private set; }

        private CurvedLinePoint[] curvedLinePoints;
        

        void Awake()
        {
            CurvedLineRenderer clr = gameObject.AddComponent<CurvedLineRenderer>();
            clr.lineWidth = 0.01f;
            clr.lineSegmentSize = 0.01f;
            clr.showGizmos = false;

            LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            lr.startWidth = 0.01f;
            lr.endWidth = 0.01f;

            curvedLinePoints = new CurvedLinePoint[4];

            initialized = false;
        }
        

        private void UpdatePorts()
        {
            curvedLinePoints[0].transform.position = origin.visBridgePort.transform.position;
            curvedLinePoints[3].transform.position = target.visBridgePort.transform.position;
        }

        private void UpdatePadding()
        {
            var optPad = GetOptimalPaddingPosition(target.visBridgePort, origin.GetComponent<AGraphicalPrimitive>());
            curvedLinePoints[1].transform.position = optPad;

            optPad = GetOptimalPaddingPosition(origin.visBridgePort, target.GetComponent<AGraphicalPrimitive>());
            curvedLinePoints[2].transform.position = optPad;
        }

        public void Init(AGraphicalPrimitive origin, AGraphicalPrimitive target, Color color, InfoObject infO)
        {
            this.ID = infO;
            Observe(origin);
            Observe(target);

            this.origin = origin;
            this.target = target;
            this.color = color;

            var lr = gameObject.GetComponent<LineRenderer>();
            lr.startColor = color;
            lr.endColor = color;
            lr.material = material;
            lr.material.color = color;

            curvedLinePoints[0] = AddCurvedLinePoint(origin.visBridgePort.transform.position);
            curvedLinePoints[1] = AddCurvedLinePoint(new Vector3());
            curvedLinePoints[2] = AddCurvedLinePoint(new Vector3());
            curvedLinePoints[3] = AddCurvedLinePoint(target.visBridgePort.transform.position);

            initialized = true;
        }

        private CurvedLinePoint AddCurvedLinePoint(Vector3 point)
        {
            var clp = new GameObject("Line Point");
            var clpComp = clp.AddComponent<CurvedLinePoint>();
            clp.transform.position = point;
            clp.transform.parent = gameObject.transform;

            return clpComp;
        }

        public static Vector3 GetOptimalPaddingPosition(GameObject visBridgeAnchor, AGraphicalPrimitive primitive)
        {
            // Set the first padding object as default
            Vector3 optimum = primitive.visBridgePortPadding[0].transform.position;
            int optimalPort = 0;
            float oldDistance, newDistance;

            // If there are more than 1 padding objects
            if(primitive.visBridgePortPadding.Length > 1)
            {
                // Lookup all but the first padding objects
                for(int i = 1; i < primitive.visBridgePortPadding.Length; i++)
                {
                    Vector3 pad = primitive.visBridgePortPadding[i].transform.position;

                    // Look if it is nearer to the other representative object than the currently set padding
                    oldDistance = Vector3.Distance(optimum, visBridgeAnchor.transform.position);
                    newDistance = Vector3.Distance(pad, visBridgeAnchor.transform.position);
                    
                    if(newDistance < oldDistance)
                    {
                        optimalPort = i;
                        optimum = pad;
                    }
                }
            }

            float anchorDistance = Vector3.Distance(primitive.visBridgePortPadding[optimalPort].transform.position, visBridgeAnchor.transform.position);
            optimum = primitive.visBridgePortPadding[optimalPort].transform.localPosition * anchorDistance;
            optimum = primitive.visBridgePort.transform.TransformPoint(optimum);
            
            return optimum;
        }

        
        // .................................................................... IPrimitiveObserver
        public void Observe(AGraphicalPrimitive observable)
        {
            observable.Subscribe(this);
        }

        public void Ignore(AGraphicalPrimitive observable)
        {

        }

        public void OnDispose(AGraphicalPrimitive observable)
        {
            if(observable.Equals(origin))
                target.Unsubscribe(this);
            else
                origin.Unsubscribe(this);

            origin = null;
            target = null;

            Dispose();
        }

        public void OnChange(AGraphicalPrimitive observable)
        {
            if(initialized)
            {
                LineRenderer lr = gameObject.GetComponent<LineRenderer>();

                // check if one port is inactive
                if(origin != null && target != null && origin.isActiveAndEnabled && target.isActiveAndEnabled)
                {
                    paused = false;

                    lr.enabled = true;
                } else
                {
                    paused = true;
                    lr.enabled = false;
                }

                if(!paused)
                {
                    UpdatePorts();
                    UpdatePadding();
                }
            }

            foreach(var o in observers)
                o.OnChange(this);
        }

        // .................................................................... IObservable

        private IList<IObserver<VisBridgeBranch>> observers = new List<IObserver<VisBridgeBranch>>();

        public void Subscribe(IObserver<VisBridgeBranch> observer)
        {
            if(!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Unsubscribe(IObserver<VisBridgeBranch> observer)
        {
            if(observers.Contains(observer))
                observers.Remove(observer);
        }

        public void Dispose()
        {
            foreach(var o in observers)
                o.OnDispose(this);
            observers.Clear();
            Destroy(gameObject);
        }
    }
}
