using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBarController : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    
    public void UpdateHealthBar(float healthPercent, float duration)
    {
        DOTween.To(() => _healthBarImage.fillAmount, 
            x => _healthBarImage.fillAmount = x, 
            healthPercent, duration);
    }

    public void LookAtPlayerCamera(Vector3 cameraPosition)
    {
        var lookAtPos = cameraPosition;
        lookAtPos.y = transform.position.y;
        transform.LookAt(lookAtPos);
    }
}