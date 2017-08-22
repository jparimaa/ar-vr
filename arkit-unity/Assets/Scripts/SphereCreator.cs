using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
	public class SphereCreator : MonoBehaviour
	{
		public GameObject sphere;

		bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					GameObject go = new GameObject();
					go.transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
					Vector3 p = go.transform.position;
					p.y += 0.3f;
					go.transform.position = p;
					go.transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
					// Create new sphere
					Instantiate(sphere, go.transform);
					return true;
				}
			}
			return false;
		}

		void Update()
		{
			if (Input.touchCount > 0) {
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began) {
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					ARHitTestResultType[] resultTypes = {
						ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
						//ARHitTestResultType.ARHitTestResultTypeExistingPlane, for infinite planes
						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
						ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					}; 

					foreach (ARHitTestResultType resultType in resultTypes) {
						if (HitTestWithResultType(point, resultType)) {
							return;
						}
					}
				}
			}
		}
	}
}
