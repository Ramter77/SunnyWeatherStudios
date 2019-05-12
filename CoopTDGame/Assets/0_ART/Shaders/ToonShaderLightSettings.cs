using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[ExecuteInEditMode]
public class ToonShaderLightSettings : MonoBehaviour
{
	private Light _light;

	void OnEnable()
	{
		_light = GetComponent<Light>();
	}
	
	void Update ()
	{
		//Shader.SetGlobalVector("_LightDirection", -transform.forward);
		Shader.SetGlobalColor("_LightColor", _light.color);
	}
}
