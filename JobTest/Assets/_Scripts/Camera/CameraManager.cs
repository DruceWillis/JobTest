using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineDollyCart _dollyCart;

    private CinemachineBrain _cinemachineBrain;
    private MainCanvas _mainCanvas;

    private Vector3 _dollyCartOriginalPosition;
    
    public Camera Camera => _camera;

    private void Awake()
    {
        _cinemachineBrain = _camera.GetComponent<CinemachineBrain>();
        _dollyCartOriginalPosition = _dollyCart.transform.position;
    }

    public void Initialize(MainCanvas mainCanvas)
    {
        _mainCanvas = mainCanvas;
        
        _mainCanvas.OnOpenMainMenu += OnOpenMainMenu;
        _mainCanvas.OnStartFighting += OnStartFighting;
    }

    private void OnOpenMainMenu()
    {
        _dollyCart.enabled = true;
        _cinemachineBrain.enabled = true;
    }
    
    private void OnStartFighting()
    {
        _dollyCart.m_Position = 0f;
        _dollyCart.transform.position = _dollyCartOriginalPosition;
        _dollyCart.enabled = false;
        
        _cinemachineBrain.enabled = false;
    }

    private void OnDestroy()
    {
        _mainCanvas.OnOpenMainMenu -= OnOpenMainMenu;
        _mainCanvas.OnStartFighting -= OnStartFighting;
    }
}
