using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoSingleton<ScreenShake>
{
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake() {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void ShakeScreen(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
