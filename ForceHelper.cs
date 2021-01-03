using UnityEngine;

public class ForceHelper
{
    public static float CalculateMaxSpeed(float forceMagnitude, float mass, float drag)
    {
        return ((forceMagnitude / drag) - Time.fixedDeltaTime * forceMagnitude) / mass;
    }

    public static float ForceNeeded(float targetSpeed, float mass, float drag)
    {
        return (drag * mass * targetSpeed) / (1 - 0.02f * drag);
    }
}