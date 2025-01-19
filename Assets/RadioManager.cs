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
    public bool isPaused;
    public float currentPlayedTime;

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

    public void SetupInitialState()
    {
        viewedTrack = tracks[0];
        playedTrack = tracks[0];
        OnChangeViewedTrackValue?.Invoke(viewedTrack);
        OnChangePlayedTrackValue?.Invoke(playedTrack);
        StartTrackCoroutine(false);
    }

    public void CreateSavedTracks()
    {
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

    public Track GetTrack(string trackName)
    {
        return tracks.Where(track => track.name == trackName).FirstOrDefault();
    }

    
    public void UnlockTrack(string trackName)
    {
        Track track = GetTrack(trackName);
        track.isUnlocked = !track.isUnlocked;
    }

    public void EnableTrack()
    {
        viewedTrack.isEnabled = !viewedTrack.isEnabled;
    }

    public void LoopTrack()
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
        else if (IsTrackEqual() && IsSongPaused())
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

        if (UI_RadioItem.Instance != null)
        {
            UI_RadioItem.Instance.PlayButtonChange();
        }
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
            currentPlayedTime = 0;
            OnChangeCurrentPlayedTimeValue?.Invoke(currentPlayedTime);
            playedTrack.source.Play();
        }
        timeUntilNextSong = TimeUntilNextSong(currentPlayedTime, playedTrack.source.clip.length);
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
        List<Track> enabledTracks = new List<Track>();
        foreach (Track track in tracks)
        {
            if (track.isEnabled && track.isUnlocked)
            {
                enabledTracks.Add(track);
            }
        }
        return enabledTracks;
    }

    private List<Track> ReturnAllAvailableTracksToView()
    {
        List<Track> unlockedTracks = new List<Track>();
        foreach (Track track in tracks)
        {
            if (track.isUnlocked)
            {
                unlockedTracks.Add(track);
            }
        }
        return unlockedTracks;
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

    private IEnumerator TimeUntilNextSong(float currentTime, float maxTime)
    {
        float newMaxTime = currentTime > 0 ? maxTime - currentTime : maxTime;
        while (!isPaused && currentPlayedTime <= newMaxTime)
        {
            currentPlayedTime++;
            OnChangeCurrentPlayedTimeValue?.Invoke(currentPlayedTime);
            if (isPaused)
            {
                StopCoroutine(timeUntilNextSong);
            }
            yield return new WaitForSeconds(1f);
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
        Track track = NextPossibleTrack(1);
        OnChangePlayedTrackValue?.Invoke(playedTrack);
        StartTrackCoroutine(false);
    }
}
