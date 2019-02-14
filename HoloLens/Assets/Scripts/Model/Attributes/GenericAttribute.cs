/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
namespace Model.Attributes
{
    /// <summary>
    /// A generic attribute holds a value for an attribute of arbitrary
    /// type.
    /// 
    /// E.g. a nominal attribute "Cloth Size" with possible values
    /// {XS, S, M, L, XL, XXL, XXXL, 4XL, 5XL} could be instantiated by
    /// 
    /// var size = new GenericValue&lt;string&gt;("size", "XL", LoM.ORDINAL);
    /// </summary>
    /// <typeparam name="T">data type to use</typeparam>
    public class Attribute<T>
    {
        public string name { get; }
        public LoM lom { get; }
        public T value { get; }
        
        /// <summary>
        /// Instantiates an attribute of the given data type.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <param name="lom">Level of Measurement</param>
        public Attribute(string name, T value, LoM lom)
        {
            this.value = value;
            this.name = name;
            this.lom = lom;
        }
    }
}
