using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatWaveSpawner : MonoBehaviour
{
    public GameObject SampleObject;
    public int NumberOfRows;
    public float StartingHeightScale;
    public float StartingRowSize;
    public float RowSizeIncrement;

    public float[] RowHeights;

    // Start is called before the first frame update
    void Start()
    {
        this.RowHeights = new float[this.NumberOfRows];
        var spawnerPosition = this.transform.position;

        for (int row = 0; row < NumberOfRows; row++)
        {
            var parentObject = new GameObject();
            parentObject.transform.parent = this.transform;
            parentObject.name = "Row" + row;

            for (int side = 0; side < 4; side++)
            {
                var gameObject = Instantiate(this.SampleObject);
                var scaleLength = StartingRowSize + RowSizeIncrement * 2 * row;
                var scale = new Vector3(scaleLength, StartingHeightScale, 1);
                gameObject.transform.localScale = scale;

                var rotation = new Vector3(0, 90 * side, 0);
                gameObject.transform.eulerAngles = rotation;

                var position = new Vector3();
                var coord = (scaleLength - 1) / 2;

                if (side % 2 == 0)
                {
                    position.z = coord;
                    position.z *= side == 2 ? -1 : 1;
                }
                else
                {
                    position.x = coord;
                    position.x *= side == 3 ? -1 : 1;
                }

                gameObject.transform.localPosition = position;

                gameObject.transform.parent = parentObject.transform;
                gameObject.name = "Side" + side;
            }

            var beatComp = parentObject.AddComponent<BeatScaleComponent>();
            beatComp.RestScale = parentObject.transform.localScale;
            beatComp.BeatScale = new Vector3(beatComp.RestScale.x, beatComp.RestScale.y * 3, beatComp.RestScale.z);
            beatComp.TimeDelay = row * .1f;
            beatComp.UseColorGradient = true;
        }
    }
}
