using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Setups
{
    public class EnumDictionarySetup<TEnum, TValue> : ScriptableObject where TEnum : Enum where TValue : class
    {
        [SerializeField] private List<EnumItem<TEnum, TValue>> _items; 

        public IReadOnlyList<EnumItem<TEnum, TValue>> Items => _items;
        
        public bool TryMapDictionary(out Dictionary<TEnum, TValue> dictionary)
        {
            dictionary = new Dictionary<TEnum, TValue>();

            foreach (var item in _items)
            {
                if (!dictionary.TryAdd(item.Key, item.Value))
                {
                    Debug.LogWarning("Duplicate ket");
                    
                    return false;
                }
            }
            
            return true;
        }
    }

    [Serializable]
    public class EnumItem<TEnum, TValue> where TEnum : Enum where TValue : class
    {
        [field: SerializeField] public TEnum Key { get; private set; }
        
        [field: SerializeField] public TValue Value { get; private set; }
    }
}