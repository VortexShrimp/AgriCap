using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseToggle : MonoBehaviour
{
    public delegate void PauseStateChangedDelegate(bool newState);
    public static PauseStateChangedDelegate OnPauseStateChanged;

    [SerializeField]
    GameObject _panel;

    [SerializeField]
    Sprite _pauseIcon;

    [SerializeField]
    Sprite _playIcon;

    public void TogglePanel()
    {
        if (_panel != null)
        {
            bool isActive = _panel.activeSelf;

            // Show/hide the panel.
            _panel.SetActive(!isActive);

            var image = GetComponent<Image>();
            if (image != null)
            {
                // Chnage the display image.
                image.overrideSprite = !isActive ? _playIcon : _pauseIcon;
            }

            // Fire the event to anything that needs this information.
            OnPauseStateChanged?.Invoke(!isActive);

            // Actually pause the game.
            Time.timeScale = !isActive ? 0 : 1;
        }
    }
}
