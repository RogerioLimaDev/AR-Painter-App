using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

namespace Com.RogerioLima.ARPaint
{
    public class SaveAssetsController : MonoBehaviour
    {
        
        [Header("Object data")]
        [Space]
        public GameObject SavedGameObject;
        public string SavedObjectName;

        private string path;


         public void Save(string objName)
         {
            // string data;
            // //Translate GameObject into path
            // string assetPath = AssetDatabase.GetAssetPath(obj);

            // if (assetPath == null) 
            //     return;
            // else 
            //     data= assetPath;
     
             //Encrypt Save Data (not nessecary, but nice to have)
             path =  Application.persistentDataPath+"/arDrawings.dat";
             BinaryFormatter formatter = new BinaryFormatter();
             FileStream file = File.Create(path);

             ObjectsToSave objToSave = new ObjectsToSave();
             objToSave.m_objName = objName;
     
             formatter.Serialize(file, objToSave);
     
             file.Close();

             Debug.Log("Object Saved with success!");
     
         }
     
          
         public void Load()
         {
            // path =  Application.persistentDataPath+"/arDrawings.dat";

            //  if (File.Exists(path))
            //  {
            //      BinaryFormatter formatter = new BinaryFormatter();
            //      FileStream file = new FileStream(path, FileMode.Open);

            //     Debug.Log("Trying to load object");
            //     ObjectsToSave data = (ObjectsToSave)formatter.Deserialize(file);
            //     file.Close();


            //     SavedGameObject = (GameObject)data.m_obj;
            //     SavedObjectName = data.m_objName;

            //     Debug.Log("Loaded object is: "+ SavedGameObject.ToString());
            //  }
            //  else
            //  {
            //      Debug.LogWarning("Save file not found.");
            //  }
         }

    }

    [Serializable]
    class ObjectsToSave
    {
        public string m_objName;
    }
}

