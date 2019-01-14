using UnityEngine;
using System.Collections.Generic;

public class LineSmoother : MonoBehaviour 
{

    internal static Vector3 Interpolate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u)
    {
        Vector3 C = 
            Mathf.Pow(1 - u, 3) * a 
            + 3 * u * Mathf.Pow(1 - u, 2) * b 
            + 3 * u * u * (1 - u)*c 
            + u * u * u * d;
        return C;
    }

    public static Vector3[] SmoothLine( Vector3[] inputPoints, float segmentSize )
	{
		//list to write smoothed values to
		List<Vector3> lineSegments = new List<Vector3>();

        for(float i=0; i<1; i+=segmentSize)
        {
            lineSegments.Add(Interpolate(inputPoints[0], inputPoints[1], inputPoints[2], inputPoints[3], i));
        }
        
		return lineSegments.ToArray();
	}

}
