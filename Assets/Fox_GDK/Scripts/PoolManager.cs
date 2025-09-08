/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * Features:
 * Object Pooling
 * Object Pushing
 * Resetting Manager
 * Pre Instantiate pooling objects 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FunFox.Utility
{
    public class PoolManager : MonoBehaviour
    {
        Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        public GameObject Instantiate(GameObject prefabObj, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            // Finally this object will be return
            GameObject obj;

            // Make sure type is not null
            CheckTypeExist(prefabObj.tag);

            // If don't have any item yet then create one & return
            if (poolDictionary[prefabObj.tag].Count == 0)
            {
                PrePopulateItem(prefabObj, 5);
                obj = GameObject.Instantiate(prefabObj, parent);
                obj.transform.localPosition = position;
                obj.transform.localRotation = rotation;
                return obj;
            }
            // Finally pool an object & return;
            obj = poolDictionary[prefabObj.tag].Dequeue();
            obj.transform.SetParent(parent);
            obj.transform.localPosition = position;
            obj.transform.localRotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        public void Destroy(GameObject obj, float killTime = 0)
        {
            if (killTime == 0) Hide_N_Store(obj);
            else StartCoroutine(DelayDestroy(obj, killTime));
        }

        IEnumerator DelayDestroy(GameObject obj, float killTime)
        {
            yield return new WaitForSeconds(killTime);

            Hide_N_Store(obj);
        }

        private void Hide_N_Store(GameObject obj)
        {
            if (obj == null)
            {
                Debug.Log("A null object can't be stored inside PoolManager.");
                return;
            }

            // Make sure type is not null
            CheckTypeExist(obj.tag);

            if (obj == null)
                return;

            obj.SetActive(false);
            obj.transform.SetParent(transform);
            poolDictionary[obj.tag].Enqueue(obj);
        }

        void CheckTypeExist(string tagName)
        {
            // If this long_Prefab type is not in dictionary,
            // then make a type by it's _tag Name.
            if (!poolDictionary.ContainsKey(tagName))
                poolDictionary[tagName] = new Queue<GameObject>();
        }

        public void ResetPoolManager()
        {
            //Transform[] children = gameObject.GetComponentsInChildren(typeof(Transform), true) as Transform[];

            for (int i = transform.childCount - 1; i > -1; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }

            //foreach (Transform item in transform)
            //{
            //    Destroy(item.gameObject);
            //}

            poolDictionary = new Dictionary<string, Queue<GameObject>>();
        }

        public void PrePopulateItem(GameObject obj, int howMany)
        {
            // Make sure type is not null
            CheckTypeExist(obj.tag);

            for (int i = 0; poolDictionary[obj.tag].Count < howMany; i++)
            {
                Hide_N_Store(GameObject.Instantiate(obj));
            }
        }

    }
}