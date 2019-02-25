using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using UnityEngine;

namespace TriangleNet
{
    public class TestDrawer : MonoBehaviour
    {
        private enum MeshDrawerState
        {
            Nothing,
            DrawContour,
            DrawHole
        }

        [SerializeField] private Material _MeshMaterial;
        [SerializeField] private Material _ContourMaterial;
        [SerializeField] private Material _HoleMaterial;
        [SerializeField] private float _UVScale;
        [SerializeField] private float _ContourLockRadius;

        [Header("Start test generation")]
        [SerializeField] private List<Transform> _points;
        [SerializeField] private DotsSpaceRender _space;
        private List<Vector2> _testPoints;

        private List<Vector2> _Contour;
        
        private MeshDrawerState _CurrentState;
        private Vector3 _CurrentMousePosition;

        private void Start()
        {
            _Contour = new List<Vector2>();

            for (int i = 0; i < _points.Count; i++)
            {
                _Contour.Add(_points[i].position);
            }

            _space.Draw(_Contour);

            Clear();
            
            //GenerateMesh();
        }

        void Update()
        {
            if (_CurrentState == MeshDrawerState.Nothing) return;

            _CurrentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                var currentContour = _Contour;

                if (currentContour.Count > 1 && Vector2.Distance(currentContour[0], _CurrentMousePosition) < _ContourLockRadius)
                {
                    _CurrentState = MeshDrawerState.Nothing;
                }
                else
                {
                    currentContour.Add(_CurrentMousePosition);

                    print(string.Format("CurrenContour = {0}", currentContour.Count));
                }
            }
        }

        public void StartDrawContour()
        {
            if (_CurrentState != MeshDrawerState.Nothing) return;
            Clear();
            _CurrentState = MeshDrawerState.DrawContour;
        }
        
        public void GenerateMesh()
        {
            if (_CurrentState != MeshDrawerState.Nothing) return;
            Polygon poly = new Polygon();
            poly.Add(_Contour);
            
            var triangleNetMesh = (TriangleNetMesh)poly.Triangulate();

            GameObject go = new GameObject("Generated mesh");
            var mf = go.AddComponent<MeshFilter>();
            var mesh = triangleNetMesh.GenerateUnityMesh();
            mesh.uv = GenerateUv(mesh.vertices);
            mf.mesh = mesh;
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = _MeshMaterial;

            var collider = go.AddComponent<PolygonCollider2D>();
            collider.points = _Contour.ToArray();

            var rb = go.AddComponent<Rigidbody2D>();
            rb.mass = triangleNetMesh.Triangles.Sum(tris => tris.Area);
            Clear();
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
        private void Clear()
        {
            _Contour.Clear();
        }

        private void OnRenderObject()
        {
            if (_Contour.Count > 0)
            {
                _ContourMaterial.SetPass(0);
                GL.PushMatrix();
                GL.Begin(GL.LINES);
                for (int i = 0; i < _Contour.Count - 1; ++i)
                {
                    GL.Vertex(_Contour[i]);
                    GL.Vertex(_Contour[i + 1]);
                }

                if (_CurrentState == MeshDrawerState.DrawContour)
                {
                    GL.Vertex(_Contour[_Contour.Count - 1]);
                    GL.Vertex(_CurrentMousePosition);
                }
                else
                {
                    GL.Vertex(_Contour[_Contour.Count - 1]);
                    GL.Vertex(_Contour[0]);
                }
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}