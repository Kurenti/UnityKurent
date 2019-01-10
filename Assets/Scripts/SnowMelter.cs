using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMelter : MonoBehaviour {

    public Shader _drawShader;
    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;
    private RaycastHit _snowHit;
    private int _snowLayerMask;
    private GameObject _snow;
    
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
                _snow = obj;
                break;
            }
        }

        _snowMaterial = _snow.GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture((int)renderTextureSize, (int)renderTextureSize, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
        _snowMaterial.SetVector("_MeshExtent",
            new Vector4(_snow.GetComponent<Collider>().bounds.extents.x / _snow.GetComponent<Transform>().lossyScale.x,
                        _snow.GetComponent<Collider>().bounds.extents.y / _snow.GetComponent<Transform>().lossyScale.y,
                        _snow.GetComponent<Collider>().bounds.extents.z / _snow.GetComponent<Transform>().lossyScale.z));
    }
	
	// Update is called once per physics update
	void FixedUpdate () {
		if (Physics.Raycast(new Vector3(GetComponent<Transform>().position.x,
                                        _snow.GetComponent<BoxCollider>().bounds.max.y + 1,
                                        GetComponent<Transform>().position.z),
                            Vector3.down,
                            out _snowHit,
                            2, _snowLayerMask))
        {
            //Get normalized unscaled local coordinates with origin at top left
            Vector3 localPosUnscaled = GetComponent<Transform>().position - _snowHit.transform.position + _snowHit.collider.bounds.extents;

            _drawMaterial.SetVector("_Coordinate", new Vector4(localPosUnscaled.x / _snowHit.collider.bounds.size.x,
                                                               localPosUnscaled.z / _snowHit.collider.bounds.size.z,
                                                               0, 0));
            _drawMaterial.SetFloat("_Size", currentBrushSize / 500);
            _drawMaterial.SetFloat("_Strength", brushStrength);
            RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(_splatMap, temp);
            Graphics.Blit(temp, _splatMap, _drawMaterial);
            RenderTexture.ReleaseTemporary(temp);

            //Reduce move speed to snow speed - this now slows down across whole snow bounding box,
            //but to be changed in next update!
            GetComponent<PlayerBehavior>().setSnowSpeed();
        } else {
            //Set speed to ground speed
            GetComponent<PlayerBehavior>().setGroundSpeed();
        }
	}
}
