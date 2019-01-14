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

using ETV;
using GraphicalPrimitive;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MetaVisualization
{
    public class MetaVisSystem : AMetaVisSystem
    {
        // .................................................................... EDITOR FIELDS

        [SerializeField]
        public DataProvider dataProvider;


        // .................................................................... PRIVATE PROPERTIES

        private ISet<MetaVisByAttributesAndETV> usedCombinationsByAttributeAndETV = new HashSet<MetaVisByAttributesAndETV>();
        private ISet<AxisPair> usedCombinations = new HashSet<AxisPair>();
        private ISet<AAxis> observedAxes = new HashSet<AAxis>();                                    // all currently observed axes of normal visualizations
        private ISet<AETV> normalVisualizations = new HashSet<AETV>();                              // all currently active visualizations


        // .................................................................... UPDATE METHOD
        void Update()
        {
            // Only update if neccessary (See OnChange())
        }

        protected void UpdateAxis(AAxis updatedAxis)
        {
            var single = new List<AAxis>();
            single.Add(updatedAxis);
            UpdateAxes(single);
        }

        protected void UpdateAxes(ICollection<AAxis> updatedAxes)
        {
            // for every pair of different axes
            foreach(var axes in DisjointPairsOf(updatedAxes, observedAxes))
            {
                int dataSetID;
                
                if(MetaVisShouldBeCreated(axes, out dataSetID))
                {
                    // Check if there is a Meta-Visualization using axes from the same ETV


                    // Create meta-visualization
                    MetaVisType mVisType;
                    var newVis = SpanMetaVisFor(axes, dataSetID, out mVisType);
                    var updaterObject = new GameObject("MetaVisUpdater");

                    AMetaVisUpdater metaVisUpdater;
                    switch(mVisType)
                    {
                        case MetaVisType.SCATTERPLOT_2D:
                            metaVisUpdater = updaterObject.AddComponent<MetaScatterplot2DUpdater>();
                            break;
                        case MetaVisType.HEATMAP:
                            metaVisUpdater = updaterObject.AddComponent<MetaHeatMapUpdater>();
                            break;
                        default: // FLA & Immersive Axes
                            metaVisUpdater = updaterObject.AddComponent<MetaImmersiveAxesUpdater>();
                            break;
                    }
                    metaVisUpdater.Init(newVis, axes, dataSetID);
                    UseCombination(axes);
                }
            }
        }
        
        /// <summary>
        /// (HELPER-MEHTOD)
        /// Are all preconditions met, to span up a meta-visualization?
        /// </summary>
        private bool MetaVisShouldBeCreated(AxisPair axes, out int dataSetID)
        {
            bool createIt = true;

            var mvisCombos = new MetaVisByAttributesAndETV(axes);



            createIt &= !usedCombinations.Contains(axes);
            createIt &= !usedCombinationsByAttributeAndETV.Contains(mvisCombos);
            createIt &= !axes.A.Base().Equals(axes.B.Base());               // not component of the same etv
            createIt &= !axes.A.attributeName.Equals(axes.B.attributeName); // do not represent the same attribute
            createIt &= CheckIfNearEnough(axes);                            // are close enought to each other
            createIt &= CheckIfCompatible(axes, out dataSetID);             // are compatible and from the same data set

            return createIt;
        }

        public override AETV SpanMetaVisFor(AxisPair axes, int dataSetID, out MetaVisType type)
        {
            // calculate distance between their origins
            var originDist = Vector3.Distance(axes.A.GetAxisBaseGlobal(), axes.B.GetAxisBaseGlobal());
            // use existing axes (if origins are very close to each other), or create duplicates?
            var createDuplicates = (originDist > .2f);
            
            // Which metavisualization will do?
            switch(type = WhichMetaVis(axes, dataSetID))
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
        
        public override bool CheckIfCompatible(AxisPair axes, out int dataSetID)
        {
            if(axes.A.stats.name == axes.B.stats.name)
            {
                //Debug.LogWarning("Given axes represent the same dimension and can't span a useful MetaVis.");
                dataSetID = -1;
                return false;
            }

            if(axes.A.Equals(axes.B))
            {
                //Debug.LogWarning("Given axes belong to the same visualization and are not connected by MetaVis'.");
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

                // Rotate and translate Meta-Visualization to match spanning axes
                // look in direction of cross product
                var forward = Vector3.Cross(
                    axes.A.GetAxisDirectionGlobal(),
                    axes.B.GetAxisDirectionGlobal());

                var angle = Vector3.SignedAngle(axes.A.GetAxisDirectionGlobal(), axes.B.GetAxisDirectionGlobal(), forward);


                string[] vars = new string[2];
                if(angle > 0)
                {
                    vars[0] = variables[1];
                    vars[1] = variables[0];
                } else
                {
                    vars[0] = variables[0];
                    vars[1] = variables[1];
                }


                var mVis = plnt.CreateMetaScatterplot2D(data, vars);

                
                if(angle > 0)
                {
                    mVis.transform.forward = -forward;
                }
                else
                {
                    mVis.transform.forward = forward;
                }

                RotateAndScaleCorrectly(mVis, new AAxis[] { axes.A, axes.B });
                
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
                
                RotateAndScaleCorrectly(mVis, new AAxis[] { axes.A, axes.B });
                
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

        /// <summary>
        /// Adds the given axis to the list of permanently observed axes.
        /// If observed, axis can span metavisualizations between them, if
        /// they fullfill given factors.
        /// </summary>
        /// <param name="axis"></param>
        public override void Observe(AAxis observable)
        {
            // do not observe axes of meta-visualizations
            if(!observable.Base().isMetaVis)
            {
                observable.Subscribe(this);
                observedAxes.Add(observable);
                normalVisualizations.Add(observable.Base());

                UpdateAxis(observable);
            }
        }

        public override void Ignore(AAxis observable)
        {
            observedAxes.Remove(observable);
            UpdateAxes(observedAxes);
            observable.Unsubscribe(this);
        }

        public override void OnDispose(AAxis observable)
        {
            // Axis.Dispose() can only be called from AETV
            // do nothing here, but unsubscribe
            observedAxes.Remove(observable);
            UpdateAxes(observedAxes);
            // No need to Unsubscribe, since Observable clears Observer-List anyways
        }

        public override void OnChange(AAxis observable)
        {
            UpdateAxis(observable);
        }

        public override void ReleaseCombination(AxisPair key)
        {
            usedCombinations.Remove(key);
            usedCombinationsByAttributeAndETV.Remove(new MetaVisByAttributesAndETV(key));
        }

        public override void UseCombination(AxisPair key)
        {
            usedCombinations.Add(key);
            usedCombinationsByAttributeAndETV.Add(new MetaVisByAttributesAndETV(key));
        }
    }
}