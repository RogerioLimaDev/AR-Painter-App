using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    [RequireComponent(typeof(CommandManager))]
    public class SavedDrawingsManager : MonoBehaviour
    {
        CommandManager commandManager;
        string currentDrawName;
        [SerializeField] GameObject drawNameBtn;
        [SerializeField] Transform drawNameBtnParent;
        GameObject currentDrawNameBtn;
        bool canInstantiate;
        public List<GameObject> loadButtons = new List<GameObject>();
        public List<GameObject> deleteButtons = new List<GameObject>();
        public List<string> allPaths = new List<string>();
        List<string> allDrawings = new List<string>();



        void Start()
        {
            commandManager = this.GetComponent<CommandManager>();

            string path =Application.persistentDataPath + "/NwllSvedDrwings.json";

            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                AllDrawingNames data = JsonUtility.FromJson<AllDrawingNames>(json);

                if(data.savedDrawings.Count<=0)
                {
                    Debug.Log("No saved names yet".ToUpper());
                    return;
                }

                for(int i = 0; i<data.savedDrawings.Count; i++)
                {
                    allDrawings.Add(data.savedDrawings[i]);
                    // Debug.Log(data.savedDrawings[i]);
                }

                if(allDrawings.Count>0)
                {
                    for(int i = 0; i>allDrawings.Count; i++)
                    {
                        string s_path = Application.persistentDataPath + "/"+allDrawings[i]+".json";
                        allPaths.Add(s_path);
                    }
                }

                Debug.Log("Theres already ".ToUpper()+allDrawings.Count.ToString()+" drawings saved".ToUpper());
            }
            else
            {
                File.Create(path);
                Debug.Log("File to save drawing names created".ToUpper());
            }
        }

        public void CheckDrawNamePaths(string drawName)
        {
            if(allDrawings.Contains(drawName))
            {
                Debug.Log("Theres already a drawing with this name");
                return;
            }
            else
            {
                allDrawings.Add(drawName);
                string path = DrawingPath(drawName);
                allPaths.Add(path);
            }
        }

        private string DrawingPath(string drawName)
        {
            string path = Application.persistentDataPath + "/"+drawName+".json";
            return path;
        }

        public void DeleteDrawing(string drawName)
        {

            string path = DrawingPath(drawName);
            

            if(File.Exists(path))
            {
                Debug.Log(("Deleting drawing "+drawName).ToUpper());
                allDrawings.Remove(drawName);
                File.Delete(path);
            }
            else
            {
                allDrawings.Remove(drawName);
                Debug.Log("No file to delete".ToUpper());
            }
        }

       
        
        public void CheckSavedFiles(string passed_currentDrawName)
        {
            currentDrawName = passed_currentDrawName;

            if(currentDrawName == "")
            {
                Debug.Log(("Empty string received").ToUpper());
                LoadAllDrawingButtons();
                return;
            }
            
            if (allDrawings.Contains(currentDrawName)== false)
            {
                if(currentDrawName != "")
                {
                    allDrawings.Add(currentDrawName);
                    InstantiateDrawNameBtn(currentDrawName);
                    
                }

                return;
            }

            CheckDrawNamePaths(currentDrawName);
            Debug.Log("O desenho "+currentDrawName+" é novo no pedaço!");

            LoadAllDrawingButtons();  
        }

         public void LoadAllDrawingButtons()
        {

        //Collect existing button names
            List<string> s_list = new List<string>();
            string s_btn;

            if(drawNameBtnParent.childCount > 0)
            {
                for(int j = 0; j<drawNameBtnParent.childCount; j++ )
                {
                    Transform btn = drawNameBtnParent.GetChild(j);
                    Text t_btn = btn.GetComponentInChildren<Text>();
                    s_btn = t_btn.text;
                    s_list.Add(s_btn);
                }
            }
            
            for(int i = 0; i< allDrawings.Count ; i++)
            {
                string s = allDrawings[i];

                if(s_list.Contains(s) == false)
                {
                    InstantiateDrawNameBtn(s);
                }
            }
        }
    

        private void InstantiateDrawNameBtn(string btnName)
        {
            {
                Debug.Log("CREATING BUTTON: "+btnName.ToUpper());
                currentDrawNameBtn = Instantiate(drawNameBtn, gameObject.transform.position,Quaternion.identity);
                currentDrawNameBtn.name = btnName;
            }


            Text drawNametext = currentDrawNameBtn.GetComponentInChildren<Text>();
            drawNametext.text = btnName;
            currentDrawNameBtn.transform.SetParent(drawNameBtnParent);
        }

        private void OnDestroy()
        {
            string path =Application.persistentDataPath + "/NwllSvedDrwings.json";
            string json = File.ReadAllText(path);
            AllDrawingNames data = JsonUtility.FromJson<AllDrawingNames>(json);
            data.savedDrawings.Clear();
            for(int i = 0; i< allDrawings.Count; i++)
            {
                data.savedDrawings.Add(allDrawings[i]);
            }

            string newJson = JsonUtility.ToJson(data);
            File.WriteAllText(path,newJson);           
        }
    }


    [Serializable]
    public class AllDrawingNames
    {
        public List<string> savedDrawings = new List<string>();
    }
}


