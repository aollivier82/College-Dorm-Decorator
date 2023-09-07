using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnTouch : MonoBehaviour
{
    public GameObject objectToPlacePrefab;

    private ARRaycastManager _arRaycastManager;
    private GameObject _spawnedObject;
    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (_arRaycastManager.Raycast(touch.position, _hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = _hits[0].pose;

                    // The forward direction for the poster should be opposite to the wall's normal (hitPose.up)
                    Vector3 posterForward = -hitPose.up;

                    // The upward direction for the poster should be aligned with global up to keep it upright
                    Vector3 posterUp = Vector3.up;

                    // Create a rotation that makes the poster parallel to the wall
                    Quaternion rotationParallelToWall = Quaternion.LookRotation(posterForward, posterUp);

                    if (_spawnedObject == null)
                    {
                        _spawnedObject = Instantiate(objectToPlacePrefab, hitPose.position, rotationParallelToWall);
                    }
                    else
                    {
                        _spawnedObject.transform.position = hitPose.position;
                        _spawnedObject.transform.rotation = rotationParallelToWall;
                    }
                }
            }
        }



    }
}