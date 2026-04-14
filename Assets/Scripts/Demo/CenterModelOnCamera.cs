using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CenterModelOnCamera : MonoBehaviour
{
    public GameObject modelToPlace; // Drag your 3D model here in the Inspector
    public float distanceFromCamera = 2f; // Adjust this value

    private Camera arCamera;

    void Start()
    {
        // Find the AR Camera automatically
        arCamera = FindAnyObjectByType<XROrigin>().GetComponentInChildren<Camera>();
        
        if (arCamera != null && modelToPlace != null)
        {
            // Make the model a child of the AR Camera
            modelToPlace.transform.SetParent(arCamera.transform);
            // Set its local position to be forward relative to the camera
            modelToPlace.transform.localPosition = new Vector3(0, 0, distanceFromCamera);
            // Reset its rotation so it faces forward
            modelToPlace.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("AR Camera or Model to Place is not assigned!");
        }
    }
}