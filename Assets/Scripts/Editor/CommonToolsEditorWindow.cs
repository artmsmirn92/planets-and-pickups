using mazing.common.Runtime.Utils;
using UnityEditor;
using UnityEngine;
using YG;

// ReSharper disable once CheckNamespace
public class MyEditorWindow : EditorWindow
{
    private static float           ButtonHeight       => EditorGUIUtility.singleLineHeight * 2f;
    private static GUILayoutOption ButtonHeightOption => GUILayout.Height(ButtonHeight);
    
    
    [MenuItem("Tools/[Mazing Common Tools]", false, int.MinValue)]
    public static void ShowWindow()
    {
        GetWindow<MyEditorWindow>("[Mazing Common Tools]");
    }

    private void OnGUI()
    {
        EditorUtilsEx.GuiButtonAction("Delete all PlayerPrefs", PlayerPrefs.DeleteAll, ButtonHeightOption);
        EditorUtilsEx.GuiButtonAction("Delete YG Saves",        ResetYandexGameSaves, ButtonHeightOption);
    }

    private void ResetYandexGameSaves()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}