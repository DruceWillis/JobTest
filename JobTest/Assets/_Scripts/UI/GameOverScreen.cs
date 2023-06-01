using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : UIScreen
{
    [SerializeField] private RectTransform _buttonsContainer;
    [SerializeField] private RectTransform _slideFromPosition;
    
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private TMP_Text _scoreText;

    public void Awake()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public override void Show()
    {
        _buttonsContainer.localPosition = _slideFromPosition.localPosition;
        _buttonsContainer
            .DOLocalMove(Vector3.zero, 2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => EnableButtons(true));
        
        gameObject.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetFinalScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    protected override void EnableButtons(bool enable)
    {
        _restartButton.interactable = enable;
        _exitButton.interactable = enable;
    }

    private void OnRestartButtonClicked()
    {
        GameStateController.Instance.GameState = eGameState.Fighting;
    }

    private void OnExitButtonClicked()
    {
        GameStateController.Instance.GameState = eGameState.MainMenu;
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}