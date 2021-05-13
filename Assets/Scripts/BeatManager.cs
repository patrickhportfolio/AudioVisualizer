using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
	public float Threshhold;
	public float ThreshholdBias;
	public float Release;

	private float PreviousAudioValue;
	private float AudioValue;
	private float Timer;

	public static bool IsBeat;

	/// <summary>
	/// Inherit this to cause some behavior on each beat
	/// </summary>
	public virtual void OnBeat()
	{
		Debug.Log("beat");
		Timer = 0;
		IsBeat = true;
	}

	public virtual void OnUpdate()
	{
		IsBeat = false;

		// update audio value
		PreviousAudioValue = AudioValue;

		AudioValue = AudioSpectrumManager.GetFrequencyBands(16)[0].Value * 10;

        // if audio value went above the bias during this frame OR
		// if the audio went above the threshhold bias. this helps with bass heavy songs
        if ((PreviousAudioValue <= Threshhold &&
            AudioValue > Threshhold) ||
			(PreviousAudioValue >= Threshhold &&
			AudioValue >= Threshhold + ThreshholdBias))
        {
            // if minimum beat interval is reached
            if (Timer > Release)
            {
                OnBeat();
            }
        }

        Timer += Time.deltaTime;
	}

	private void Update()
	{
		OnUpdate();
	}
}
