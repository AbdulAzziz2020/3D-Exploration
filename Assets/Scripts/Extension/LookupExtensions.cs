using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class LookupExtensions
    {
        public static Dictionary<TKey, TData> EnsureLookup<TKey, TData, TSource>(
            this Dictionary<TKey, TData> dictionary,
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TData> dataSelector
        )
        {
            dictionary ??= new();

            if (dictionary.Count == 0)
            {
                foreach (var item in source)
                {
                    var key = keySelector(item);
                    var data = dataSelector(item);

                    if (dictionary.ContainsKey(key))
                    {
                        Debug.LogWarning($"Duplicate key: {key}, has been overriden.");
                    }

                    dictionary[key] = data;
                }
            }

            return dictionary;
        }

        
        public static TData GetFromLookup<TKey, TData, TSource>(
            this Dictionary<TKey, TData> dictionary,
            IList<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TData> dataSelector,
            TKey find,
            uint lookupThreshold = 0
        )
        {
            if (source.Count < lookupThreshold)
            {
                foreach (var item in source)
                {
                    var key = keySelector(item);
                    
                    if (EqualityComparer<TKey>.Default.Equals(key, find))
                    {
                        //Debug.Log($"Found {key} in lookup without dictionary. because data size is less than {MIN_LOOKUP_SIZE}");
                        return dataSelector(item);
                    }
                }

                return default;
            }
            
            return dictionary.EnsureLookup(source, keySelector, dataSelector).GetValueOrDefault(find);
        }
    }
}