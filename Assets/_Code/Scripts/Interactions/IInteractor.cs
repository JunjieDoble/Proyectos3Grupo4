using UnityEngine;

namespace Interactions
{
    public interface IInteractor
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
    }
}