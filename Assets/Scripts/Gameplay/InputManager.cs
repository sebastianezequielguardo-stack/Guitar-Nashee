using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K, KeyCode.L };

    void Update()
    {
        for (int i = 0; i < laneKeys.Length; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                CheckHit(i);
            }
        }
    }

    void CheckHit(int lane)
    {
        Collider[] hits = Physics.OverlapSphere(GameplayManager.Instance.spawner.lanes[lane].position, 1f);

        foreach (Collider hit in hits)
        {
            Note note = hit.GetComponent<Note>();
            if (note != null && note.lane == lane)
            {
                note.Hit();
                return;
            }
        }

        Debug.Log("❌ Fallo en lane " + lane);
    }
}
