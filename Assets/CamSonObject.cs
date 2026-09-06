using UnityEngine;

public class CamSonObject : MonoBehaviour
{
    [Header("Distance Settings")]
    public float distanceInFront;
    public float scaleSize;

    public void BondToCam(GameObject childObject)
    {
        childObject.transform.SetParent(transform);

        Centralize(childObject);
        RotateForward(childObject);
        Scale(childObject);

        Debug.Log("Objeto esta vinculado a Camera!");
    }

    public void Centralize(GameObject childObject)
    {
        childObject.transform.localPosition = new Vector3(0, 0, distanceInFront);
    }

    public void RotateForward(GameObject childObject)
    {
        childObject.transform.localRotation = Quaternion.identity;
    }

    public void Scale(GameObject childObject)
    {
        childObject.transform.localScale *= scaleSize;
    }
}
