using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UnderWaterDistortion : MonoBehaviour
{
    public PostProcessVolume volume;
    private LensDistortion _distortion;

    private void Start()
    {
        volume.profile.TryGetSettings(out _distortion);
    }

    private void Update()
    {
        if (_distortion is not null)
            _distortion.intensity.value = -30f + Mathf.Sin(Time.time * 0.5f) * 5f;
    }
}