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
