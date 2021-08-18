// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
using UnityEditor;

namespace AmplifyShaderEditor
{
	public class TemplateMenuItems
	{
		[MenuItem( "Assets/Create/Amplify Shader/Universal/Unlit", false, 85 )]
		public static void ApplyTemplate0()
		{
			AmplifyShaderEditorWindow.CreateConfirmationTemplateShader( "2992e84f91cbeb14eab234972e07ea9d" );
		}
	}
}
