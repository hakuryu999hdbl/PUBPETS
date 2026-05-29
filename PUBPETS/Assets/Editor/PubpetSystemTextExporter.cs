#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PubpetSystemTextExporter
{
    private class TextRow
    {
        public string Id;
        public string Kind;
        public string AssetType;
        public string AssetPath;
        public string ObjectPath;
        public string Component;
        public string CurrentText;
        public string J;
        public string C1;
        public string C2;
        public string E;
        public string K;
        public string Note;
    }
    private static bool IsPubpetLanguageComponent(Component comp)
    {
        if (comp == null)
            return false;

        return HasStringProperty(comp, "J")
            && HasStringProperty(comp, "C_1")
            && HasStringProperty(comp, "C_2")
            && HasStringProperty(comp, "E")
            && HasStringProperty(comp, "K");
    }

    private static bool HasStringProperty(UnityEngine.Object obj, string propertyName)
    {
        try
        {
            SerializedObject so = new SerializedObject(obj);
            SerializedProperty prop = so.FindProperty(propertyName);

            return prop != null && prop.propertyType == SerializedPropertyType.String;
        }
        catch
        {
            return false;
        }
    }

    [MenuItem("Tools/PUBPET/Export System Texts")]
    public static void ExportSystemTexts()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        string folder = EditorUtility.SaveFolderPanel(
            "Export PUBPET System Texts",
            Application.dataPath,
            ""
        );

        if (string.IsNullOrEmpty(folder))
            return;

        string originalScenePath = SceneManager.GetActiveScene().path;

        var rows = new List<TextRow>();

        ExportSceneTexts(rows);
        ExportPrefabTexts(rows);

        string sceneCsv = Path.Combine(folder, "pubpet_scene_prefab_texts.csv");
        WriteTextRowsCsv(sceneCsv, rows);

        string scriptCsv = Path.Combine(folder, "pubpet_csharp_string_literals.csv");
        int scriptCount = ExportCSharpStringLiterals(scriptCsv);

        if (!string.IsNullOrEmpty(originalScenePath))
            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);

        AssetDatabase.Refresh();
        EditorUtility.RevealInFinder(sceneCsv);

        Debug.Log($"PUBPET export complete. Scene/Prefab rows: {rows.Count}, C# literals: {scriptCount}");
    }

    private static void ExportSceneTexts(List<TextRow> rows)
    {
        foreach (string guid in AssetDatabase.FindAssets("t:Scene"))
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                CollectFromGameObject(root, "Scene", scenePath, rows);
            }
        }
    }

    private static void ExportPrefabTexts(List<TextRow> rows)
    {
        foreach (string guid in AssetDatabase.FindAssets("t:Prefab"))
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject root = null;

            try
            {
                root = PrefabUtility.LoadPrefabContents(prefabPath);
                CollectFromGameObject(root, "Prefab", prefabPath, rows);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to scan prefab: {prefabPath}\n{e.Message}");
            }
            finally
            {
                if (root != null)
                    PrefabUtility.UnloadPrefabContents(root);
            }
        }
    }

    private static void CollectFromGameObject(
        GameObject root,
        string assetType,
        string assetPath,
        List<TextRow> rows
    )
    {
        Component[] components = root.GetComponentsInChildren<Component>(true);

        foreach (Component comp in components)
        {
            if (comp == null)
                continue;

            string typeName = comp.GetType().Name;
            string fullName = comp.GetType().FullName ?? typeName;

            if (IsPubpetLanguageComponent(comp))
            {
                rows.Add(MakeLanguageRow(comp, assetType, assetPath));
                continue;
            }

            if (IsTextComponent(fullName))
            {
                bool hasLanguageOnSameObject = comp
     .gameObject
     .GetComponents<Component>()
     .Any(IsPubpetLanguageComponent);

                if (hasLanguageOnSameObject)
                    continue;

                string text = ReadTextComponentValue(comp);

                if (!LooksLikeHumanText(text))
                    continue;

                rows.Add(MakePlainTextRow(comp, assetType, assetPath, text));
            }
        }
    }

    private static TextRow MakeLanguageRow(Component comp, string assetType, string assetPath)
    {
        string objectPath = GetObjectPath(comp.transform);

        string j = ReadSerializedString(comp, "J");
        string c1 = ReadSerializedString(comp, "C_1");
        string c2 = ReadSerializedString(comp, "C_2");
        string e = ReadSerializedString(comp, "E");
        string k = ReadSerializedString(comp, "K");

        string key = ReadPossibleKey(comp);
        string id = string.IsNullOrWhiteSpace(key)
            ? "lang_" + MakeStableHash(assetPath + "|" + objectPath + "|" + comp.GetType().Name)
            : key;

        string currentText = ReadTextFromSameGameObject(comp.gameObject);

        return new TextRow
        {
            Id = id,
            Kind = "LanguageComponent",
            AssetType = assetType,
            AssetPath = assetPath,
            ObjectPath = objectPath,
            Component = comp.GetType().Name,
            CurrentText = currentText,
            J = j,
            C1 = c1,
            C2 = c2,
            E = e,
            K = k,
            Note = ""
        };
    }

    private static TextRow MakePlainTextRow(
        Component comp,
        string assetType,
        string assetPath,
        string text
    )
    {
        string objectPath = GetObjectPath(comp.transform);
        string id = "txt_" + MakeStableHash(assetPath + "|" + objectPath + "|" + comp.GetType().FullName);

        return new TextRow
        {
            Id = id,
            Kind = "TextWithoutLanguage",
            AssetType = assetType,
            AssetPath = assetPath,
            ObjectPath = objectPath,
            Component = comp.GetType().Name,
            CurrentText = text,
            J = "",
            C1 = "",
            C2 = "",
            E = "",
            K = "",
            Note = "Text component has no Language component. Check whether it needs localization."
        };
    }

    private static bool IsTextComponent(string fullName)
    {
        return fullName == "UnityEngine.UI.Text"
            || fullName == "TMPro.TextMeshProUGUI"
            || fullName == "TMPro.TextMeshPro";
    }

    private static string ReadTextFromSameGameObject(GameObject go)
    {
        foreach (Component c in go.GetComponents<Component>())
        {
            if (c == null)
                continue;

            string fullName = c.GetType().FullName ?? "";

            if (IsTextComponent(fullName))
                return ReadTextComponentValue(c);
        }

        return "";
    }

    private static string ReadTextComponentValue(Component comp)
    {
        try
        {
            var prop = comp.GetType().GetProperty("text");

            if (prop != null && prop.PropertyType == typeof(string))
                return prop.GetValue(comp, null) as string ?? "";
        }
        catch
        {
            // ignored
        }

        return "";
    }

    private static string ReadSerializedString(UnityEngine.Object obj, string propertyName)
    {
        try
        {
            SerializedObject so = new SerializedObject(obj);
            SerializedProperty prop = so.FindProperty(propertyName);

            if (prop != null && prop.propertyType == SerializedPropertyType.String)
                return prop.stringValue ?? "";
        }
        catch
        {
            // ignored
        }

        return "";
    }

    private static string ReadPossibleKey(UnityEngine.Object obj)
    {
        string[] possibleNames = { "Key", "key", "ID", "Id", "id" };

        foreach (string name in possibleNames)
        {
            string value = ReadSerializedString(obj, name);

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();
        }

        return "";
    }

    private static bool LooksLikeHumanText(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return false;

        s = s.Trim();

        if (s.Length > 3000)
            return false;

        if (s.StartsWith("Assets/"))
            return false;

        if (s.StartsWith("http://") || s.StartsWith("https://"))
            return false;

        return s.Any(IsNaturalLanguageChar);
    }

    private static bool IsNaturalLanguageChar(char c)
    {
        return char.IsLetter(c)
            || (c >= 0x3040 && c <= 0x30FF) // Japanese kana
            || (c >= 0x4E00 && c <= 0x9FFF) // CJK
            || (c >= 0xAC00 && c <= 0xD7AF); // Korean
    }

    private static string GetObjectPath(Transform t)
    {
        var names = new Stack<string>();

        while (t != null)
        {
            names.Push(t.name);
            t = t.parent;
        }

        return string.Join("/", names);
    }

    private static string MakeStableHash(string input)
    {
        unchecked
        {
            uint hash = 2166136261;

            foreach (char c in input)
            {
                hash ^= c;
                hash *= 16777619;
            }

            return hash.ToString("x8");
        }
    }

    private static void WriteTextRowsCsv(string path, List<TextRow> rows)
    {
        var sb = new StringBuilder();

        sb.AppendLine("ID,圖檔,分類,備註,限制(EN),原文,EN,JA,KO,SC");

        foreach (TextRow r in rows)
        {
            // 交付表只放真正有 Language 组件的文本
            if (r.Kind != "LanguageComponent")
                continue;

            string memo =
                r.AssetPath + "\n" +
                r.ObjectPath + "\n" +
                r.Component;

            sb.AppendLine(string.Join(",", new[]
            {
            Csv(r.Id),          // ID
            Csv(""),            // 圖檔：这个还是人工补
            Csv(""),            // 分類：Title / Settings / UI 等，人工补更安全
            Csv(memo),          // 備註
            Csv(""),            // 限制(EN)
            Csv(r.C2),          // 原文 = TC / 繁中
            Csv(r.E),           // EN
            Csv(r.J),           // JA
            Csv(r.K),           // KO
            Csv(r.C1)           // SC / 简中
        }));
        }

        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
    }

    private static int ExportCSharpStringLiterals(string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("id,file,line,literal,note");

        int count = 0;
        Regex stringRegex = new Regex("@?\"(?:\"\"|\\\\.|[^\"\\\\])*\"");

        foreach (string guid in AssetDatabase.FindAssets("t:Script"))
        {
            string scriptPath = AssetDatabase.GUIDToAssetPath(guid);

            if (!scriptPath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                continue;

            string[] lines;

            try
            {
                lines = File.ReadAllLines(scriptPath);
            }
            catch
            {
                continue;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                foreach (Match m in stringRegex.Matches(lines[i]))
                {
                    string literal = m.Value;

                    if (!LooksLikeHumanText(literal))
                        continue;

                    string id = "cs_" + MakeStableHash(scriptPath + "|" + (i + 1) + "|" + literal);

                    sb.AppendLine(string.Join(",", new[]
                    {
                        Csv(id),
                        Csv(scriptPath),
                        Csv((i + 1).ToString()),
                        Csv(literal),
                        Csv("Hardcoded C# string. Manual review needed.")
                    }));

                    count++;
                }
            }
        }

        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        return count;
    }

    private static string Csv(string s)
    {
        if (s == null)
            return "";

        bool mustQuote = s.Contains(",")
            || s.Contains("\"")
            || s.Contains("\n")
            || s.Contains("\r");

        s = s.Replace("\"", "\"\"");

        return mustQuote ? $"\"{s}\"" : s;
    }
}
#endif