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

    private static readonly GUIStyle _errorStyle = new GUIStyle(EditorStyles.label);
    private static bool _createInScene;
    private bool _configError;

    [MenuItem("CustomTools/WeaponCreator")]
    public static void OpenWindow()
    {
        WeaponCreator window = GetWindow<WeaponCreator>();

        window.wantsMouseMove = true;

        _errorStyle.normal.textColor = Color.red;

        window.minSize = new Vector2(500, 330);
    }

    private void OnGUI()
    {
        _weaponName = EditorGUILayout.TextField("Weapon Name:", _weaponName);
        EditorGUILayout.Space();
        _weaponQualityLevel = (WeaponConfig.Quality) EditorGUILayout.EnumPopup("Weapon Quality Level", _weaponQualityLevel);
        EditorGUILayout.Space();
        _weaponDamage = EditorGUILayout.FloatField("Weapon Damage", _weaponDamage);
        EditorGUILayout.Space();
        _weaponRecoil = EditorGUILayout.FloatField("Weapon Recoil", _weaponRecoil);
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        _weaponMesh = (Mesh) EditorGUILayout.ObjectField("Weapon Mesh", _weaponMesh, typeof(Mesh), false);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        _weaponMaterial = (Material) EditorGUILayout.ObjectField("Weapon Material", _weaponMaterial, typeof(Material), false);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        _createInScene = EditorGUILayout.Toggle("Generate in scene", _createInScene);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        _loadConfig = (WeaponConfig) EditorGUILayout.ObjectField("Load Config", _loadConfig, typeof(WeaponConfig), false);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
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
        }
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
