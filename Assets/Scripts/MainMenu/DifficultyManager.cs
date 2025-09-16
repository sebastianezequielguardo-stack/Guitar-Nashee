using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public Button easyButton;
    public Button hardButton;

    void Start()
    {
        if (easyButton == null || hardButton == null)
        {
            Debug.LogError("❌ [DifficultyManager] Botones de dificultad no asignados en el Inspector.");
            return;
        }

        easyButton.onClick.AddListener(() => SetDifficulty("Facil"));
        hardButton.onClick.AddListener(() => SetDifficulty("Dificil"));
    }

    void SetDifficulty(string difficulty)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ [DifficultyManager] GameManager.Instance es null.");
            return;
        }

        GameManager.Instance.selectedDifficulty = difficulty;
        GameManager.Instance.UpdatePlayButtonState();
        Debug.Log("🎯 Dificultad seleccionada: " + difficulty);
    }
}
