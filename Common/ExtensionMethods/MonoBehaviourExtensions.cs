using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static T GetOrAddComponent<T>(this MonoBehaviour monoBehaviour) where T : Component
    {
        T component = monoBehaviour.GetComponent<T>();
        if (component == null)
        {
            component = monoBehaviour.gameObject.AddComponent<T>();
        }
        return component;
    }
}
