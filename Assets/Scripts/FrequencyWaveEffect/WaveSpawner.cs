using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public int NumberOfBands;
    public int WaveRows;
    public GameObject SampleObject;

    public float[,] FrequencyBands;
    public bool UseBuffer;
    public float StartingBufferAmount = .02f;

    // Start is called before the first frame update
    void Start()
    {
        FrequencyBands = new float[NumberOfBands, WaveRows];

        for (var row = 0; row < this.WaveRows; row++)
        {
            for (var band = 0; band < NumberOfBands; band++)
            {
                var instantiatedObject = Instantiate(this.SampleObject);
                instantiatedObject.transform.position = this.transform.position;
                instantiatedObject.transform.parent = this.transform;
                instantiatedObject.name = "SampleCube" + row + " " + band;
                instantiatedObject.transform.localPosition = Vector3.right * (2 * band) + Vector3.forward * (2 * row);
                var waveComponent = instantiatedObject.AddComponent<WaveComponent>();
                waveComponent.Setup(this, band, row);
            }
        }
    }

    void Update()
    {
        this.MoveBands();
        this.MakeFrequencyBands();
    }

    private void MoveBands()
    {
        for (int row = this.WaveRows - 1; row > 0; row--)
        {
            for (int band = this.NumberOfBands - 1; band >= 0; band--)
            {
                this.FrequencyBands[band, row] = this.FrequencyBands[band, row - 1];
            }
        }
    }

    private void MakeFrequencyBands()
    {
        var frequencyBands = AudioSpectrumManager.GetFrequencyBands(this.NumberOfBands);

        for (var band = 0; band < frequencyBands.Length; band++)
        {
            this.FrequencyBands[band, 0] = frequencyBands[band].Value;
        }
    }
}
