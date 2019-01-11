using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageGrowth : MonoBehaviour {

    [Range(0.0f, 1.0f)] public float growthSpeed = 0.5f;
    [Range(0.0f, 0.5f)] public float sizeVariance = 0.2f;

    private float yOffset;
    private Vector3 endScale;

	// Use this for initialization
	void Start () {
        yOffset = 0.3f;
        endScale = transform.localScale * (1 + sizeVariance * (Random.value * 2 - 1));
        transform.localScale = Vector3.zero;
        transform.position -= new Vector3(0.0f, yOffset, 0.0f);
	}

    // FixedUpdate is called once per physics update
    void FixedUpdate () {
        if (transform.localScale.x < endScale.x)
        {
            transform.position += new Vector3(0.0f, yOffset * growthSpeed * Time.fixedDeltaTime, 0.0f);
            transform.localScale += endScale * growthSpeed * Time.fixedDeltaTime;
        }
        else
            Destroy(this);
	}
}
