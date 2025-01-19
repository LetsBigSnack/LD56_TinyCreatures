using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Data;
public class RadioManager : MonoBehaviour
{
    public static RadioManager Instance { get; private set; }

    [SerializeField] private List<Track> tracks = new List<Track>();
    [SerializeField] private List<TrackState> savedTracks = new List<TrackState>();

    private Track playedTrack;
    private Track viewedTrack;
    private bool isPaused;
    
    public Action<Track> OnChangePlayedTrackValue;
    public Action<Track> OnChangeViewedTrackValue;
    public Action<float> OnChangeCurrentPlayedTimeValue;

    private IEnumerator timeUntilNextSong;

    public List<TrackState> SavedTracks
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
        //SaveState --> SavedTracks
        //how to handle new songs / etc.
        if(savedTracks.Count > 0) {
            return;
        }
        
        for (int i = 0; i < tracks.Count; i++)
        {
            TrackState currSavedTrack = new TrackState();

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

    public void SetupInitialState()
    {
        viewedTrack = tracks[0];
        playedTrack = tracks[0];
        OnChangeViewedTrackValue?.Invoke(viewedTrack);
        OnChangePlayedTrackValue?.Invoke(playedTrack);
        StartTrackCoroutine(false);
    }

    public Track GetTrack(string trackName)
    {
        return tracks.Where(track => track.name == trackName).FirstOrDefault();
    }

    
    public void UnlockTrack(string trackName)
    {
        Track track = GetTrack(trackName);
        track.isUnlocked = true;
    }

    public void ToggleEnableTrack()
    {
        viewedTrack.isEnabled = !viewedTrack.isEnabled;
    }

    public void ToggleLoopTrack()
    {
        viewedTrack.source.loop = !viewedTrack.source.loop;
    }

    public void PausePlayTrack()
    {
        if (IsViewedTrackPlaying())
        {
            isPaused = true;
            viewedTrack.source.Pause();
        }
        else if (IsViewedTrackPaused())
        {
            isPaused = false;
            StartTrackCoroutine(true);
        }
        else
        {
            isPaused = false;
            playedTrack.source.Stop();
            playedTrack = viewedTrack;
            OnChangePlayedTrackValue?.Invoke(playedTrack);
            StartTrackCoroutine(false);
        }
        
        //observer pattern maybe ??!?!👉👈🥺
        if (UI_RadioItem.Instance != null)
        {
            UI_RadioItem.Instance.PlayButtonChange();
        }
    }

    private bool IsViewedTrackPaused()
    {
        return IsTrackEqual() && IsSongPaused();
    }

    public void StartTrackCoroutine(bool wasPaused)
    {
        if (timeUntilNextSong != null) StopCoroutine(timeUntilNextSong);
        if (wasPaused)
        {
            playedTrack.source.UnPause();
        }
        else
        {
            playedTrack.source.Play();
        }
        timeUntilNextSong = TimeUntilNextSong();
        StartCoroutine(timeUntilNextSong);
    }

    public void NextTrack()
    {
        isPaused = false;
        playedTrack.source.Stop();
        playedTrack = NextPossibleTrack();
        StartTrackCoroutine(false);
    }

    public void PreviousTrack()
    {
        isPaused = false;
        playedTrack.source.Stop();
        playedTrack = NextPossibleTrack(-1);
        StartTrackCoroutine(false);
    }

    private Track NextPossibleTrack(int amount = 1, bool isPlayTrack = true)
    {
        List<Track> tracks = isPlayTrack ? ReturnAllAvailableTracksToPlay() : ReturnAllAvailableTracksToView();
        int index = isPlayTrack ? tracks.IndexOf(playedTrack) : tracks.IndexOf(viewedTrack);
        Track track;

        if ((index + amount) < 0)
        {
            track = tracks[tracks.Count - 1];
        }
        else if ((index + amount) > tracks.Count - 1)
        {
            track = tracks[0];
        }
        else
        {
            track = tracks[index + amount];
        }

        if (isPlayTrack)
        {
            playedTrack = track;
            OnChangePlayedTrackValue?.Invoke(playedTrack);
        }
        else
        {
            viewedTrack = track;
            OnChangeViewedTrackValue?.Invoke(viewedTrack);
        }

        return track;
    }

    private List<Track> ReturnAllAvailableTracksToPlay()
    {
        return tracks.Where(track => track.isEnabled && track.isUnlocked).ToList();
    }

    private List<Track> ReturnAllAvailableTracksToView()
    {
        return tracks.Where(track => track.isUnlocked).ToList();
    }

    public void ViewNextTrack()
    {
        viewedTrack = NextPossibleTrack(1, false);
    }
    public void ViewPreviousTrack()
    {
        viewedTrack = NextPossibleTrack(-1, false);
    }

    public bool IsTrackEqual()
    {
        return viewedTrack == playedTrack;
    }
    public bool IsViewedTrackPlaying()
    {
        if (IsTrackEqual())
        {
            return playedTrack.source.isPlaying;
        }
        return false;
    }

    private bool IsSongPaused()
    {
        return playedTrack.source.time > 0;
    }

    private IEnumerator TimeUntilNextSong()
    {
        float currentTime = playedTrack.source.time;
        float maxTime = playedTrack.source.clip.length;
        float newMaxTime = currentTime > 0 ? maxTime - currentTime : maxTime;
        
        while (!isPaused && playedTrack.source.time <= newMaxTime)
        {
            
            OnChangeCurrentPlayedTimeValue?.Invoke(playedTrack.source.time);
            if (isPaused)
            {
                StopCoroutine(timeUntilNextSong);
            }
            yield return new WaitForSeconds(1f);
        }
        
        if (isPaused)
        {
            StopCoroutine(timeUntilNextSong);
        }

        if (!isPaused) PlayNextSong();
    }

    private void PlayNextSong()
    {
        if (playedTrack.source.loop)
        {
            StopCoroutine(timeUntilNextSong);
            StartTrackCoroutine(false);
            return;
        }
        playedTrack.source.Stop();
        NextPossibleTrack(1);
        OnChangePlayedTrackValue?.Invoke(playedTrack);
        StartTrackCoroutine(false);
    }
}
