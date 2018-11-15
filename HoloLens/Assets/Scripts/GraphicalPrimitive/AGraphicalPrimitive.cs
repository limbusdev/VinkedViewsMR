using ETV;
using HoloToolkit.Unity.InputModule;
using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    /// <summary>
    /// Base class of all graphical primitives to be used in visualizations.
    /// A graphical primitive can be a dot, a sphere, a line, a bar, an axis 
    /// or another 1-, 2- or 3-dimensional element to be used to assemble
    /// visualizations from. It is sensitive to eye gaze and click input.
    /// A graphical primitive usually represents one or several information objects.
    /// In a direct relation one graphical primitive represents only one
    /// information object. In non-direct relations it can represent more.
    /// A list of represented information objects is kept.
    /// Graphical primitives have a default color, a highlighting color and a brushing color. 
    /// When focused, they are highlighted automatically. When selected,
    /// they activate a VisBridge connecting them to all other graphical
    /// primitives representing the same information object(s).
    /// </summary>
    public class AGraphicalPrimitive : MonoBehaviour, IFocusable, IInputClickHandler, IObservablePrimitive<InfoObject>, IDisposable
    {
        public enum Status { ENABLED, DISABLED }
        public enum Visibility { VISIBLE, INVISIBLE }

        // .................................................................... PROPERTIES
        private AETV etv;

        // .......................................................... game object components
        public GameObject pivot;
        public GameObject label;
        public GameObject visBridgePort;
        public GameObject[] visBridgePortPadding;
        
        // .......................................................... model components

        // .......................................................... status
        public Status status { get; protected set; }
        public Visibility visibility { get; protected set; }
        private bool highlighted = false;
        private bool brushed = false;
        private Color primitiveColor = Color.white;
        private Color brushColor = Color.green;
        private Color highlightColor = Color.yellow;

        public bool disposed { get; private set; } = false;

        private IList<IObserver<AGraphicalPrimitive>> observers = new List<IObserver<AGraphicalPrimitive>>();

        // .................................................................... METHODS

        public void Assign(AETV etv)
        {
            this.etv = etv;
        }

        public AETV Base()
        {
            return etv;
        }

        protected virtual void ApplyColor(Color color) { }
        
        public void SetColor(Color color, Color brushingColor)
        {
            brushColor = brushingColor;
            primitiveColor = color;
            ApplyColor(color);
        }

        public void Brush(Color color)
        {
            brushed = true;
            ApplyColor(brushColor);
        }

        public virtual void Unbrush()
        {
            brushed = false;

            if(highlighted)
            {
                Highlight();
            } else
            {
                ApplyColor(primitiveColor);
            }
        }
        

        public virtual void Highlight()
        {
            highlighted = true;
            ApplyColor(highlightColor);
        }

        public virtual void Unhighlight()
        {
            highlighted = false;

            if(brushed)
            {
                ApplyColor(brushColor);
            } else
            {
                ApplyColor(primitiveColor);
            }
        }


        // .................................................................... IMPLEMENTED INTERFACES
        // .......................................................... IFocusable
        public void OnFocusEnter()
        {
            if(!highlighted) Highlight();
        }

        public void OnFocusExit()
        {
            Unhighlight();
        }

        // .......................................................... IInputClickHandler
        public void OnInputClicked(InputClickedEventData eventData)
        {
            eventData.Use();
            Services.VisBridgeSys().ToggleVisBridgeFor(this); 
        }

        // .......................................................... IObservable, IDisposable
 
        public void Dispose()
        {
            gameObject.SetActive(false);
            disposed = true;

            foreach(var obs in observers)
            {
                obs.OnDispose(this);
            }
            observers.Clear();
            Destroy(gameObject);
        }

        public void Subscribe(IObserver<AGraphicalPrimitive> observer)
        {
            if(!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Unsubscribe(IObserver<AGraphicalPrimitive> observer)
        {
            observers.Remove(observer);
        }
        
    }
}
