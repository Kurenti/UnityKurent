using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMelter : MonoBehaviour {

    public Shader _drawShader;
    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;
    private RaycastHit _groundHit;
    int _snowLayerMask;

    public enum RenderTxtSizes {_1024 = 1024, _2048 = 2048 };
    public RenderTxtSizes renderTextureSize = RenderTxtSizes._1024;

    [Range(1, 10)] public float brushSize = 5;
    [Range(1, 10)] public float brushStrength = 5;
    [HideInInspector] public float currentBrushSize;

    // Use this for initialization
    void Start () {
        currentBrushSize = brushSize;

        _snowLayerMask = LayerMask.GetMask("Snow");
        _drawMaterial = new Material(_drawShader);

        //Get snow material
        var allObjects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in allObjects) {
            if (obj.layer == LayerMask.NameToLayer("Snow")) {
                _snowMaterial = obj.GetComponent<MeshRenderer>().material;
                _splatMap = new RenderTexture((int)renderTextureSize, (int)renderTextureSize, 0, RenderTextureFormat.ARGBFloat);
                _snowMaterial.SetTexture("_Splat", _splatMap);
                _snowMaterial.SetVector("_MeshExtent",
                    new Vector4(obj.GetComponent<Collider>().bounds.extents.x / obj.GetComponent<Transform>().lossyScale.x,
                                obj.GetComponent<Collider>().bounds.extents.y / obj.GetComponent<Transform>().lossyScale.y,
                                obj.GetComponent<Collider>().bounds.extents.z / obj.GetComponent<Transform>().lossyScale.z));
                break;
            }
        }
    }
	
	// Update is called once per physics update
	void FixedUpdate () {
		if (Physics.Raycast(GetComponent<Transform>().position, Vector3.down, out _groundHit, 15, _snowLayerMask))
        {
            //Get normalized unscaled local coordinates with origin at top left
            Vector3 localPosUnscaled = GetComponent<Transform>().position - _groundHit.transform.position + _groundHit.collider.bounds.extents;

            _drawMaterial.SetVector("_Coordinate", new Vector4(localPosUnscaled.x / _groundHit.collider.bounds.size.x,
                                                               localPosUnscaled.z / _groundHit.collider.bounds.size.z,
                                                               0, 0));
            _drawMaterial.SetFloat("_Size", currentBrushSize / 500);
            _drawMaterial.SetFloat("_Strength", brushStrength);
            RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(_splatMap, temp);
            Graphics.Blit(temp, _splatMap, _drawMaterial);
            RenderTexture.ReleaseTemporary(temp);

            //Reduce move speed to snow speed
            GetComponent<PlayerBehavior>().setSnowSpeed();
        } else {
            //Set speed to ground speed
            GetComponent<PlayerBehavior>().setGroundSpeed();
        }
	}
}
