using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyBandComponent : MonoBehaviour
{
    public int FrequencyBand;
    public float StartScale = 1;
    public float ScaleMultiplier = 2;
    public bool UseBuffer = true;
    public bool UseColorGradient = true;

    private float BandBufferAmount;
    private FrequencyBandSpawner SpawnerComponent;
    private float PreviousBandValue;
    private float StartingBufferAmount;

    private Material Material;

    public void Setup(FrequencyBandSpawner spawner, int frequencyBand)
    {
        this.FrequencyBand = frequencyBand;
        this.SpawnerComponent = spawner;
        this.StartingBufferAmount = this.SpawnerComponent.StartingBufferAmount;
        this.UseBuffer = this.SpawnerComponent.UseBuffer;

        this.BandBufferAmount = this.StartingBufferAmount;
    }

    private void Start()
    {
        this.Material = this.GetComponentInChildren<Renderer>().material;
    }

    void Update()
    {
        var currentBand = this.SpawnerComponent.FrequencyBands[FrequencyBand];
        var currentBandValue = currentBand.Value;

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

        if (this.UseColorGradient)
        {
            var amplitude = currentBand.RelativeAmplitude;
            var newColor = new Color(amplitude, amplitude, amplitude);
            this.Material.SetColor("_EmissionColor", newColor);
        }
    }
}
