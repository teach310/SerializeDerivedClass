using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DerivedClassListGenerator : CodeGenerator {
    public void CreateAt (Type baseType) {
        var derivedTypes = GetAllDerivedTypes (AppDomain.CurrentDomain, baseType);
        if (derivedTypes.Count == 0) {
            Debug.LogError ($"No Derived Class : {baseType.Name}");
            return;
        }
        // テンプレートをロード
        var templateLoader = new TemplateLoader ();
        var writer = new Writer ();
        var template = templateLoader.LoadTemplate (TemplatesDir + "DerivedClassList.txt");
        // 各項目の文言を生成
        string className = baseType.Name + "List";
        string fieldContent = GetField(derivedTypes);
        string switchContent = GetSwitch(derivedTypes);
        string convertContent = GetConvert(derivedTypes, baseType);
        string getAllDerivedTypesContent = GetGetAllDerivedTypes(derivedTypes);
        var content = template.text;
        content = content.Replace("$ClassName$", className);
        content = content.Replace("$Field$", fieldContent);
        content = content.Replace("$DataName$", baseType.Name);
        content = content.Replace("$Switch$", switchContent);
        content = content.Replace("$Convert$", convertContent);
        content = content.Replace("$GetAllDerivedTypes$", getAllDerivedTypesContent);
        // スクリプト生成
        writer.CreateNewScriptAt(className, content);
    }

    string GetField (List<Type> derivedTypes) {
        var stringBuilder = new StringBuilder ();
        bool isFirst = true;
        foreach (var type in derivedTypes) {
            string className = type.Name;
            string camelClassName = UpperCamelToCamel (className);
            string prefix = isFirst ? "" : Tabs(1);
            stringBuilder.AppendLine (prefix + $"[SerializeField] List<int> {camelClassName}IndexList = new List<int>();");
            stringBuilder.AppendLine (Tabs(1) + $"[SerializeField] List<{className}> {camelClassName}List = new List<{className}>();");
            isFirst = false;
        }
        return stringBuilder.ToString ();
    }

    string GetSwitch (List<Type> derivedTypes) {
        var stringBuilder = new StringBuilder ();
        bool isFirst = true;
        foreach (var type in derivedTypes) {
            string className = type.Name;
            string camelClassName = UpperCamelToCamel (className);
            string prefix = isFirst ? "" : Tabs(4);
            stringBuilder.AppendLine (prefix + $"case {className} {camelClassName}:");
            stringBuilder.AppendLine (Tabs(5) + $"{camelClassName}IndexList.Add(i);");
            stringBuilder.AppendLine (Tabs(5) + $"{camelClassName}List.Add({camelClassName});");
            stringBuilder.AppendLine (Tabs(5) + "break;");
            isFirst = false;
        }
        return stringBuilder.ToString ();
    }

    string GetConvert(List<Type> derivedTypes, Type baseType){
        var stringBuilder = new StringBuilder ();
        stringBuilder.AppendLine("int length = ");
        for (int i = 0; i < derivedTypes.Count; i++){
            string className = derivedTypes[i].Name;
            string camelClassName = UpperCamelToCamel (className);
            if(i == derivedTypes.Count -1 ){
                stringBuilder.AppendLine (Tabs(3) + $"{camelClassName}IndexList.Count;");
            }else{
                stringBuilder.AppendLine (Tabs(3) + $"{camelClassName}IndexList.Count +");
            }
        }
        stringBuilder.AppendLine(Tabs(2) + $"var rtn = new {baseType.Name}[length];");
        for (int i = 0; i < derivedTypes.Count; i++){
            string className = derivedTypes[i].Name;
            string camelClassName = UpperCamelToCamel (className);
            stringBuilder.AppendLine (Tabs(2) + $"for (int i = 0; i < {camelClassName}IndexList.Count; i++)");
            stringBuilder.AppendLine (Tabs(2) + "{");
            stringBuilder.AppendLine (Tabs(3) + $"rtn[{camelClassName}IndexList[i]] = {camelClassName}List[i];");
            stringBuilder.AppendLine (Tabs(2) + "}");
        }
        stringBuilder.Append(Tabs(2) +$"return new List<{baseType.Name}>(rtn);");
        return stringBuilder.ToString ();
    }

    string GetGetAllDerivedTypes(List<Type> derivedTypes){
        var stringBuilder = new StringBuilder ();
        for (int i = 0; i < derivedTypes.Count; i++){
            string className = derivedTypes[i].Name;
            string camelClassName = UpperCamelToCamel (className);
            if(i == derivedTypes.Count -1 ){
                stringBuilder.Append ( Tabs(3) + $"typeof({className})");
            }else{
                string prefix = i == 0 ? "" : Tabs(3);
                stringBuilder.AppendLine (prefix + $"typeof({className}),");
            }
        }
        return stringBuilder.ToString ();
    }

    public List<System.Type> GetAllDerivedTypes (System.AppDomain appDomain, System.Type baseType) {
        var result = new List<System.Type> ();
        var assemblies = appDomain.GetAssemblies ();
        for (int i = 0; i < assemblies.Length; i++) {
            var types = assemblies[i].GetTypes ();
            for (int j = 0; j < types.Length; j++) {
                if (types[j].IsSubclassOf (baseType)) {
                    result.Add (types[j]);
                }
            }
        }
        return result;
    }
}