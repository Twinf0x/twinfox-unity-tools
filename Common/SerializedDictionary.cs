using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twinfox
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct Pair
        {
            public TKey key;
            public TValue value;

            public static implicit operator KeyValuePair<TKey, TValue>(Pair pair)
            {
                return new KeyValuePair<TKey, TValue>(pair.key, pair.value);
            }

            public static implicit operator Pair(KeyValuePair<TKey, TValue> pair)
            {
                return new Pair
                {
                    key = pair.Key,
                    value = pair.Value
                };
            }
        }

        [SerializeField] private Pair[] entries = new Pair[0];

        public SerializedDictionary() { }

        public SerializedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public void OnBeforeSerialize()
        {
            // Copy dict to entries
            if (entries != null)
            {
                return;
            }

            int i = 0;
            entries = new Pair[this.Count];
            foreach (var kvp in this)
            {
                entries[i++] = kvp;
            }
        }

        public void OnAfterDeserialize()
        {
            // Copy entries to dict
            Clear();
            foreach (var entry in entries)
            {
                this[entry.key] = entry.value;
            }
        }
    }
}
