using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using InfinityCode.RealWorldTerrain;

namespace DrRock.Bathymetry
{
    /// <summary>
    /// Main entry point. Loads the csv, builds <see cref="WaterBody"/>s, and populates them with <see cref="WaterDataPoint"/> parsed from the bathy data
    /// </summary>
    public class CSVVisualizer : MonoBehaviour
    {
        [SerializeField] private WaterBody _waterBodyPrefab; //This is serialized so i can drag and drop to assign it in inspector
        [SerializeField] private WaterDataPoint _dataPointPrefab; //This is serialized so i can drag and drop to assign it in inspector
        [SerializeField] private string _csvFileName; //This is serialized so i can drag and drop to assign it in inspector
        [SerializeField] private RealWorldTerrainContainer _cottonwoodCreekBay; //The bing map I generated. Also use it to convert lat/long to Unity world space. Need to make more for each waterbody

        [Tooltip("The name of the water body to visualize")][SerializeField] private string _wbToGenerate;

        public Dictionary<string, WaterBody> WaterBodies = new Dictionary<string, WaterBody>();

        void Start()
        {
            LoadBathymetryData();//currently only loads _wbToGenerate which you can assign in th inspector
            WaterBodies[_wbToGenerate].Initialize(_cottonwoodCreekBay);//Initialize the WaterBody we created, and provide it a Real World Terrain aka Bing Map to use
        }

        /// <summary>
        /// Parse the csv data and set up our data structures
        /// </summary>
        private void LoadBathymetryData()
        {
            string filePath = Path.Combine(Application.dataPath, _csvFileName);
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                // Debug.Log(line);
                string[] tokens = line.Split(',');
                var bodyName = tokens[8];
                if (bodyName != _wbToGenerate)//Takes a long time so it's only set up to do one waterbody at a time currently
                {
                    continue;
                }
                if (!WaterBodies.ContainsKey(bodyName))
                {
                    Debug.Log("Add water body : " + bodyName);
                    var newBody = Instantiate(_waterBodyPrefab, Vector3.zero, Quaternion.identity);
                    newBody.name = bodyName;
                    WaterBodies.Add(bodyName, newBody);
                }

                var newDataPoint = Instantiate(_dataPointPrefab, WaterBodies[bodyName].transform);
                newDataPoint.SetCollectionDate(DateTime.Parse(tokens[0]));
                newDataPoint.SetDepth(float.Parse(tokens[1]));
                newDataPoint.SetTemp(float.Parse(tokens[2]));
                newDataPoint.PH = float.Parse(tokens[3]);
                newDataPoint.Cond_uScm = float.Parse(tokens[4]);
                newDataPoint.DO_mgl = float.Parse(tokens[5]);
                newDataPoint.DO_Percent = float.Parse(tokens[6]);
                newDataPoint.ORP = float.Parse(tokens[7]);
                var lat = float.Parse(tokens[9]);
                var lon = float.Parse(tokens[10]);
                newDataPoint.LatLongCoords = new LatLongCoord(lat, lon);
                WaterBodies[bodyName].DataPoints.Add(newDataPoint);
            }
        }
    }
    public class LatLongCoord
    {
        public float Latitude;
        public float Longitude;

        public LatLongCoord(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}