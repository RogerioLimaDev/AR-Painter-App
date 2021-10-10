using System.Collections.Generic;
using UnityEngine;
using System;

    [Serializable]
    public class LineProperties
    {
        // public List<Vector3> linePositions;
        public List<Vector3> drawLinePositions = new List<Vector3>();
        public int lineMaterial;
        public float lineStartSize;
        public float lineEndSize;
        public Color lineStartColor;
        public Color lineEndColor;
    }

    [Serializable]
    public class  NewDrawing
    {
        public List<LineProperties> newDrawing = new List<LineProperties>();
    }

