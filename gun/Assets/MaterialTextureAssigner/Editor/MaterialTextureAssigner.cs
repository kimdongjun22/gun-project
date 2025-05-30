using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace JdogLabs
{
    public class MaterialTextureAssigner : EditorWindow
    {
        private Material targetMaterial;
        private List<Texture2D> selectedTextures = new List<Texture2D>();
        private string newMaterialName = "New Material";
        private string savePath = "Assets/Materials/";
        private Vector2 scrollPosition;
        private bool showTextureList = true;
        private readonly string[] commonTextureNames = new string[] 
        {
            "_BaseMap",
            "_BumpMap",
            "_MetallicGlossMap",
            "_OcclusionMap",
            "_EmissionMap",
            "_DetailAlbedoMap",
            "_DetailNormalMap"
        };

        [MenuItem("Tools/Material Texture Assigner")]
        public static void ShowWindow()
        {
            GetWindow<MaterialTextureAssigner>("Material Texture Assigner");
        }

        private bool IsLikelyNormalMap(Texture2D texture)
        {
            string textureName = texture.name.ToLower();
            string path = AssetDatabase.GetAssetPath(texture);
            
            Debug.Log($"Checking if {textureName} is a normal map");
            
            // Check for common normal map naming patterns
            bool hasNormalKeyword = textureName.Contains("normal") || 
                                textureName.Contains("nrm") || 
                                textureName.EndsWith("_n") || 
                                textureName.Contains("_normal_") ||
                                textureName.Contains("norm") || 
                                textureName.Contains("opengl");

            if (hasNormalKeyword)
            {
                Debug.Log($"Detected normal map by name: {textureName}");
                return true;
            }

            // Check texture import settings
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                if (importer.textureType == TextureImporterType.NormalMap)
                {
                    Debug.Log($"Texture is already configured as normal map: {textureName}");
                    return true;
                }
                
                // Additional checks for potential normal maps
                if (!importer.sRGBTexture && importer.textureCompression == TextureImporterCompression.Compressed)
                {
                    Debug.Log($"Texture has normal map-like import settings: {textureName}");
                    return true;
                }
            }

            Debug.Log($"Not detected as normal map: {textureName}");
            return false;
        }

        private void ConfigureMetallicMap(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                Debug.Log($"Configuring metallic map: {path}");
                importer.textureType = TextureImporterType.Default;
                importer.sRGBTexture = false;
                importer.mipmapEnabled = true;
                importer.streamingMipmaps = true;
                
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }

        private void ConfigureRoughnessMap(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                Debug.Log($"Configuring roughness map: {path}");
                importer.textureType = TextureImporterType.Default;
                importer.sRGBTexture = false;
                importer.mipmapEnabled = true;
                importer.streamingMipmaps = true;
                
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }

        private void ConfigureNormalMap(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                Debug.Log($"Configuring normal map: {path}");
                
                // Configure the texture as a normal map
                importer.textureType = TextureImporterType.NormalMap;
                importer.sRGBTexture = false;
                importer.mipmapEnabled = true;
                importer.streamingMipmaps = true;
                
                // For OpenGL normal maps, you might need to invert the green channel
                if (path.ToLower().Contains("opengl"))
                {
                    Debug.Log($"OpenGL normal map detected - setting appropriate import settings");
                    // You might want to add specific handling for OpenGL normal maps here
                }

                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
                
                Debug.Log($"Normal map configuration complete: {path}");
            }
        }

        void OnSelectionChange()
        {
            // First check if a material is selected
            Object[] selection = Selection.objects;
            foreach (Object obj in selection)
            {
                if (obj is Material material)
                {
                    targetMaterial = material;
                    Repaint();
                    break;
                }
            }

            // Handle texture selection separately
            HashSet<Texture2D> currentSelection = new HashSet<Texture2D>();
            bool texturesChanged = false;

            foreach (Object obj in selection)
            {
                if (obj is Texture2D texture)
                {
                    currentSelection.Add(texture);
                    if (!selectedTextures.Contains(texture))
                    {
                        selectedTextures.Add(texture);
                        texturesChanged = true;
                    }
                }
            }

            // Remove unselected textures
            for (int i = selectedTextures.Count - 1; i >= 0; i--)
            {
                if (!currentSelection.Contains(selectedTextures[i]))
                {
                    selectedTextures.RemoveAt(i);
                    texturesChanged = true;
                }
            }

            if (texturesChanged)
            {
                Repaint();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            
            // Material Selection Section
            EditorGUILayout.LabelField("Material Selection", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", targetMaterial, typeof(Material), false);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(10);

            // New Material Creation Section
            if (targetMaterial == null)
            {
                EditorGUILayout.LabelField("Create New Material", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                newMaterialName = EditorGUILayout.TextField("Material Name", newMaterialName);
                savePath = EditorGUILayout.TextField("Save Path", savePath);

                if (GUILayout.Button("Create New Material"))
                {
                    CreateNewMaterial();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(10);
            }

            // Texture Selection Section
            EditorGUILayout.LabelField("Texture Selection", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            SelectTextures();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Selected Textures", EditorStyles.boldLabel);
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                for (int i = 0; i < selectedTextures.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    selectedTextures[i] = (Texture2D)EditorGUILayout.ObjectField(selectedTextures[i], typeof(Texture2D), false);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        selectedTextures.RemoveAt(i);
                        i--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(10);

            // Assignment Section
            GUI.enabled = targetMaterial != null && selectedTextures.Count > 0;
            if (GUILayout.Button("Assign Textures"))
            {
                AssignTextures();
            }
            GUI.enabled = true;
        }

        private void SelectTextures()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear All", GUILayout.Width(60)))
            {
                selectedTextures.Clear();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.HelpBox("Select textures in the Project window to add them here", MessageType.Info);
        }

        private void CreateNewMaterial()
        {
            if (!System.IO.Directory.Exists(savePath))
            {
                System.IO.Directory.CreateDirectory(savePath);
            }

            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            string path = $"{savePath}{newMaterialName}.mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            AssetDatabase.CreateAsset(material, path);
            AssetDatabase.SaveAssets();
            
            targetMaterial = material;
            EditorGUIUtility.PingObject(material);
        }

        private void AssignTextures()
        {
            Undo.RecordObject(targetMaterial, "Assign Textures");

            foreach (Texture2D texture in selectedTextures)
            {
                string textureName = texture.name.ToLower();
                bool assigned = false;

                Debug.Log($"Processing texture: {texture.name}");
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                
                if (importer != null)
                {
                    string textureType = "";
                    bool isNormalMap = IsLikelyNormalMap(texture);
                    
                    if (isNormalMap)
                    {
                        ConfigureNormalMap(path);
                        textureType = "_BumpMap";
                        Debug.Log($"Detected and configured normal map: {texture.name}");
                    }
                    // Check for metallic-roughness workflow textures
                    else if (textureName.Contains("metallic"))
                    {
                        textureType = "_MetallicGlossMap";
                        ConfigureMetallicMap(path);
                        Debug.Log($"Detected metallic map: {texture.name}");
                    }
                    else if (textureName.Contains("roughness"))
                    {
                        textureType = "_MetallicGlossMap";
                        ConfigureRoughnessMap(path);
                        Debug.Log($"Detected roughness map: {texture.name}");
                    }
                    else if (textureName.Contains("_ao") || textureName.Contains("ambient") || 
                        textureName.Contains("occlusion") || textureName.EndsWith("ao"))
                    {
                        textureType = "_OcclusionMap";
                    }
                    else if (importer.textureType == TextureImporterType.NormalMap)
                    {
                        textureType = "_BumpMap";
                    }
                    else
                    {
                        // Default albedo detection
                        if (textureName.Contains("albedo") || textureName.Contains("diffuse") || 
                            textureName.Contains("basecolor") || textureName.Contains("color"))
                        {
                            textureType = "_BaseMap";
                        }
                    }

                    if (!string.IsNullOrEmpty(textureType))
                    {
                        Debug.Log($"Attempting to assign {texture.name} to {textureType}");
                        if (targetMaterial.HasProperty(textureType))
                        {
                            targetMaterial.SetTexture(textureType, texture);
                            assigned = true;
                            Debug.Log($"Successfully assigned {texture.name} to {textureType}");
                        }
                        else
                        {
                            Debug.LogWarning($"Material does not have property {textureType} for texture {texture.name}");
                        }
                    }
                }

                if (!assigned)
                {
                    Debug.LogWarning($"Could not automatically assign {texture.name}. Please assign manually.");
                }
            }

            EditorUtility.SetDirty(targetMaterial);
        }
    }
}