using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

[Serializable] public class Track
{
    public string name;
    public AudioSource source;
    public bool isEnabled;
    public bool isUnlocked;
}
[Serializable]
public class SavedTrack
{
    public string name;
    public bool isEnabled;
    public bool isUnlocked;
}

public class RadioManager : MonoBehaviour
{
    public static RadioManager Instance { get; private set; }

    [SerializeField] private List<Track> tracks = new List<Track>();
    [SerializeField] private List<SavedTrack> savedTracks = new List<SavedTrack>();

    public List<SavedTrack> SavedTracks
    {
        get => savedTracks;
        set => savedTracks = value;
    }
    public List<Track> Tracks
    {
        get => tracks;
        set => tracks = value;
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CreateSavedTracks();
            UpdateTracks();
        }
    }

    public void CreateSavedTracks()
    {
        if(savedTracks.Count > 0) {
            return;
        }

        for (int i = 0; i < tracks.Count; i++)
        {
            SavedTrack currSavedTrack = new SavedTrack();

            currSavedTrack.name = tracks[i].name;
            currSavedTrack.isEnabled = tracks[i].isEnabled;
            currSavedTrack.isUnlocked = tracks[i].isUnlocked;

            savedTracks.Add(currSavedTrack);
        }
    }

    private void UpdateTracks()
    {
        for (int i = 0; i < savedTracks.Count; i++)
        {
            Track track = GetTrack(savedTracks[i].name);
            track.isEnabled = savedTracks[i].isEnabled;
            track.isUnlocked = savedTracks[i].isUnlocked;
        }
    }

    public Track GetTrack(string trackName)
    {
        return tracks.Where(track => track.name == trackName).FirstOrDefault();
    }

    
    public void UnlockTrack(string trackName)
    {
        Track track = GetTrack(trackName);
        track.isUnlocked = !track.isUnlocked;
    }

}
