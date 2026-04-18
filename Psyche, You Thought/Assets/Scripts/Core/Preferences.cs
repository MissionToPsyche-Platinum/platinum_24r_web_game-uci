using System;

[Serializable]
public class Preferences
{
    public int effectivenessMultiplier = 1;
    public int chaosMultiplier = 1;
    public int accuracyMultiplier = 1;

    public Preferences() { }

    public Preferences(int effectiveness, int chaos, int accuracy)
    {
        effectivenessMultiplier = effectiveness;
        chaosMultiplier = chaos;
        accuracyMultiplier = accuracy;
    }

    public static Preferences Random()
    {
        return new Preferences(
            UnityEngine.Random.Range(1, 5),
            UnityEngine.Random.Range(1, 5),
            UnityEngine.Random.Range(1, 5)
        );
    }
}
