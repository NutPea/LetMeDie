using UnityEngine;

public class PickupHandler : MonoBehaviour
{

    //Wird später durch das Gewicht berechnet
    [SerializeField] private float followSpeed = 2f;
    private bool hasBeenPickedUp = false;
    private Transform followObject;
    private Transform prevParent;

    public void PickUp(Transform transform)
    {

        followObject = transform;
        prevParent = transform.parent;
        transform.parent = null;
        hasBeenPickedUp = true;
    }

    public void Drop()
    {
        transform.parent = prevParent;
        hasBeenPickedUp = false;
    }

    private void Update()
    {
        if (!hasBeenPickedUp)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, followObject.transform.position, followSpeed * Time.deltaTime);

    }

}
