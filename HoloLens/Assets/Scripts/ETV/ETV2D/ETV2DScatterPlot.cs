using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DScatterPlot : AETV2D
{
    public GameObject Anchor;
    public GameObject particleSystem;
    private DataSetPoints data;
    private ParticleSystem.Particle[] particles;
    private float rangeX, rangeY;
    private float minX = 0;
    private float maxX = 1;
    private float minY = 0;
    private float maxY = 1;
    private float ticksX = 1;
    private float ticksY = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(DataSetPoints data, float minX, float maxX, float minY, float maxY, float ticksX, float ticksY)
    {
        this.data = data;
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        rangeX = maxX - minX;
        rangeY = maxY - minY;
        this.ticksX = ticksX;
        this.ticksY = ticksY;
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme) { }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.variables[0], data.units[0], AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        xAxis2D.tickResolution = ticksX;
        xAxis2D.min = minX;
        xAxis2D.max = maxX;
        xAxis2D.UpdateAxis();
        bounds[0] += 1f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.variables[1], data.units[1], AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        yAxis2D.tickResolution = ticksY;
        yAxis2D.min = minY;
        yAxis2D.max = maxY;
        yAxis2D.UpdateAxis();


        bounds[1] += 1f + .5f;

    }

    public void DrawGraph()
    {
        particles = new ParticleSystem.Particle[data.points.Length];
        float pointSize = (particles.Length < 50) ? (.03f) : (1f / particles.Length * 2f);
        for (int i = 0; i < data.points.Length; i++)
        {
            Vector3 scaledPosition = new Vector3(data.points[i].x / rangeX, data.points[i].y / rangeY, 0);

            ParticleSystem.Particle p = new ParticleSystem.Particle();
            p.position = scaledPosition;
            p.startColor = Color.white;
            p.startSize = pointSize;
            particles[i] = p;
        }
        particleSystem.GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
        particleSystem.GetComponent<ParticleSystem>().maxParticles = particles.Length;
    }   
}






