using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectController : MonoBehaviour
{
    PostProcessVolume volume;
    Vignette vignette;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        vignette = volume.profile.GetSetting<Vignette>();
    }

    public void setIntensity(float intensity) {
        vignette.intensity.Override(intensity);
    }
}
