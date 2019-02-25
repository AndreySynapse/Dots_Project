using System.Collections.Generic;
using UnityEngine;
using TriangleNet;
using TriangleNet.Geometry;

public class DotsSpaceRender : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRender;
    [SerializeField] private Material _contourMaterial;
    [SerializeField] private Material _meshMaterial;
    [SerializeField] private float _UVScale;

    private List<Vector2> _contour;

    public void Draw(List<Vector2> points)
    {
        _contour = new List<Vector2>(points);

        DrawContour();
        CreateMeshSpace();
    }

    private void DrawContour()
    {
        if (_contour.Count > 0)
        {
            _lineRender.positionCount = _contour.Count;
            
            for (int i = 0; i < _contour.Count; i++)
            {
                _lineRender.SetPosition(i, _contour[i]);
            }
        }
    }

    private void CreateMeshSpace()
    {
        Polygon poly = new Polygon();
        poly.Add(_contour);

        var triangleNetMesh = (TriangleNetMesh)poly.Triangulate();

        GameObject go = new GameObject("Generated space");
        var mf = go.AddComponent<MeshFilter>();
        var mesh = triangleNetMesh.GenerateUnityMesh();
        
        mesh.uv = GenerateUv(mesh.vertices);
        mf.mesh = mesh;
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = _meshMaterial;
    }

    private Vector2[] GenerateUv(Vector3[] vertices)
    {
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x * _UVScale, vertices[i].y * _UVScale);
        }

        return uvs;
    }
}
