using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class ETV3DScatterPlot : AETV3D
{
    public ParticleSystem particleSystem;

    public GameObject Anchor;
    private DataSetPoints data;
    private ParticleSystem.Particle[] particles;
    private Vector3 ranges = new Vector3(1, 1, 1);
    private Vector3 mins = new Vector3(0, 0, 0);
    private Vector3 maxs = new Vector3(1, 1, 1);
    private Vector3 ticks = new Vector3(1, 1, 1);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(DataSetPoints data, float[] mins, float[] maxs, float[] ticks)
    {
        this.data = data;
        this.mins = new Vector3(mins[0], mins[1], mins[2]);
        this.maxs = new Vector3(maxs[0], maxs[1], maxs[2]);
        this.ticks = new Vector3(ticks[0], ticks[1], ticks[2]);
        this.ranges = this.maxs - this.mins;

        UpdateETV();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        throw new System.NotImplementedException();
    }

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        throw new System.NotImplementedException();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.variables[0], data.units[0], AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, 0);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        xAxis2D.tickResolution = ticks.x;
        xAxis2D.min = mins.x;
        xAxis2D.max = maxs.x;
        xAxis2D.UpdateAxis();

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.variables[1], data.units[1], AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, 0);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        yAxis2D.tickResolution = ticks.y;
        yAxis2D.min = mins.y;
        yAxis2D.max = maxs.y;
        yAxis2D.UpdateAxis();


        // z-Axis
        GameObject zAxis = factory2D.CreateAxis(Color.white, data.variables[2], data.units[2], AxisDirection.Z, 1f, .01f, true, true);
        zAxis.transform.localPosition = new Vector3(0, 0, 0);
        zAxis.transform.parent = Anchor.transform;
        Axis2D zAxis2D = zAxis.GetComponent<Axis2D>();
        zAxis2D.ticked = true;
        zAxis2D.tickResolution = ticks.z;
        zAxis2D.min = mins.z;
        zAxis2D.max = maxs.z;
        zAxis2D.UpdateAxis();
    }

    public void DrawGraph()
    {
        particles = new ParticleSystem.Particle[data.points.Length];
        float pointSize = (particles.Length < 50) ? (.03f) : (1f / particles.Length * 2f);
        for (int i = 0; i < data.points.Length; i++)
        {
            Vector3 scaledPosition = new Vector3(data.points[i].x / ranges.x, data.points[i].y / ranges.y, data.points[i].z / ranges.z);

            ParticleSystem.Particle p = new ParticleSystem.Particle();
            p.position = scaledPosition;
            p.startColor = Color.white;
            p.startSize = pointSize;
            particles[i] = p;
        }
        particleSystem.GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
        particleSystem.GetComponent<ParticleSystem>().maxParticles = particles.Length;
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }

    
}
