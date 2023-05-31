﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : UIScreen
{
    [SerializeField] private RectTransform _buttonsContainer;
    [SerializeField] private RectTransform _slideFromPosition;
    
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    public void Awake()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public override void Show()
    {
        _buttonsContainer.localPosition = _slideFromPosition.localPosition;
        _buttonsContainer.DOLocalMove(Vector3.zero, 2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => EnableButtons(true));
        
        gameObject.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        EnableButtons(true);
    }

    private void OnRestartButtonClicked()
    {
        GameStateController.Instance.GameState = eGameState.Fighting;
    }

    private void OnExitButtonClicked()
    {
        GameStateController.Instance.GameState = eGameState.MainMenu;
    }

    protected override void EnableButtons(bool enable)
    {
        _restartButton.interactable = enable;
        _exitButton.interactable = enable;
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}