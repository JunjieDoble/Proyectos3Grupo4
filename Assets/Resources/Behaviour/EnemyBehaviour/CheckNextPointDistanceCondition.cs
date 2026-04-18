using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckNextPointDistance", story: "[DistanceToNextPoint] is [condition] [minimDistance]", category: "Conditions", id: "a3e402f491f7df935e76a6fe60bdc0ee")]
public partial class CheckNextPointDistanceCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> DistanceToNextPoint;
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Condition;
    [SerializeReference] public BlackboardVariable<float> MinimDistance;

    public override bool IsTrue()
    {
        if (DistanceToNextPoint == null || Condition == null || MinimDistance == null)
            return false;

        return DistanceToNextPoint.Value <= MinimDistance.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
