using System.Collections;
using UnityEngine;

public class LightFlickerLoop : MonoBehaviour
{
    public Light spotlight;
    public float flickerMin = 0.05f;
    public float flickerMax = 0.2f;
    public float steadyTime = 5f;
    private AudioManager audioManager;

    private void Start()
    {
        StartCoroutine(FlickerLoop());
    }
    private void Awake()
    {
        audioManager=GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
            if (spotlight == null)
            {
                Debug.LogError("Light component not found on this GameObject!");
                enabled = false; 
            }
        }
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            
            int flickerCount = Random.Range(1, 3);

            for (int i = 0; i < flickerCount * 2; i++)  
            {
                spotlight.enabled = !spotlight.enabled;
                if (audioManager != null && audioManager.vfxClips != null)
                {
                    audioManager.PlaySFX(audioManager.vfxClips);
                }
                float wait = Random.Range(flickerMin, flickerMax);
                yield return new WaitForSeconds(wait);
            }

      
            spotlight.enabled = true;
            yield return new WaitForSeconds(steadyTime);
        }
    }
}