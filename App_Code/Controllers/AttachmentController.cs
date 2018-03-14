using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AttachmentController
/// </summary>
public class AttachmentController
{
    public static string getMimeType(string fileName) {
        string mimeType = "application/unknown";
        string ext = System.IO.Path.GetExtension(fileName).ToLower();
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (regKey != null && regKey.GetValue("Content Type") != null)
            mimeType = regKey.GetValue("Content Type").ToString();
        return mimeType;
    }

    public static string ApplicationPath() {
        string appPath = HttpContext.Current.Request.ApplicationPath;
        string physicalPath = HttpContext.Current.Request.MapPath(appPath);
        return physicalPath;
    }
}
