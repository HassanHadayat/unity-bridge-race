using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        transform.position = target.position;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.position + offset;
            transform.position = newPos;
        }
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

}
