using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetDistanceToNextPoint", story: "Set Distance between [self] and [currentPatrolPoint] and save it to [DistanceToNextPoint]", category: "Action", id: "4ec9297b81034931e64bf45158f4feec")]
public partial class SetDistanceToNextPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> CurrentPatrolPoint;
    [SerializeReference] public BlackboardVariable<float> DistanceToNextPoint;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value == null || CurrentPatrolPoint.Value == null)
            return Status.Failure;

        DistanceToNextPoint.Value = Vector3.Distance(Self.Value.transform.position, CurrentPatrolPoint.Value.transform.position);

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

