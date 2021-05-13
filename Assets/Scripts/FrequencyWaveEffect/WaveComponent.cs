using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveComponent : MonoBehaviour
{
    public int FrequencyBand;
    public int WaveRow;
    public int ScaleMultiplier = 2;
    public int StartScale = 1;

    private WaveSpawner SpawnerComponent;

    private bool UseBuffer;
    private float BandBufferAmount;
    private float PreviousBandValue;
    private float StartingBufferAmount;

    public void Setup(WaveSpawner spawner, int frequencyBand, int waveRow)
    {
        this.FrequencyBand = frequencyBand;
        this.WaveRow = waveRow;
        this.SpawnerComponent = spawner;
        this.PreviousBandValue = this.SpawnerComponent.FrequencyBands[FrequencyBand, WaveRow];
        this.StartingBufferAmount = this.SpawnerComponent.StartingBufferAmount;
        this.UseBuffer = this.SpawnerComponent.UseBuffer;

        this.BandBufferAmount = this.StartingBufferAmount;
    }

    // Update is called once per frame
    void Update()
    {
        var currentBandValue = this.SpawnerComponent.FrequencyBands[FrequencyBand, WaveRow];
        if (this.UseBuffer)
        {
            // We only buffer to lows so if the amplitude is higher we set that as our new value to compare against.
            if (currentBandValue > this.PreviousBandValue)
            {
                this.PreviousBandValue = currentBandValue;
                this.BandBufferAmount = this.StartingBufferAmount;
            }

            // Buffer the lows to give the "drop" effect
            if (currentBandValue < this.PreviousBandValue)
            {
                this.PreviousBandValue -= this.BandBufferAmount;
                this.BandBufferAmount *= 1.2f;
            }

            // Smooth out the moments of quiet so cubes dont appear to shake.
            if (this.PreviousBandValue < this.StartingBufferAmount)
            {
                this.PreviousBandValue = 0;
            }

            transform.localScale = new Vector3(
                transform.localScale.x,
                (this.PreviousBandValue * ScaleMultiplier) + StartScale,
                transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(
                transform.localScale.x,
                (currentBandValue * ScaleMultiplier) + StartScale,
                transform.localScale.z);
        }
    }
}
