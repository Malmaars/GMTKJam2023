using System.Collections;
using UnityEngine;
// using XInputDotNetPure;

public class ImpactController : MonoBehaviour
{
    public static ImpactController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    public void CreateImpact(float intensity)
    {
        ApplyScreenshake(intensity);
        switch (intensity)
        {
            case > 0.9f:
                ApplyHitstun(0.25f);
        //        GlobalSfxPlayer.instance.Play(SFX.ImpactHeavy);
                break;
            case > 0.75f:
                ApplyHitstun(0.1f);
         //       GlobalSfxPlayer.instance.Play(SFX.ImpactMedium);
                break;
            default:
          //      GlobalSfxPlayer.instance.Play(SFX.ImpactLight);
                break;
        }
    }

    private void ApplyScreenshake(float intensity)
    {
        StartCoroutine(_ApplyScreenshake(intensity));
        // StartCoroutine(_VibrateController(intensity * 0.5f));
    }
    
    private IEnumerator _ApplyScreenshake(float initIntensity)
    {
        float intensity = initIntensity;
        while (intensity > 0)
        {
            intensity = Mathf.Lerp(intensity, 0, 0.1f);
            if (intensity < 0.1f) intensity = 0;

            transform.parent.position = new Vector3(0, 0, -10) + new Vector3(RNG(intensity), RNG(intensity));
            yield return new WaitForSeconds(0.0167f);
        }
    }
    
    // private IEnumerator _VibrateController(float duration)
    // {
    //     GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
    //     yield return new WaitForSeconds(duration);
    //     GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
    // }
    
    private void ApplyHitstun(float seconds)
    {
        StartCoroutine(_ApplyHitstun(seconds));
    }

    private float RNG(float intensity)
    {
        return Random.Range(0, intensity);
    }

    private IEnumerator _ApplyHitstun(float seconds)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(seconds * 0.1f);
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        EventManager<float>.RemoveListener(EventType.CreateImpact, CreateImpact);
    }
}
