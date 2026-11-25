using System.Collections.Generic;
using UnityEngine;

// small helper so any script can play a one shot sound by name.
// clips live in Assets/Audio/Resources/SFX
public static class SFX
{
    static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();

    public static AudioClip Get(string name)
    {
        if (!cache.TryGetValue(name, out AudioClip clip))
        {
            clip = Resources.Load<AudioClip>("SFX/" + name);
            cache[name] = clip;
        }
        return clip;
    }

    public static void Play(string name, Vector3 pos, float volume = 1f)
    {
        var clip = Get(name);
        if (clip != null) AudioSource.PlayClipAtPoint(clip, pos, volume);
    }
}
