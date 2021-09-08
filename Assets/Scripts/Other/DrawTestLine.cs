using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;



namespace Com.RogerioLima.ARPaint
{
    public class DrawTestLine : MonoBehaviour
    {
        public Transform[] linePoints;
        public SaveAssetsController saveController;
        
        [SerializeField] GameObject lineRendererObject;

        public string loadedObjectName;

        Vector3[] newVertices;
        Vector2[] newUV;
        int[] newTriangles;
        
        void Start()
        {
            Vector3 objectPosition = new Vector3(0,0,10);
            saveController.Load();
            DrawLine();
        }
       
        void DrawLine()
        {
            // GameObject emptyGameObject = new GameObject("LineRendererObject",typeof(LineRenderer),typeof(MeshFilter));
            GameObject emptyGameObject = Instantiate(lineRendererObject, Camera.main.transform.position, Quaternion.identity);
            LineRenderer currentLine = emptyGameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
            currentLine.numCapVertices = 3;
            currentLine.numCornerVertices = 5;
            Vector3[] linePositions = {linePoints[0].position,linePoints[1].position,linePoints[2].position,linePoints[3].position,linePoints[4].position};
            currentLine.positionCount = linePositions.Length;
            currentLine.SetPositions(linePositions);

            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );

            currentLine.colorGradient = gradient;
            saveController.Save(emptyGameObject.name);
            //BakeLineRenderer(currentLine, emptyGameObject);

           int curLinePos =  currentLine.GetPositions(linePositions);
           Debug.Log(curLinePos.ToString());


        }

        void BakeLineRenderer(LineRenderer lr, GameObject go)
        {
            Mesh mesh = new Mesh();
            Material material = new Material(Shader.Find("Particles/Standard Unlit"));
            LineRenderer currentLine = lr;
            currentLine.material = material;
            GameObject emptyGameObject = go;
            emptyGameObject.GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = newVertices;
            mesh.uv = newUV;
            mesh.triangles = newTriangles;
            currentLine.BakeMesh(mesh, true);

        }

    }


}

