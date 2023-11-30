using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundSettings
{
    public static bool IsSoundMuted { get; private set; } = false;

    public static void SetSoundMuted(bool state)
    {
        IsSoundMuted = state;
    }
}
