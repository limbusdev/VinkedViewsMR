using GraphicalPrimitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VisBridge
{
    public class ObjectBasedVisBridge : MonoBehaviour
    {
        [SerializeField]
        public Material material;

        public AGraphicalPrimitive origin { get; set; }
        public AGraphicalPrimitive target { get; set; }

        private bool initialized = false;
        private bool paused = false;

        private CurvedLinePoint[] curvedLinePoints;

        public ObjectBasedVisBridge()
        {
            
        }

        public ObjectBasedVisBridge(AGraphicalPrimitive origin, AGraphicalPrimitive target)
        {
            this.origin = origin;
            this.target = target;
        }

        void Awake()
        {
            CurvedLineRenderer clr = gameObject.AddComponent<CurvedLineRenderer>();
            clr.lineWidth = 0.01f;
            clr.lineSegmentSize = 0.01f;
            clr.showGizmos = false;

            LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            lr.material = material;
            lr.startWidth = 0.01f;
            lr.endWidth = 0.01f;

            curvedLinePoints = new CurvedLinePoint[4];

            initialized = false;
        }

        void FixedUpdate()
        {
            if(initialized)
            {
                LineRenderer lr = gameObject.GetComponent<LineRenderer>();

                // check if one port is inactive
                if(origin.isActiveAndEnabled && target.isActiveAndEnabled)
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
        }

        private void UpdatePorts()
        {
            curvedLinePoints[0].transform.position = origin.visBridgePort.transform.position;
            curvedLinePoints[3].transform.position = target.visBridgePort.transform.position;
        }

        private void UpdatePadding()
        {
            Vector3 optPad = GetOptimalPaddingPosition(target.visBridgePort, origin.GetComponent<AGraphicalPrimitive>());
            curvedLinePoints[1].transform.position = optPad;

            optPad = GetOptimalPaddingPosition(origin.visBridgePort, target.GetComponent<AGraphicalPrimitive>());
            curvedLinePoints[2].transform.position = optPad;
        }

        public void Init(AGraphicalPrimitive origin, AGraphicalPrimitive target)
        {
            this.origin = origin;
            this.target = target;

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

        public override bool Equals(object other)
        {
            if(other is ObjectBasedVisBridge)
            {
                ObjectBasedVisBridge otherBridge = (other as ObjectBasedVisBridge);
                return ((otherBridge.origin.Equals(origin) && otherBridge.target.Equals(target)) ||
                        (otherBridge.origin.Equals(target) && otherBridge.target.Equals(origin)));
            } else
            {
                return false;
            }
        }
    }
}
