using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrRock.Bathymetry
{
    /// <summary>
    /// Just a container for <see cref="WaterDataPoint"/>
    /// </summary>
    public class DataPointSet : MonoBehaviour
    {
        public List<WaterDataPoint> DataPoints = new List<WaterDataPoint>();

        public void AddDataPoint(WaterDataPoint newPoint)
        {
            DataPoints.Add(newPoint);
        }
    } 
}
