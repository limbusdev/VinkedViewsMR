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

using ETV;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GraphicalPrimitive
{
    /// <summary>
    /// Abstract Axis class to represent a data dimension. AAxis is observable,
    /// which is important for meta visualizations to appear, according to
    /// their position and orientation to each other.
    /// AAxis inform their oberservers, if their position or orientation has
    /// changed.
    /// </summary>
    public abstract class AAxis : MonoBehaviour, IObservableAxis, IDisposable, IETVComponent
    {
        private AETV etv;
        
        private ISet<IObserver<AAxis>> observers = new HashSet<IObserver<AAxis>>();
        
        protected Vector3 direction = Vector3.up;
        public string labelVariableText { get; set; }

        public GameObject labelVariable;
        public string attributeName { get; private set; }

        public float diameter {get; set;} = 0.01f;
        public float min { get; set; } = 0.0f;
        public float max { get; set; } = 1.0f;
        public float tickResolution { get; set; } = 0.1f;
        public float length { get; set; } = 1;
        public bool tipped { get; set; } = true;
        protected IList<Tick> ticks;
        public bool ticked { get; set; } = false;
        public Color color { get; set; } = Color.white;
        public int decimals { get; set; } = 2;
        protected float endOffset = 0;

        public AxisDirection axisDirection = AxisDirection.Y;

        // InformationObject Data
        public AttributeStats stats;

        public bool visible { get; private set; } = true;
        

        /// <summary>
        /// Tells observer, if axis transform has changed.
        /// </summary>
        void Update()
        {
            if(transform.hasChanged)
            {
                transform.hasChanged = false; // Otherwise it get's called forever

                // ToList() creates a copy of the list of observers and alows Unsubscription during foreach loop
                foreach(var observer in observers.ToList())
                {
                    observer.OnChange(this);
                }
            }
        }

        public void Init(string name, bool clean, AxisDirection dir = AxisDirection.Y)
        {
            if(clean)
            {
                this.attributeName = name;
                this.observers = new HashSet<IObserver<AAxis>>();
                axisDirection = dir;

                switch(dir)
                {
                    case AxisDirection.X:
                        direction = Vector3.right;
                        break;
                    case AxisDirection.Y:
                        direction = Vector3.up;
                        break;
                    default:
                        direction = Vector3.forward;
                        break;
                }

                this.tipped = false;
                this.ticked = false;

            } else
            {
                Init(name, dir);
            }
        }

        /// <summary>
        /// Generic axis initializer.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dir"></param>
        public void Init(string name, AxisDirection dir = AxisDirection.Y)
        {
            this.attributeName = name;
            this.observers = new HashSet<IObserver<AAxis>>();
            this.labelVariableText = name;
            this.ticks = new List<Tick>();
            axisDirection = dir;

            switch(dir)
            {
                case AxisDirection.X:
                    direction = Vector3.right;
                    break;
                case AxisDirection.Y:
                    direction = Vector3.up;
                    break;
                default:
                    direction = Vector3.forward;
                    break;
            }
        }
        
        /// <summary>
        /// Generic axis initializer.
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="dir"></param>
        public void Init(AttributeStats stats, AxisDirection dir = AxisDirection.Y)
        {
            Init(stats.name, dir);
            this.stats = stats;
        }
        
        /// <summary>
        /// Numerical axis initializer.
        /// </summary>
        public void Init(string name, float max, AxisDirection dir = AxisDirection.Y)
        {
            Init(name, dir);
            this.min = 0;
            this.max = max;
            this.length = 1f;
            this.tipped = true;
            this.ticked = true;
            CalculateTickResolution();
            AssembleRatioAxis();
        }

        /// <summary>
        /// Nominal axis initializer.
        /// </summary>
        public void Init(
            NominalAttributeStats stats, 
            AxisDirection dir = AxisDirection.Y, 
            bool manualLength = false, 
            float length = 1)
        {
            Init(stats as AttributeStats, dir);
            this.min = 0;
            this.max = stats.max;
            
            if(manualLength)
            {
                this.length = length;
                this.tickResolution = 1f / (stats.numberOfUniqueValues - 1);
            } else
            {
                this.length = .15f * stats.numberOfUniqueValues + .15f;
                this.tickResolution = .15f;
            }

            this.tipped = false;
            this.ticked = true;
            AssembleNominalAxis(stats, manualLength, this.tickResolution);
        }

        /// <summary>
        /// Ordinal axis initializer.
        /// </summary>
        public void Init(OrdinalAttributeStats m, AxisDirection dir = AxisDirection.Y, bool manualLength = false, float length = 1)
        {
            Init(m as AttributeStats, dir);
            this.min = m.min;
            this.max = m.max;

            if(manualLength)
            {
                this.length = length;
                this.tickResolution = 1f / (m.numberOfUniqueValues - 1);
            } else
            {
                this.length = .15f * m.numberOfUniqueValues + .15f;
                this.tickResolution = .15f;
            }

            this.tipped = true;
            this.ticked = true;
            AssembleOrdinalAxis(m, manualLength, this.tickResolution);
        }

        /// <summary>
        /// Interval axis initializer.
        /// </summary>
        /// <param name="name">name of interval scaled attribute</param>
        /// <param name="max"></param>
        /// <param name="dir"></param>
        public void Init(IntervalAttributeStats m, AxisDirection dir = AxisDirection.Y)
        {
            Init(m as AttributeStats, dir);
            this.min = m.min;
            this.max = m.max;
            this.length = 1f;
            this.tipped = true;
            this.ticked = true;
            CalculateTickResolution();
            AssembleIntervalAxis(m);
        }

        /// <summary>
        /// Rational axis initializer.
        /// </summary>
        /// <param name="name">name of rational scaled attribute</param>
        /// <param name="max"></param>
        /// <param name="dir"></param>
        public void Init(RatioAttributeStats m, AxisDirection dir = AxisDirection.Y)
        {
            Init(m as AttributeStats, dir);
            this.min = m.zeroBoundMin;
            this.max = m.zeroBoundMax;
            this.length = 1f;
            this.tipped = true;
            this.ticked = true;
            CalculateTickResolution();
            AssembleRatioAxis();
        }

        // (HELPER METHOD)
        private void AssembleNominalAxis(NominalAttributeStats m, bool manualLength = false, float tickRes = .15f)
        {
            DrawBaseAxis();
            GenerateNominalTicks(m, manualLength, tickRes);
            UpdateLabels();
        }

        // (HELPER METHOD)
        private void AssembleOrdinalAxis(OrdinalAttributeStats m, bool manualLength = false, float tickRes = .15f)
        {
            DrawBaseAxis();
            GenerateOrdinalTicks(m, manualLength, tickRes);
            UpdateLabels();
        }

        // (HELPER METHOD)
        private void AssembleIntervalAxis(IntervalAttributeStats m)
        {
            DrawBaseAxis();
            GenerateIntervalTicks(m);
            UpdateLabels();
        }

        // (HELPER METHOD)
        private void AssembleRatioAxis()
        {
            DrawBaseAxis();
            GenerateRatioTicks();
            UpdateLabels();
        }

        // Template methods
        protected abstract void DrawBaseAxis();
        protected abstract void GenerateNominalTicks(NominalAttributeStats m, bool manualTickRes = false, float tickRes = .15f);
        protected abstract void GenerateOrdinalTicks(OrdinalAttributeStats m, bool manualTickRes = false, float tickRes = .15f);
        protected abstract void GenerateIntervalTicks(IntervalAttributeStats m);
        protected abstract void GenerateRatioTicks();
        protected abstract void UpdateLabels();

        public void CalculateTickResolution()
        {
            float range = Mathf.Abs(max - min);
            int tickCount = 11;
            float unroundedTickSize = range / (tickCount - 1);
            float x = Mathf.Ceil(Mathf.Log10(unroundedTickSize) - 1);
            float pow10x = Mathf.Pow(10, x);
            tickResolution = Mathf.Ceil(unroundedTickSize / pow10x) * pow10x;
        }

        public Vector3 DefineTickDirection(AxisDirection dir)
        {
            Vector3 tickDirection;
            switch(axisDirection)
            {
                case AxisDirection.Y:
                    tickDirection = Vector3.left;
                    break;
                case AxisDirection.Z:
                    tickDirection = Vector3.left;
                    break;
                default: /* case X */
                    tickDirection = Vector3.down;
                    break;
            }
            return tickDirection;
        }

        public Vector3 GetAxisBaseGlobal()
        {
            return gameObject.transform.position;
        }

        public Vector3 GetAxisTipGlobal()
        {
            return transform.TransformPoint(direction);
        }

        public Vector3 GetAxisDirectionGlobal()
        {
            return transform.TransformDirection(direction);
        }

        public Vector3 GetAxisBaseLocal()
        {
            return Vector3.zero;
        }

        public Vector3 GetAxisTipLocal()
        {
            return direction;
        }

        /// <summary>
        /// Returns a three dimensional position vector of the axis position, which represents
        /// the given attribute value.
        /// </summary>
        /// <param name="value">attribute value to locate on the axis in global space</param>
        /// <returns>global 3D position vector of axis position, representing the given attribute value</returns>
        public Vector3 GetLocalValueInGlobalSpace(float value)
        {
            return transform.TransformPoint(direction * TransformToAxisSpace(value));
        }

        public void SetVisibility(bool visible)
        {
            // Only do work, if neccessary
            if(this.visible == visible) return;

            this.visible = visible;

            foreach(var mr in GetComponents<MeshRenderer>())
            {
                mr.enabled = visible;
            }
            foreach(var mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = visible;
            }
            foreach(var mr in GetComponents<LineRenderer>())
            {
                mr.enabled = visible;
            }
            foreach(var mr in GetComponentsInChildren<LineRenderer>())
            {
                mr.enabled = visible;
            }
        }


        // Abstract Methods
        public abstract void UpdateAxis();

        /// <summary>
        /// Transforms the given attribute value to it's 1 dimensional position on the axis.
        /// </summary>
        /// <param name="value">attribute value to map to the axis</param>
        /// <returns>position on the axis</returns>
        public float TransformToAxisSpace(float value)
        {
            if(max - min == 0)
                return 0f;
            return ((value - min) / (max - min)) * (length-2*endOffset) + endOffset;
        }

        /// <summary>
        /// Transforms the 1 dimensional position the the according attribute value.
        /// </summary>
        /// <param name="value">position on the axis</param>
        /// <returns>represented attribute value</returns>
        public float TransformFromAxisSpace(float value)
        {
            if(max - min == 0)
                return 0f;
            return (value / (length - 2*endOffset)) * (max - min) + min;
        }


        public void Subscribe(IObserver<AAxis> observer)
        {
            observers.Add(observer);
        }
        

        public void Dispose()
        {
            gameObject.SetActive(false);
            foreach(var obs in observers)
            {
                obs.OnDispose(this);
            }
            observers.Clear();
            Destroy(gameObject);
        }

        public void Unsubscribe(IObserver<AAxis> observer)
        {
            observers.Remove(observer);
        }

        public void Assign(AETV etv)
        {
            this.etv = etv;
        }

        public AETV Base()
        {
            return etv;
        }

        // Removes ticks and labels
        public void Clean()
        {
            foreach(var t in ticks)
            {
                t.gameObject.SetActive(false);
            }

            labelVariable.SetActive(false);
        }
        
        private void OnDisable()
        {
                foreach(var obs in observers)
                {
                    obs.OnChange(this);
                }
        }

        private void OnEnable()
        {
            foreach(var obs in observers)
            {
                obs.OnChange(this);
            }
        }
    }
}
