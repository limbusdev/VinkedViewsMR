using ETV;
using GraphicalPrimitive;
using mikado;
using Model;
using UnityEngine;

namespace MetaVisualization
{
    /// <summary>
    /// Meta-visualization system. Observes lots of axes and checks, whether they
    /// are in a constellation, which implicitly indicates a meta-visualization
    /// to be spanned between them.
    /// </summary>
    public abstract class AMetaVisSystem : MonoBehaviour, IAxisObserver
    {
        // .................................................................... STATIC PARAMETERS

        public static float triggerMetaVisDistance = .8f;
        public static float triggerInvisibleDistance = .05f;
        public static float triggerInvisibleAngleDiscrepancy = 5;


        /// <summary>
        /// Blocks combination from being registered with another meta-visualization.
        /// </summary>
        /// <param name="key"></param>
        public abstract void UseCombination(AxisPair key);

        /// <summary>
        /// Frees combination for new meta-visualizations.
        /// </summary>
        /// <param name="key"></param>
        public abstract void ReleaseCombination(AxisPair key);

        /// <summary>
        /// Removes the given axis from the list of permanently observed axes.
        /// Use this, when destroying or disabling a visualization.
        /// </summary>
        /// <param name="axis"></param>

        /// <summary>
        /// Generate a metavisualization between the given axes.
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>A new metavisualization</returns>
        public abstract AETV SpanMetaVisFor(AxisPair pair, int dataSetID, out MetaVisType type);

