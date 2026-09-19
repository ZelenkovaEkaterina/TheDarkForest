using UnityEngine;

public class MovementCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 12f;
    public float minDistance = 6f, maxDistance = 20f;
    public float zoomSpeed = 8f;

    public float pitch = 50f; 
    public float yaw = 45f;                 
    public float rotateSpeed = 90f;

    public float followLerp = 8f;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    void Update()
    {
        if (!target) return;
        
        if (Input.GetKey(KeyCode.Q)) yaw -= rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) yaw += rotateSpeed * Time.deltaTime;
        if (Input.GetMouseButton(2)) yaw += Input.GetAxis("Mouse X") * 3f;
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPos = target.position + offset - rot * Vector3.forward * distance;

        transform.position = Vector3.Lerp(transform.position, desiredPos, followLerp * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, followLerp * Time.deltaTime);
    }
}
