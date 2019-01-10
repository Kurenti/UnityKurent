using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageGrowth : MonoBehaviour {

    public float growthSpeed = 0.5f;

    private float endScale;

	// Use this for initialization
	void Start () {
        endScale = transform.localScale.x;
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
	}

    // FixedUpdate is called once per physics update
    void FixedUpdate () {
        if (transform.localScale.x < endScale)
            transform.localScale += new Vector3(0.5f, 0.5f, 0.5f) * endScale * Time.fixedDeltaTime;
	}
}
