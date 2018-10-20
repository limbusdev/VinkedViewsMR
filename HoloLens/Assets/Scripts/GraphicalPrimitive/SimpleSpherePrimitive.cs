using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class SimpleSpherePrimitive : AGraphicalPrimitive
    {
        public MeshRenderer rend;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public override void ApplyColor(Color color)
        {
            rend.material.color = color;
        }
    }
}