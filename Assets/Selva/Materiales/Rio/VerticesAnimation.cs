using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class VerticesAnimation : MonoBehaviour
{
    Material m_Material;
    float counter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        m_Material.SetFloat("_DeltaTime", counter);
        counter += Time.deltaTime;
    }
}
