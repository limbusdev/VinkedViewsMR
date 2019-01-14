/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
