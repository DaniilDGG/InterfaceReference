using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InterfaceReference
{
    [Serializable]
    public class InterfaceReference<T> where T : class
    {
        [SerializeField] private Object _target;

        public T Value => _target as T;
        public bool IsValid => _target is T;
        
        public static implicit operator T(InterfaceReference<T> reference) => reference.Value;
    }
}
