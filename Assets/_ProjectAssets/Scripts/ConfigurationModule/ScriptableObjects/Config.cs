using UnityEngine;

namespace Anura.ConfigurationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Config", menuName = "Configurations/Config", order = 1)]
    public class Config : ScriptableObject
    {
        [Header("Player configurations")]
        [Space]

        [SerializeField, Tooltip("Whether or not a player can steer while jumping")]
        private bool airControl = false;

        [SerializeField] private float playerSpeed;
        
        [SerializeField, Tooltip("Amount of force added when the player jumps.")]
        private float playerJumpForce;
        
        [Range(0, .3f), SerializeField, Tooltip("How much to smooth out the movement")] 
        private float movementSmoothing = .05f;

        public bool GetAirControl()
        {
            return airControl;
        }
     
        public float GetPlayerSpeed()
        {
            return playerSpeed;
        }

        public float GetPlayerJumpForce()
        {
            return playerJumpForce;
        }

        public float GetMovementSmoothing()
        {
            return movementSmoothing;
        }
    }
}
