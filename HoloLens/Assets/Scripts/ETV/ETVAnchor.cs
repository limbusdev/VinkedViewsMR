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
