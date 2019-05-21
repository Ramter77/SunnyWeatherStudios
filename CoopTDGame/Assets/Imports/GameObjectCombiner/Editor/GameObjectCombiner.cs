using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectCombiner : EditorWindow
{
	private delegate void ChangePrefab(GameObject go);
	private static string parentName = "Combined prefab";
	[MenuItem("Window/GameObjectCombiner")]

	//Activate Editor Window
	public static void GameObjectCombineWindow()
	{
		GetWindow<GameObjectCombiner>("GameObjectCombiner");
	}
	//Editor Window Layout
	private void OnGUI()
	{
		GUILayout.Label("Select all GameObjects in your scene you want to combine");
		GUILayout.Space(20);
		GUILayout.Label("Warning: Don't use the same name twice, because \nit will overwrite the prefab with the same name. \nCombined prefab name:");
		parentName = GUILayout.TextField(parentName);
		GUILayout.Space(20);
		
		if (GUILayout.Button("Combine selected GameObjects"))
		{
			IsSelected();
			Combine(Apply);
		}
		GUILayout.Space(20);
		GUILayout.Label("GameObjects Selected: " + Selection.gameObjects.Length);
	}

	public void OnInspectorUpdate()
	{
		// This will only get called 10 times per second.
		Repaint();
	}

	//check if any objects in the scene are selected
	private static bool IsSelected()
	{
		return Selection.activeTransform != null;
	}

	//Check for prefab connections
	private static void Combine(ChangePrefab changePrefabAction)
	{
		GameObject[] selectedGameObjects = Selection.gameObjects;	//the GameObjects that are selected in the scene
		int numberOfGameObjects = selectedGameObjects.Length;		//the amount of the selected GameObjects
		GameObject parent = new GameObject();						//Create a new GameObject that will be the parent of all selected GameObjects
		List<Vector3> vectors = new List<Vector3>();				//List of positions from all selected GameObjects
		//Add all positions to the list
		foreach(GameObject g in Selection.gameObjects)
		{
			vectors.Add(g.transform.position);
		}
		//Calculate the average position from all selected GameObjects
		Vector3 average = Vector3.zero;
		for (int i = 0; i < vectors.Count; i++)
		{
			average += vectors[i];
		}
		average /= vectors.Count;
		//Move the parent to the calculated position
		parent.transform.position = average;
		//Make the selected GameObjects childs from the parent GameObject
		foreach (GameObject g in Selection.gameObjects)
		{
			g.transform.parent = parent.transform;
		}
		try
		{
			for (int i = 0; i < numberOfGameObjects; i++)
			{
				//show a progress bar
				var go = selectedGameObjects[i];
				EditorUtility.DisplayProgressBar("Combining GameObjects", "Combining Object " + go.name + " (" + i + "/" + numberOfGameObjects + ")",
					(float)i / (float)numberOfGameObjects);
			}
		}
		finally
		{
			//remove progress bar,  set the prefabs name and save
			EditorUtility.ClearProgressBar();
			Debug.LogFormat("{0} GameObjects combined", numberOfGameObjects);
			parent.name = parentName;
			Apply(parent);
		}
	}

	//Save the combined GameObjects as a prefab    
	private static void Apply(GameObject go)
	{
		Object targetPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + go.name + ".prefab");
		Debug.Log("Prefab saved in Assets/" + go.name + ".prefab");
		PrefabUtility.ReplacePrefab(go, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
	}
}
