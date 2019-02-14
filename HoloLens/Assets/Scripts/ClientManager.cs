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
    //float pixel2cm, etvWidthCM, etvHeightCM, anchorSideLengthInCM;

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

        //pixel2cm = 2.54f/Screen.dpi;
        //var screenWidthInCm = Screen.width * pixel2cm;
        //var screenHeightInCm = Screen.height * pixel2cm;

        //anchorSideLengthInCM = padding * scale * pixel2cm;
        //etvWidthCM = (widthPix-2*padding) * scale * pixel2cm;
        //etvHeightCM = (heightPix-2*padding) * scale * pixel2cm;
    }
}
