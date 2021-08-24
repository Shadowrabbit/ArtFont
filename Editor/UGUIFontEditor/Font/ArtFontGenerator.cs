// ******************************************************************
//       /\ /|       @file       ArtFontGenerator.cs
//       \ V/        @brief      艺术字生成器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-24 12:47:00
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using UnityEngine;
using UnityEditor;

namespace RabiSquare.ArtFont
{
    public static class ArtFontGenerator
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        public static void BatchCreateArtistFont()
        {
            var fontName = SelectedAssetUtils.SelectObjectPathInfo(out var dirName).Split('.')[0];
            Debug.Log($"字体文件名称:{fontName} 所在目录:{dirName}");
            //生成字体
            var customFont = GenerateCustomFont(dirName, fontName);
            //设置字体信息
            var characterInfo = GetCharacterInfo(dirName, fontName);
            customFont.characterInfo = characterInfo;
            //设置材质
            var mat = GenerateMaterial(dirName, fontName);
            customFont.material = mat;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 获取NGUI封装的字体信息
        /// </summary>
        /// <param name="texFolder"></param>
        /// <param name="texFileName"></param>
        /// <returns></returns>
        private static CharacterInfo[] GetCharacterInfo(string texFolder, string texFileName)
        {
            var fntFileName = texFolder + texFileName + ".txt";
            var textAsset = AssetDatabase.LoadAssetAtPath(fntFileName, typeof(TextAsset)) as TextAsset ??
                            throw new Exception("找不到字体文件:" + fntFileName);
            var mbFont = new BMFont();
            //借用NGUI封装的读取类
            BMFontReader.Load(mbFont, textAsset.name, textAsset.bytes);
            var characterInfo = new CharacterInfo[mbFont.glyphs.Count];
            for (var i = 0; i < mbFont.glyphs.Count; i++)
            {
                var bmInfo = mbFont.glyphs[i];
                var info = new CharacterInfo {index = bmInfo.index};
                var uvx = bmInfo.x / (float) mbFont.texWidth;
                var uvy = 1 - bmInfo.y / (float) mbFont.texHeight;
                var uvw = bmInfo.width / (float) mbFont.texWidth;
                var uvh = -1f * bmInfo.height / mbFont.texHeight;
                info.uvBottomLeft = new Vector2(uvx, uvy);
                info.uvBottomRight = new Vector2(uvx + uvw, uvy);
                info.uvTopLeft = new Vector2(uvx, uvy + uvh);
                info.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);
                info.minX = bmInfo.offsetX;
                info.minY = (int) (bmInfo.offsetY + (float) bmInfo.height);
                info.glyphWidth = bmInfo.width;
                info.glyphHeight = -bmInfo.height;
                info.advance = bmInfo.advance;
                characterInfo[i] = info;
            }

            return characterInfo;
        }

        /// <summary>
        /// 生成自定义字体
        /// </summary>
        /// <param name="texFolder"></param>
        /// <param name="texFileName"></param>
        /// <returns></returns>
        private static Font GenerateCustomFont(string texFolder, string texFileName)
        {
            var customFont = new Font();
            //创建字体设置文件
            AssetDatabase.CreateAsset(customFont, texFolder + texFileName + ".fontsettings");
            AssetDatabase.SaveAssets();
            return customFont;
        }

        /// <summary>
        /// 生成字体材质
        /// </summary>
        /// <param name="texFolder"></param>
        /// <param name="texFileName"></param>
        /// <returns></returns>
        private static Material GenerateMaterial(string texFolder, string texFileName)
        {
            var texPath = texFolder + texFileName + ".png";
            var shader = Shader.Find("GUI/Text Shader");
            var mat = new Material(shader);
            var tex = AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture)) as Texture;
            mat.SetTexture(MainTex, tex);
            AssetDatabase.CreateAsset(mat, texFolder + texFileName + ".mat");
            return mat;
        }
    }
}