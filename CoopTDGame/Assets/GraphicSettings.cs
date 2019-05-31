using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicSettings : MonoBehaviour
{
    /// <summary>
    /// Low, Medium, High, Ultra
    /// </summary>
    /// <param name="qualityIndex"></param>
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
