using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineDollyCart _dollyCart;

    private CinemachineBrain _cinemachineBrain;
    public Camera Camera => _camera;

    private void Awake()
    {
        _cinemachineBrain = _camera.GetComponent<CinemachineBrain>();
        GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(eGameState state)
    {
        switch (state)
        {
            case eGameState.MainMenu:
                OnOpenMainMenu();
                break;
            case eGameState.Fighting:
                OnStartFighting();
                break;
        }
    }

    private void OnOpenMainMenu()
    {
        _dollyCart.m_Position = 0f;
        _cinemachineBrain.enabled = true;
    }
    
    private void OnStartFighting()
    {
        _cinemachineBrain.enabled = false;
    }

    private void OnDestroy()
    {
        GameStateController.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
