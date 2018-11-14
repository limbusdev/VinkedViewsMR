using ETV;
using GraphicalPrimitive;
using Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MetaVisualization
{
    
    public class MetaVisSystem : AMetaVisSystem, IGPObserver<AAxis>
    {
        // .................................................................... EDITOR FIELDS

        [SerializeField]
        public DataProvider dataProvider;


        // .................................................................... STATIC PARAMETERS

        public static float triggerMetaVisDistance = 1f;
        public static float triggerInvisibleDistance = .05f;
        public static float triggerInvisibleAngleDiscrepancy = 5;


        // .................................................................... PRIVATE PROPERTIES

        private IDictionary<AETV, IList<AAxis>> etv2axes = new Dictionary<AETV, IList<AAxis>>();
        private IDictionary<AxisPair, AETV> axisPair2MetaVis = new Dictionary<AxisPair, AETV>();
        // Maps original axes to their (shadow) counterpart in the meta visualization
        private IDictionary<AxisPair, AETV> Original2Shadow = new Dictionary<AxisPair, AETV>();   
        

        // .................................................................... UPDATE METHOD
        void Update()
        {
            var toBeAdded = new Dictionary<AxisPair, AETV>();
            var toBeShred = new Dictionary<AxisPair, AETV>();
            
            foreach(var etvs in DisjointPairsOf(etv2axes.Keys, etv2axes.Keys))
            {
                foreach(var axes in DisjointPairsOf(etv2axes[etvs.A], etv2axes[etvs.B]))
                {
                    // ................................................ CREATE NEW METAVIS
                    int dataSetID;
                    if(MetaVisShouldBeCreated(etvs, axes, out dataSetID))
                    {
                        if(!toBeAdded.ContainsKey(axes))
                        {
                            var newVis = SpanMetaVisFor(axes, dataSetID);
                            toBeAdded.Add(axes, newVis);
                            try
                            {
                                if(newVis.registeredAxes.ContainsKey(axes.A.attributeName))
                                {
                                    var newKey = new AxisPair(axes.A, newVis.registeredAxes[axes.A.attributeName]);
                                    Original2Shadow.Add(newKey, newVis);
                                }
                                if(newVis.registeredAxes.ContainsKey(axes.B.attributeName))
                                {
                                    var newKey = new AxisPair(axes.B, newVis.registeredAxes[axes.B.attributeName]);
                                    Original2Shadow.Add(newKey, newVis);
                                }

                            } catch(Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    }
                    // ................................................ UPDATE EXISTING METAVIS
                    else
                    {
                        // If there is a metavis and it is too far stretched
                        if(!CheckIfNearEnough(axes))
                        {
                            // Check if there is a MetaVis already,
                            // and if so - remove it.

                            if(axisPair2MetaVis.ContainsKey(axes) && !toBeShred.ContainsKey(axes))
                            {
                                toBeShred.Add(axes, axisPair2MetaVis[axes]);
                            }
                        }
                    }
                }
            }
            
            // Hide very close axes of meta-visualizations
            foreach(var axisPair in Original2Shadow.Keys)
            {
                try
                {
                    var originalAxis = axisPair.A;
                    var metaAxis = axisPair.B;
                    var showMetaAxis = !CheckIfNearEnoughToHideAxis(originalAxis, metaAxis);

                    var metaVis = Original2Shadow[axisPair];

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
            foreach(var key in toBeShred.Keys)
            {
                var metaVis = axisPair2MetaVis[key];
                metaVis.Dispose();
                Destroy(metaVis);
                axisPair2MetaVis.Remove(key);
            }

            foreach(var key in toBeAdded.Keys)
            {
                axisPair2MetaVis.Add(key, toBeAdded[key]);
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
                if(!etv2axes.ContainsKey(etv))
                {
                    etv2axes.Add(etv, new List<AAxis>());
                }
                etv2axes[etv].Add(axis);
                Observe(axis);
            }
        }


        public override void StopObservationOf(AETV etv, AAxis axis)
        {
            if(etv2axes.ContainsKey(etv))
            {
                etv2axes[etv].Remove(axis);
            }
        }

        /// <summary>
        /// (HELPER-MEHTOD)
        /// Are all preconditions met, to span up a meta-visualization?
        /// </summary>
        private bool MetaVisShouldBeCreated(ETVPair etvs, AxisPair axes, out int dataSetID)
        {
            bool createIt = true;

            createIt &= !axes.A.attributeName.Equals(axes.B.attributeName);
            createIt &= !axisPair2MetaVis.ContainsKey(axes);
            createIt &= CheckIfNearEnough(axes);
            createIt &= CheckIfCompatible(etvs, axes, out dataSetID);

            return createIt;
        }

        public override AETV SpanMetaVisFor(AxisPair axes, int dataSetID)
        {
            // calculate distance between their origins
            var originDist = Vector3.Distance(axes.A.GetAxisBaseGlobal(), axes.B.GetAxisBaseGlobal());
            // use existing axes (if origins are very close to each other), or create duplicates?
            var createDuplicates = (originDist > .2f);
            
            // Which metavisualization will do?
            switch(WhichMetaVis(axes, dataSetID))
            {
                case MetaVisType.SCATTERPLOT_2D:
                    return SpanMetaVisScatterplot2D(axes, dataSetID, createDuplicates);
                case MetaVisType.HEATMAP:
                    return SpanMetaVisHeatmap3D(axes, dataSetID, createDuplicates);
                default:
                    return SpanMetaVisImmersiveAxis(axes, dataSetID);
            }
        }

        // .................................................................... Controller Methods

        public override bool CheckIfNearEnough(AxisPair axes)
        {
            if(
                ((axes.A.GetAxisBaseGlobal() - axes.B.GetAxisBaseGlobal()).magnitude < triggerMetaVisDistance
                || (axes.A.GetAxisBaseGlobal() - axes.B.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance)
                &&
                ((axes.A.GetAxisTipGlobal() - axes.B.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance
                || (axes.A.GetAxisTipGlobal() - axes.B.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance)
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

        public override bool CheckIfCompatible(ETVPair etvs, AxisPair axes, out int dataSetID)
        {
            if(axes.A.stats.name == axes.B.stats.name)
            {
                Debug.LogWarning("Given axes represent the same dimension and can't span a useful MetaVis.");
                dataSetID = -1;
                return false;
            }

            if(axes.A.Equals(axes.B))
            {
                Debug.LogWarning("Given axes belong to the same visualization and are not connected by MetaVis'.");
                dataSetID = -1;
                return false;
            }

            // Check every available Data Set
            for(int id = 0; id < dataProvider.dataSets.Length; id++)
            {
                var data = dataProvider.dataSets[id];

                bool existsA = data.attIDbyNAME.ContainsKey(axes.A.stats.name);
                bool existsB = data.attIDbyNAME.ContainsKey(axes.B.stats.name);

                // If both variables exist in the same data set
                if(existsA && existsB)
                {
                    bool correctLoMA = data.TypeOf(axes.A.stats.name) == axes.A.stats.type;
                    bool correctLoMB = data.TypeOf(axes.B.stats.name) == axes.B.stats.type;

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

        public override MetaVisType WhichMetaVis(AxisPair axes, int dataSetID)
        {
            // Which MetaVis is defined by the implicit combination grammar?

            // ............................................ IMPLICIT GRAMMAR RULES

            // orthogonal case - 3 axes: scatterplot 3D
            // TODO - if enough time

            // calculate angle between axes
            var angle = Vector3.Angle(axes.A.GetAxisDirectionGlobal(), axes.B.GetAxisDirectionGlobal());

            // are axes orthogonal to each other? (between 85 and 95 degrees)
            var orthogonalCase = (Mathf.Abs(angle - 90) < 5);

            var lomA = axes.A.stats.type;
            var lomB = axes.B.stats.type;

            if(orthogonalCase) // ......................... ORTHOGONAL
            {
                if((lomA == LoM.NOMINAL || lomA == LoM.ORDINAL) &&
                   (lomB == LoM.NOMINAL || lomB == LoM.ORDINAL)) 
                     // if both categorical
                {
                    return MetaVisType.HEATMAP;
                } 
                else // if both numerical or mixed
                {
                    return MetaVisType.SCATTERPLOT_2D;
                }
            } 
            else // ....................................... NOT ORTHOGONAL
            {
                return MetaVisType.IMMERSIVE_AXES;
            }
        }

        public override AETV SpanMetaVisImmersiveAxis(AxisPair axes, int dataSetID)
        {
            return GenerateImmersiveAxes(dataSetID, new string[] { axes.A.stats.name, axes.B.stats.name }, axes);
        }
        

        // .................................................................... Factory Methods

        public override AETV SpanMetaVisFlexibleLinedAxes(AxisPair axes, int dataSetID)
        {
            return GenerateFlexibleLinkedAxes(dataSetID, new string[] { axes.A.stats.name, axes.B.stats.name }, axes);
        }

        public override AETV SpanMetaVisScatterplot2D(AxisPair axes, int dataSetID, bool duplicateAxes)
        {
            return GenerateScatterplot2D(dataSetID, new string[] { axes.A.stats.name, axes.B.stats.name }, axes, duplicateAxes);
        }

        public override AETV SpanMetaVisHeatmap3D(AxisPair axes, int dataSetID, bool duplicateAxes)
        {
            return GenerateHeatmap3D(dataSetID, new string[] { axes.A.stats.name, axes.B.stats.name }, axes, duplicateAxes);
        }




        ///////////////////////////////////////////////////////////////////////////////////////////
        // ........................................................................ MetaVis FACTORY

        public override AETV GenerateImmersiveAxes(int dataSetID, string[] variables, AxisPair axes)
        {
            try
            {
                var ds = dataProvider.dataSets[dataSetID];

                var factory = Services.instance.FactoryMetaVis;
                var vis = factory.CreateFlexiblePCP(ds, variables, axes.A, axes.B);
                
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

        public override AETV GenerateScatterplot2D(int dataSetID, string[] variables, AxisPair axes, bool duplicateAxes)
        {
            try
            {
                // Create Meta-Visualization
                var data = dataProvider.dataSets[dataSetID];
                var plnt = Services.MetaVisFactory();
                var mVis = plnt.CreateMetaScatterplot2D(data, variables);

                RotateAndScaleCorrectly(mVis, new AAxis[] { axes.A, axes.B });

                // Rotate and translate Meta-Visualization to match spanning axes
                // look in direction of cross product
                var cross = Vector3.Cross(
                    axes.A.GetAxisDirectionGlobal(),
                    axes.B.GetAxisDirectionGlobal());

                mVis.transform.forward = cross;

                // move to point between axes' origins
                mVis.transform.position = (axes.A.GetAxisBaseGlobal() + axes.B.GetAxisBaseGlobal()) / 2f;

                

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

        public override AETV GenerateHeatmap3D(int dataSetID, string[] variables, AxisPair axes, bool duplicateAxes)
        {
            try
            {
                // Create Meta-Visualization
                var data = dataProvider.dataSets[dataSetID];
                var plnt = Services.MetaVisFactory();
                
                // Rotate and translate Meta-Visualization to match spanning axes
                // look in direction of cross product
                var up = Vector3.Cross(
                    axes.A.GetAxisDirectionGlobal(),
                    axes.B.GetAxisDirectionGlobal());

                var angle = Vector3.SignedAngle(axes.A.GetAxisDirectionGlobal(), axes.B.GetAxisDirectionGlobal(), up);

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
                    rot.SetLookRotation(axes.A.GetAxisDirectionGlobal(), up);
                    mVis.transform.rotation = rot;
                } else
                {
                    var rot = new Quaternion();
                    rot.SetLookRotation(axes.B.GetAxisDirectionGlobal(), up);
                    mVis.transform.rotation = rot;
                }

                


                RotateAndScaleCorrectly(mVis, new AAxis[] { axes.A, axes.B });

                // move to point between axes' origins
                mVis.transform.position = (axes.A.GetAxisBaseGlobal() + axes.B.GetAxisBaseGlobal()) / 2f;

                

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

        public override AETV GenerateFlexibleLinkedAxes(int dataSetID, string[] variables, AxisPair axes)
        {
            // Create Meta-Visualization

            // Rotate and translate Meta-Visualization to match spanning axes

            throw new NotImplementedException();
        }

        public void RotateAndScaleCorrectly(AETV mVis, AAxis[] axes)
        {
            mVis.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        // -------------------------------------------------------------------- HELPER METHODS

        private IEnumerable<ETVPair> DisjointPairsOf(ICollection<AETV> etvs1, ICollection<AETV> etvs2)
        {
            return from etv1 in etvs1      // for each evt1 in axes.Keys
                   from etv2 in etvs2      // and each etv2 in axes.Keys
                   where (!etv1.Equals(etv2))  // if they are not the same
                   select new ETVPair(etv1, etv2);  // make a tuple of them
        }

        private IEnumerable<AxisPair> DisjointPairsOf(ICollection<AAxis> axesA, ICollection<AAxis> axesB)
        {
            return from a in axesA              // for each evt1 in axes.Keys
                   from b in axesB              // and each etv2 in axes.Keys
                   where (!a.Equals(b))         // if they are not the same
                   select new AxisPair(a, b);   // make a tuple of them
        }


        ///////////////////////////////////////////////////////////////////////////////////
        //..................................................................... IGPObserver

        public void OnDispose(AAxis observable)
        {
            foreach(var etv in etv2axes.Keys)
            {
                etv2axes[etv].Remove(observable);
            }

            var toBeRemoved = new List<AxisPair>();
            foreach(var axisPair in axisPair2MetaVis.Keys)
            {
                if(axisPair.A.Equals(observable) || axisPair.B.Equals(observable))
                {
                    toBeRemoved.Add(axisPair);
                }
            }
            foreach(var key in toBeRemoved)
            {
                axisPair2MetaVis.Remove(key);
            }

            toBeRemoved.Clear();
            foreach(var axisPair in Original2Shadow.Keys)
            {
                if(axisPair.A.Equals(observable) || axisPair.B.Equals(observable))
                {
                    toBeRemoved.Add(axisPair);
                }
            }
            foreach(var key in toBeRemoved)
            {
                Original2Shadow.Remove(key);
            }
        }

        public void Notify(AAxis observable)
        {
            // Do nothing
        }

        public void Observe(AAxis observable)
        {
            observable.Subscribe(this);
        }
    }
}