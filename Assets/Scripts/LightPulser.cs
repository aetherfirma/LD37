using UnityEngine;

public class LightPulser : MonoBehaviour
{
    public Material LightMaterial;

	// Update is called once per frame
    private void Update () {
		LightMaterial.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(Mathf.PingPong(Time.time / 2, 1f)));
	}
}
