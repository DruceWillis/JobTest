using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] private eScreenType _screenType;

    public eScreenType ScreenType => _screenType;

    public virtual void Show()
    {
        gameObject.SetActive(true);
        EnableButtons(true);
    }
    
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        EnableButtons(false);
    }

    protected virtual void EnableButtons(bool enable) { }
}