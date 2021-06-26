using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponCreator : EditorWindow
{
    private static string _weaponName;
    private static WeaponConfig.Quality _weaponQualityLevel;
    private static float _weaponDamage;
    private static float _weaponRecoil;
    private static Mesh _weaponMesh;
    private static Material _weaponMaterial;
    private WeaponConfig _loadConfig;

    private static readonly GUIStyle _titleStyle = new GUIStyle(EditorStyles.label);
    private static readonly GUIStyle _style = new GUIStyle(EditorStyles.label);
    private static readonly GUIStyle _errorStyle = new GUIStyle(EditorStyles.label);
    private static bool _createInScene;
    
    private bool _configError;

    [MenuItem("CustomTools/WeaponCreator")]
    public static void OpenWindow()
    {
        WeaponCreator window = GetWindow<WeaponCreator>();

        window.wantsMouseMove = true;

        _errorStyle.normal.textColor = Color.red;
        _titleStyle.normal.textColor = Color.black;
        _style.normal.textColor = Color.black;
        _titleStyle.fontSize = 20;
        
        window.minSize = new Vector2(370, 340);
        window.maxSize = new Vector2(370, 340);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.LabelField(new Rect(position.width / 2 - 80, 10, 200, 200), "Weapon Creator", _titleStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(10, 50, 200,25),"Weapon Name: ",_style);
        _weaponName = EditorGUI.TextField(new Rect(150 , 50, 200,15), _weaponName);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(10, 80, 200,25),"Weapon Quality Level: ",_style);
        _weaponQualityLevel = (WeaponConfig.Quality) EditorGUI.EnumPopup(new Rect(150 , 80, 200,15), _weaponQualityLevel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(10, 110, 200,25),"Weapon Mesh: ",_style);
        _weaponMesh = (Mesh) EditorGUI.ObjectField(new Rect(150, 110, 200, 15), _weaponMesh, typeof(Mesh), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(10, 140, 200,25),"Weapon Material: ",_style);
        _weaponMaterial = (Material) EditorGUI.ObjectField(new Rect(150, 140, 200, 15), _weaponMaterial, typeof(Material), false);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(10, 170, 200,25),"Generate in Scene: ",_style);
        _createInScene = EditorGUI.Toggle(new Rect(150, 170, 200, 15), _createInScene);
        EditorGUILayout.EndHorizontal();
        
        DrawUILine(Color.gray, 2, 380);
        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(10, 200, 200,25),"Load Config: ",_style);
        _loadConfig = (WeaponConfig) EditorGUI.ObjectField(new Rect(150, 200, 200, 15), _loadConfig, typeof(WeaponConfig), false);
        EditorGUILayout.EndHorizontal();

        if (GUI.Button(new Rect(position.width / 2 - 100, 230, 200, 25), "Load Configuration"))
        {
            if (_loadConfig != null)
            {
                _configError = false;
                LoadConfig();
            }
            else
            {
                _configError = true;
            }
        }

        if (_configError)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.Label(new Rect(position.width / 2 - 100, 260, 200,25),"There is no configuration selected!", _errorStyle);
            EditorGUILayout.EndHorizontal();
        }
        
        DrawUILine(Color.gray, 2, -220);
        
        if (_weaponName != null && _weaponDamage >= 0 && _weaponRecoil >= 0 && _weaponMesh != null &&
           _weaponMaterial != null)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUI.Button(new Rect(10, 285, 350, 50), "Generate!"))
                GenerateWeapon();
            EditorGUILayout.EndHorizontal();
        }
        /*
        if (GUILayout.Button("Load Configuration", GUILayout.Height(30)))
        {
            if (_loadConfig != null)
            {
                _configError = false;
                LoadConfig();
            }
            else
            {
                _configError = true;
            }
        }

        if (_configError)
            EditorGUILayout.LabelField("There is no configuration selected!", _errorStyle);

        EditorGUILayout.Space();
        
        if (_weaponName != null && _weaponDamage >= 0 && _weaponRecoil >= 0 && _weaponMesh != null &&
            _weaponMaterial != null)
        {
            if (GUILayout.Button("Generate!", GUILayout.Height(50)))
                GenerateWeapon();   
        }*/
    }

    private void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y+=padding/2;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }
    
    private void LoadConfig()
    {
        _weaponName = _loadConfig.Name;
        _weaponQualityLevel = _loadConfig.QualityLevel;
        _weaponDamage = _loadConfig.Damage;
        _weaponRecoil = _loadConfig.Recoil;
        _weaponMesh = _loadConfig.Mesh;
        _weaponMaterial = _loadConfig.Material;
    }

    private void SaveScriptableObject(out WeaponConfig newConfig)
    {
        newConfig = CreateInstance<WeaponConfig>();

        newConfig.Name = _weaponName;
        newConfig.QualityLevel = _weaponQualityLevel;
        newConfig.Damage = _weaponDamage;
        newConfig.Recoil = _weaponRecoil;
        newConfig.Mesh = _weaponMesh;
        newConfig.Material = _weaponMaterial;
        
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/Tarea 2/SO/" + _weaponName + ".asset");

        AssetDatabase.CreateAsset(newConfig, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void GenerateWeapon()
    {
        _configError = false;
        
        WeaponConfig newConfig = null;
        SaveScriptableObject(out newConfig);
        
        if (_createInScene)
        {
            GameObject newWeapon = new GameObject(_weaponName);
        
            MeshRenderer rederer = newWeapon.AddComponent<MeshRenderer>();
            MeshFilter filter = newWeapon.AddComponent<MeshFilter>();
            Weapon weapon =  newWeapon.AddComponent<Weapon>();
            weapon.config = newConfig;
            filter.mesh = _weaponMesh;
            rederer.material = _weaponMaterial;

            Debug.Log("Object generated successfully!");    
        }
        
        Debug.Log("Scripteable Object saved!");
    }
}
