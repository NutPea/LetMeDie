using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float offsetSpeed = 10f;

    void Update()
    {
        transform.position = _camera.position;
        transform.forward = Vector3.Lerp(transform.forward,_camera.transform.forward, offsetSpeed * Time.deltaTime);
    }
}
