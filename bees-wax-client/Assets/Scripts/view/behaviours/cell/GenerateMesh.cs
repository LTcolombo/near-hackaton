using System.Collections.Generic;
using UnityEngine;

namespace view.behaviours.cell
{
#if UNITY_EDITOR
    public class GenerateMesh : MonoBehaviour, IGenerateMesh
    {
        List<Vector3> _vertices;
        List<Vector3> _normals;
        List<int> _tris;
        List<Vector2> _uvs;
        
        // Use this for initialization
        public void SetVertices(List<Vector3> worldVertices)
        {
            var filter = GetComponent<MeshFilter>();
            var mesh = new Mesh();
            filter.mesh = mesh;
            
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _tris = new List<int>();
            _uvs = new List<Vector2>();

            var center = Vector3.zero;
            foreach (var vertex in worldVertices) center += vertex;
            center /= worldVertices.Count;

            var rotation = Quaternion.FromToRotation(center, Vector3.up);

            transform.position = center;
            transform.rotation = Quaternion.Inverse(rotation);

            var thickness = 0.95f;
            for (var i = 0; i < worldVertices.Count; i++)
            {
                var v1 = rotation * (worldVertices[i] - center);
                var v2 = rotation * (worldVertices[(i + 1) % worldVertices.Count] - center);

                WriteFace(new List<Vector3>
                {
                    v1, 
                    v2, 
                    v2 * thickness, 
                    v1 * thickness
                }, new Vector2(thickness, thickness));

                WriteFace(new List<Vector3>
                {
                    v1 * thickness,
                    v2 * thickness,
                    v2 * 0.65f + Vector3.down * 2f,
                    v1 * 0.65f + Vector3.down * 2f
                }, new Vector2(0.5f, 0));

                WriteFace(new List<Vector3>
                {
                    v2, 
                    v1, 
                    v1* 0.75f + Vector3.down * 2f, 
                    v2* 0.75f + Vector3.down * 2f
                }, new Vector2(1f, 0));
            }
            
            WriteFace(new List<Vector3>
            {
                rotation * (worldVertices[0] - center) * 0.65f + Vector3.down * 2f,
                rotation * (worldVertices[1] - center) * 0.65f + Vector3.down * 2f,
                rotation * (worldVertices[2] - center) * 0.65f + Vector3.down * 2f,
                rotation * (worldVertices[3] - center) * 0.65f + Vector3.down * 2f
            }, new Vector2(0, 0), -center);
            
            
            WriteFace(new List<Vector3>
            {
                rotation * (worldVertices[worldVertices.Count-3] - center) * 0.65f + Vector3.down * 2f,
                rotation * (worldVertices[worldVertices.Count-2] - center) * 0.65f + Vector3.down * 2f,
                rotation * (worldVertices[worldVertices.Count-1] - center) * 0.65f + Vector3.down * 2f,
                rotation * (worldVertices[0] - center) * 0.65f + Vector3.down * 2f
            }, new Vector2(0, 0), -center);

            mesh.vertices = _vertices.ToArray();
            mesh.normals = _normals.ToArray();
            mesh.triangles = _tris.ToArray();
            mesh.SetUVs(0, _uvs);
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        void WriteFace(IReadOnlyList<Vector3> face, Vector2 gradientRange, Vector3 normOverride = default(Vector3))
        {
            var norm = -Vector3.Cross(face[0] - face[2], face[0] - face[1]);

            if (normOverride != default(Vector3))
                norm = normOverride;

            var first = _vertices.Count;
            _vertices.AddRange(face);

            for (var i = 0; i < face.Count; i++)
                _normals.Add(norm);

            for (var i = 1; i < face.Count - 1; i++)
            {
                _tris.AddRange(new[] {first, first + i, first + i + 1});
            }

            _uvs.AddRange(new List<Vector2>
            {
                new Vector2(0, gradientRange.x),
                new Vector2(1, gradientRange.x),
                new Vector2(1, gradientRange.y),
                new Vector2(0, gradientRange.y)
            });
        }
    }

    public interface IGenerateMesh
    {
        void SetVertices(List<Vector3> worldVertices);
    }
#endif
}