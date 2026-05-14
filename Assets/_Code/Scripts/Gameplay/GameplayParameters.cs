using System;
using UnityEngine;

namespace _Code.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayParameters", menuName = "Parameters/Gameplay", order = 0)]
    public class GameplayParameters : ScriptableObject
    {
        
        [Header("Scenes")]
        public String[] scenes;
        
    }
}