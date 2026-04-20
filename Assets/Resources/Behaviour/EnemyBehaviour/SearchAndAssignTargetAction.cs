using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SearchAndAssignTarget", story: "Update [RangeDetector] and assign [Target]", category: "Action", id: "0a897171498fb982645d41e62f6d0000")]
public partial class SearchAndAssignTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<RangeDetector> RangeDetector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = RangeDetector.Value.UpdateDetector();
        return Target.Value == null ? Status.Failure : Status.Success;
    }
}

