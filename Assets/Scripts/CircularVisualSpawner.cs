using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularVisualSpawner : MonoBehaviour
{
    public GameObject SampleObject;
    public int MaxScale;

    private GameObject[] SampleObjectArray = new GameObject[512];

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < 512; i++)
        {
            var instantiatedObject = Instantiate(this.SampleObject);
            instantiatedObject.transform.position = this.transform.position;
            instantiatedObject.transform.parent = this.transform;
            instantiatedObject.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instantiatedObject.transform.position = Vector3.forward * 200;
            SampleObjectArray[i] = instantiatedObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(var i = 0; i < SampleObjectArray.Length; i++)
        {
            this.SampleObjectArray[i].transform.localScale = new Vector3(1, (AudioSpectrumManager.Samples[i] * this.MaxScale) + 2, 1);
        }
    }
}
