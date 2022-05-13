using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class QuickTools : EditorWindow
{
    
    [MenuItem("Window/Core Library/Saving&Loading/Save Manager")]
    public static void ShowExample()
    {
        QuickTools wnd = GetWindow<QuickTools>();
        wnd.titleContent = new GUIContent("Save&Loading Manager");
    }
    
    
    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gameplay.core-library/Editor/QuickTools.uxml");
        visualTree.CloneTree(root);
        
        
        // Bind buttons
        // currently unused.
        // Button saveDeleteButton = rootVisualElement.Query<Button>("save-delete-button");
        // saveDeleteButton.clicked += deletus;
        Button openPersistentDatapathButton = rootVisualElement.Query<Button>("open-persistent-datapath-button");
        openPersistentDatapathButton.clicked += openPersistentDatapath;
    }
    
    
    // void deletus() { // Currently unused.
    //     // SavingLoading.DeleteSave("");
    //     Debug.LogWarning("Save file deleted");
    // }
    
    void openPersistentDatapath() {
        Application.OpenURL("file://" + Application.persistentDataPath);
    }
    
    
}