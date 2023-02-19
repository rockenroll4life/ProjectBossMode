using System;
using UnityEngine;

[Serializable]
public class FloatReference {
    [SerializeField] bool useConstant;
    [SerializeField] float constantValue;
    [SerializeField] FloatVariable floatVariable;

    public float value {
        get { return useConstant ? constantValue : floatVariable.value; }
    }
}
