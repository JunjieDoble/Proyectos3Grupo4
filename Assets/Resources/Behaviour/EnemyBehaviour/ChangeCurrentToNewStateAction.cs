using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangeCurrentToNewState", story: "Set [CurrentState] to [NewState]", category: "Action", id: "79a67c8cc47d733a9007f502385bf5a0")]
public partial class ChangeCurrentToNewStateAction : Action
{
    [SerializeReference] public BlackboardVariable<States> CurrentState;
    [SerializeReference] public BlackboardVariable<States> NewState;

    protected override Status OnStart()
    {
        if (CurrentState == null || NewState == null)
            return Status.Failure;

        CurrentState.Value = NewState.Value;

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

