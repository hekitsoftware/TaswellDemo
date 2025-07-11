using UnityEngine;

// Serializable struct to define a music track with a name and audio clip
[System.Serializable]
public struct MusicTrack
{
    public string trackName;  // Name of the music track
    public AudioClip clip;    // Audio clip associated with the track
}

// This class holds a list of music tracks and allows retrieval by name
public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks; // Array of all music tracks in the library

    /// <summary>
    /// Returns the AudioClip that matches the given track name.
    /// </summary>
    /// <param name="trackName">The name of the track to find</param>
    /// <returns>The AudioClip if found; otherwise, null</returns>
    public AudioClip GetClipFromName(string trackName)
    {
        // Loop through all tracks in the library
        foreach (var track in tracks)
        {
            // If the track name matches, return the corresponding clip
            if (track.trackName == trackName)
            {
                return track.clip;
            }
        }

        // If no track was found, return null
        return null;
    }
}
