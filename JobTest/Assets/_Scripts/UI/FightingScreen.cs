using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightingScreen : UIScreen
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _healthBarImage;

    public void UpdateHealthBar(float healthPercent, float duration)
    {
        DOTween.To(() => _healthBarImage.fillAmount, 
            x => _healthBarImage.fillAmount = x, 
            healthPercent, duration);
    }
    
    public void UpdateScore(int newScore)
    {
        _scoreText.text = newScore.ToString();
    }
}