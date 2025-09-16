using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        if (playButton == null)
        {
            Debug.LogError("❌ [PlayButton] El botón 'playButton' no está asignado en el Inspector.");
            return;
        }

        playButton.onClick.AddListener(PlayGame);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayButtonStateChanged += UpdateState;
            UpdateState();
        }
        else
        {
            Debug.LogError("❌ [PlayButton] GameManager.Instance es null.");
        }
    }

    void UpdateState()
    {
        bool canPlay = GameManager.Instance.songSelected && GameManager.Instance.difficultySelected;
        playButton.interactable = canPlay;
    }

    void PlayGame()
    {
        if (!GameManager.Instance.songSelected || !GameManager.Instance.difficultySelected)
        {
            Debug.LogWarning("⚠️ [PlayButton] Falta seleccionar canción o dificultad.");
            return;
        }

        SceneManager.LoadScene("Gameplay");
    }
}
