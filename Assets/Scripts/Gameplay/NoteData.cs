public class NoteData
{
    public float time; // Tiempo en segundos
    public int lane;   // Índice del lane (0 a 4)

    public NoteData(float time, int lane)
    {
        this.time = time;
        this.lane = lane;
    }
}
