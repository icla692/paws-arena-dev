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

        [Header("Indicator configurations")]
        [Space]

        [SerializeField] private float indicatorSpeed;
        [SerializeField] private Vector2 bulletSpeed;
        [SerializeField, Min(0.3f)] private Vector2 pressTimer;
        [SerializeField] private float bufferMaxTimer;
        [SerializeField] private float factorRotationRocket;

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

        //indicator

        public float GetIndicatorSpeed()
        {
            return indicatorSpeed;
        }

        public float GetBulletSpeed(float seconds)
        {
            return seconds * GetSpeedPerSecond();
        }

        public Vector2 GetPressTimer()
        {
            return pressTimer;
        }

        public float GetBufferMaxTimer()
        {
            return bufferMaxTimer;
        }

        public float GetValidIndicatorTime()
        {
            return GetPressTimer().y + GetBufferMaxTimer();
        }

        public float GetFactorRotationIndicator()
        {
            return factorRotationRocket;
        }

        private float GetSpeedPerSecond()
        {
            return bulletSpeed.y / GetPressTimer().y;
        }
    }
}
