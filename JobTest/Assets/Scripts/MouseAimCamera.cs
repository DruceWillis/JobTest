using UnityEngine;

public class MouseAimCamera : MonoBehaviour
{

    [SerializeField] private Transform _target;

    private Vector3 _offset;

    private Transform _cachedTransform;

    private void Awake()
    {
        _cachedTransform = transform;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _offset = target.transform.position - new Vector3(-1, 0, 7);
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