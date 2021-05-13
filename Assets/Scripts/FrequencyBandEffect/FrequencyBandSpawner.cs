using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrequencyBandSpawner : MonoBehaviour
{
    public int NumberOfBands;
    public GameObject SampleObject;
    public FrequencyBand[] FrequencyBands;
    public float StartingBufferAmount = .02f;
    public bool UseBuffer;

    // Start is called before the first frame update
    void Start()
    {
        this.FrequencyBands = new FrequencyBand[this.NumberOfBands];

        for (var i = 0; i < this.NumberOfBands; i++)
        {
            var instantiatedObject = Instantiate(this.SampleObject);
            instantiatedObject.transform.position = this.transform.position;
            instantiatedObject.transform.parent = this.transform;
            instantiatedObject.name = "SampleCube" + i;
            instantiatedObject.transform.localPosition = Vector3.right * (2 * i);
            var freqBandComp = instantiatedObject.AddComponent<FrequencyBandComponent>();
            freqBandComp.Setup(this, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.FrequencyBands = AudioSpectrumManager.GetFrequencyBands(this.NumberOfBands);
    }
}
