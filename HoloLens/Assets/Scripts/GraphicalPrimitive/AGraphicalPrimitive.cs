using HoloToolkit.Unity.InputModule;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class AGraphicalPrimitive : MonoBehaviour, IFocusable, IInputClickHandler
    {
        public GameObject pivot;
        public GameObject label;
        public GameObject visBridgePort;
        public GameObject[] visBridgePortPadding;
        public IList<InfoObject> representedInformationObjects = new List<InfoObject>();

        private bool highlighted = false;

        private Color primitiveColor = Color.white;

        public void AddRepresentedInformationObjects(InfoObject[] objs)
        {
            foreach(var o in objs)
            {
                if(!representedInformationObjects.Contains(o))
                   representedInformationObjects.Add(o);
            }
                
        }

        public void SetColor(Color color)
        {
            primitiveColor = color;
        }

        public void Brush(Color color)
        {
            ApplyColor(color);
        }

        public void Unbrush()
        {
            ApplyColor(primitiveColor);
        }

        public virtual void ApplyColor(Color color) { }

        public virtual void Highlight()
        {
            highlighted = true;
            Brush(Color.yellow);
        }

        public virtual void Unhighlight()
        {
            highlighted = false;
            Unbrush();
        }


        // .................................................................... IFocusable
        public void OnFocusEnter()
        {
            if(!highlighted) Highlight();
        }

        public void OnFocusExit()
        {
            Unhighlight();
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            eventData.Use();
            foreach(var o in representedInformationObjects)
            {
                ServiceLocator.instance.visualizationFactory.ToggleVisBridgesBetweenAllRepresentativeGameObjectsOf(o); 
            }
        }
    }
}
