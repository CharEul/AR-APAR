using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ARObjectManager : MonoBehaviour
{
    [Header("AR References")]
    public GameObject objectToSpawn;
    public ARRaycastManager raycastManager;
    public Button resetButton;

    private GameObject spawnedObject;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Vector2 startTouchPos;
    private float rotationSpeed = 0.3f;
    private float moveSpeed = 0.001f;

    void Start()
    {
        resetButton.onClick.AddListener(ResetObject);
    }

    void Update()
    {
        if (spawnedObject == null)
        {
            HandleSpawn();
        }
        else
        {
            HandleManipulation();
        }
    }

    void HandleSpawn()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    spawnedObject = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
                }
            }
        }
    }

    void HandleManipulation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 newPosition = spawnedObject.transform.position + 
                                      new Vector3(touch.deltaPosition.x * moveSpeed, 0, touch.deltaPosition.y * moveSpeed);
                spawnedObject.transform.position = newPosition;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            float angle = (touchOne.deltaPosition.x - touchZero.deltaPosition.x) * rotationSpeed;
            spawnedObject.transform.Rotate(0, -angle, 0);
        }
    }

    public void ResetObject()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }
}
