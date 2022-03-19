using System.Collections.Generic;
using UnityEngine;

namespace model.data.geometry
{
    public class Icosahedron
    {
        //(1.0f + Mathf.Sqrt (5.0f)) / 2.0f;
        private const float T = 1.61803398875f;
        public readonly List<List<int>> Faces;

        public readonly List<Vector3> Vertices;

        public Icosahedron(float radius)
        {
            Vertices = new List<Vector3>
            {
                new Vector3(-1, T, 0).normalized * radius,
                new Vector3(1, T, 0).normalized * radius,
                new Vector3(-1, -T, 0).normalized * radius,
                new Vector3(1, -T, 0).normalized * radius,
                new Vector3(0, -1, T).normalized * radius,
                new Vector3(0, 1, T).normalized * radius,
                new Vector3(0, -1, -T).normalized * radius,
                new Vector3(0, 1, -T).normalized * radius,
                new Vector3(T, 0, -1).normalized * radius,
                new Vector3(T, 0, 1).normalized * radius,
                new Vector3(-T, 0, -1).normalized * radius,
                new Vector3(-T, 0, 1).normalized * radius
            };

            Faces = new List<List<int>>
            {
                new List<int> {0, 11, 5},
                new List<int> {0, 5, 1},
                new List<int> {0, 1, 7},
                new List<int> {0, 7, 10},
                new List<int> {0, 10, 11},

                // 5 adjacent faces 
                new List<int> {1, 5, 9},
                new List<int> {5, 11, 4},
                new List<int> {11, 10, 2},
                new List<int> {10, 7, 6},
                new List<int> {7, 1, 8},

                // 5 faces around point 3
                new List<int> {3, 9, 4},
                new List<int> {3, 4, 2},
                new List<int> {3, 2, 6},
                new List<int> {3, 6, 8},
                new List<int> {3, 8, 9},

                // 5 adjacent faces 
                new List<int> {4, 9, 5},
                new List<int> {2, 4, 11},
                new List<int> {6, 2, 10},
                new List<int> {8, 6, 7},
                new List<int> {9, 8, 1}
            };
        }
    }
}