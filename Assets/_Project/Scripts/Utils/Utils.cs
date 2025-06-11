using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Utils
    {
        public static void ClearTransformChilds(Transform parent)
        {
            List<GameObject> toRemove = new List<GameObject>();

            foreach (Transform child in parent)
                toRemove.Add(child.gameObject);

            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(toRemove[i]);
                else
                    GameObject.DestroyImmediate(toRemove[i]);
            }

            toRemove.Clear();
        }

        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}