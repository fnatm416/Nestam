using Cinemachine;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float ShakeDuration;
    public float ShakeAmplitudeGain;
    public float ShakeFrequencyGain;

    [SerializeField] CinemachineVirtualCamera VirtualCamera;
    CinemachineBasicMultiChannelPerlin channelPerlin;
    float shakeElapsedTime = 0;

    void Start()
    {
        if (VirtualCamera != null)
            channelPerlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (shakeElapsedTime > 0)
        {
            channelPerlin.m_AmplitudeGain = ShakeAmplitudeGain;
            channelPerlin.m_FrequencyGain = ShakeFrequencyGain;

            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            channelPerlin.m_AmplitudeGain = 0;
            shakeElapsedTime = 0;
        }
    }

    public void CameraShake()
    {
        shakeElapsedTime = ShakeDuration;
    }
}
