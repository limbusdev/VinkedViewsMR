using UnityEngine;

namespace Model
{
    public class DataSetPoints
    {
        public Vector3[] points { get; set; }
        public string[] variables;
        public string[] units;

        public DataSetPoints(Vector3[] points, string[] variables, string[] units)
        {
            this.points = points;
            this.variables = variables;
            this.units = units;
        }
    }
}
