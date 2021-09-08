using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Com.RogerioLima.ARPaint
{
    public class SaveDrawingCommand : MonoBehaviour, ICommand
    {
        private string path;
        [SerializeField] CommandManager commandManager;
        DrawLineCommand drawLineCommand;

        void Start()
        {
            drawLineCommand = gameObject.GetComponent<DrawLineCommand>();
        }

         public bool CanExecute()
        {
            if(drawLineCommand.lineDrawing.Count>0)
            {
                Debug.Log("READY TO SAVE DRAWING");
                return true;
            }
            else
            {
                Debug.Log("No drawing to save");
                return false;
            }
        }

        public void Execute()
        {
            Debug.Log("FILE PATH: "+ path);
            path =  Application.persistentDataPath+"/arDrawings.dat";
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            formatter.Serialize(file, drawLineCommand.lineDrawing);
            file.Close();
            Debug.Log("OBJECT SAVED WITH SUCCESS");
        }

        public void Undo()
        {

        }

        public void SaveCurrentDrawing()
        {
            Debug.Log("SAVE COMMAND INVOKED");
            commandManager.Invoke(this);
        }
    }
}

