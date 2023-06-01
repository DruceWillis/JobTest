using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _target;

    private Transform _cachedTransform;
    private Vector3 _offset = new Vector3(0, -1.5f, 8.5f);

    public void SetTarget(Transform target)
    {
        _cachedTransform = transform;
        _target = target;
        
        ResetCameraPosition();
    }

    public void ResetCameraPosition()
    {
        _cachedTransform.position = _target.position - _offset;
        _cachedTransform.rotation = Quaternion.identity;
    }
    
    void LateUpdate()
    {
        if (GameStateController.Instance.GameState != eGameState.Fighting) return;
        
        float desiredAngle = _target.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        
        var calculatedPos = _target.position - rotation * _offset;
        var newPos = new Vector3(calculatedPos.x, _target.position.y - _offset.y, calculatedPos.z);
        
        _cachedTransform.position = newPos;
        _cachedTransform.LookAt(_target);
    }

}