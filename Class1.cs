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

    private static bool IsMatchAsterisk(string folderName, string pattern)
    {
        folderName = folderName.ToLower();
        pattern = pattern.ToLower();

        if (pattern.StartsWith("*"))
        {
            return folderName.EndsWith($"{pattern.Substring(pattern.LastIndexOf("*") + 1)}$");
        }
        else if (pattern.EndsWith("*"))
        {
            return folderName.StartsWith($"^{pattern.Substring(0, pattern.IndexOf("*"))}");
        }

        return false;
    }

    private static bool IsMatchRegular(string folderName, string pattern)
    {
        return Regex.IsMatch(folderName.ToLower(), $"^{pattern.ToLower()}$");
    }

    private static string GetBitmapPath(string sub, string imgPathStr)
    {
        string path = "";
        string imgStr = imgPathStr.Substring(imgPathStr.IndexOf(".img/") + 5);

        string[] sp = sub.Split("/");
        int j = 0;
        for (int i = 0; i < sp.Length; i++)
        {
            if (sp[i] == "*")
            {
                path += imgStr.Substring(j) + "/";
                j = imgStr.Length + 1;
            }
            else
            {
                Match m = Regex.Match(imgStr.Substring(j), sp[i]);
                if (m.Success)
                {
                    path += m.Value + "/";
                    j += m.Value.Length + 1;
                }
                else
                {
                    path += sp[i] + "/";
                    j += sp[i].Length + 1;
                }    
            }
        }

        return imgPathStr.Substring(0, imgPathStr.IndexOf(".img/") + 5) + path.Substring(0, path.Length - 1);
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
            
            string filePathStr2 = $"{filePathStr}/{GetBitmapPath(sub, imgPathStr.Substring(1, imgPathStr.Length - 2))}.png";

            Directory.CreateDirectory(filePathStr2.Substring(0, filePathStr2.LastIndexOf('/')));
            bmp.Save(filePathStr2);
            
            Console.WriteLine($"Saved into '{filePathStr2}'");
        }
        
        if (wzDir.WzProperties != null)
        {
            foreach (WzImageProperty wzProperty in wzDir.WzProperties)
            {
                if (i >= sp.Length || IsMatchAsterisk(wzProperty.Name, sp[i]) || IsMatchRegular(wzProperty.Name, sp[i]))
                {
                    imgPathStr = imgPathStr + wzProperty.Name + ".";
                    
                    ExtractBitmapFromWzNode(wzProperty, sp, i + 1);
                    
                    imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                    imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf(".") + 1);
                    
                    if (i < sp.Length && !IsMatchAsterisk(wzProperty.Name.ToLower(), sp[i]) && !sp[i].Contains("*") && !sp[i].Contains("\\")) break;
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
                if (sp[sp.Length - 1] == "*.img" || IsMatchRegular(wzDir.Name, sp[sp.Length - 1]))
                {
                    imgPathStr = imgPathStr + wzDir.Name + "/";

                    foreach (WzImageProperty wzProperty in wzDir.WzProperties)
                    {
                        if (IsMatchAsterisk(wzProperty.Name, sp2[0]) || IsMatchRegular(wzProperty.Name, sp2[0]))
                        {
                            imgPathStr = imgPathStr + wzProperty.Name + ".";
                            
                            ExtractBitmapFromWzNode(wzProperty, sp2, 1);

                            imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                            imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
                            
                            if (!IsMatchAsterisk(wzProperty.Name.ToLower(), sp2[0]) && !sp2[0].Contains("*") && !sp2[0].Contains("\\")) break;
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
            if (i >= sp.Length || IsMatchAsterisk(wzFolder.Name, sp[i]) || IsMatchRegular(wzFolder.Name, sp[i]))
            {
                imgPathStr = imgPathStr + wzFolder.Name + "/";

                ExtractBitmapFromWzDirectory(wzFolder, sp, i + 1, sp2);
                    
                imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
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
            string[] sp2 = sub.Replace("/", ".").Split(".");

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
            imgPathStr = "/";
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

        ExtractBitmapsFromWz(msPathStr, wz, img, sub);

        return 0;
    }

    private static void ExtractXmlsFromWzDirectory(WzDirectory folder, string[] sp, int i)
    {
        if (i >= sp.Length - 1)
        {
            foreach (WzImage wzDir in folder.WzImages)
            {
                if (sp[sp.Length - 1] == "*.img" || IsMatchAsterisk(wzDir.Name, sp[i]) || IsMatchRegular(wzDir.Name, sp[sp.Length - 1]))
                {
                    imgPathStr = imgPathStr + wzDir.Name + "/";

                    string filePathStr2 = $"{filePathStr}{imgPathStr.Substring(0, imgPathStr.Length - 1)}.xml";

                    Directory.CreateDirectory(filePathStr2.Substring(0, filePathStr2.LastIndexOf('/')));
                    using (StreamWriter writer = new StreamWriter(filePathStr2))
                    {
                        wzDir.ExportXml(writer, true, 0);
                    }
                    Console.WriteLine($"Saved into '{filePathStr2}'");

                    imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                    imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
                    
                    if (!IsMatchAsterisk(wzDir.Name.ToLower(), sp[sp.Length - 1]) && !sp[sp.Length - 1].Contains("*") && !sp[sp.Length - 1].Contains("\\")) break;
                }
            }
        }

        foreach (WzDirectory wzFolder in folder.WzDirectories)
        {
            if (i >= sp.Length || IsMatchAsterisk(wzFolder.Name, sp[i]) || IsMatchRegular(wzFolder.Name, sp[i]))
            {
                imgPathStr = imgPathStr + wzFolder.Name + "/";

                ExtractXmlsFromWzDirectory(wzFolder, sp, i + 1);
                    
                imgPathStr = imgPathStr.Substring(0, imgPathStr.Length - 1);
                imgPathStr = imgPathStr.Substring(0, imgPathStr.LastIndexOf("/") + 1);
            }
        }
    }

    private static void ExtractXmlsFromWz(string msPath, string wz, string path)
    {
        WzFile wzFile = new WzFile(msPath + "/" + wz, WzMapleVersion.GMS);
        try
        {
            wzFile.ParseWzFile();

            imgPathStr = imgPathStr + wz + "/";

            string[] sp = path.Split("/");

            // Access the root directory
            WzDirectory root = wzFile.WzDirectory;
            
            ExtractXmlsFromWzDirectory(root, sp, 0);
            
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
            imgPathStr = "/";
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "cs_extract_xml")]
    public static int cs_extract_xml(IntPtr maplePath, IntPtr filePath, IntPtr wzPath)
    {
        string msPathStr = Marshal.PtrToStringAnsi(maplePath);
        filePathStr = Marshal.PtrToStringAnsi(filePath);
        string wzPathStr = Marshal.PtrToStringAnsi(wzPath);

        var tuple = GetPathNames(wzPathStr);
        string wz = tuple.wz;
        img = tuple.img;

        ExtractXmlsFromWz(msPathStr, wz, img);

        return 0;
    }

}
