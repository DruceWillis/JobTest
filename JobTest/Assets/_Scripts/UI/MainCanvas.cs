using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private Image _blackOverlay;
    [SerializeField] private List<UIScreen> _screens;

    public Action OnOpenMainMenu;
    public Action OnStartFighting;
    
    private UIScreen _currentScreen;

    public void Initialize()
    {
        GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
        _screens.ForEach(s => (s.ScreenType == eScreenType.MainMenu ? (Action)s.Show : s.Hide)());
        _currentScreen = _screens.First(s => s.ScreenType == eScreenType.MainMenu);
    }

    public void SelectScreen(eScreenType screenType)
    {
        _currentScreen.Hide();
        _currentScreen = _screens.First(s => s.ScreenType == screenType);
        _currentScreen.Show();
    }

    public void FadeOverlay(bool fadeIn, Action action = null)
    {
        var newColor = _blackOverlay.color;
        newColor.a = fadeIn ? 1 : 0;
        DOTween.To(() => _blackOverlay.color, x => _blackOverlay.color = x, newColor, 1).OnComplete(() =>
        {
            action?.Invoke();
        });
    }

    private void FadeInOut(Action action)
    {
        _currentScreen.Hide();
        FadeOverlay(true, () =>
        {
            action?.Invoke();
            SelectScreen(Helpers.GetAppropriateScreenTypeByGameState());
            FadeOverlay(false);
        });
    }

    private void OnGameStateChanged(eGameState state)
    {
        switch (state)
        {
            case eGameState.MainMenu:
                FadeInOut(OnOpenMainMenu);
                break;
            case eGameState.Fighting:
                FadeInOut(OnStartFighting);
                break;
            case eGameState.GameOver:
                SelectScreen(Helpers.GetAppropriateScreenTypeByGameState());
                break;
        }
    }
}
