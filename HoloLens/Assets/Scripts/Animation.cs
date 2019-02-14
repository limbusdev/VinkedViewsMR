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
using UnityEngine;

public class Animation : MonoBehaviour
{
    public static bool LEFT = true;
    public static bool RIGHT = false;

    public class LinearCircle : MonoBehaviour
    {
        private float RotateSpeed = .3f;
        private float Radius = 0.5f;
        

        private Vector3 center;
        private float angle;

        private void Start()
        {
            center = transform.position;
        }

        private void Update()
        {
            angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * Radius;
            transform.position = center + offset;
        }
    }

    public class Rotation : MonoBehaviour
    {
        public Vector3 Euler = new Vector3(0, 0, .5f);

        private void Update()
        {
            transform.Rotate(Euler);
        }
    }

    public class LinearLeftRight : MonoBehaviour
    {
        private bool direction = false;

        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.position;
        }


        // Update is called once per frame
        void Update()
        {
            if(transform.position.x < initialPosition.x - 1.2f)
            {
                direction = RIGHT;
            }

            if(transform.position.x > initialPosition.x + 1.2f)
            {
                direction = LEFT;
            }

            var factor = 1f;

            if(direction == LEFT)
            {
                factor = -1f;
            } else
            {
                factor = 1f;
            }

            var newPos = transform.position;
            newPos.x += factor * Time.deltaTime * .1f;
            transform.position = newPos;
        }
    }

    
}
