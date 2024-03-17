using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooling 
{
    string objectName { get; }
    bool isUsing { get;set; }

    void OnCollect();

    void OnRelease();
}
