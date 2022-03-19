using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace model.data.geometry
{
    public class IcoSphere
    {
        private readonly Dictionary<Vector3, List<Vector3>> _created;
        public readonly List<List<Vector3>> Faces;

        public IcoSphere(int splits, float radius)
        {
            _created = new Dictionary<Vector3, List<Vector3>>();

            var icosahedron = new Icosahedron(radius);

            foreach (var tri in icosahedron.Faces)
            {
                var v1 = icosahedron.Vertices[tri[0]];
                var v2 = icosahedron.Vertices[tri[1]];
                var v3 = icosahedron.Vertices[tri[2]];

                CreateHexes(v1, v2, v3, splits, radius);
            }

            Faces = _created.Values.ToList();
        }

        private void CreateHexes(Vector3 v1, Vector3 v2, Vector3 v3, int splits, float radius)
        {
            var center = (v1 + v2 + v3) / 3;
            for (var pass = 0; pass < splits; pass++)
            {
                float passWidth = Mathf.Max(1, splits - pass);
                for (var i = 0; i < passWidth; i++)
                {
                    var vertexCount = pass + i == 0 ? 5 : 6;

                    var weight1 = (float) i / splits + pass / (3f * splits);
                    var weight2 = (passWidth - i) / splits + pass / (3f * splits);
                    var weight3 = pass / (3f * splits);

                    var pivotWeight1 = weight1 + 1f / (3f * splits);
                    var pivotWeight2 = weight2 - 1f / (3f * splits);

                    CreateFace(v1 * weight1 + v2 * weight2 + v3 * weight3,
                        v1 * pivotWeight1 + v2 * pivotWeight2 + v3 * weight3, vertexCount, center, radius);
                    CreateFace(v2 * weight1 + v3 * weight2 + v1 * weight3,
                        v2 * pivotWeight1 + v3 * pivotWeight2 + v1 * weight3, vertexCount, center, radius);
                    CreateFace(v3 * weight1 + v1 * weight2 + v2 * weight3,
                        v3 * pivotWeight1 + v1 * pivotWeight2 + v2 * weight3, vertexCount, center, radius);
                }
            }

            CreateFace(center, center + (v2 - v1) / (3 * splits), 6, center, radius);
        }

        private void CreateFace(Vector3 v1, Vector3 v2, int vertexCount, Vector3 normal, float radius)
        {
            var vertices = new List<Vector3>();
            var r = v2 - v1;

            for (var idx = 0; idx < vertexCount; idx++)
            {
                vertices.Add((r + v1).normalized * radius);
                // ReSharper disable once PossibleLossOfFraction (degrees are precise enough)
                r = Quaternion.AngleAxis(360 / vertexCount, normal) * r;
            }

            if (_created.ContainsKey(v1))
            {
                var oldVertices = _created[v1];

                //update own vertices (should be on same plane as current icosahedron face)
                var ownVerticesIndexes = vertexCount == 6 ? new[] {4, 5} : new[] {0};

                foreach (var idx in ownVerticesIndexes) SnapNearest(oldVertices, vertices[idx]);
                _created[v1] = oldVertices;
            }
            else
            {
                _created.Add(v1, vertices);
            }
        }

	    /// <summary>
	    ///     Finds a point in source array closest to snap point and assings snapPoint value to it.
	    /// </summary>
	    /// <param name="source">Array of points, one of which (closest) will be snapped.</param>
	    /// <param name="snapPoint">Snap point.</param>
	    private static void SnapNearest([NotNull] IList<Vector3> source, Vector3 snapPoint)
        {
            var closestDistance = -1f;
            var closestIndex = 0;
            for (var i = 0; i < source.Count; i++)
            {
                var distance = Vector3.Distance(source[i].normalized, snapPoint.normalized);
                if (closestDistance > 0 && distance >= closestDistance)
                    continue;
                closestDistance = distance;
                closestIndex = i;
            }

            source[closestIndex] = snapPoint;
        }
    }
}