using Anura.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anura.ConfigurationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShapeList", menuName = "Configurations/ShapeList", order = 2)]
    public class ShapeList : ScriptableObject
    {
        [SerializeField] 
        private List<ShapeConfig> shapes;

        internal ShapeConfig GetShape(int idx)
        {
            return shapes[idx];
        }
    }
}
