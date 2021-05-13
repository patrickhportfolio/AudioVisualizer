using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FrequencyBandHelper
{
    public static void ResetValue(this FrequencyBand[] bandArray)
    {
        bandArray?.ToList()?.ForEach(band => 
        {
            if (band != null)
            {
                band.Value = default;
                band.RelativeAmplitude = default;
            }
        });
    }
}
