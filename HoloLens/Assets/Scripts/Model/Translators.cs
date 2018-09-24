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
