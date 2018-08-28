using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DataSetLines
    {
        public LineObject[] lineObjects { get; set; }
        public string variableX, unitX, variableY, unitY;

        public DataSetLines(LineObject[] lineObjects, string variableX, string unitX, string variableY, string unitY)
        {
            this.lineObjects = lineObjects;
            this.variableX = variableX;
            this.unitX = unitX;
            this.variableY = variableY;
            this.unitY = unitY;
        }
    }
}
