/*
Copyright 2018 Georg Eckert

(Lincensed under MIT license)

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
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
*/

using GraphicalPrimitive;
using System;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This is the main entry point for this application. It initializes the sceen
/// and sets up some example visualizations. Do all initialization stuff here.
/// </summary>
public class ARVisTools : NetworkBehaviour
{
    private bool startup = false;

    /// <summary>
    /// Main method.
    /// </summary>
    public void Startup()
    {
        //Physics.autoSimulation = false;

        switch(GlobalSettings.scenario)
        {
            case GlobalSettings.Scenario.TEST_MetaVis:
                TESTMetaVis();
                break;
            case GlobalSettings.Scenario.TEST_BostonPD:
                TESTSetupBostonPD();
                break;
            case GlobalSettings.Scenario.TEST_Playground:
                TESTPlayground();
                break;
            default:
                // Nothing;
                break;
        }
    }

    public override void OnStartServer()
    {
        if(!startup)
        {
            Startup();
            startup = true;
        }
    }

    // ........................................................................ TEST SETUPS

    private void TESTPlayground()
    {
        try
        {
            // Tryout for meta-visualizations
            var visPlant = Services.VisFactory();
            
            // Single Axes
            {
                var ye = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var po = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var vc = visPlant.GenerateVisFrom(0, "Violent crime", VisType.SingleAxis3D, false);
                var mu = visPlant.GenerateVisFrom(0, "Murder/MS.", VisType.SingleAxis3D, false);
                var rl = visPlant.GenerateVisFrom(0, "Rape (legacy)", VisType.SingleAxis3D, false);
                var rr = visPlant.GenerateVisFrom(0, "Rape (rev)", VisType.SingleAxis3D, false);
                var ro = visPlant.GenerateVisFrom(0, "Robbery", VisType.SingleAxis3D, false);
                var pc = visPlant.GenerateVisFrom(0, "Property crime", VisType.SingleAxis3D, false);
                var mr = visPlant.GenerateVisFrom(0, "Murder/MS. rate", VisType.SingleAxis3D, false);
                var rlr = visPlant.GenerateVisFrom(0, "Rape (legacy) rate", VisType.SingleAxis3D, false);
                var rrr = visPlant.GenerateVisFrom(0, "Rape (rev) rate", VisType.SingleAxis3D, false);
                var ror = visPlant.GenerateVisFrom(0, "Robbery rate", VisType.SingleAxis3D, false);
                var pcr = visPlant.GenerateVisFrom(0, "Property crime rate", VisType.SingleAxis3D, false);

                ye.transform.position = new Vector3(-5, 0, -5);
                po.transform.position = new Vector3(-5, 0, -4);
                vc.transform.position = new Vector3(-5, 0, -3);
                mu.transform.position = new Vector3(-5, 0, -2);
                rl.transform.position = new Vector3(-5, 0, -1);
                rr.transform.position = new Vector3(-5, 0, 0);
                ro.transform.position = new Vector3(-5, 0, 1);
                pc.transform.position = new Vector3(-5, 0, 2);

                mr.transform.position = new Vector3(-4, 0, -3);
                rlr.transform.position = new Vector3(-4, 0, -2);
                rrr.transform.position = new Vector3(-4, 0, -1);
                ror.transform.position = new Vector3(-4, 0, 0);
                pcr.transform.position = new Vector3(-4, 0, 2);
            }

            // Combined Axes
            // FLA: PCP-Ring
            {
                var ye = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var po = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var vc = visPlant.GenerateVisFrom(0, "Violent crime rate", VisType.SingleAxis3D, false);
                var mu = visPlant.GenerateVisFrom(0, "Murder/MS. rate", VisType.SingleAxis3D, false);
                var rl = visPlant.GenerateVisFrom(0, "Rape (legacy) rate", VisType.SingleAxis3D, false);
                var rr = visPlant.GenerateVisFrom(0, "Rape (rev) rate", VisType.SingleAxis3D, false);
                var ro = visPlant.GenerateVisFrom(0, "Property crime rate", VisType.SingleAxis3D, false);

                ye.transform.position = new Vector3(-2, 0, -1);
                po.transform.position = new Vector3(-2.7f, 0, -1);
                vc.transform.position = new Vector3(-1.3f, 0, -1);
                mu.transform.position = new Vector3(-2.3f, 0, -1.5f);
                rr.transform.position = new Vector3(-1.7f, 0, -1.5f);
                rl.transform.position = new Vector3(-2.3f, 0, -.5f);
                ro.transform.position = new Vector3(-1.7f, 0, -.5f);
            }

            // Combined Axes
            // FLA: PCP-Ring 2
            {
                var po = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var vc = visPlant.GenerateVisFrom(0, "Violent crime rate", VisType.SingleAxis3D, false);
                var mu = visPlant.GenerateVisFrom(0, "Murder/MS. rate", VisType.SingleAxis3D, false);
                var rl = visPlant.GenerateVisFrom(0, "Rape (legacy) rate", VisType.SingleAxis3D, false);
                var rr = visPlant.GenerateVisFrom(0, "Rape (rev) rate", VisType.SingleAxis3D, false);
                var ro = visPlant.GenerateVisFrom(0, "Property crime rate", VisType.SingleAxis3D, false);

                po.transform.position = new Vector3(-2.7f, 0, -4);
                vc.transform.position = new Vector3(-1.3f, 0, -4);
                mu.transform.position = new Vector3(-2.3f, 0, -4.5f);
                rr.transform.position = new Vector3(-1.7f, 0, -4.5f);
                rl.transform.position = new Vector3(-2.3f, 0, -3.5f);
                ro.transform.position = new Vector3(-1.7f, 0, -3.5f);
            }

            // Star Glyph
            {
                var ye = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var po = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var cr = visPlant.GenerateVisFrom(0, "Violent crime rate", VisType.SingleAxis3D, false);
                var mu = visPlant.GenerateVisFrom(0, "Murder/MS. rate", VisType.SingleAxis3D, false);
                var ra = visPlant.GenerateVisFrom(0, "Rape (legacy) rate", VisType.SingleAxis3D, false);
                var rr = visPlant.GenerateVisFrom(0, "Rape (rev) rate", VisType.SingleAxis3D, false);
                var ro = visPlant.GenerateVisFrom(0, "Property crime rate", VisType.SingleAxis3D, false);

                ye.transform.position = new Vector3(2,0,0);
                po.transform.position = new Vector3(2, 0, 0);
                cr.transform.position = new Vector3(2, 0, 0);
                mu.transform.position = new Vector3(2, 0, 0);
                rr.transform.position = new Vector3(2, 0, 0);
                ra.transform.position = new Vector3(2, 0, 0);
                ro.transform.position = new Vector3(2, 0, 0);

                ye.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0,0,0);
                po.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 51);
                cr.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 102);
                mu.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 153);
                rr.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 204);
                ra.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 255);
                ro.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 306);
            }

            // Accordion Star Glyph
            {
                var y = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var p = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var c = visPlant.GenerateVisFrom(0, "Violent crime rate", VisType.SingleAxis3D, false);
                var m = visPlant.GenerateVisFrom(0, "Murder/MS. rate", VisType.SingleAxis3D, false);
                var ra = visPlant.GenerateVisFrom(0, "Rape (legacy) rate", VisType.SingleAxis3D, false);
                var rr = visPlant.GenerateVisFrom(0, "Rape (rev) rate", VisType.SingleAxis3D, false);
                var ro = visPlant.GenerateVisFrom(0, "Property crime rate", VisType.SingleAxis3D, false);

                y.transform.position = new Vector3(4, 0, 0);
                p.transform.position = new Vector3(4, 0, .2f);
                c.transform.position = new Vector3(4, 0, .4f);
                m.transform.position = new Vector3(4, 0, .6f);
                rr.transform.position = new Vector3(4, 0, .8f);
                ra.transform.position = new Vector3(4, 0, 1f);
                ro.transform.position = new Vector3(4, 0, 1.2f);

                y.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
                p.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 51);
                c.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 102);
                m.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 153);
                rr.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 204);
                ra.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 255);
                ro.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 306);
            }

            // Star Ring
            {
                var y = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var p = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var c = visPlant.GenerateVisFrom(0, "Violent crime rate", VisType.SingleAxis3D, false);
                var m = visPlant.GenerateVisFrom(0, "Murder/MS. rate", VisType.SingleAxis3D, false);
                var ra = visPlant.GenerateVisFrom(0, "Rape (legacy) rate", VisType.SingleAxis3D, false);
                var rr = visPlant.GenerateVisFrom(0, "Rape (rev) rate", VisType.SingleAxis3D, false);
                var ro = visPlant.GenerateVisFrom(0, "Property crime rate", VisType.SingleAxis3D, false);

                y.transform.position = new Vector3(6, .3f, 0);
                p.transform.position = new Vector3(6 -.235f, 0.187f,     0);
                c.transform.position = new Vector3(6 -.292f, -0.067f,    0);
                m.transform.position = new Vector3(6 -.130f, -0.270f,    0);
                rr.transform.position = new Vector3(6 + 0.130f, -0.270f, 0);
                ra.transform.position = new Vector3(6 + 0.292f, -0.067f, 0);
                ro.transform.position = new Vector3(6 + 0.235f, 0.187f, 0);

                y.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
                p.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 51);
                c.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 102);
                m.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 153);
                rr.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 204);
                ra.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 255);
                ro.GetComponent<ETVAnchor>().Rotatable.transform.rotation = Quaternion.Euler(0, 0, 306);
            }

            // Complete Visualizations
            {
                var w = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var c = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = w.GetComponent<ETVAnchor>();
                w.transform.position = new Vector3(-.1f, 0, -2.9f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(75, 0, 0));

                etv = c.GetComponent<ETVAnchor>();
                c.transform.position = new Vector3(.1f, 0, -3.1f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -92);

                var d = visPlant.GenerateVisFrom(1, "District", VisType.SingleAxis3D, false);
                etv = d.GetComponent<ETVAnchor>();
                etv.transform.position = new Vector3(-.6f, 0, -2.9f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(90, 0, 0);

                var i = visPlant.GenerateVisFrom(1, "Inside/Outside", VisType.SingleAxis3D, false);
                etv = i.GetComponent<ETVAnchor>();
                etv.transform.position = new Vector3(-.7f, 0, -3f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 90);

            }

            {
                var w = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                w.transform.position = new Vector3(0,.5f,3.5f);

                var c = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);
                c.transform.position = new Vector3(7, 0, 2);
            }

            // Single Axes
            {
                var c = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);
                var i = visPlant.GenerateVisFrom(1, "Inside/Outside", VisType.SingleAxis3D, false);
                var w = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var d = visPlant.GenerateVisFrom(1, "District", VisType.SingleAxis3D, false);
                var p = visPlant.GenerateVisFrom(1, "Premise", VisType.SingleAxis3D, false);

                c.transform.position = new Vector3(8, 0, -5);
                i.transform.position = new Vector3(8, 0, -4);
                w.transform.position = new Vector3(8, 0, -3);
                d.transform.position = new Vector3(8, 0, -2);
                p.transform.position = new Vector3(8, 0, -1);
            }


        } catch(Exception e)
        {
            Debug.LogException(e);
        }
    }


    private void TESTSetupBostonPD()
    {
        try
        {
            var factory = Services.VisFactory();

            var etvMan = Services.instance.etvManager;

            var ypPCP = factory.GenerateVisFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (legacy)" }, VisType.PCP2D, false);
            etvMan.AutoPlaceETV(ypPCP);

            var ypvcPCP3D = factory.GenerateVisFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (legacy)" }, VisType.PCP3D, false);
            etvMan.AutoPlaceETV(ypvcPCP3D);

            var ymuSP = factory.GenerateVisFrom(0, new string[] { "Year", "Murder/MS." }, VisType.ScatterPlot2D, false);
            etvMan.AutoPlaceETV(ymuSP);

            var yemuvcSP3D = factory.GenerateVisFrom(0, new string[] { "Year", "Murder/MS.", "Violent crime" }, VisType.ScatterPlot3D, false);
            etvMan.AutoPlaceETV(yemuvcSP3D);

            var yerlLC = factory.GenerateVisFrom(0, new string[] { "Year", "Rape (legacy)" }, VisType.LineChart2D, false);
            etvMan.AutoPlaceETV(yerlLC);

            var yemuLC = factory.GenerateVisFrom(0, new string[] { "Year", "Murder/MS." }, VisType.LineChart2D, false);
            etvMan.AutoPlaceETV(yemuLC);

            var muSA = factory.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
            muSA.transform.position = new Vector3(1, 0, -3);

            var poSA = factory.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
            poSA.transform.position = new Vector3(1.5f, 0, -3);

        }
        catch(Exception e)
        {
            Debug.LogError("TESTSetupFBI failed.");
            Debug.LogException(e);
        }
    }

    private void TESTMetaVis()
    {
        try
        {
            // Tryout for meta-visualizations
            var visPlant = Services.VisFactory();


            ///////////////////////////////////////////////////////////////////
            // DYNAMIC

            // One single axis static, the other moving left and right
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(3, 0, 0);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(3, 0, 0);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.gameObject.AddComponent<Animation.LinearLeftRight>();
            }

            // Two single axes static, the thirde moving left and right between them
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var etvAxisVC = visPlant.GenerateVisFrom(0, "Violent crime", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(5.2f, 0, 0);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = etvAxisVC.GetComponent<ETVAnchor>();
                etvAxisVC.transform.position = new Vector3(6.8f, 0, 0);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(6, 0, 0);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.gameObject.AddComponent<Animation.LinearLeftRight>();
            }
            
            // One single axis static, the other rotating around the origin
            {
                var axisPopu = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var axisYear = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);

                var etv = axisPopu.GetComponent<ETVAnchor>();
                axisPopu.transform.position = new Vector3(3, 0, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = axisYear.GetComponent<ETVAnchor>();
                axisYear.transform.position = new Vector3(3, 0, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.Rotatable.AddComponent<Animation.Rotation>();
            }
            
            // One single axis static, the other rotating about it's origin in 0.5m distance
            {
                var axisP = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var axisY = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);

                var etv = axisP.GetComponent<ETVAnchor>();
                axisP.transform.position = new Vector3(5, 0, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = axisY.GetComponent<ETVAnchor>();
                axisY.transform.position = new Vector3(5, 0, 1.5f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.Rotatable.AddComponent<Animation.Rotation>();
            }
            
            // One single axis static, the other moving in circles around the first
            {
                var axisPopu = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var axisYear = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);

                var etv = axisPopu.GetComponent<ETVAnchor>();
                axisPopu.transform.position = new Vector3(3, 0, 2);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = axisYear.GetComponent<ETVAnchor>();
                axisYear.transform.position = new Vector3(3, 0, 2);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.gameObject.AddComponent<Animation.LinearCircle>();
            }

            // One single axis static, the other moving in circles around it and rotating
            {
                var axisPopu = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var axisYear = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);

                var etv = axisPopu.GetComponent<ETVAnchor>();
                axisPopu.transform.position = new Vector3(3, 0, 3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = axisYear.GetComponent<ETVAnchor>();
                axisYear.transform.position = new Vector3(3, 0, 3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.Rotatable.AddComponent<Animation.Rotation>();
                etv.gameObject.AddComponent<Animation.LinearCircle>();
            }

            // One single axis static, the other being in a 90° angle circulating around it
            {
                var axisPopu = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var axisYear = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);

                var etv = axisPopu.GetComponent<ETVAnchor>();
                axisPopu.transform.position = new Vector3(3, 0, 4);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = axisYear.GetComponent<ETVAnchor>();
                axisYear.transform.position = new Vector3(3, 0, 4);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);

                etv.gameObject.AddComponent<Animation.LinearCircle>();
            }

            // One axis static and pointing forward, the other one circling around and pointing right
            {
                var axisWeapon = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var axisCrime = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = axisWeapon.GetComponent<ETVAnchor>();
                axisWeapon.transform.position = new Vector3(3, 0, 5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

                etv = axisCrime.GetComponent<ETVAnchor>();
                axisCrime.transform.position = new Vector3(3, 0, 5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -90);

                etv.gameObject.AddComponent<Animation.LinearCircle>();
            }

            // Two categorical axes, one rotating around the other
            {
                var axisWeapon = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var axisCrime = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = axisWeapon.GetComponent<ETVAnchor>();
                axisWeapon.transform.position = new Vector3(3, 0, 7);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

                etv = axisCrime.GetComponent<ETVAnchor>();
                axisCrime.transform.position = new Vector3(3, 0, 7);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -90);

                etv.Rotatable.AddComponent<Animation.Rotation>();
            }

            // One single axis static, the other being in a 90° angle circulating around it
            {
                var axisPopu = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var axisYear = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);

                var etv = axisPopu.GetComponent<ETVAnchor>();
                axisPopu.transform.position = new Vector3(7, 0, 2);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                var rotation = etv.Rotatable.AddComponent<Animation.Rotation>();
                rotation.Euler = new Vector3(.5f, 0, 0);

                etv = axisYear.GetComponent<ETVAnchor>();
                axisYear.transform.position = new Vector3(7, 0, 2);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            /////////////////////////////////////////////////////////////////////
            //// FLEXIBLE LINKED AXES

            // FLA: perfect
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-3, 0, -5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-2.5f, 0, -5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            // FLA: tilted
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-3, 0, -4);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 20, 20));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-2.5f, 0, -4);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, -20, -20);
            }

            // FLA: nearly Scatterplot
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-3, 0, -3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -85));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-3f, 0, -3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -5);
            }

            // FLA: PCP-Ring
            {
                var etvYear = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvPopulation = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);
                var etvViolentCrime = visPlant.GenerateVisFrom(0, "Violent crime", VisType.SingleAxis3D, false);
                var etvMurder = visPlant.GenerateVisFrom(0, "Murder/MS.", VisType.SingleAxis3D, false);
                var etvRapeLegacy = visPlant.GenerateVisFrom(0, "Rape (legacy)", VisType.SingleAxis3D, false);
                var etvRapeRev = visPlant.GenerateVisFrom(0, "Rape (rev)", VisType.SingleAxis3D, false);
                var etvRobbery = visPlant.GenerateVisFrom(0, "Robbery", VisType.SingleAxis3D, false);

                etvYear.transform.position = new Vector3(-2, 0, -1);
                etvPopulation.transform.position = new Vector3(-2.7f, 0, -1);
                etvViolentCrime.transform.position = new Vector3(-1.3f, 0, -1);
                etvMurder.transform.position = new Vector3(-2.3f, 0, -1.5f);
                etvRapeRev.transform.position = new Vector3(-1.7f, 0, -1.5f);
                etvRapeLegacy.transform.position = new Vector3(-2.3f, 0, -.5f);
                etvRobbery.transform.position = new Vector3(-1.7f, 0, -.5f);
            }


            /////////////////////////////////////////////////////////////////////
            //// SCATTERPLOT


            // Scatterplot: perfect
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-5, 0, -5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5, 0, -5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            // Scatterplot: perfect flat
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-5, 0, 3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5, 0, 3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(90, 0, 0);
            }


            // Scatterplot: tilted
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-5, 0, -3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5, 0, -3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(20, 0, 0);
            }

            // Scatterplot: axes set apart
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-4.75f, 0, -1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5, 0, -1.25f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            // Scatterplot: axes set apart 2
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-4.75f, 0, 0);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5, 0, 0f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            // Scatterplot: axes set apart 3
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-4.9f, -.1f, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5.1f, .1f, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }


            // Scatterplot: imperfect
            {
                var etvAxisYear0 = visPlant.GenerateVisFrom(0, "Year", VisType.SingleAxis3D, false);
                var etvAxisPopulation0 = visPlant.GenerateVisFrom(0, "Population", VisType.SingleAxis3D, false);

                var etv = etvAxisYear0.GetComponent<ETVAnchor>();
                etvAxisYear0.transform.position = new Vector3(-5, 0, 2);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -87));

                etv = etvAxisPopulation0.GetComponent<ETVAnchor>();
                etvAxisPopulation0.transform.position = new Vector3(-5, 0, 2);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            /////////////////////////////////////////////////////////////////////
            //// BAR MAP

            // Bar Map: perfect
            {
                var etvAxisWeapon0 = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var etvAxisCrime0 = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = etvAxisWeapon0.GetComponent<ETVAnchor>();
                etvAxisWeapon0.transform.position = new Vector3(-7, 0, -5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

                etv = etvAxisCrime0.GetComponent<ETVAnchor>();
                etvAxisCrime0.transform.position = new Vector3(-7, 0, -5);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -90);
            }

            // Bar Map: tilted
            {
                var etvAxisWeapon0 = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var etvAxisCrime0 = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = etvAxisWeapon0.GetComponent<ETVAnchor>();
                etvAxisWeapon0.transform.position = new Vector3(-7, 0, -3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

                etv = etvAxisCrime0.GetComponent<ETVAnchor>();
                etvAxisCrime0.transform.position = new Vector3(-7, 0, -3);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -60);
            }

            // Bar Map: tilted 2
            {
                var etvAxisWeapon0 = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var etvAxisCrime0 = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = etvAxisWeapon0.GetComponent<ETVAnchor>();
                etvAxisWeapon0.transform.position = new Vector3(-7, 0, -1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(90, 20, 0));

                etv = etvAxisCrime0.GetComponent<ETVAnchor>();
                etvAxisCrime0.transform.position = new Vector3(-7, 0, -1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 20, -60);
            }

            // Bar Map: imperfect
            {
                var etvAxisWeapon0 = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var etvAxisCrime0 = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = etvAxisWeapon0.GetComponent<ETVAnchor>();
                etvAxisWeapon0.transform.position = new Vector3(-7, 0, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(93, 0, 0));

                etv = etvAxisCrime0.GetComponent<ETVAnchor>();
                etvAxisCrime0.transform.position = new Vector3(-7, 0, 1);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -60);
            }

            // Bar Map: set apart
            {
                var etvAxisWeapon0 = visPlant.GenerateVisFrom(1, "Weapon", VisType.SingleAxis3D, false);
                var etvAxisCrime0 = visPlant.GenerateVisFrom(1, "Crime", VisType.SingleAxis3D, false);

                var etv = etvAxisWeapon0.GetComponent<ETVAnchor>();
                etvAxisWeapon0.transform.position = new Vector3(-7.1f, 0, 3.1f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

                etv = etvAxisCrime0.GetComponent<ETVAnchor>();
                etvAxisCrime0.transform.position = new Vector3(-6.9f, 0, 2.9f);
                etv.Rotatable.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        } catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void TESTPlaceAxes()
    {
        var dataProvider = Services.DataBase();

        var etvMan = Services.instance.etvManager;
        var fact2 = Services.instance.Factory2DETV;
        var prim2Dfactory = Services.instance.Factory2DPrimitives;

        var a1 = prim2Dfactory.CreateAutoTickedAxis("Year", AxisDirection.X, dataProvider.dataSets[0]);
        var ae1 = fact2.PutETVOnAnchor(a1);

        etvMan.AutoPlaceETV(ae1);

        var a2 = prim2Dfactory.CreateAutoTickedAxis("Population", AxisDirection.X, dataProvider.dataSets[0]);
        var ae2 = fact2.PutETVOnAnchor(a2);

        etvMan.AutoPlaceETV(ae2);

        var a3 = prim2Dfactory.CreateAutoTickedAxis("Crime", AxisDirection.X, dataProvider.dataSets[1]);
        var ae3 = fact2.PutETVOnAnchor(a3);

        etvMan.AutoPlaceETV(ae3);

        var a4 = prim2Dfactory.CreateAutoTickedAxis("Weapon", AxisDirection.X, dataProvider.dataSets[1]);
        var ae4 = fact2.PutETVOnAnchor(a4);

        etvMan.AutoPlaceETV(ae4);

        var fact3 = Services.instance.Factory3DETV;
        var prim3Dfactory = Services.instance.Factory3DPrimitives;
        var a3d1 = prim3Dfactory.CreateAutoTickedAxis("Year", AxisDirection.X, dataProvider.dataSets[0]);
        var a3de1 = fact3.PutETVOnAnchor(a3d1);

        etvMan.AutoPlaceETV(a3de1);

        var a3d2 = prim3Dfactory.CreateAutoTickedAxis("Population", AxisDirection.X, dataProvider.dataSets[0]);
        var a3de2 = fact3.PutETVOnAnchor(a3d2);

        etvMan.AutoPlaceETV(a3de2);

        var a3d3 = prim3Dfactory.CreateAutoTickedAxis("Crime", AxisDirection.X, dataProvider.dataSets[1]);
        var a3de3 = fact3.PutETVOnAnchor(a3d3);

        etvMan.AutoPlaceETV(a3de3);

        var a3d4 = prim3Dfactory.CreateAutoTickedAxis("Weapon", AxisDirection.X, dataProvider.dataSets[1]);
        var a3de4 = fact3.PutETVOnAnchor(a3d4);

        etvMan.AutoPlaceETV(a3de4);


    }
}
