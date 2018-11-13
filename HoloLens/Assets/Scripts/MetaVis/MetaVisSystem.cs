using ETV;
using GraphicalPrimitive;
using Model;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MetaVisualization
{
    public class MetaVisSystem : AMetaVisSystem, IGPObserver<AAxis>
    {
        [SerializeField]
        public DataProvider dataProvider;

        public static float triggerMetaVisDistance = 1f;
        public static float triggerInvisibleDistance = .05f;
        public static float triggerInvisibleAngleDiscrepancy = 5;

        private IDictionary<AETV, IList<AAxis>> axes = new Dictionary<AETV, IList<AAxis>>();
        private IDictionary<MetaVisKey, AETV> metaVisualizations = new Dictionary<MetaVisKey, AETV>();
        private IDictionary<MetaVisKey, AETV> Original2metaAxis = new Dictionary<MetaVisKey, AETV>();   // Maps original axes to their counterpart in the meta visualization


        // Use this for initialization
        void Start()
        {

        }



        // Update is called once per frame
        void Update()
        {
            var newMetaVis = new Dictionary<MetaVisKey,AETV>();
            var metaVisToBeRemoved = new Dictionary<MetaVisKey, AETV>();

            foreach(var etvA in axes.Keys)
            {
                foreach(var axisA in axes[etvA])
                {
                    foreach(var etvB in axes.Keys)
                    {
                        foreach(var axisB in axes[etvB])
                        {
                            var key = new MetaVisKey(axisA, axisB);
                            // Check if there isn't a MetaVis with both axes already

                            // ................................................ CREATE NEW METAVIS
                            if(!axisA.attributeName.Equals(axisB.attributeName) && !metaVisualizations.ContainsKey(key))
                            {
                                // If they're not identical and not representing the same data dimension
                                // and are near enough to each other
                                if(CheckIfNearEnough(axisA, axisB))
                                {
                                    int dataSetID;
                                    if(CheckIfCompatible(etvA, etvB, axisA, axisB, out dataSetID))
                                    {
                                        // span a new metavisualization between them
                                        if(!newMetaVis.ContainsKey(new MetaVisKey(axisA, axisB)))
                                        {
                                            var newVis = SpanMetaVisBetween(axisA, axisB, dataSetID);
                                            newMetaVis.Add(new MetaVisKey(axisA, axisB), newVis);
                                            try
                                            {
                                                if(newVis.registeredAxes.ContainsKey(axisA.attributeName))
                                                {
                                                    Original2metaAxis.Add(new MetaVisKey(axisA, newVis.registeredAxes[axisA.attributeName]), newVis);
                                                }
                                                if(newVis.registeredAxes.ContainsKey(axisB.attributeName))
                                                {
                                                    Original2metaAxis.Add(new MetaVisKey(axisB, newVis.registeredAxes[axisB.attributeName]), newVis);
                                                }
                                                
                                            } catch(Exception e)
                                            {
                                                Debug.LogException(e);
                                            }
                                        }
                                    }
                                } 
                            } 
                            // ................................................ UPDATE EXISTING METAVIS
                            else
                            {
                                // If there is a metavis and it is too far stretched
                                if(!CheckIfNearEnough(axisA, axisB))
                                {
                                    // Check if there is a MetaVis already,
                                    // and if so - remove it.
                                    
                                    if(metaVisualizations.ContainsKey(key) && !metaVisToBeRemoved.ContainsKey(key))
                                    {
                                        metaVisToBeRemoved.Add(key, metaVisualizations[key]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Hide very close axes of meta-visualizations
            foreach(var axisPair in Original2metaAxis.Keys)
            {
                try
                {
                    var originalAxis = axisPair.axisA;
                    var metaAxis = axisPair.axisB;
                    var showMetaAxis = !CheckIfNearEnoughToHideAxis(originalAxis, metaAxis);

                    var metaVis = Original2metaAxis[axisPair];

                    metaAxis.SetVisibility(showMetaAxis);

                    var distProjOnOrigBase = ProjectedDistanceToAxis(metaAxis.GetAxisBaseGlobal(), originalAxis);
                    var distProjOnOrigTip = ProjectedDistanceToAxis(metaAxis.GetAxisTipGlobal(), originalAxis);

                    // if one axis is parallel it's metavis axis and the other is not,
                    // stick to the parallel one
                    if(distProjOnOrigBase < .01f && distProjOnOrigTip < .01f)
                    {
                        metaVis.transform.position = originalAxis.GetAxisBaseGlobal();
                    }


                } catch(Exception e)
                {
                    Debug.LogError("Checking vicinity of original and meta axis failed, because of exception.");
                    Debug.LogException(e);
                }
            }

            // Apply made changes
            foreach(var key in metaVisToBeRemoved.Keys)
            {
                var metaVis = metaVisualizations[key];
                metaVis.Dispose();
                Destroy(metaVis);
                metaVisualizations.Remove(key);
            }

            foreach(var key in newMetaVis.Keys)
            {
                metaVisualizations.Add(key, newMetaVis[key]);
            } 
        }

        public static float ProjectedDistanceToAxis(Vector3 point, AAxis axis)
        {
            Ray ray = new Ray(axis.GetAxisBaseGlobal(), (axis.GetAxisTipGlobal() - axis.GetAxisBaseGlobal()));
            return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
        }
        

        public override void ObserveAxis(AETV etv, AAxis axis)
        {
            if(etv.isMetaVis)
            {
                return;
            } else
            {
                if(!axes.ContainsKey(etv))
                {
                    axes.Add(etv, new List<AAxis>());
                }
                axes[etv].Add(axis);
                axis.Subscribe(this);
            }
        }


        public override void StopObservationOf(AETV etv, AAxis axis)
        {
            if(!axes.ContainsKey(etv))
                return;

            axes[etv].Remove(axis);
        }


        public override AETV SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID)
        {
            // calculate distance between their origins
            var originDist = Vector3.Distance(axisA.GetAxisBaseGlobal(), axisB.GetAxisBaseGlobal());
            // use existing axes (if origins are very close to each other), or create duplicates?
            var createDuplicates = (originDist > .2f);
            
            // Which metavisualization will do?
            switch(WhichMetaVis(axisA, axisB, dataSetID))
            {
                case MetaVisType.Scatterplot2D:
                    return SpanMetaVisScatterplot2D(axisA, axisB, dataSetID, createDuplicates);
                case MetaVisType.HeatMap:
                    return SpanMetaVisHeatmap3D(axisA, axisB, dataSetID, createDuplicates);
                default:
                    return SpanMetaVisImmersiveAxis(axisA, axisB, dataSetID);
            }
        }

        // .................................................................... Controller Methods

        public override bool CheckIfNearEnough(AAxis axisA, AAxis axisB)
        {
            if(
                ((axisA.GetAxisBaseGlobal() - axisB.GetAxisBaseGlobal()).magnitude < triggerMetaVisDistance
                || (axisA.GetAxisBaseGlobal() - axisB.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance)
                &&
                ((axisA.GetAxisTipGlobal() - axisB.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance
                || (axisA.GetAxisTipGlobal() - axisB.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance)
                )
            {
                return true;
            } else
            {
                return false;
            }
        }

        private bool CheckIfNearEnoughToHideAxis(AAxis originalAxis, AAxis metaAxis)
        {
            if(
                ((originalAxis.GetAxisBaseGlobal() - metaAxis.GetAxisBaseGlobal()).magnitude < triggerInvisibleDistance
                || (originalAxis.GetAxisBaseGlobal() - metaAxis.GetAxisTipGlobal()).magnitude < triggerInvisibleDistance)
                &&
                ((originalAxis.GetAxisTipGlobal() - metaAxis.GetAxisTipGlobal()).magnitude < triggerInvisibleDistance
                || (originalAxis.GetAxisTipGlobal() - metaAxis.GetAxisTipGlobal()).magnitude < triggerInvisibleDistance)
                )
            {
                return true;
            } else
            {
                return false;
            }
        }

        public override bool CheckIfCompatible(AETV etvA, AETV etvB, AAxis axisA, AAxis axisB, out int dataSetID)
        {
            if(axisA.stats.name == axisB.stats.name)
            {
                Debug.LogWarning("Given axes represent the same dimension and can't span a useful MetaVis.");
                dataSetID = -1;
                return false;
            }

            if(etvA.Equals(etvB))
            {
                Debug.LogWarning("Given axes belong to the same visualization and are not connected by MetaVis'.");
                dataSetID = -1;
                return false;
            }

            // Check every available Data Set
            for(int id = 0; id < dataProvider.dataSets.Length; id++)
            {
                var data = dataProvider.dataSets[id];

                bool existsA = data.attIDbyNAME.ContainsKey(axisA.stats.name);
                bool existsB = data.attIDbyNAME.ContainsKey(axisB.stats.name);

                // If both variables exist in the same data set
                if(existsA && existsB)
                {
                    bool correctLoMA = data.TypeOf(axisA.stats.name) == axisA.stats.type;
                    bool correctLoMB = data.TypeOf(axisB.stats.name) == axisB.stats.type;

                    if(existsA && existsB && correctLoMA && correctLoMB)
                    {
                        dataSetID = id;
                        return true;
                    }
                }
            }

            dataSetID = -1;
            return false;
        }

        public override MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            // Which MetaVis is defined by the implicit combination grammar?

            // ............................................ IMPLICIT GRAMMAR RULES

            // orthogonal case - 3 axes: scatterplot 3D
            // TODO - if enough time

            // calculate angle between axes
            var angle = Vector3.Angle(axisA.GetAxisDirectionGlobal(), axisB.GetAxisDirectionGlobal());

            // are axes orthogonal to each other? (between 85 and 95 degrees)
            var orthogonalCase = (Mathf.Abs(angle - 90) < 5);

            var lomA = axisA.stats.type;
            var lomB = axisB.stats.type;

            if(orthogonalCase) // ......................... ORTHOGONAL
            {
                if((lomA == LoM.NOMINAL || lomA == LoM.ORDINAL) &&
                   (lomB == LoM.NOMINAL || lomB == LoM.ORDINAL)) 
                     // if both categorical
                {
                    return MetaVisType.HeatMap;
                } 
                else // if both numerical or mixed
                {
                    return MetaVisType.Scatterplot2D;
                }
            } 
            else // ....................................... NOT ORTHOGONAL
            {
                return MetaVisType.ImmersiveAxis;
            }
        }

        public override AETV SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return GenerateImmersiveAxes(dataSetID, new string[] { axisA.stats.name, axisB.stats.name }, axisA, axisB);
        }
        

        // .................................................................... Factory Methods

        public override AETV SpanMetaVisFlexibleLinedAxes(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return GenerateFlexibleLinkedAxes(dataSetID, new string[] { axisA.stats.name, axisB.stats.name }, axisA, axisB);
        }

        public override AETV SpanMetaVisScatterplot2D(AAxis axisA, AAxis axisB, int dataSetID, bool duplicateAxes)
        {
            return GenerateScatterplot2D(dataSetID, new string[] { axisA.stats.name, axisB.stats.name }, axisA, axisB, duplicateAxes);
        }

        public override AETV SpanMetaVisHeatmap3D(AAxis axisA, AAxis axisB, int dataSetID, bool duplicateAxes)
        {
            return GenerateHeatmap3D(dataSetID, new string[] { axisA.stats.name, axisB.stats.name }, axisA, axisB, duplicateAxes);
        }




        ///////////////////////////////////////////////////////////////////////////////////////////
        // ........................................................................ MetaVis FACTORY

        public override AETV GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
        {
            try
            {
                var ds = dataProvider.dataSets[dataSetID];

                var factory = ServiceLocator.instance.FactoryMetaVis;
                var vis = factory.CreateFlexiblePCP(ds, variables, axisA, axisB);
                
                return vis;
            } catch(Exception e)
            {
                Debug.Log("Creation of requested MetaVis for variables " + variables + " failed.");
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                var o = new GameObject("Creation Failed");
                return o.AddComponent<ETV3DFlexiblePCP>();
            }
        }

        public override AETV GenerateScatterplot2D(int dataSetID, string[] variables, AAxis axisA, AAxis axisB, bool duplicateAxes)
        {
            try
            {
                // Create Meta-Visualization
                var data = dataProvider.dataSets[dataSetID];
                var plnt = ServiceLocator.MetaVisPlant();
                var mVis = plnt.CreateMetaScatterplot2D(data, variables);

                RotateAndScaleCorrectly(mVis, new AAxis[] { axisA, axisB });

                // Rotate and translate Meta-Visualization to match spanning axes
                // look in direction of cross product
                var cross = Vector3.Cross(
                    axisA.GetAxisDirectionGlobal(),
                    axisB.GetAxisDirectionGlobal());

                mVis.transform.forward = cross;

                // move to point between axes' origins
                mVis.transform.position = (axisA.GetAxisBaseGlobal() + axisB.GetAxisBaseGlobal()) / 2f;

                

                return mVis;
            } 
            catch(Exception e)  // Handle potential failures
            {
                Debug.Log("Creation of requested MetaVis for variables " + variables + " failed.");
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                var o = new GameObject("Creation Failed");
                return o.AddComponent<ETV3DFlexiblePCP>();
            }
        }

        public override AETV GenerateHeatmap3D(int dataSetID, string[] variables, AAxis axisA, AAxis axisB, bool duplicateAxes)
        {
            try
            {
                // Create Meta-Visualization
                var data = dataProvider.dataSets[dataSetID];
                var plnt = ServiceLocator.MetaVisPlant();
                
                // Rotate and translate Meta-Visualization to match spanning axes
                // look in direction of cross product
                var up = Vector3.Cross(
                    axisA.GetAxisDirectionGlobal(),
                    axisB.GetAxisDirectionGlobal());

                var angle = Vector3.SignedAngle(axisA.GetAxisDirectionGlobal(), axisB.GetAxisDirectionGlobal(), up);

                string[] vars = new string[2];
                if(angle > 0)
                {
                    vars[0] = variables[1];
                    vars[1] = variables[0];
                }
                else {
                    vars[0] = variables[0];
                    vars[1] = variables[1];
                }

                var mVis = plnt.CreateMetaHeatmap3D(data, vars, true, 1, 1);

                if(angle > 0)
                {
                    var rot = new Quaternion();
                    rot.SetLookRotation(axisA.GetAxisDirectionGlobal(), up);
                    mVis.transform.rotation = rot;
                } else
                {
                    var rot = new Quaternion();
                    rot.SetLookRotation(axisB.GetAxisDirectionGlobal(), up);
                    mVis.transform.rotation = rot;
                }

                


                RotateAndScaleCorrectly(mVis, new AAxis[] { axisA, axisB });

                // move to point between axes' origins
                mVis.transform.position = (axisA.GetAxisBaseGlobal() + axisB.GetAxisBaseGlobal()) / 2f;

                

                return mVis;
            } catch(Exception e)  // Handle potential failures
            {
                Debug.Log("Creation of requested MetaVis for variables " + variables + " failed.");
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                var o = new GameObject("Creation Failed");
                return o.AddComponent<ETV3DFlexiblePCP>();
            }
        }

        public override AETV GenerateFlexibleLinkedAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
        {
            // Create Meta-Visualization

            // Rotate and translate Meta-Visualization to match spanning axes

            throw new NotImplementedException();
        }

        public void RotateAndScaleCorrectly(AETV mVis, AAxis[] axes)
        {
            mVis.transform.localScale = new Vector3(.5f, .5f, .5f);
        }


        ///////////////////////////////////////////////////////////////////////////////////
        //..................................................................... IGPObserver

        public void OnDispose(AAxis observable)
        {

        }

        public void Notify(AAxis observable)
        {
            // Do nothing
        }

    }
}