
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace utils.generators
{
    [ExecuteInEditMode]
    public class ReplacePrefab : MonoBehaviour
    {
        public GameObject Prefab;
#if UNITY_EDITOR
        // Update is called once per frame
        private void Update()
        {
            if (Prefab == null) return;

            if (PrefabUtility.GetPrefabType(Prefab) != PrefabUtility.GetPrefabType(gameObject))
            {
                //create
                var newObj = Instantiate(Prefab, transform.parent);

                //transform
                newObj.transform.position = transform.position;
                newObj.transform.rotation = transform.rotation;

                var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

                //mesh
                var filter = newObj.GetComponent<MeshFilter>();
                if (filter != null) filter.mesh = mesh;

                //collider
                // ReSharper disable once LocalVariableHidesMember
                var collider = newObj.GetComponent<MeshCollider>();
                if (collider != null) collider.sharedMesh = mesh;

                //replace in list
                var idx = transform.GetSiblingIndex();
                DestroyImmediate(gameObject);
                newObj.transform.SetSiblingIndex(idx);
            }
        }
#endif
    }
}