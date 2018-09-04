using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DataSetMatrix2x2Nominal
    {
        public string[] categoriesX;
        public string[] categoriesY;
        public float[,] ordinalValues;
        public float range, zeroBoundRange;
        public float min, zeroBoundMin;
        public float max, zeroBoundMax;
        public string ordinalVariable;
        public string ordinalUnit;

        public DataSetMatrix2x2Nominal(string[] categoriesX, string[] categoriesY, float[,] ordinalValues, string ordinalVariable, string ordinalUnit)
        {
            this.categoriesX = categoriesX;
            this.categoriesY = categoriesY;
            this.ordinalValues = ordinalValues;
            this.ordinalVariable = ordinalVariable;
            this.ordinalUnit = ordinalUnit;

            this.min = ordinalValues[0, 0];
            this.max = ordinalValues[0, 0];
            for(int row=0; row<ordinalValues.Rank; row++)
            {
                for(int col=0; col<ordinalValues.GetLength(row); col++)
                {
                    float value = ordinalValues[row, col];
                    if(value < this.min)
                    {
                        this.min = value;
                    }
                    if(value > this.max)
                    {
                        this.max = value;
                    }
                }
            }

            this.range = this.max - this.min;
            this.zeroBoundMin = (this.min > 0) ? 0 : this.min;
            this.zeroBoundMax = (this.max < 0) ? 0 : this.max;
            this.zeroBoundRange = this.zeroBoundMax - this.zeroBoundMin;
        }
    }
}
