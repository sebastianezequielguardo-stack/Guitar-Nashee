using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Note : MonoBehaviour
{
    public int lane; // Asignado por el spawner
    public float speed = 5f;
    public float missThresholdY = -6f;

    void Start()
    {
        // Asegurar que el sprite esté en una capa visible
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Default"; // Cambiá si usás otra capa
        sr.sortingOrder = 10; // Asegura que esté por encima del fondo
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < missThresholdY)
        {
            Miss();
        }
    }

    public void Hit()
    {
        Debug.Log("✅ Nota acertada en lane " + lane);
        Destroy(gameObject);
    }

    public void Miss()
    {
        Debug.Log("❌ Nota fallada en lane " + lane);
        Destroy(gameObject);
    }
}
