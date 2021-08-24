// ******************************************************************
//       /\ /|       @file       EditorArtFontGenerator.cs
//       \ V/        @brief      艺术字生成编辑器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-24 12:49:12
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEditor;

namespace RabiSquare.ArtFont
{
    public static class EditorArtFontGenerator
    {
        [MenuItem("Assets/生成艺术字")]
        public static void GenerateArtFont()
        {
            ArtFontGenerator.BatchCreateArtistFont();
        }
    }
}