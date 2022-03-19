using System.Collections;
using System.Collections.Generic;
using model;
using model.data;
using UnityEngine;
using utils.injection;

public class Source : InjectableBehaviour
{
    [Inject] ResourceModel _resources;

    public float remaining;

    void Start()
    {
        _resources.RegisterSource();
        _resources.Updated.Add(UpdateRemaining);
    }

    private void UpdateRemaining(Faction p)
    {
        remaining = _resources.total / _resources.sourceCount;
        _resources.Updated.Remove(UpdateRemaining);
    }

    public bool Reduce(float value)
    {
        remaining -= value;
        if (remaining > 0) return true;

        Destroy(gameObject);
        return false;
    }
}