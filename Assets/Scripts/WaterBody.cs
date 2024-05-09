using InfinityCode.RealWorldTerrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrRock.Bathymetry
{
    /// <summary>
    /// Represents a body of water
    /// </summary>
    public class WaterBody : MonoBehaviour
    {
        public string Name;
        public List<WaterDataPoint> DataPoints = new List<WaterDataPoint>();
        public Dictionary<DateTime, DataPointSet> _dataSets = new Dictionary<DateTime, DataPointSet>();

        [SerializeField] private DataPointSet _dataSetPrefab;//Serialized and assigned via drag and drop in editor

        public void PopulateDataSet(WaterDataPoint[] newPoints)
        {

        }
        /// <summary>
        /// Filter the data into dates and position them via the provided container
        /// </summary>
        /// <param name="terrainContainer"></param>
        public void Initialize(RealWorldTerrainContainer terrainContainer)
        {
            foreach (var item in DataPoints)
            {
                if (!_dataSets.ContainsKey(item.CaptureDate))
                {
                    var newDataSet = Instantiate(_dataSetPrefab);
                    newDataSet.name = item.CaptureDate.ToString();
                    newDataSet.gameObject.SetActive(false);
                    _dataSets.Add(item.CaptureDate, newDataSet);
                }
                item.transform.SetParent(_dataSets[item.CaptureDate].transform);
                _dataSets[item.CaptureDate].AddDataPoint(item);
                Vector3 worldPos = Vector3.zero;
                terrainContainer.GetWorldPosition(item.LatLongCoords.Latitude, item.LatLongCoords.Longitude, out worldPos);
                worldPos.y -= item.DepthMeters;
                item.transform.position = worldPos;
            }
            _dataSets[DataPoints[0].CaptureDate].gameObject.SetActive(true);
        }
    }
}