using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageGrowth : MonoBehaviour {

    [Range(0.0f, 1.0f)] public float growthSpeed = 0.5f;
    [Range(0.0f, 0.5f)] public float sizeVariance = 0.2f;

    private float endScale;

	// Use this for initialization
	void Start () {
        endScale = transform.localScale.x + sizeVariance * (Random.value * 2 - 1);
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
	}

    // FixedUpdate is called once per physics update
    void FixedUpdate () {
        if (transform.localScale.x < endScale)
            transform.localScale += Vector3.one * endScale * growthSpeed * Time.fixedDeltaTime;
        else
            Destroy(this);
	}
}
