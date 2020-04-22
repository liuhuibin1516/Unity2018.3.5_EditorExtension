using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateCompentCode : MonoBehaviour
{
   [MenuItem("GameObject/@CreateCode",false,0)]
   static void CreateCode()
    {
        Debug.Log("CreateCode");

        var scriptsFolder = Application.dataPath + "/Scripts";
        if(!Directory.Exists(scriptsFolder))
        {
            Directory.CreateDirectory(scriptsFolder);
        }
        var scriptFile = scriptsFolder + "/Root.cs";
        File.Create(scriptFile);

    }
}
