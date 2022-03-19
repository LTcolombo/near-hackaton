using System.Collections.Generic;
using UnityEngine;

namespace model.data.geometry
{
    public class Hexagon
    {
        public readonly List<Vector2> Vertices;

        public Hexagon()
        {
            Vertices = new List<Vector2>
            {
                new Vector2(0.5f, 0),
                new Vector2(1, 0.25f),
                new Vector2(1, 0.75f),
                new Vector2(0.5f, 1),
                new Vector2(0, 0.75f),
                new Vector2(0, 0.25f)
            };
        }
    }
}