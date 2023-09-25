using UnityEngine;

namespace Characters
{
    public class AICharacter : Character
    {
        [SerializeField] private MainCharacter targetCharacter;
        protected override Vector2 TargetPosition => targetCharacter.transform.position;
    }
}