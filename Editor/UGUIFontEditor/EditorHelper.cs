using UnityEngine;
using UnityEditor;

public class EditorHelper : MonoBehaviour {

	[MenuItem("Assets/生成艺术字")]
	public static void BatchCreateArtistFont()
	{
		ArtistFont.BatchCreateArtistFont();
	}
}
