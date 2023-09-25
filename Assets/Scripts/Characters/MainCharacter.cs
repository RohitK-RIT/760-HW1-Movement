using UnityEngine;

namespace Characters
{
    public class MainCharacter : Character
    {
        protected override Vector2 TargetPosition => InputHandler.Instance.MousePosition;
    }
}