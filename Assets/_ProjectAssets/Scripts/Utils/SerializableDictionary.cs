using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableStringColorDictionary : SerializableDictionary<string, Color> {

    protected override void _mTryAdd(string key, Color value)
    {
        if (this.ContainsKey(key))
        {
            key += " 1";
        }
        this.Add(key, value);
    }
}

[System.Serializable]
public class SerializableDictionary<TK, TV> : Dictionary<TK, TV>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TK> keys;
    [SerializeField]
    private List<TV> values;


    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach(KeyValuePair<TK, TV> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize()
    {

        if (values.Count > keys.Count)
        {
            Debug.LogWarning("Keys should be more than values!");
        }

        this.Clear();

        int idx = 0;
        while(idx < values.Count)
        {
            this._mTryAdd(keys[idx], values[idx]);
            idx++;
        }

        while(idx < keys.Count)
        {
            this._mTryAdd(keys[idx], default);
        }
    }

    protected virtual void _mTryAdd(TK key, TV value)
    {
    }
}
