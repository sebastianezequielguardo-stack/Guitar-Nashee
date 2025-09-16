using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SongLoader : MonoBehaviour
{
    public Transform contentParent;
    public GameObject songButtonPrefab;

    void Start()
    {
        LoadSongs();
    }

    void LoadSongs()
    {
        string songsPath = Path.Combine(Application.streamingAssetsPath, "Songs");

        if (!Directory.Exists(songsPath))
        {
            Debug.LogError("❌ Carpeta 'Songs' no encontrada en StreamingAssets.");
            return;
        }

        // Limpiar contenido anterior
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        string[] songFolders = Directory.GetDirectories(songsPath);

        foreach (string folder in songFolders)
        {
            string songName = Path.GetFileName(folder);

            GameObject buttonObj = Instantiate(songButtonPrefab, contentParent);
            buttonObj.transform.localScale = Vector3.one;

            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = songName;
            else
                Debug.LogWarning("⚠️ El prefab no tiene TextMeshProUGUI.");

            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    GameManager.Instance.selectedSongPath = songName;
                    GameManager.Instance.UpdatePlayButtonState();
                    Debug.Log("🎵 Canción seleccionada: " + songName);
                });
            }
            else
            {
                Debug.LogWarning("⚠️ El prefab no tiene componente Button.");
            }
        }
    }
}
