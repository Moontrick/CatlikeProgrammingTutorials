using UnityEngine;
using System;

public class ClockAnimator : MonoBehaviour 
{


    private const float hoursToDegrees = 360f / 12f;
    private const float minutesToDegrees = 360f / 60f;
    private const float secondsToDegrees = 360f / 60f;


    [SerializeField]
    private Transform hours, minutes, seconds;


    [SerializeField]
    private bool analog;


    // -----------------------------------------
    // Mono Functions

    void Update()
    {
        if (analog)
            UpdateClockArmRotationAnalog();
        else
            UpdateClockArmRotationDiscrete();
    }


    // -----------------------------------------
    // Private Functions

    private void UpdateClockArmRotationDiscrete()
    {
        DateTime time = DateTime.Now;
        hours.localRotation = Quaternion.Euler(0f, 0f, time.Hour * -hoursToDegrees);
        minutes.localRotation = Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
        seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
    }


    private void UpdateClockArmRotationAnalog()
    {
        TimeSpan timeSpan = DateTime.Now.TimeOfDay;
        hours.localRotation = Quaternion.Euler(0f, 0f, (float)timeSpan.TotalHours * -hoursToDegrees);
        minutes.localRotation = Quaternion.Euler(0f, 0f, (float)timeSpan.TotalMinutes * -minutesToDegrees);
        seconds.localRotation = Quaternion.Euler(0f, 0f, (float)timeSpan.TotalSeconds * -secondsToDegrees);
    }


}
