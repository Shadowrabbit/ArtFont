using System;
using UnityEngine;
using UnityEditor;

public class ArtistFont : MonoBehaviour
{
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    public static void BatchCreateArtistFont()
    {
        var dirName = "";
        var fntname = EditorUtils.SelectObjectPathInfo(ref dirName).Split('.')[0];
        Debug.Log(fntname);
        Debug.Log(dirName);
        var fntFileName = dirName + fntname + ".txt";
        var customFont = new Font();
        AssetDatabase.CreateAsset(customFont, dirName + fntname + ".fontsettings");
        AssetDatabase.SaveAssets();
        TextAsset bmFontText = null;
        bmFontText = AssetDatabase.LoadAssetAtPath(fntFileName, typeof(TextAsset)) as TextAsset ??
                     throw new Exception("找不到字体文件:" + fntFileName);
        var mbFont = new BMFont();
        BMFontReader.Load(mbFont, bmFontText.name, bmFontText.bytes); // 借用NGUI封装的读取类
        var characterInfo = new CharacterInfo[mbFont.glyphs.Count];
        for (var i = 0; i < mbFont.glyphs.Count; i++)
        {
            var bmInfo = mbFont.glyphs[i];
            var info = new CharacterInfo {index = bmInfo.index};
            var uvx = bmInfo.x / (float) mbFont.texWidth;
            var uvy = 1 - bmInfo.y / (float) mbFont.texHeight;
            var uvw = bmInfo.width / (float) mbFont.texWidth;
            var uvh = -1f * bmInfo.height / (float) mbFont.texHeight;
            info.uvBottomLeft = new Vector2(uvx, uvy);
            info.uvBottomRight = new Vector2(uvx + uvw, uvy);
            info.uvTopLeft = new Vector2(uvx, uvy + uvh);
            info.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);
            info.minX = bmInfo.offsetX;
            info.minY = (int) ((float) bmInfo.offsetY + (float) bmInfo.height);
            info.glyphWidth = bmInfo.width;
            info.glyphHeight = -bmInfo.height;
            info.advance = bmInfo.advance;
            characterInfo[i] = info;
        }

        customFont.characterInfo = characterInfo;
        var textureFilename = dirName + fntname + ".png";
        var shader = Shader.Find("GUI/Text Shader");
        var mat = new Material(shader);
        var tex = UnityEditor.AssetDatabase.LoadAssetAtPath(textureFilename, typeof(Texture)) as Texture;
        mat.SetTexture(MainTex, tex);
        AssetDatabase.CreateAsset(mat, dirName + fntname + ".mat");
        customFont.material = mat;
        var fontPath = dirName + fntname + ".fontsettings";
        AssetDatabase.CreateAsset(customFont, fontPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}