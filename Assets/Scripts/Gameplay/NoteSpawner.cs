using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] lanes;
    public float noteSpeed = 5f;

    private List<NoteData> notes = new List<NoteData>();
    private float bpm = 120f;
    private float resolution = 192f;
    private float offset = 0f;

    IEnumerator Start()
    {
        while (string.IsNullOrEmpty(GameplayManager.Instance.selectedChartSection))
            yield return null;

        ExtractChartTiming();
        ParseChart();
    }

    void Update()
    {
        float songTime = GameplayManager.Instance.GetSongTime();

        foreach (NoteData note in notes)
        {
            if (!note.spawned && songTime >= note.time)
            {
                SpawnNote(note);
                note.spawned = true;
            }
        }
    }

    void ExtractChartTiming()
    {
        string chart = GameplayManager.Instance.chartData;

        Match resMatch = Regex.Match(chart, @"Resolution\s*=\s*(\d+)");
        if (resMatch.Success)
            resolution = float.Parse(resMatch.Groups[1].Value);

        Match bpmMatch = Regex.Match(chart, @"B\s+(\d+)");
        if (bpmMatch.Success)
            bpm = float.Parse(bpmMatch.Groups[1].Value) / 1000f;

        Match offsetMatch = Regex.Match(chart, @"Offset\s*=\s*(-?\d+(\.\d+)?)");
        if (offsetMatch.Success)
            offset = float.Parse(offsetMatch.Groups[1].Value);
    }

    void ParseChart()
    {
        string[] lines = GameplayManager.Instance.selectedChartSection.Split('\n');

        foreach (string line in lines)
        {
            if (line.Contains(" = N "))
            {
                string[] parts = line.Split(new[] { " = N " }, System.StringSplitOptions.None);
                string[] noteParts = parts[1].Split(' ');

                if (int.TryParse(parts[0].Trim(), out int tick) &&
                    int.TryParse(noteParts[0], out int laneIndex))
                {
                    if (laneIndex >= 0 && laneIndex <= 4)
                    {
                        float time = TickToSeconds(tick);
                        notes.Add(new NoteData { time = time, laneIndex = laneIndex, spawned = false });
                    }
                }
            }
        }
    }

    void SpawnNote(NoteData note)
    {
        if (note.laneIndex >= 0 && note.laneIndex < lanes.Length)
        {
            Vector3 spawnPosition = lanes[note.laneIndex].position;
            GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);

            Note noteScript = newNote.GetComponent<Note>();
            if (noteScript != null)
            {
                noteScript.lane = note.laneIndex;
                noteScript.speed = noteSpeed;
            }

            Debug.Log("🎯 Nota instanciada en lane " + note.laneIndex);
        }
    }

    float TickToSeconds(int tick)
    {
        return ((tick / resolution) * (60f / bpm)) + offset;
    }

    private class NoteData
    {
        public float time;
        public int laneIndex;
        public bool spawned;
    }
}
