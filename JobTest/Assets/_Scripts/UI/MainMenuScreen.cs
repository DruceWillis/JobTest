using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : UIScreen
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;

    public void Awake()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Start()
    {
        EnableButtons(true);
    }

    private void OnPlayButtonClicked()
    {
        GameStateController.Instance.GameState = eGameState.Fighting;
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    protected override void EnableButtons(bool enable)
    {
        _playButton.interactable = enable;
        _exitButton.interactable = enable;
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}