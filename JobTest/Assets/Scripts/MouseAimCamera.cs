using System;
using UnityEngine;

public class MouseAimCamera : MonoBehaviour {

    public GameObject target;

    public float rotateSpeed = 5;

    Vector3 offset;

    private Transform _cachedTransform;

    private void Awake()
    {
        _cachedTransform = transform;
    }

    void Start() {

        offset = target.transform.position - new Vector3(-1, 2, 6);
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        
        float desiredAngle = target.transform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);

        _cachedTransform.position = target.transform.position - (rotation * offset);

        _cachedTransform.LookAt(target.transform);

        _cachedTransform.eulerAngles += new Vector3(-20, 0, 0);
    }

}