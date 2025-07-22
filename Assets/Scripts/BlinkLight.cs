using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    public Light targetLight;
    public float blinkInterval = 0.5f;

    private float timer;

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkInterval)
        {
            targetLight.enabled = !targetLight.enabled;
            timer = 0f;
        }
    }
}
