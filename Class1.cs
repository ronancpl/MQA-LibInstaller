/*
    This file is part of the MapleQuestAdvisor planning tool
    Copyleft (L) 2026 RonanLana

    GNU General Public License v3.0

    Permissions of this strong copyleft license are conditioned on making available complete
    source code of licensed works and modifications, which include larger works using a licensed
    work, under the same license. Copyright and license notices must be preserved. Contributors
    provide an express grant of patent rights.
*/
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Drawing;

using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;

public class WzPngInstaller
{

    private static string filePathStr;
    private static string imgPathStr = "/";
    private static string img;
    private static string sub;

    private static (string wz, string img, string sub) GetPathNames(string fullPath)
    {
        string pattern = "(.*\\.wz?|.*\\.img?|.+$)";
        string[] text = new string[3];

        MatchCollection matches = Regex.Matches(fullPath, pattern);
        int i = 0;
        foreach (Match match in matches)
        {
            text[i] = new string(match.Value.Substring(match.Value.StartsWith("/") ? 1 : 0, match.Value.Length - (match.Value.StartsWith("/") ? 1 : 0)));

            i++;
            if (i == 3) i = 2;
        }

        return (text[0], text[1], text[2]);
    }

    private static bool isMatchAsterisk(string folderName, string pattern)
    {
        if (pattern.StartsWith("*"))
        {
            return folderName.EndsWith(pattern.Substring(pattern.LastIndexOf("*") + 1));
        }
        else if (pattern.EndsWith("*"))
        {
            return folderName.StartsWith(pattern.Substring(0, pattern.IndexOf("*")));
        }

        return false;
    }

    private static void ExtractBitmapFromWzNode(WzImageProperty wzDir, string[] sp, int i) {
        if (i >= sp.Length && wzDir is WzCanvasProperty)
        {
            Bitmap bmp;

            WzCanvasProperty canvas = (WzCanvasProperty) wzDir;
            if (canvas != null)
            {
                bmp = canvas.GetBitmap();
            }
            else
            {
                bmp = new Bitmap(1, 1);
            }
            
            string filePathStr2 = $"{filePathStr}/{imgPathStr.Substring(1, imgPathStr.Length - 2)}.png";

            Directory.CreateDirectory(filePathStr2.Substring(0, filePathStr2.LastIndexOf('/')));
            bmp.Save(filePathStr2);
            
            Console.WriteLine($"Saved into '{filePathStr2}'");
        }
        
        if (wzDir.WzProperties != null)
        {
            foreach (WzImageProperty wzProperty in wzDir.WzProperties)
            {
                if (i >= sp.Length || isMatchAsterisk(wzProperty.Name.ToLower(), sp[i]) || Regex.IsMatch(wzProperty.Name.ToLower(), Regex.Escape(sp[i].ToLower())))
                {
                    imgPathStr = imgPathStr + wzProperty.Name + ".";
                    
                    ExtractBitmapFromWzNode(wzProperty, sp, i + 1);
                    
                    imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                    imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf(".") + 1);
                    
                    if (i < sp.Length && !isMatchAsterisk(wzProperty.Name.ToLower(), sp[i])) break;
                }
            }    
        }
    }

    private static void ExtractBitmapFromWzDirectory(WzDirectory folder, string[] sp, int i, string[] sp2)
    {
        if (i >= sp.Length - 1)
        {
            foreach (WzImage wzDir in folder.WzImages)
            {
                if (sp[sp.Length - 1] == "*.img" || Regex.IsMatch(wzDir.Name.ToLower(), Regex.Escape(sp[sp.Length - 1].ToLower())))
                {
                    imgPathStr = imgPathStr + wzDir.Name + "/";

                    foreach (WzImageProperty wzProperty in wzDir.WzProperties)
                    {
                        if (isMatchAsterisk(wzProperty.Name.ToLower(), sp2[0]) || Regex.IsMatch(wzProperty.Name.ToLower(), Regex.Escape(sp2[0].ToLower())))
                        {
                            imgPathStr = imgPathStr + wzProperty.Name + ".";
                            
                            ExtractBitmapFromWzNode(wzProperty, sp2, 1);

                            imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                            imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
                            
                            if (!isMatchAsterisk(wzProperty.Name.ToLower(), sp2[0])) break;
                        }
                    }

                    imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                    imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
                    
                    if (sp[sp.Length - 1] != "*.img") break;
                }
            }
        }

        foreach (WzDirectory wzFolder in folder.WzDirectories)
        {
            if (i >= sp.Length || isMatchAsterisk(wzFolder.Name.ToLower(), sp[i]) || Regex.IsMatch(wzFolder.Name.ToLower(), Regex.Escape(sp[i].ToLower())))
            {
                ExtractBitmapFromWzDirectory(wzFolder, sp, i + 1, sp2);
            }
        }
    }

    private static void ExtractBitmapsFromWz(string msPath, string wz, string img, string sub)
    {
        WzFile wzFile = new WzFile(msPath + "/" + wz, WzMapleVersion.GMS);
        try
        {
            wzFile.ParseWzFile();

            imgPathStr = imgPathStr + wz + "/";

            string[] sp = img.Split("/");
            string[] sp2 = sub.Split(".");

            // Access the root directory
            WzDirectory root = wzFile.WzDirectory;
            
            ExtractBitmapFromWzDirectory(root, sp, 0, sp2);
            
            imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
            imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading .wz file: {ex}");
        }
        finally
        {
            wzFile.Dispose(); // Always dispose to free resources
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "cs_extract_bitmap")]
    public static int cs_extract_bitmap(IntPtr maplePath, IntPtr filePath, IntPtr wzPath)
    {
        string msPathStr = Marshal.PtrToStringAnsi(maplePath);
        filePathStr = Marshal.PtrToStringAnsi(filePath);
        string wzPathStr = Marshal.PtrToStringAnsi(wzPath);

        var tuple = GetPathNames(wzPathStr);
        string wz = tuple.wz;
        img = tuple.img;
        sub = tuple.sub;

        wz = wz.Replace('\\', '/');
        img = img.Replace('\\', '/');
        sub = sub.Replace('\\', '/');

        ExtractBitmapsFromWz(msPathStr, wz, img, sub);

        return 0;
    }

}
