﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AFPC_UnderWaterDisplaceEffect : MonoBehaviour {

	public Material EffectMaterial;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (EffectMaterial != null)
			Graphics.Blit(src, dst, EffectMaterial);
	}
}
