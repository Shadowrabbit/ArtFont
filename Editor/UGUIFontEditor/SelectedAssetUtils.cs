// ******************************************************************
//       /\ /|       @file       SelectedAssetUtils.cs
//       \ V/        @brief      选中资源信息获取工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-24 12:50:18
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.IO;

namespace RabiSquare.ArtFont
{
    public static class SelectedAssetUtils
    {
        /// <summary>
        /// 获取选中资源的信息
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns>filename</returns>
        public static string SelectObjectPathInfo(out string dirName)
        {
            dirName = string.Empty;
            if (UnityEditor.Selection.activeInstanceID < 0)
            {
                return "";
            }

            var path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeInstanceID);
            dirName = Path.GetDirectoryName(path) + "/";
            return Path.GetFileName(path);
        }
    }
}