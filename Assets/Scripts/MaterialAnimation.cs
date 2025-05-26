using UnityEngine;

public class MaterialAnimation : MonoBehaviour
{
    private static readonly int DeltaTime = Shader.PropertyToID("_DeltaTime");
    private Material _mMaterial;
    private float _counter = 0.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _mMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        _mMaterial.SetFloat(DeltaTime, _counter);
        _counter += Time.deltaTime;
    }
}
