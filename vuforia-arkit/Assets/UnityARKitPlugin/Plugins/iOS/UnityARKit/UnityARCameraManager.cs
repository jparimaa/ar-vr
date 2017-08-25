using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class UnityARCameraManager : MonoBehaviour
{
	public Camera m_camera;
	public Vuforia.DefaultTrackableEventHandler handler;

	private Quaternion initRotation;
	private Vector3 initPosition;
	private Vector3 lastArPos = new Vector3(0, 0, 0);
	private float sin = 0.0f;
	private float cos = 0.0f;

	private UnityARSessionNativeInterface m_session;
	private Material savedClearMaterial;

	void Start()
	{		
		m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

#if !UNITY_EDITOR
		Application.targetFrameRate = 60;
		ARKitWorldTackingSessionConfiguration config = new ARKitWorldTackingSessionConfiguration();
		config.planeDetection = UnityARPlaneDetection.Horizontal;
		config.alignment = UnityARAlignment.UnityARAlignmentGravity;
		config.getPointCloudData = true;
		config.enableLightEstimation = true;
		m_session.RunWithConfig(config);

		if (m_camera == null) {
			m_camera = Camera.main;
		}
#else
		//put some defaults so that it doesnt complain
		UnityARCamera scamera = new UnityARCamera();
		scamera.worldTransform = new UnityARMatrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
		Matrix4x4 projMat = Matrix4x4.Perspective(60.0f, 1.33f, 0.1f, 30.0f);
		scamera.projectionMatrix = new UnityARMatrix4x4(projMat.GetColumn(0), projMat.GetColumn(1), projMat.GetColumn(2), projMat.GetColumn(3));

		UnityARSessionNativeInterface.SetStaticCamera(scamera);

#endif

		initRotation = handler.initRotation;
		initPosition = handler.initPosition;
		Vuforia.TrackerManager.Instance.GetTracker<Vuforia.ObjectTracker>().Stop();
		//Vuforia.VuforiaBehaviour.Instance.enabled = false;
		m_camera.transform.localPosition = initPosition;
		sin = Mathf.Sin(Mathf.Deg2Rad * initRotation.eulerAngles.y);
		cos = Mathf.Cos(Mathf.Deg2Rad * initRotation.eulerAngles.y);
	}

	public void SetCamera(Camera newCamera)
	{
		if (m_camera != null) {
			UnityARVideo oldARVideo = m_camera.gameObject.GetComponent<UnityARVideo>();
			if (oldARVideo != null) {
				savedClearMaterial = oldARVideo.m_ClearMaterial;
				Destroy(oldARVideo);
			}
		}
		SetupNewCamera(newCamera);
	}

	private void SetupNewCamera(Camera newCamera)
	{
		m_camera = newCamera;

		if (m_camera != null) {
			UnityARVideo unityARVideo = m_camera.gameObject.GetComponent<UnityARVideo>();
			if (unityARVideo != null) {
				savedClearMaterial = unityARVideo.m_ClearMaterial;
				Destroy(unityARVideo);
			}
			unityARVideo = m_camera.gameObject.AddComponent<UnityARVideo>();
			unityARVideo.m_ClearMaterial = savedClearMaterial;
		}
	}

	void Update()
	{
		if (m_camera != null) {
			Matrix4x4 matrix = m_session.GetCameraPose();

			Vector3 arPos = UnityARMatrixOps.GetPosition(matrix);
			Vector3 diff = arPos - lastArPos;
			Vector3 correctedDiff = new Vector3(
				sin * diff.z + cos * diff.x, 
				diff.y, 
				cos * diff.z - sin * diff.x);
			m_camera.transform.localPosition += correctedDiff;
			lastArPos = arPos;

			Quaternion r = UnityARMatrixOps.GetRotation(matrix);
			m_camera.transform.localRotation = Quaternion.Euler(
				r.eulerAngles.x, 
				r.eulerAngles.y + initRotation.eulerAngles.y, 
				r.eulerAngles.z);

			m_camera.projectionMatrix = m_session.GetCameraProjection();
		}
	}

	void OnGUI()
	{
		GUIStyle style = GUI.skin.GetStyle("TextArea");
		style.fontSize = 30;
		string s = m_camera.transform.localRotation.eulerAngles.ToString("F1") + "\n" + 
				   initRotation.eulerAngles.ToString("F1") + "\n\n" +
		           m_camera.transform.localPosition.ToString("F2") + "\n" + 
				   initPosition.ToString("F2") + "\n" +
				   lastArPos.ToString("F2");
		GUI.TextArea(new Rect(0, 100, 400, 300), s);
	}

}
