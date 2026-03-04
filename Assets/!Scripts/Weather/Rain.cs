using System;
using UnityEngine;

namespace Capstone
{
    public class Rain : Weather
    {
        private void Start()
        {
            Water.SetRippleStrength(1);
        }
    }
}
