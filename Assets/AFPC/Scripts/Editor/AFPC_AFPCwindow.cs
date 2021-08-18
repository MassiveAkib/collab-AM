using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class AFPC_AFPCwindow : EditorWindow {

	[MenuItem("AFPC/Contact Us")]
	public static void ShowWebSite()
	{
		Application.OpenURL ("https://gamedevtips9854.wixsite.com/gamedevmode");
	}

	[MenuItem("AFPC/Forum")]
	public static void ShowForumSite()
	{
		Application.OpenURL ("https://gamedevtips9854.wixsite.com/gamedevmode/forum/afpc-advanced-fpc");
	}


}
#endif
