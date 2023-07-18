using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float currentTime;
    bool timing;

    public void StartTimer()
    {
        timing = true;
    }

    public void StopTimer()
    {
        timing = false;
    }

    public float GetTime()
    {
        return currentTime;
    }

    void Update()
    {
        if (timing == true)
        {
            currentTime += Time.deltaTime;
        }
    }
}
