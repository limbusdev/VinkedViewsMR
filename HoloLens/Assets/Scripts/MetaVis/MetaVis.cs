using ETV;
using GraphicalPrimitive;
using System.Collections.Generic;

public abstract class MetaVis : AETV, IGPObserver<AAxis>
{
    private IList<AAxis> subscriptions = new List<AAxis>();

    public void Notify(AAxis observable)
    {
        if(gameObject.activeSelf)
        {
            // If observerd axes change, update meta-visualization
            UpdateETV();
        }
    }

    public void OnDispose(AAxis observable)
    {
        // If observed axes are disposed, dispose MetaVis as well
        Dispose();
    }

    public override void Dispose()
    {
        foreach(var obs in subscriptions)
        {
            obs.Unsubscribe(this);
        }
        subscriptions.Clear();
        base.Dispose();
    }

    public void Observe(AAxis observable)
    {
        subscriptions.Add(observable);
    }
}
