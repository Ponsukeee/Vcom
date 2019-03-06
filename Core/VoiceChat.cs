using System;
using System.Collections;
using MagicOnionClient;
using UnityEngine;

namespace VRSNS.Core
{
public class VoiceChat : MonoBehaviour
{
    private AudioSource audio;
    private AudioClip micClip;
    private float[] samples;
    public event Action<int, float[]> OnSampleReady;
    private int lastIndex = 0;
    private bool isRecording = false;

    private AudioClip playClip;
    private int lengthSamples = 16000;
    private int frequencySamples = 16000;
    private float[] empty;

    private void Awake()
    {
        audio = gameObject.AddComponent<AudioSource>();
//        OnSampleReady += VoiceStream;
        playClip = AudioClip.Create("clip", lengthSamples, 1, frequencySamples, false);
        audio.clip = playClip;
        audio.loop = true;
        samples = new float[lengthSamples/10];
        empty = new float[samples.Length];
    }

    private void Update()
    {
//        if (GamingHubReceiver.IsSynchronizing && !isRecording)
//        {
//            StartVoiceChat();
//            Debug.Log("start");
//        }
//        else if(!GamingHubReceiver.IsSynchronizing && isRecording)
//        {
//            StopVoiceChat();
//        }
        
        //前回再生した配列を空にする
        if (isRecording)
        {
            var playIndex = (int) (audio.Position() * 10);
            if (playIndex == 10) playIndex--;
            
            if (lastIndex != playIndex)
            {
                playClip.SetData(empty, lastIndex * samples.Length);
                lastIndex = playIndex;
            }
        }
    }

    public void StartVoiceChat()
    {
        isRecording = true;
        micClip = Microphone.Start(null, true, 1, frequencySamples);
        StartCoroutine(ReadRawAudio());
    }
    
    public void StopVoiceChat()
    {
        if (!Microphone.IsRecording(null)) return;

        isRecording = false;

        Microphone.End(null);
        Destroy(micClip);
        micClip = null;
        audio.Stop();

        StopCoroutine(ReadRawAudio());
    }

    private int offset;
    private int count;

    public void VoiceStream(int index, float[] segment)
    {
        var playIndex = (int) (audio.Position() * 10);
        if (playIndex == 10) playIndex--;
        var bufferIndex = (index + offset) % 10;

        if (playIndex == bufferIndex)
            offset--;

//        ノイズキャンセル
        int radius = 2;
        for (int i = radius; i < segment.Length - radius; i++)
        {
            float temp = 0;
            for (int j = i - radius; j < i + radius; j++)
                temp += segment[j];
            segment[i] = temp / (2 * radius + 1);
        }

        var localIndex = (index + offset) % 10;
        if(localIndex >= 0 && localIndex < 10)
        {
            playClip.SetData(segment, localIndex * segment.Length);
            Debug.Log(segment[0]);
        }
        
        if(++count == 10 - 1)
            audio.Play();
    }

    private IEnumerator ReadRawAudio()
    {
        int loops = 0;
        int readAbsPos = 0;
        int prevPos = 0;
        int sampleCount = 0;
        var temp = new float[samples.Length];

        while (micClip != null && Microphone.IsRecording(null))
        {
            bool isNewDataAvailable = true;

            while (isNewDataAvailable)
            {
                int currPos = Microphone.GetPosition(null);
                if (currPos < prevPos)
                    loops++;
                prevPos = currPos;

                var currAbsPos = loops * micClip.samples + currPos;
                var nextReadAbsPos = readAbsPos + temp.Length;

                if (nextReadAbsPos < currAbsPos)
                {
                    micClip.GetData(temp, readAbsPos % micClip.samples);
                    samples = temp;
                    sampleCount++;
                    OnSampleReady?.Invoke(sampleCount, samples);
                    Debug.Log("send");

                    readAbsPos = nextReadAbsPos;
                    isNewDataAvailable = true;
                }
                else
                {
                    isNewDataAvailable = false;
                }
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopVoiceChat();
    }
}

public static class Extensions
{
    /// <summary>
    /// Returns the position on of the AudioSource on the AudioClip from 0 to 1.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static float Position(this AudioSource source)
    {
        return (float) source.timeSamples / source.clip.samples;
    }
}
}