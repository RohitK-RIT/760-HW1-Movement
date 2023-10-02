using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Class to handle AI character's movement.
    /// </summary>
    public class AICharacter : Character
    {
        /// <summary>
        /// The main character which is being targeted by the AI.
        /// </summary>
        [SerializeField] private MainCharacter targetCharacter;
        protected override Vector2 TargetPosition => targetCharacter.transform.position;
    }
}