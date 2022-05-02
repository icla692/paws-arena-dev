using Anura.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Anura.ConfigurationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShapeList", menuName = "Configurations/ShapeList", order = 2)]
    public class ShapeList : ScriptableObject
    {
        [SerializeField] 
        private List<ShapeConfig> shapes;

        public ShapeConfig GetRandomShape()
        {
            return shapes[Random.Range(0, shapes.Count)];
        }
    }
}
