using UnityEngine;

public class MouseAimCamera : MonoBehaviour
{

    [SerializeField] private Transform _target;

    private Vector3 _offset;

    private Transform _cachedTransform;

    public void SetTarget(Transform target)
    {
        _cachedTransform = transform;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        _target = target;
        _offset = target.position - new Vector3(-1, -1.5f, 8.5f);
        _cachedTransform.position = _offset;
        enabled = true;
    }

    void LateUpdate()
    {
        float desiredAngle = _target.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);

        _cachedTransform.position = _target.position - rotation * _offset;

        _cachedTransform.LookAt(_target);
    }

}