        /// <summary>
        /// Checks whether the given axis tips and bases are both nearer to each other
        /// than a predefined border value.
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>If they are near enough to span a MetaVis.</returns>
        public static bool CheckIfNearEnough(AxisPair axes)
        {
            var tipA = axes.A.GetAxisTipGlobal();
            var baseA = axes.A.GetAxisBaseGlobal();
            var tipB = axes.B.GetAxisTipGlobal();
            var baseB = axes.B.GetAxisBaseGlobal();

            if(((baseA - baseB).magnitude < triggerMetaVisDistance && (tipA - tipB).magnitude < triggerMetaVisDistance)
                ||
                ((baseA - tipB).magnitude < triggerMetaVisDistance && (tipA - baseB).magnitude < triggerMetaVisDistance))
            {
                if(Vector3.Angle(axes.A.GetAxisDirectionGlobal(), axes.B.GetAxisDirectionGlobal()) < 100)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }

        public static float MeanDistanceBetween(AAxis axisA, AAxis axisB)
        {
            var tipA = axisA.GetAxisTipGlobal();
            var baseA = axisA.GetAxisBaseGlobal();
            var tipB = axisB.GetAxisTipGlobal();
            var baseB = axisB.GetAxisBaseGlobal();

            return ((baseA - baseB).magnitude + (baseA - tipB).magnitude + (tipA - baseB).magnitude + (tipA - tipB).magnitude)/4f;
        }

        public static bool CheckIfNearEnoughToHideAxis(AAxis originalAxis, AAxis metaAxis)
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

        public static float ProjectedDistanceToAxis(Vector3 point, AAxis axis)
        {
            var ray = new Ray(axis.GetAxisBaseGlobal(), (axis.GetAxisTipGlobal() - axis.GetAxisBaseGlobal()));
            return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
        }

        /// <summary>
        /// Checks for a dataset which contains both represented attributes and 
        /// returns it's ID.
        /// Not compatible if they belong to the same ETV
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>Whether such a dataset exists</returns>
        public abstract bool CheckIfCompatible(AxisPair axisPair, out int dataSetID);

        /// <summary>
        /// Checks, which meta-visualization form would be appropriate
        /// for the given axes.
        /// 
        /// For example the following MetaVis' are appropriate:
        /// 
        /// | Axis constellation                                                | MetaVis                       |
        /// +-------------------------------------------------------------------+-------------------------------+
        /// | two arbitrarily constellated axes                                 | immersive axes                |
        /// | two arbitrarily constellated axes in the same layer               | FLA                           |
        /// | two orthogonal constellated numerical axes in the same layer      | scatterplot                   |
        /// | two orthogonal constellated categorical axes in the same layer    | height map / 3D bar chart     |
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <returns>appropriate meta-visualization form</returns>
        public static MetaVisType WhichMetaVis(AxisPair axes, int dataSetID)
        {
            // Which MetaVis is defined by the implicit combination grammar?

            // ............................................ IMPLICIT GRAMMAR RULES

            // orthogonal case - 3 axes: scatterplot 3D
            // TODO - if enough time

            var constellation = new AAxis[] { axes.A, axes.B };

            if(MikadoIRuleScatterplot2D.I.RuleApplies(constellation))
            {
                return MetaVisType.SCATTERPLOT_2D;
            }
            else if(MikadoIRuleHeatMap.I.RuleApplies(constellation))
            {
                return MetaVisType.HEATMAP;
            }
            else
                return MetaVisType.IMMERSIVE_AXES;
        }

        /// <summary>
        /// Generates an immersive axis meta-visualization between the given axes of
        /// the given data set.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">secon axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <returns>new immersive axes meta-visualization</returns>
        public abstract AETV SpanMetaVisImmersiveAxis(AxisPair pair, int dataSetID);

        /// <summary>
        /// Generates a Flexible Linked Axes visualization between the given axes
        /// of the given data set.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set id of them</param>
        /// <returns>new FLA meta-visualization</returns>
        public abstract AETV SpanMetaVisFlexibleLinedAxes(AxisPair pair, int dataSetID);
        
        /// <summary>
        /// Generates a two dimensional scatterplot visualization between the
        /// given axes of the given data set. If axes origin's do not touch,
        /// axes will get duplicated and made perfectly orthogonal.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <param name="duplicateAxes">if axes should be ducplicated, because they do not touch</param>
        /// <returns>new 2d scatterplot meta-visualization</returns>
        public abstract AETV SpanMetaVisScatterplot2D(AxisPair pair, int dataSetID, bool duplicateAxes);

        /// <summary>
        /// Generates a three dimensional heat map / bar chart, representing the relative
        /// distribution of samples according to the given axes.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <param name="duplicateAxes">if axes should be ducplicated</param>
        /// <returns>new 3d histogramm meta-visualization</returns>
        public abstract AETV SpanMetaVisHeatmap3D(AxisPair pair, int dataSetID, bool duplicateAxes);



        ///////////////////////////////////////////////////////////////////////////////////////////
        // ........................................................................ MetaVis FACTORY

        /// <summary>
        /// Generates a 2D Parallel Coordinates Plot for n attributes.
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables">Attributes to be present in the PCP.</param>
        /// <returns>GameObject containing the anchored visualization.</returns>
        public abstract AETV GenerateImmersiveAxes(int dataSetID, string[] variables, AxisPair pair);

        /// <summary>
        /// (HELPER METHOD)
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables"></param>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns></returns>
        public abstract AETV GenerateFlexibleLinkedAxes(int dataSetID, string[] variables, AxisPair pair);

        /// <summary>
        /// (HELPER METHOD)
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables"></param>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <param name="duplicateAxes"></param>
        /// <returns></returns>
        public abstract AETV GenerateScatterplot2D(int dataSetID, string[] variables, AxisPair pair, bool duplicateAxes);

        /// <summary>
        /// (HELPER METHOD)
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables"></param>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <param name="duplicateAxes"></param>
        /// <returns></returns>
        public abstract AETV GenerateHeatmap3D(int dataSetID, string[] variables, AxisPair pair, bool duplicateAxes);


        // IAxisObserver
        public abstract void Observe(AAxis observable);
        public abstract void Ignore(AAxis observable);
        public abstract void OnDispose(AAxis observable);
        public abstract void OnChange(AAxis observable);
    }
}