using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IdleWaitTime", story: "[self] wait [WaitTime]", category: "Action", id: "6153acffee7c77148d8ecdcdef074804")]
public partial class IdleWaitTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> WaitTime;

    private float _timer;

    protected override Status OnStart()
    {
        _timer = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer > WaitTime.Value)
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

