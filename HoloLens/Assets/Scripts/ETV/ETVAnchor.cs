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

public class ETVAnchor : MonoBehaviour
{
    public GameObject VisAnchor;
    public GameObject Rotatable;
    public bool resetRotation = false;
    public Quaternion defaultLocalRotation = Quaternion.Euler(new Vector3(0, 0, 0));

    public void PutETVintoAnchor(GameObject ETV)
    {
        ETV.transform.localScale = new Vector3(.5f, .5f, .5f);
        ETV.transform.parent = VisAnchor.transform;
    }

    public Vector3 GetVisEulerAngles()
    {
        return Rotatable.transform.localEulerAngles;
    }
    
	// Update is called once per frame
	void Update ()
    {
		if(resetRotation)
        {
            Rotatable.transform.localRotation = Quaternion.RotateTowards(Rotatable.transform.localRotation, defaultLocalRotation, Time.deltaTime*50f);
            if(Vector3.Distance(Rotatable.transform.localRotation.eulerAngles, defaultLocalRotation.eulerAngles) < .1f)
            {
                Rotatable.transform.localRotation = defaultLocalRotation;
                resetRotation = false;
            }
        }
	}
}
