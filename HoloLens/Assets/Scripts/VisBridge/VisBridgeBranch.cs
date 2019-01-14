/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
