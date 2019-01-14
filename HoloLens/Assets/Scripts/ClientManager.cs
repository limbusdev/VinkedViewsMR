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
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public GameObject etvPosition;


    private GameObject currentlyBoundETV;
    public GameObject CurrentlyBoundETV
    {
        get
        {
            return currentlyBoundETV;
        }
        set
        {
            Destroy(currentlyBoundETV);
            currentlyBoundETV = value;
            value.transform.parent = etvPosition.transform;
            value.transform.localPosition = Vector3.zero;
        }
    }

    public VisualizationFactory vf;
    public Canvas canvas;
    public CanvasScaler scaler;
    public RectTransform canvasRect;
    public RectTransform freeSpaceRect;

    public float scale, widthPix, heightPix, padding;
    public Vector2 blPix, brPix, tlPix, trPix;
    public Vector3 bl, br, tl, tr;
    float pixel2cm, etvWidthCM, etvHeightCM, anchorSideLengthInCM;

    private void Start()
    {
        Services.instance.clientManager = this;

        padding = 48;

        var etvYearPopulationCrimePCP2D = vf.GenerateVisFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (old)" }, VisType.PCP2D);
        etvYearPopulationCrimePCP2D.transform.parent = etvPosition.transform;
        Services.instance.clientManager.currentlyBoundETV = etvYearPopulationCrimePCP2D;
        
        scale = canvasRect.localScale.x;

        blPix = (new Vector2(padding, padding))*scale;
        widthPix = canvasRect.rect.width;
        heightPix = canvasRect.rect.height;
        brPix = (new Vector2(widthPix-padding, padding)) * scale;
        tlPix = (new Vector2(padding, heightPix-padding)) * scale;
        trPix = (new Vector2(widthPix-padding, heightPix-padding));

        
        bl = canvas.worldCamera.ScreenPointToRay(blPix).origin;
        br = canvas.worldCamera.ScreenPointToRay(brPix).origin;
        tl = canvas.worldCamera.ScreenPointToRay(tlPix).origin;
        tr = canvas.worldCamera.ScreenPointToRay(trPix).origin;

        bl.z = br.z = tl.z = tr.z = 0;

        etvPosition.transform.position = bl;

        pixel2cm = 2.54f/Screen.dpi;
        var screenWidthInCm = Screen.width * pixel2cm;
        var screenHeightInCm = Screen.height * pixel2cm;

        anchorSideLengthInCM = padding * scale * pixel2cm;
        etvWidthCM = (widthPix-2*padding) * scale * pixel2cm;
        etvHeightCM = (heightPix-2*padding) * scale * pixel2cm;
    }
}
