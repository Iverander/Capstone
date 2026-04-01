using System;
using UnityEngine;

namespace Capstone
{
    public class Rain : Weather
    {
        public override void  Apply() 
        {
            base.Apply();
            //Water.UpdateRipple?.Invoke(1);
        }
    }
}
