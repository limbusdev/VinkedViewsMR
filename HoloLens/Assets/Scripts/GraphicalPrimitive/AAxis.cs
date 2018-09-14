using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    public enum AxisDirection
    {
        X,Y,Z
    }

    public abstract class AAxis : MonoBehaviour
    {
        public GameObject labelVariable;
        public GameObject labelUnit;

        public float diameter {get; set;} = 0.01f;
        public float min { get; set; } = 0.0f;
        public float max { get; set; } = 1.0f;
        public float tickResolution { get; set; } = 0.1f;
        public float length { get; set; } = 1;
        public bool tipped { get; set; } = true;
        protected IList<GameObject> ticks;
        public bool ticked { get; set; } = false;
        public Color color { get; set; } = Color.white;
        public int decimals { get; set; } = 2;

        public AxisDirection axisDirection = AxisDirection.Y;
        public AxisDirection AxisDirection {
            get { return axisDirection; }
            set
            {
                axisDirection = value;
                switch (axisDirection)
                {
                    case AxisDirection.X: direction = Vector3.right; break;
                    case AxisDirection.Y: direction = Vector3.up; break;
                    default: direction = Vector3.forward; break;
                }
            }
        }

        public void CalculateTickResolution()
        {
            float range = Mathf.Abs(max - min);
            int tickCount = 11;
            float unroundedTickSize = range / (tickCount - 1);
            float x = Mathf.Ceil(Mathf.Log10(unroundedTickSize) - 1);
            float pow10x = Mathf.Pow(10, x);
            tickResolution = Mathf.Ceil(unroundedTickSize / pow10x) * pow10x;

            
            //float range = Mathf.Abs(max - min);
            //tickResolution = range / 10f;
        }

        protected Vector3 direction = Vector3.up;
        public string labelVariableText { get; set; }
        public string labelUnitText { get; set; }

        // Abstract Methods
        public abstract void UpdateAxis();
    }
}
