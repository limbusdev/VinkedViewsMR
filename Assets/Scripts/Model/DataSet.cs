using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSet
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Variables { get; set; }
    public string[] Units { get; set; }
    public IDictionary<string, DataObject> dataObjects {get; set;}

    public DataSet(string title, string description, string[] variables, string[] units, IDictionary<string, DataObject> dataObjects)
    {
        Title = title;
        Description = description;
        Variables = variables;
        Units = units;
        this.dataObjects = dataObjects;
    }
}
