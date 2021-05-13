using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioSpectrumManager : MonoBehaviour
{
    public static float[] Samples = new float[512];

    public int NumberOfFrequencyBands = 32;

    private AudioSource AudioSource;

    private static FrequencyBand[] FrequencyBands32;
    private static FrequencyBand[] FrequencyBands16;
    private static FrequencyBand[] FrequencyBands8;
    private static FrequencyBand[] FrequencyBands4;

    // Start is called before the first frame update
    void Start()
    {
        FrequencyBands32 = new FrequencyBand[32];
        FrequencyBands16 = new FrequencyBand[16];
        FrequencyBands8 = new FrequencyBand[8];
        FrequencyBands4 = new FrequencyBand[4];

        this.AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetSpectrumAudio();
        StartCoroutine(ResetBands());
    }

    private void GetSpectrumAudio()
    {
        this.AudioSource.GetSpectrumData(Samples, 0, FFTWindow.Hanning);
    }

    private IEnumerator ResetBands()
    {
        yield return new WaitForEndOfFrame();

        FrequencyBands32.ResetValue();
        FrequencyBands16.ResetValue();
        FrequencyBands8.ResetValue();
        FrequencyBands4.ResetValue();
    }

    private static FrequencyBand[] MakeFrequencyBands(FrequencyBand[] frequencyBands)
    {
        var currentSample = 0;
        for (var i = 1; i <= frequencyBands.Length; i++)
        {
            var totalAmplitude = 0f;

            var sampleIncrement = (int)Mathf.Lerp(2f, Samples.Length - 1f, Mathf.Pow(i, 2) / Mathf.Pow(frequencyBands.Length, 2));
            //Debug.Log("sampleIncrement: " + sampleIncrement);

            for (var j = currentSample; j < sampleIncrement; j++)
            {
                totalAmplitude += Samples[currentSample] * (currentSample + 1);
                currentSample++;
            }

            var averageAmplitude = totalAmplitude / currentSample;

            var frequencyBand = frequencyBands[i - 1] ?? new FrequencyBand();
            frequencyBand.Value = averageAmplitude * 10;
            frequencyBands[i - 1] = frequencyBand;
        }

        return frequencyBands;
    }

    private static FrequencyBand[] SetAmplitudeValues(FrequencyBand[] frequencyBands)
    {
        for (int band = 0; band < frequencyBands.Length; band++)
        {
            var currentBand = frequencyBands[band];

            if (currentBand.Value > currentBand.HighestAmplitude)
            {
                currentBand.HighestAmplitude = currentBand.Value;
            }

            currentBand.RelativeAmplitude = currentBand.Value / currentBand.HighestAmplitude;
        }

        return frequencyBands;
    }

    public static FrequencyBand[] GetFrequencyBands(int numberOfBands)
    {
        switch(numberOfBands)
        {
            case 16:
                return Get16FrequencyBands();
            case 8:
                return Get8FrequencyBands();
            case 4:
                return Get4FrequencyBands();
            default:
                return Get32FrequencyBands();
        }
    }

    private static FrequencyBand[] Get32FrequencyBands()
    {
        if (FrequencyBands32.Any(b => b == null || b.Value == 0))
        {
            FrequencyBands32 = SetAmplitudeValues(MakeFrequencyBands(FrequencyBands32));
        }

        return FrequencyBands32;
    }

    private static FrequencyBand[] Get16FrequencyBands()
    {
        if (FrequencyBands16.Any(b => b == null || b.Value == 0))
        {
            FrequencyBands16 = SetAmplitudeValues(MakeFrequencyBands(FrequencyBands16));
        }

        return FrequencyBands16;
    }

    private static FrequencyBand[] Get8FrequencyBands()
    {
        if (FrequencyBands8.Any(b => b == null || b.Value == 0))
        {
            FrequencyBands8 = SetAmplitudeValues(MakeFrequencyBands(FrequencyBands8));
        }

        return FrequencyBands8;
    }

    private static FrequencyBand[] Get4FrequencyBands()
    {
        if (FrequencyBands4.Any(b => b == null || b.Value == 0))
        {
            FrequencyBands4 = SetAmplitudeValues(MakeFrequencyBands(FrequencyBands4));
        }

        return FrequencyBands4;
    }
}
