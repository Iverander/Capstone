using System.Collections.Generic;
using UnityEngine;

namespace Capstone
{
    public static class Arithmetic 
    {
        public static float Average(List<float>  values)
        {
            var average = 0f;

            foreach (var rate in values)
            {
                average += rate;
            }

            average /= values.Count;
            return average;
        }
        public static float Lows(List<float>  values)
        {
            var fpsLows = 0f; 
            values.Sort();

            for (int i = 0; i < Mathf.RoundToInt(values.Count / 100); i++)
            {
                fpsLows += values[i];
            }

            fpsLows /= Mathf.RoundToInt(values.Count / 100);
            return fpsLows;
        }
        public static float Highs(List<float>  values)
        {
            var fpsLows = 0f; 
            values.Sort();
            values.Reverse();

            for (int i = 0; i < Mathf.RoundToInt(values.Count / 100); i++)
            {
                fpsLows += values[i];
            }

            fpsLows /= Mathf.RoundToInt(values.Count / 100);
            return fpsLows;

        }
        
        
        public static double Average(List<double>  values)
        {
            var average = (double)0;

            foreach (var rate in values)
            {
                average += rate;
            }

            average /= values.Count;
            return average;
        }
        public static double Lows(List<double>  values)
        {
            var fpsLows = (double)0; 
            values.Sort();

            for (int i = 0; i < Mathf.RoundToInt(values.Count / 100); i++)
            {
                fpsLows += values[i];
            }

            fpsLows /= Mathf.RoundToInt(values.Count / 100);
            return fpsLows;
        }
        public static double Highs(List<double>  values)
        {
            var fpsLows = (double)0; 
            values.Sort();
            values.Reverse();

            for (int i = 0; i < Mathf.RoundToInt(values.Count / 100); i++)
            {
                fpsLows += values[i];
            }

            fpsLows /= Mathf.RoundToInt(values.Count / 100);
            return fpsLows;

        }
    }
}
