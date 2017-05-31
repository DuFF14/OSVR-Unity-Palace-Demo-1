//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: Masks out pixels that cannot be seen through the connected hmd.
//
//=============================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SteamVR_CameraMask : MonoBehaviour
{
	static Material material;
	static Mesh[] hiddenAreaMeshes = new Mesh[] { null, null };

	MeshFilter meshFilter;
    public Mesh eyeMesh;

	void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
      //  eyeMesh = Resources.Load<Mesh>("leftEyeMesh");
        if (material == null)
			material = new Material(Shader.Find("Custom/SteamVR_HiddenArea"));

		var mr = GetComponent<MeshRenderer>();
		mr.material = material;
		mr.shadowCastingMode = ShadowCastingMode.Off;
		mr.receiveShadows = false;
#if !(UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0)
		mr.lightProbeUsage = LightProbeUsage.Off;
#else
		mr.useLightProbes = false;
#endif
		mr.reflectionProbeUsage = ReflectionProbeUsage.Off;
	}

	/*public void Set(SteamVR vr, Valve.VR.EVREye eye)
	{
		int i = (int)eye;
		if (hiddenAreaMeshes[i] == null)
			hiddenAreaMeshes[i] = SteamVR_Utils.CreateHiddenAreaMesh(vr.hmd.GetHiddenAreaMesh(eye), vr.textureBounds[i]);
		meshFilter.mesh = hiddenAreaMeshes[i];

        
	}*/

    public void SetMesh()
    {
        meshFilter.mesh = eyeMesh == null ? Resources.Load<Mesh>("leftEyeMesh") : eyeMesh;
        UpdateMeshScale();

    }

    public void SetMesh(string meshResourceName)
    {
        meshFilter.mesh = eyeMesh = Resources.Load<Mesh>(meshResourceName);
        UpdateMeshScale();

    }

    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public float ScaleZ = 1.0f;
    public bool RecalculateNormals = false;
    private Vector3[] _baseVertices;
    private bool testMode = true;
    public float testSpeed = 100f;
    public void Update()
    {
        if(testMode)
        {
            ScaleX -= Time.deltaTime / testSpeed;
            ScaleY -= Time.deltaTime / testSpeed;
            ScaleZ -= Time.deltaTime / testSpeed;

        }
        UpdateMeshScale();
    }

    private void UpdateMeshScale()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        if (_baseVertices == null)
            _baseVertices = mesh.vertices;
        var vertices = new Vector3[_baseVertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _baseVertices[i];
            vertex.x = vertex.x * ScaleX;
            vertex.y = vertex.y * ScaleY;
            vertex.z = vertex.z * ScaleZ;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        if (RecalculateNormals)
            mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void SetMeshScale(float x, float y, float z)
    {
        ScaleX = x;
        ScaleY = y;
        ScaleZ = z;
    }

    /* void Update()
     {
         if (Input.GetKeyDown(KeyCode.Space))
         {
             Debug.Log("Save mesh!");
             SaveAsset();
         }
     }

     void SaveAsset()
     {
         Mesh m1 = hiddenAreaMeshes[0];
         AssetDatabase.CreateAsset(m1, "Assets/leftEyeMesh.asset"); // saves to "assets/"
                                                                                   //AssetDatabase.SaveAssets(); // not needed?
         Mesh m2 = hiddenAreaMeshes[1];
         AssetDatabase.CreateAsset(m2, "Assets/rightEyeMesh.asset"); // saves to "assets/"
                                                                                   //AssetDatabase.SaveAssets(); // not needed?
     }*/

    public void Clear()
	{
		meshFilter.mesh = null;
	}
}

