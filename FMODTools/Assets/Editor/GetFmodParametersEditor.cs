using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEditor;
using UnityEngine;
using FMODUnity;

namespace FMODUnity
{
    public class GetFmodParametersEditor : Editor
    {
        public static string parameterAssetPath = "Assets/Audio/FMOD Parameters";
        public static string busAssetPath = "Assets/Audio/Buses";


        [MenuItem("FMOD/Get All Parameters")]
        public static void GetParametersFromFmod()
        {
            
            if (!Directory.Exists(parameterAssetPath))
            {
                Directory.CreateDirectory(parameterAssetPath);
            }

            EventDescription[] descriptions;
            FMOD.Studio.Bank bank;
            RuntimeManager.StudioSystem.update();
            RuntimeManager.StudioSystem.getBank("bank:/Master", out bank);
            //EditorUtils.System.getBank("bank:/Master", out bank);

            bank.getEventList(out descriptions);
            

            for (int i = 0; i < descriptions.Length; i++)
            {
                int count;
                descriptions[i].getParameterDescriptionCount(out count);

                for (int j = 0; j < count; j++)
                {
                    PARAMETER_DESCRIPTION parameterDescription;
                    descriptions[i].getParameterDescriptionByIndex(j, out parameterDescription);

                    FMODParameter asset =
                        AssetDatabase.LoadAssetAtPath<FMODParameter>(
                            parameterAssetPath + "/" + parameterDescription.name + ".asset");
                    if (asset == null)
                    {
                        FMODParameter newRef = ScriptableObject.CreateInstance<FMODParameter>();

                        newRef.name = parameterDescription.name;
                        newRef.id.data1 = parameterDescription.id.data1;
                        newRef.id.data2 = parameterDescription.id.data2;
                        AssetDatabase.CreateAsset(newRef, parameterAssetPath + "/" + parameterDescription.name + ".asset");
                        Debug.Log("Created Event: " + parameterDescription.name);
                    }
                    else
                    {
                        EditorUtility.SetDirty(asset);
                    }
                }
            }
        }
        
        
        
        [MenuItem("FMOD/Get All Buses")]
        public static void GetBusesFromFmod()
        {
            
            if (!Directory.Exists(busAssetPath))
            {
                Directory.CreateDirectory(busAssetPath);
            }

            Bus[] buses;
            FMOD.Studio.Bank bank;
            RuntimeManager.StudioSystem.getBank("bank:/Master", out bank);
            //EditorUtils.System.getBank("bank:/Master", out bank);

            bank.getBusList(out buses);
            

            for (int i = 0; i < buses.Length; i++)
            {
                string path;
                
                FMODBus asset = AssetDatabase.LoadAssetAtPath<FMODBus>(
                        busAssetPath + "/" + buses[i].getPath(out path) + ".asset");
                    
                    if (asset == null)
                    {
                        FMODBus newRef = ScriptableObject.CreateInstance<FMODBus>();
                        newRef.busName = path;
                        buses[i].getID(out newRef.guid);
                        newRef.name = System.IO.Path.GetFileNameWithoutExtension(path);
                        
                        AssetDatabase.CreateAsset(newRef, busAssetPath + "/" + newRef.name + ".asset");
                        
                        Debug.Log("Created Event: " + newRef.name);
                    }
                    else
                    {
                        EditorUtility.SetDirty(asset);
                    }
            }
        }
    }
}