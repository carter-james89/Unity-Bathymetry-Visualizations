using System;
using UnityEngine;

namespace DrRock.Bathymetry
{
    /// <summary>
    /// Represents a point generated from Bathy Data
    /// </summary>
    public class WaterDataPoint : MonoBehaviour
    {
        public WaterBody WaterBody;
        public LatLongCoord LatLongCoords;
        public float ORP;
        public float DO_mgl;
        public float DO_Percent;
        public float Cond_uScm;
        public float PH;
        public float Temp;
        public float DepthMeters;
        public DateTime CaptureDate;

        [SerializeField] private TMPro.TextMeshPro _depthText;//serialized and dragged and dropped in editor
        [SerializeField] private TMPro.TextMeshPro _tempText;//serialized and dragged and dropped in editor
        [Tooltip("The mesh that will change color")][SerializeField] private MeshRenderer _renderer;//serialized and dragged and dropped in editor

        public void SetDepth(float depth)
        {
            DepthMeters = depth;
            _depthText.text = "Depth : " + depth.ToString();
        }
        public void SetTemp(float temp)
        {
            Temp = temp;
            _tempText.text = "Temp : " + temp.ToString();
            _renderer.material.color = GetTemperatureColor(temp);
        }

        private Color GetTemperatureColor(float temp)
        {
            // Normalize temp to 0-1
            float t = MapValue(temp, 0, 30, 0, 1);// use 0 as cold and 30 as hot

            // Create a gradient: Blue (cold) -> White (neutral) -> Red (hot)
            if (t < 0.5f) // From 0 to 0.5, lerp between blue and white
                return Color.Lerp(Color.blue, Color.white, t * 2); // Scale t to 0-1 range for this segment
            else // From 0.5 to 1, lerp between white and red
                return Color.Lerp(Color.white, Color.red, (t - 0.5f) * 2); // Adjust and scale t for this segment
        }
        private float MapValue(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }

        internal void SetCollectionDate(DateTime dateTime)
        {
            CaptureDate = dateTime;
        }
    }
}
