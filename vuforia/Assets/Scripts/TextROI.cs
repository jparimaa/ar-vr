using UnityEngine;
using System.Collections;
using Vuforia;

public class TextROI : MonoBehaviour, IVideoBackgroundEventHandler
{
	public GUISkin skin;
	
	private bool videoBackgroundChanged = true;
	private Rect trackingRect = new Rect (Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.8f, Screen.height * 0.4f);
	private Rect detectionRect = new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.2f);

	void Update ()
	{
		if (videoBackgroundChanged) {
			Debug.Log ("ROI Added");
			TextTracker textTracker = (TextTracker)TrackerManager.Instance.GetTracker<TextTracker> ();
			textTracker.SetRegionOfInterest (detectionRect, trackingRect);
			videoBackgroundChanged = false;
		}
	}

	void OnGUI ()
	{
		GUI.skin = skin;
		GUI.Box (detectionRect, "Detection box");
	}

	#region IVideoBackgroundEventHandler_IMPLEMENTATION

	// set a flag that the video background has changed. This means the region of interest has to be set again.
	public void OnVideoBackgroundConfigChanged ()
	{
		videoBackgroundChanged = true;
	}

	#endregion // IVideoBackgroundEventHandler_IMPLEMENTATION
}
