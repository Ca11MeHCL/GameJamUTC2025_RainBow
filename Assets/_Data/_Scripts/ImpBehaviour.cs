using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImpBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        //For override
    }

    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValue();
    }

    protected virtual void LoadComponents()
    {
        //For override
    }

    protected virtual void ResetValue()
    {
        //For override
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    //protected T LoadComponent<T>(ref T component, params object[] linkToObject) where T : Component
    //{
    //    if (component != null) return component;

    //    var trans = this.FindObjectByLinks(linkToObject);
    //    if (trans == null) return null;

    //    component = trans.GetComponent<T>();

    //    if (component == null)
    //    {
    //        Debug.LogError($"\"{typeof(T)}\" component cannot be found in \"{trans.gameObject.name}\"");
    //        return null;
    //    }

    //    return component;
    //}

    //protected List<T> LoadListComponents<T>(ref List<T> components, params object[] linkToContainer) where T : Component
    //{
    //    if (components != null && components.Count > 0) return components;

    //    var container = this.FindObjectByLinks(linkToContainer);
    //    if (container == null) return null;

    //    components = container.GetComponentsInChildren<T>(true).ToList<T>();

    //    if (components == null || components.Count == 0)
    //    {
    //        Debug.LogError($"\"{typeof(T)}\" components cannot be found in \"{container.gameObject.name}\"");
    //        return null;
    //    }

    //    return components;
    //}

    //private Transform FindObjectByLinks(params object[] links)
    //{
    //    Transform trans = transform;
    //    foreach (object o in links)
    //    {
    //        if (o is string s)
    //        {
    //            trans = this.FindChildObjectByName(trans, s);
    //        }
    //        else if (o is int i)
    //        {
    //            trans = this.GetChildObjectByIndex(trans, i);
    //        }
    //        else
    //        {
    //            Debug.LogError($"Links params must be string or integer");
    //        }

    //        if (trans == null) return null;
    //    }
    //    return trans;
    //}

    //private Transform FindChildObjectByName(Transform trans, string name)
    //{
    //    var t = trans.Find(name);
    //    if (t == null)
    //    {
    //        Debug.LogError($"Object named \"{name}\" cannot be found in \"{trans.gameObject.name}\"");
    //        return null;
    //    }
    //    return t;
    //}

    //private Transform GetChildObjectByIndex(Transform trans, int index)
    //{
    //    if (index < 0)
    //    {
    //        return this.GetParentObjectByGrade(trans, index);
    //    }
    //    else if (index >= trans.childCount)
    //    {
    //        Debug.LogError($"Cannot get object at index {index} in \"{trans.gameObject.name}\"");
    //        return null;
    //    }

    //    return trans.GetChild(index);
    //}

    //private Transform GetParentObjectByGrade(Transform trans, int grade)
    //{
    //    var parent = trans;
    //    while (grade++ < 0)
    //    {
    //        parent = trans.parent;

    //        if (parent == null)
    //        {
    //            Debug.LogWarning($"Cannot get parent of \"{trans.gameObject.name}\"");
    //            return trans;
    //        }

    //        trans = parent;
    //    }
    //    return parent;
    //}
}
