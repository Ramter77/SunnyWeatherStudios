using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShaderGraph;
using System.Reflection;

[ExecuteInEditMode]
public class ToonShaderLightSettings : MonoBehaviour
{
	private Light light;

	void OnEnable()
	{
		light = GetComponent<Light>();
	}
	
	void Update ()
	{
		//Shader.SetGlobalVector("_LightDirection", -transform.forward);
		Shader.SetGlobalColor("_LightColor", light.color);
		//Shader.SetGlobalVector("_LightColor", light.shadowAngle);
		//light.shadowAttenuation;
		
	}
}
