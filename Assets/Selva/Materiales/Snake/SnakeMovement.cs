using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float speed = 1.0f;      // Velocidad del movimiento
    public float distance = 2.0f;   // Distancia máxima a los lados

    private float startZ;

    void Start()
    {
        startZ = transform.position.z;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        Vector3 pos = transform.position;
        pos.z = startZ + offset;
        transform.position = pos;
    }
}
