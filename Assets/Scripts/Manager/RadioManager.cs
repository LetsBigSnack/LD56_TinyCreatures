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
    private  List<Track> defaultTracks = new List<Track>();
    
    
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
            defaultTracks = new List<Track>();

            foreach (Track track in tracks)
            {
                Track tempTrack = new Track();
                tempTrack.name = track.name;
                tempTrack.isUnlocked = track.isUnlocked;
                tempTrack.isEnabled = track.isEnabled;
                tempTrack.source = track.source;
                defaultTracks.Add(tempTrack);
            }
            
            CreateSavedTracks();
            UpdateTracks();
        }
    }
    
    public void Start()
    {
        SetupInitialState();
    }

    public void CreateSavedTracks()
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            TrackState currSavedTrack = new TrackState();

            currSavedTrack.name = tracks[i].name;
            currSavedTrack.isEnabled = tracks[i].isEnabled;
            currSavedTrack.isUnlocked = tracks[i].isUnlocked;

            savedTracks.Add(currSavedTrack);
        }
    }
    
    public List<TrackState> GetStateTracks(bool isdefault = true)
    {
        Debug.Log("FFS");
        List<Track> checkTracks = isdefault ? defaultTracks : tracks.ToList();
        List<TrackState> result = new List<TrackState>();
        for (int i = 0; i < checkTracks.Count; i++)
        {
            TrackState currSavedTrack = new TrackState();
            currSavedTrack.name = checkTracks[i].name;
            currSavedTrack.isEnabled = checkTracks[i].isEnabled;
            currSavedTrack.isUnlocked = checkTracks[i].isUnlocked;
            result.Add(currSavedTrack);
        }
        return result;
    }

    public void UpdateSavedTracks(List<TrackState> saveFileTracks)
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            if (saveFileTracks.FirstOrDefault(x => x.name == tracks[i].name) == null)
            {
                TrackState currSavedTrack = new TrackState();
                currSavedTrack.name = tracks[i].name;
                currSavedTrack.isEnabled = tracks[i].isEnabled;
                currSavedTrack.isUnlocked = tracks[i].isUnlocked;
                saveFileTracks.Add(currSavedTrack);
            }
        }
    }

    public void UpdateTracks()
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
        
        if (playedTrack != null && playedTrack.source.isPlaying)
        { 
            Debug.LogWarning("Playing track is already played");
            //song played
            if (tracks.FirstOrDefault(x => x.name == playedTrack.name && x.isEnabled && x.isUnlocked) != null)
            {
                viewedTrack = playedTrack;
                OnChangeViewedTrackValue?.Invoke(viewedTrack);
                OnChangePlayedTrackValue?.Invoke(playedTrack);
                return;
            }
            
            playedTrack.source.Stop();
        }

        if (timeUntilNextSong != null)
        {
            StopCoroutine(timeUntilNextSong);
        }
        
        bool areAllTracksDisabled = tracks.All(x => !x.isEnabled);

        if (!areAllTracksDisabled)
        {
            playedTrack = tracks.FirstOrDefault(x => x.isUnlocked && x.isEnabled);
        }
        else
        {
            playedTrack = tracks.FirstOrDefault(x => x.isUnlocked);
        }
        viewedTrack = playedTrack;
        OnChangeViewedTrackValue?.Invoke(viewedTrack);
        OnChangePlayedTrackValue?.Invoke(playedTrack);
        if (!areAllTracksDisabled)
        {
            StartTrackCoroutine(false);
        }
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
        Track temp = playedTrack;
        playedTrack.source.Stop();
        playedTrack = NextPossibleTrack();
        if (playedTrack == null)
        {
            playedTrack = temp;
            return;
        }
        StartTrackCoroutine(false);
    }

    public void PreviousTrack()
    {
        isPaused = false;
        Track temp = playedTrack;
        playedTrack.source.Stop();
        playedTrack = NextPossibleTrack(-1);
        if (playedTrack == null)
        {
            playedTrack = temp;
            return;
        }
        StartTrackCoroutine(false);
    }

    private Track NextPossibleTrack(int amount = 1, bool isPlayTrack = true)
    {
        List<Track> newTracks = isPlayTrack ? ReturnAllAvailableTracksToPlay() : ReturnAllAvailableTracksToView();

        if (newTracks.Count <= 0)
        {
            Debug.LogWarning("No more tracks available");
            return null;
        }
        
        int index = isPlayTrack ? newTracks.IndexOf(playedTrack) : newTracks.IndexOf(viewedTrack);
        Track track;
        
        if ((index + amount) < 0)
        {
            track = newTracks[newTracks.Count - 1];
        }
        else if ((index + amount) > newTracks.Count - 1)
        {
            track = newTracks[0];
        }
        else
        {
            track = newTracks[index + amount];
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
        
        while (!isPaused && currentTime <= newMaxTime)
        {
            currentTime++;
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
