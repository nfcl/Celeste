using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{

    public interface ISetActivable 
    { 
    
        public void SetActive(bool active);
    
    }

    /// <summary>
    /// Unity场景物体的对象池 需要对象继承MonoBehaviour
    /// </summary>
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, ISetActivable
    {

        public static ObjectPool<T> instance;

        [SerializeField] private T Prefab;

        [SerializeField] private List<T> content;

        public void Start()
        {

            content = new List<T>();

        }

        public T GetInstance()
        {

            T Result = null;

            if(instance.content.Count == 0)
            {

                Result = GameObject.Instantiate(Prefab, transform);

            }
            else
            {

                Result = content[0];

                content.RemoveAt(0);

            }

            Result.SetActive(true);

            return Result;

        }

        public void ReturnInstance(T obj)
        {

            content.Add(obj);

            obj.transform.SetParent(transform);

            obj.SetActive(false);

        }

        public void Awake()
        {

            instance = this;

        }

    }
}