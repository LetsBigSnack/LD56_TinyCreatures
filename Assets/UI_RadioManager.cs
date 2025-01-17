using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RadioManager : MonoBehaviour
{
    public static UI_RadioManager Instance { get; private set; }

    private Track playedTrack;
    private Track viewedTrack;

    public Action<Track> OnChangePlayedTrackValue;
    public Action<Track> OnChangeViewedTrackValue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void EnableTrack()
    {
        Track track = RadioManager.Instance.GetTrack(viewedTrack.name);
        track.isEnabled = !track.isEnabled;
    }

    public void PausePlayTrack()
    {
        if (viewedTrack.source.isPlaying)
        {
            viewedTrack.source.Pause();
            return;
        }
        if(viewedTrack == playedTrack)
        {
            viewedTrack.source.UnPause();
            return;
        }
        playedTrack.source.Stop();
        viewedTrack.source.Play();
        playedTrack = viewedTrack;

        OnChangePlayedTrackValue?.Invoke(playedTrack);
    }

    public void NextTrack()
    {
        List<Track> tracks = RadioManager.Instance.Tracks;

        int index = tracks.IndexOf(playedTrack);
        tracks[index].source.Stop();
        if (index == tracks.Count - 1)
        {
            tracks[0].source.Play();
            playedTrack = tracks[0];
            OnChangePlayedTrackValue?.Invoke(playedTrack);
            return;
        }
        tracks[index + 1].source.Play();
        playedTrack = tracks[index + 1];
        OnChangePlayedTrackValue?.Invoke(playedTrack);
    }

    public void PreviousTrack()
    {
        List<Track> tracks = RadioManager.Instance.Tracks;

        int index = tracks.IndexOf(playedTrack);
        tracks[index].source.Stop();
        if (index == 0)
        {
            tracks[tracks.Count - 1].source.Play();
            playedTrack = tracks[tracks.Count - 1];
            OnChangePlayedTrackValue?.Invoke(playedTrack);
            return;
        }
        tracks[index - 1].source.Play();
        playedTrack = tracks[index - 1];
        OnChangePlayedTrackValue?.Invoke(playedTrack);
    }

    public void LoopTrack()
    {
        viewedTrack.source.loop = !viewedTrack.source.loop;
    }

    public void ViewNextTrack()
    {
        List<Track> tracks = RadioManager.Instance.Tracks;

        int index = tracks.IndexOf(viewedTrack);
        if (index == tracks.Count - 1)
        {
            viewedTrack = tracks[0];
            OnChangeViewedTrackValue?.Invoke(viewedTrack);
            return;
        }
        viewedTrack = tracks[index + 1];
        OnChangeViewedTrackValue?.Invoke(viewedTrack);
    }
    public void ViewPreviousTrack()
    {
        List<Track> tracks = RadioManager.Instance.Tracks;

        int index = tracks.IndexOf(viewedTrack);
        if (index == tracks.Count - 1)
        {
            viewedTrack = tracks[0];
            OnChangeViewedTrackValue?.Invoke(viewedTrack);
            return;
        }
        viewedTrack = tracks[index + 1];
        OnChangeViewedTrackValue?.Invoke(viewedTrack);
    }

}
