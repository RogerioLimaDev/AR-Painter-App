using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class DeleteBtnScript : MonoBehaviour
    {
        SavedDrawingsManager savedDrawingsManager;

        void OnEnable()
        {
            savedDrawingsManager = FindObjectOfType<SavedDrawingsManager>();
        }

        public void DeleteDrawing()
        {
            Debug.Log("DELETE DRAWING PRESSIONADO");
            string drawName = gameObject.transform.parent.name;
            savedDrawingsManager.DeleteDrawing(drawName);
            GameObject btnToDestoy = gameObject.transform.parent.gameObject;
            Destroy(btnToDestoy, 0.3f);
        }

    }
}

