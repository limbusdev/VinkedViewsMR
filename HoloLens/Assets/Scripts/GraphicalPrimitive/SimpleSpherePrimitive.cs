﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class SimpleSpherePrimitive : AGraphicalPrimitive
    {
        public MeshRenderer rend;

        protected override void ApplyColor(Color color)
        {
            rend.material.color = color;
        }
    }
}