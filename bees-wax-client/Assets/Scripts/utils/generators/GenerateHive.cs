#if UNITY_EDITOR
#endif
using System.Collections.Generic;
using model.data.geometry;
using UnityEditor;
using UnityEngine;
using view.behaviours.cell;
using Object = UnityEngine.Object;

namespace utils.generators
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GenerateHive))]
    public class GenerateHiveEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var generator = (GenerateHive) target;
            if (GUILayout.Button("Generate")) generator.Recreate();
        }
    }

    [ExecuteInEditMode]
    public class GenerateHive : MonoBehaviour
    {
        public GameObject Prefab;
        public float Radius = 10f;
        public int Splits = 4;
        public bool CreateGameplayObjects = false;

        enum IdEntities
        {
            SpawnPoint,
            NetworkObject
        };

        private Dictionary<IdEntities, int> IdCounter = new Dictionary<IdEntities, int>
        {
            {IdEntities.NetworkObject, 0},
            {IdEntities.SpawnPoint, 0}
        };
        
        public void Recreate()
        {
            var icoSphere = new IcoSphere(Splits, Radius);

            while (transform.childCount > 0) 
            {
                var child = transform.GetChild(transform.childCount - 1);
                child.parent = null;
                DestroyImmediate(child.gameObject);
            }

            for (var i = 0; i < icoSphere.Faces.Count; i++)
            {
                var face = icoSphere.Faces[i];
                var obj = AssignIds(Instantiate(Prefab, transform));
                obj.name = "Cell" + transform.childCount;
                obj.GetComponent<IGenerateMesh>()?.SetVertices(face);

                if (i % 15 == 0 && CreateGameplayObjects)
                {
                    var resource = (GameObject) Instantiate(Resources.Load("Prefabs/Cells/Source"), obj.transform);
                    resource.transform.parent = obj.transform.parent;
                    AssignIds(resource);
                    DestroyImmediate(obj);
                }

                if ((i == 8 || i == 200) && CreateGameplayObjects)
                {
                    AssignIds(Instantiate(Resources.Load("Prefabs/Cells/SpawnPoint"), obj.transform));
                }
            }
        }

        private GameObject AssignIds(Object target)
        {
            if (!(target is GameObject go)) return null;
            
            //todo you can do better c#
            var networkId = go.GetComponent<NetworkId>();
            if (networkId != null) 
                networkId.Id = IdCounter[IdEntities.NetworkObject]++;
            
            var spawnPoint = go.GetComponent<SpawnPoint>();
            if (spawnPoint != null) 
                spawnPoint.Id = IdCounter[IdEntities.SpawnPoint]++;
            
            return go;

        }
    }
#endif
}