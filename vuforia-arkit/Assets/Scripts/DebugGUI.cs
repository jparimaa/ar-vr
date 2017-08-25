using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnGUI()
	{
		GUIStyle style = GUI.skin.GetStyle ("TextArea");
		style.fontSize = 30;
		GUI.TextArea(new Rect(0, 0, 200, 100), "Ready");
	}
}
