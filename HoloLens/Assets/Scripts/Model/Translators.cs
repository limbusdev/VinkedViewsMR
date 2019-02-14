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
using System;
using System.Collections.Generic;

namespace Model
{
    public static class OrdinalValueTranslator
    {
        public static IDictionary<int, string> CreateDictionary(string dictionaryFileContent)
        {
            var dict = new Dictionary<int, string>();
            string[] lines = dictionaryFileContent.Split("\n"[0]);

            foreach(var line in lines)
            {
                string[] columns = line.Split(":"[0]);
                dict.Add(int.Parse(columns[0]), columns[1]);
            }


            return dict;
        }
    }

    public static class IntervalValueConverters
    {
        public static string Translate(int value, string translator)
        {
            switch(translator)
            {
                case "date": return Date.Convert(value);
                case "day": return Day.Convert(value);
                case "minute": return Minute.Convert(value);
                case "year": return Year.Convert(value);
                default: return "";
            }
        }

        public static class Date
        {
            public static string Convert(int value)
            {
                DateTime date = new DateTime(1899, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc);
                date.AddDays(value);
                return date.ToString("d");
            } 
        }

        public class Minute
        {
            public static string Convert(int value)
            {
                return (value/60) + ":" + (value-(value/60));
            }
        }

        public class Year
        {
            public static string Convert(int value)
            {
                return value.ToString();
            }
        }

        public class Day
        {
            public static string Convert(int value)
            {
                return value.ToString();
            }
        }
    }
}
