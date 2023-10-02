using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Class to handle Main character's movement.
    /// </summary>
    public class MainCharacter : Character
    {
        protected override Vector2 TargetPosition => InputHandler.Instance.MousePosition;
    }
}