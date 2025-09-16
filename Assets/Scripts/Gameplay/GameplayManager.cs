using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Audio")]
    public AudioSource musicSource;

    [Header("Spawner")]
    public NoteSpawner spawner;

    public string chartData;
    public string selectedChartSection;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(LoadAudio());
        LoadChartSection();
    }

    IEnumerator LoadAudio()
    {
        string songFolder = Path.Combine(Application.streamingAssetsPath, "Songs", GameManager.Instance.selectedSongPath);
        string audioPath = Path.Combine(songFolder, "song.ogg");

        if (!File.Exists(audioPath))
        {
            Debug.LogError("❌ Audio no encontrado: " + audioPath);
            yield break;
        }

        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + audioPath, AudioType.OGGVORBIS);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error al cargar audio: " + www.error);
        }
        else
        {
            musicSource.clip = DownloadHandlerAudioClip.GetContent(www);
            musicSource.volume = 0.4f;
            musicSource.Play();
        }
    }

    void LoadChartSection()
    {
        string songFolder = Path.Combine(Application.streamingAssetsPath, "Songs", GameManager.Instance.selectedSongPath);
        string chartPath = Path.Combine(songFolder, "notes.chart");

        if (!File.Exists(chartPath))
        {
            Debug.LogError("❌ Chart no encontrado: " + chartPath);
            return;
        }

        chartData = File.ReadAllText(chartPath);

        string[] possibleTags = GameManager.Instance.selectedDifficulty == "Facil"
            ? new[] { "[EasySingle]", "[EasyGuitar]", "[Single]" }
            : new[] { "[HardSingle]", "[HardGuitar]", "[ExpertSingle]", "[ExpertGuitar]" };

        foreach (string tag in possibleTags)
        {
            int startIndex = chartData.IndexOf(tag);
            if (startIndex != -1)
            {
                int endIndex = chartData.IndexOf('[', startIndex + tag.Length);
                if (endIndex == -1) endIndex = chartData.Length;

                selectedChartSection = chartData.Substring(startIndex, endIndex - startIndex);
                Debug.Log("🎯 Sección de dificultad encontrada: " + tag);
                return;
            }
        }

        Debug.LogError("❌ No se encontró ninguna sección válida para la dificultad seleccionada.");
    }

    public float GetSongTime()
    {
        if (musicSource != null && musicSource.clip != null && musicSource.isPlaying)
            return musicSource.time;
        else
            return 0f;
    }
}
