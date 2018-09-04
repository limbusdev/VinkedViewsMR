using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Container class for storing multi dimensional data
    /// points. Rows represent data objects, columns represent
    /// the various dimensions. Every dimension is defined by
    /// a variable name and a variable unit.
    /// 
    /// Example:
    /// 
    /// x [m]   y [m]   z [m]   temp [°C]   moisture [%]
    /// ------------------------------------------------
    /// 0.0     0.0     0.0     20          60
    /// 0.1     0.2     0.4     21          59
    /// 0.2     0.4     0.4     19          63
    /// ...
    /// 
    /// nDpoints = new float[4][];
    /// nDpoints[0] = new float[] {0.0, 0.1, 0.2, ...};
    /// nDpoints[1] = new float[] {0.0, 0.2, 0.4, ...};
    /// nDpoints[2] = new float[] {0.0, 0.4, 0.4, ...};
    /// nDpoints[3] = new float[] {20,  21,  19,  ...};
    /// nDpoints[3] = new float[] {60,  59,  63,  ...};
    /// 
    /// </summary>
    public class DataSetMultiDimensionalPoints
    {
        public float[][] nDpoints { get; set; }
        public string[] variables;
        public string[] units;
        public float[] ranges { get; set; }
        public float[] zeroBoundRanges { get; set; }
        public float[] mins { get; set; }
        public float[] zeroBoundMins { get; set; }
        public float[] maxs { get; set; }
        public float[] zeroBoundMaxs { get; set; }
        public float[] ticks { get; set; }
        public int dimension { get; set; }
        public int dataPointCount { get; set; }

        public DataSetMultiDimensionalPoints(float[][] nDpoints, string[] variables, string[] units)
        {
            this.nDpoints = nDpoints;
            this.variables = variables;
            this.units = units;
            this.dimension = variables.Length;
            this.dataPointCount = nDpoints[0].Length;

            // Calculate ranges, minx and maxs
            mins = new float[nDpoints.Length];
            maxs = new float[nDpoints.Length];
            ranges = new float[nDpoints.Length];
            zeroBoundMins = new float[nDpoints.Length];
            zeroBoundMaxs = new float[nDpoints.Length];
            zeroBoundRanges = new float[nDpoints.Length];

            for (int i = 0; i < nDpoints.Length; i++)
            {
                mins[i] = nDpoints[i].Min();
                maxs[i] = nDpoints[i].Max();
                ranges[i] = maxs[i] - mins[i];

                zeroBoundMins[i] = (mins[i] > 0) ? 0 : mins[i];
                zeroBoundMaxs[i] = (maxs[i] < 0) ? 0 : maxs[i];
                zeroBoundRanges[i] = zeroBoundMaxs[i] - zeroBoundMins[i];
            }
        }
        
    }
}
