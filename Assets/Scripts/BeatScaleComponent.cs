using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatScaleComponent : MonoBehaviour
{
	public float RestSmoothTime = 4f;
	public float TimeToBeat = .1f;
	public float TimeDelay;

	public Vector3 BeatScale;
	public Vector3 RestScale;

	public bool UseColorGradient;

	private IEnumerable<Renderer> Rends;

    private void Start()
    {
		this.Rends = this.GetComponentsInChildren<Renderer>();
    }

    public void OnBeat()
	{
		StartCoroutine("TheRealOnBeat");
	}

	private IEnumerator TheRealOnBeat()
    {
		yield return new WaitForSeconds(TimeDelay);
		StopCoroutine("MoveToScale");
		StartCoroutine("MoveToScale", BeatScale);
	}

	private IEnumerator MoveToScale(Vector3 _target)
	{
		Vector3 _curr = transform.localScale;
		Vector3 _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Vector3.Lerp(_initial, _target, _timer / TimeToBeat);
			_timer += Time.deltaTime;

			transform.localScale = _curr;

			yield return null;
		}
	}

    private void Update()
    {
		if (this.UseColorGradient)
		{
			var amplitude = this.transform.localScale.y / this.BeatScale.y;
			var newColor = new Color(amplitude, amplitude, amplitude);
			var propBlock = new MaterialPropertyBlock();
			propBlock.SetColor("_EmissionColor", newColor);
			foreach (var rend in this.Rends)
			{
				rend.SetPropertyBlock(propBlock);
			}
		}
	}

    private void LateUpdate()
	{
		if (BeatManager.IsBeat)
		{
			OnBeat();
		}

		transform.localScale = Vector3.Lerp(transform.localScale, RestScale, RestSmoothTime * Time.deltaTime);
	}
}
