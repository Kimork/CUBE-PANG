using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolAble
{
    public int GetIndex();
    public void SetIndex(int index);
}

public class ObjectPoolManager<T> where T : MonoBehaviour, IPoolAble
{
    private T[] m_Pool;
    private bool[] m_IsEnable;
    private int m_Index;
    private Vector3 m_SponePos;

    public ObjectPoolManager(GameObject target, Transform parent, int poolMaxNum, Vector3 sponePos)
    {
        if (ReferenceEquals(target.GetComponent<T>(), null))
            Debug.Log($"해당 오브젝트에 풀링 가능한 스크립트가 없습니다.");
        else
        {
            m_Pool = new T[poolMaxNum];
            m_IsEnable = new bool[poolMaxNum];
            m_Index = 0;
            m_SponePos = sponePos;

            for (int i = 0; i < poolMaxNum; i++)
            {
                var _createdObejct = Object.Instantiate(target, parent);
                _createdObejct.transform.localPosition = m_SponePos;
                _createdObejct.name = $"Created_{i}";
                _createdObejct.SetActive(false);

                m_Pool[i] = _createdObejct.GetComponent<T>();
                m_Pool[i].SetIndex(i);

                m_IsEnable[i] = false;
            }
        }
    }

    public void PushObject(T target)
    {
        m_IsEnable[target.GetIndex()] = false;
        target.gameObject.SetActive(false);
    }

    public T GetObject()
    {
        if (m_Index >= m_Pool.Length)
            m_Index = 0;

        if (!m_IsEnable[m_Index])
        {
            m_IsEnable[m_Index] = true;
            //m_Pool[m_Index].gameObject.SetActive(true);
            return m_Pool[m_Index++];
        }
        else
        {
            m_Index++;
            return GetObject();
        }
    }
}
