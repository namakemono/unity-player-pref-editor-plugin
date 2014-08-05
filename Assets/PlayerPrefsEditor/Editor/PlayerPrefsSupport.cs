using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PlayerPrefsEditor
{
    public class PlayerPrefsSupport : MonoBehaviour
    {
        public static string[] GetAllKeys ()
        {
            var fileInfo = new FileInfo (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + "/Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist");
            var plist = PlistCS.Plist.readPlist (fileInfo.FullName) as Dictionary<string, object>;
            return plist.Keys.ToArray ();
        }
    }
}