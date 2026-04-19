using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "LineOfSightCheck", story: "Check [target] with [LineOfSightDetector]", category: "Conditions", id: "023d879baeecfedaf3cb9f66ba6dbbcc")]
public partial class LineOfSightCheckCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<LineOfSightDetector> LineOfSightDetector;

    public override bool IsTrue()
    {
        return LineOfSightDetector.Value.PerformDetection(Target.Value) != null;
    }
}
