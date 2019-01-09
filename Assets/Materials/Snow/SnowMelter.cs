using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMelter : MonoBehaviour {

    public Shader _drawShader;
    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;
    public Transform player;
    RaycastHit _groundHit;
    int _snowLayerMask;

    [Range(0, 1)] public float _brushSize;
    [Range(0, 5)] public float _brushStrength;

    // Use this for initialization
    void Start () {
        _snowLayerMask = LayerMask.GetMask("Snow");

        _drawMaterial = new Material(_drawShader);

        _snowMaterial = GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
        _snowMaterial.SetVector("_MeshExtent",
            new Vector4(GetComponent<Collider>().bounds.extents.x / GetComponent<Transform>().lossyScale.x,
                        GetComponent<Collider>().bounds.extents.y / GetComponent<Transform>().lossyScale.y,
                        GetComponent<Collider>().bounds.extents.z / GetComponent<Transform>().lossyScale.z));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Physics.Raycast(player.position, Vector3.down, out _groundHit, 15, _snowLayerMask))
        {
            //Get normalized unscaled local coordinates with origin at top left
            Vector3 localPosUnscaled = player.position - _groundHit.transform.position + _groundHit.collider.bounds.extents;

            _drawMaterial.SetVector("_Coordinate", new Vector4(localPosUnscaled.x / _groundHit.collider.bounds.size.x,
                                                               localPosUnscaled.z / _groundHit.collider.bounds.size.z,
                                                               0, 0));
            _drawMaterial.SetFloat("_Size", _brushSize);
            _drawMaterial.SetFloat("_Strength", _brushStrength);
            RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(_splatMap, temp);
            Graphics.Blit(temp, _splatMap, _drawMaterial);
            RenderTexture.ReleaseTemporary(temp);

            //Also slow down player move speed - change this script to be owned by player so
            //this works correctly in case of multiple snow patches
            player.GetComponent<PlayerControlsBehavior>().setSnowSpeed();
        } else
        {
            //Change player move speed to normal - change this script to be owned by player so
            //this works correctly in case of multiple snow patches
            player.GetComponent<PlayerControlsBehavior>().setGroundSpeed();
        }
	}
}
