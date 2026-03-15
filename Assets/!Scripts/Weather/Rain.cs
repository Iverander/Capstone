using System;
using UnityEngine;

namespace Capstone
{
    public class Rain : Weather
    {
        protected override void Start()
        {
            base.Start();
            Water.SetRippleStrength(1);
        }
    }
}
