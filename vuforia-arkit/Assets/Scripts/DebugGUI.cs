using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
	public Vuforia.DefaultTrackableEventHandler handler;

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
		string s = handler.initPosition.ToString("F2") + "\n" + handler.initRotation.eulerAngles.ToString();
		GUI.TextArea(new Rect(0, 0, 200, 100), s);
	}
}
