using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CodeGenerator {

    [System.Serializable]
    public class Field {
        public string name;
        public string fieldType;

        public string GetText (bool isPublic = true) {
            string format = isPublic ? "public {0} {1};" : "{0} {1};";
            return string.Format (format, fieldType, name);
        }
    }

    public static readonly string TemplatesDir = "Assets/Scripts/Editor/CodeGenerator/Templates/";

    public class TemplateLoader {
        public TextAsset LoadTemplate (string path) {
            return AssetDatabase.LoadAssetAtPath<TextAsset> (path);
        }
    }

    public class Writer {

        public void CreateNewScript (string path, string content) {
            using (StreamWriter streamWriter = File.CreateText (path + ".cs")) {
                streamWriter.WriteLine (content);
            }
            AssetDatabase.Refresh ();
        }

        public void CreateNewScriptAt (string fileName, string content) {
            var path = EditorUtility.OpenFolderPanel ("Select Create Folder", "", "");
            CreateNewScript (System.IO.Path.Combine (path, fileName), content);
        }
    }

    public string Tabs (int level) {
        string rtn = "";
        for (int i = 0; i < level; i++) {
            rtn += "\t";
        }
        return rtn;
    }

    public string UpperCamelToCamel (string src) {
        if (string.IsNullOrEmpty (src)) return src;
        return char.ToLowerInvariant (src[0]) + src.Substring (1, src.Length - 1);
    }
}