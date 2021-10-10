using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.RogerioLima.ARPaint
{
    [RequireComponent(typeof(CommandManager))]
    public class LoadFromJsonCommand : MonoBehaviour
    {
        [SerializeField] PanelsController panelsController;
        [SerializeField] GameObject drawLineObject;
        [SerializeField] DrawLineCommand drawLine;
        [SerializeField] DrawOnPlaneCommand drawOnPlane;
        [SerializeField] LineSettingsCommand lineSettingsCommand;
         public void LoadDrawingFromJson(string drawName)
        {
            string path =Application.persistentDataPath + "/"+drawName+".json";
            Debug.Log(drawName.ToUpper()+" TO LOAD FROM JSON");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                NewDrawing data = JsonUtility.FromJson<NewDrawing>(json);
                Debug.Log(drawName.ToUpper()+" LOADED FROM JSON");
                // Debug.Log(json);
                Draw(data);
            }
            else
            {
                Debug.Log("NO JSON FILE TO LOAD");
            }

            panelsController.EnableLoadDrawingPanel();
        }

        private void Draw(NewDrawing data)
        {
            List<LineProperties> l_data = data.newDrawing;

            for(int i = 0; i<l_data.Count; i++)
            {
                // LineRenderer line = new GameObject().AddComponent<LineRenderer>();
                LineRenderer line = Instantiate(drawLineObject, gameObject.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
                
                line.positionCount = l_data[i].drawLinePositions.Count;
                
                // Debug.Log("NUMERO DE PONTOS: "+line.positionCount);

                Vector3[] l_positions = l_data[i].drawLinePositions.ToArray();
                line.SetPositions(l_positions);
                line.loop = false;
                line.numCapVertices = 3;
                line.numCornerVertices = 5;
                //Remove line material from prefab
                line.material = null;
                //Assign new line material to prefab
                line.material = lineSettingsCommand.brushMaterials[l_data[i].lineMaterial];
                line.endColor = l_data[i].lineEndColor;
                line.endWidth = l_data[i].lineEndSize;
                line.startColor = l_data[i].lineStartColor;
                line.startWidth = l_data[i].lineStartSize;

            }
        }
    }      
}




