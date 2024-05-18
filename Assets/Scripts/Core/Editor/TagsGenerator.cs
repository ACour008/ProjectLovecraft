using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public static class TagsGenerator 
{
    const string filename = "Assets/Scripts/Core/Runtime/Tags.cs";

    [MenuItem("Tools/Code/Generate Tags.cs")]
    static void Generate()
    {
        var sb = new StringBuilder();
        sb.AppendLine("// Generated from TagsGenerator.cs");
        sb.AppendLine();
        sb.AppendLine("public static class Tags");
        sb.AppendLine("{");
        foreach (string tag in UnityEditorInternal.InternalEditorUtility.tags)
            sb.AppendLine($"    public const string {Sanitize(tag)} = nameof({Sanitize(tag)});");
        sb.AppendLine("}");

        sb.AppendLine();
        sb.AppendLine("public static class Layers");
        sb.AppendLine("{");
        foreach (string layer in UnityEditorInternal.InternalEditorUtility.layers)
            sb.AppendLine($"    public const int {Sanitize(layer)} = {LayerMask.NameToLayer(layer)};");
        sb.Append("}");

        sb.AppendLine();
        sb.AppendLine("public static class LayerMasks");
        sb.AppendLine("{");
        foreach (string layer in UnityEditorInternal.InternalEditorUtility.layers)
            sb.AppendLine($"    public const int {Sanitize(layer)} = 1 << Layers.{Sanitize(layer)};");
        sb.AppendLine("}");

        File.WriteAllText(filename, sb.ToString());
        AssetDatabase.ImportAsset(filename, ImportAssetOptions.ForceUpdate);
    }

    static string Sanitize(string name)
    {
        name = name.Replace(" ", "");
        return name;
    }
}
