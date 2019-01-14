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

using System;
using System.Collections.Generic;
using ETV;
using GraphicalPrimitive;
using MetaVisualization;
using UnityEngine;

/// <summary>
/// Uncoupling update-class. Observed axes which span a meta-visualization
/// and tells it about changes in them. When one of the axes gets destroyed 
/// of conditions for the meta-visualization are not met anymore, it
/// destroys the meta-visualization and itself.
/// </summary>
public abstract class AMetaVisUpdater : MonoBehaviour, IAxisObserver, IDisposable
{
    // Public

    // Protected
    protected AETV metaVisualization;
    protected AxisPair spanningAxes;
    protected Vector3 normal;
    protected float signedAngle;

    // Private
    private int dataSetID;
    private bool disposed = false;
    private bool initialized = false;
    private bool valid = true;
    private IDictionary<AAxis, AAxis> shadowAxes = new Dictionary<AAxis, AAxis>();

    // ........................................................................ Abstract Methods
    protected abstract void UpdatePosition();
    protected abstract void UpdateNormalVector();
    protected abstract void UpdateRotation();
    protected abstract MetaVisType Type();

    // ........................................................................ Template Method
    /// <summary>
    /// Template method. Defines the correct way to update meta-ETV.
    /// </summary>
    /// <param name="observable"></param>
    public void OnChange(AAxis observable)
    {
        // Update meta-visualization, when spanning axes
        // change.
        if(!disposed && initialized)
        {
            CheckValidity();
            if(valid)
            {
                metaVisualization.UpdateETV();
                TryHidingCloseAxes();
                UpdatePosition();
                UpdateSignedAngleBetweenAxes();
                UpdateNormalVector();
                UpdateRotation();
            } else
            {
                Dispose();
            }
        }
    }

    // ........................................................................ Explicit Methods

    public void Init(AETV etv, AxisPair axes, int dsID)
    {
        metaVisualization = etv;
        spanningAxes = axes;
        dataSetID = dsID;
        initialized = true;

        FindShadowAxes();
        Observe(axes.A);
        Observe(axes.B);
    }

    private void FindShadowAxes()
    {
        // FlexiblePCP does not contain shadow axes
        try
        {
            // for both of the spanning axes, add their shadow-counterparts to the list
            foreach(var ax in new AAxis[] { spanningAxes.A, spanningAxes.B })
            {
                if(metaVisualization.registeredAxes.ContainsKey(ax.attributeName))
                {
                    // If a metavisualization contains more than one matching
                    // shadow axis, add the nearest one.
                    foreach(var a in metaVisualization.registeredAxes[ax.attributeName])
                    {
                        if(shadowAxes.ContainsKey(ax))
                        {
                            var alreadyRegisteredShadowAxis = shadowAxes[ax];
                            if(AMetaVisSystem.MeanDistanceBetween(ax,a) < AMetaVisSystem.MeanDistanceBetween(ax, shadowAxes[ax]))
                            {
                                shadowAxes[ax] = a;
                            }
                        } else
                        {
                            shadowAxes.Add(ax, a);
                        }
                    }
                }
            }

        } catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void TryHidingCloseAxes()
    {
        foreach(var originalAxis in new AAxis[] { spanningAxes.A, spanningAxes.B })
        {
            if(shadowAxes.ContainsKey(originalAxis))
            {
                var shadowAxis = shadowAxes[originalAxis];
                try
                {
                    var visible = !AMetaVisSystem.CheckIfNearEnoughToHideAxis(originalAxis, shadowAxis);
                    shadowAxis.SetVisibility(visible);

                    var distProjOnOrigBase = AMetaVisSystem.ProjectedDistanceToAxis(shadowAxis.GetAxisBaseGlobal(), originalAxis);
                    var distProjOnOrigTip = AMetaVisSystem.ProjectedDistanceToAxis(shadowAxis.GetAxisTipGlobal(), originalAxis);

                    // if one axis is parallel it's metavis axis and the other is not,
                    // stick to the parallel one
                    if(distProjOnOrigBase < .01f && distProjOnOrigTip < .01f)
                    {
                        metaVisualization.transform.position = originalAxis.GetAxisBaseGlobal();
                    }

                } catch(Exception e)
                {
                    Debug.LogError("Checking vicinity of original and meta axis failed, because of exception.");
                    Debug.LogException(e);
                }
            }
        }
    }

    /// <summary>
    /// Checks whether the conditions of this meta-visualization still apply.
    /// That is the case, if visualization type hasn't changed and the spanning
    /// axes are still close enough to each other.
    /// </summary>
    private void CheckValidity()
    {
        valid = AMetaVisSystem.CheckIfNearEnough(spanningAxes)
            && Type().Equals(AMetaVisSystem.WhichMetaVis(spanningAxes, dataSetID));
    }

    private void UpdateSignedAngleBetweenAxes()
    {
        signedAngle = Vector3.SignedAngle(spanningAxes.A.GetAxisDirectionGlobal(), spanningAxes.B.GetAxisDirectionGlobal(), normal);
    }



    // ........................................................................ IAxisObserver
    public void Ignore(AAxis observable)
    {
        // Not neccessary, drestroys itself
        // when OnDispose() is called by observable.
    }

    public void Observe(AAxis observable)
    {
        observable.Subscribe(this);
    }
    
    

    public void OnDispose(AAxis observable)
    {
        Dispose();
    }

    // ........................................................................ IDisposable
    public void Dispose()
    {
        disposed = true;
        // Tell meta-visualization to destroy itself,
        // since AETV doesn't observe the spanning axes,
        // it doesn't know otherwise, when to destroy itself.
        metaVisualization.Dispose();

        // Tell MetaVisSystem to release combination
        // for new meta-visualizations
        Services.MetaVisSys().ReleaseCombination(spanningAxes);

        // Self destruction
        Destroy(gameObject);
    }
}
