
using UnityEngine;

/*
   Created by DennyLiu on 2021.08.07
   For new player controller, taking care of underline. 
 */
namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerController_ : ControllerBase
    {
        PlayerCharacter m_playerCharacter;


        protected new void Awake()
        {
            base.Awake();
            Debug.Log("PlayerController_ Awake");
            m_playerCharacter = GetComponent<PlayerCharacter>();
        }

        protected new void Update()
        {
            base.Update();

        }

        protected new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void Move(float moveSpeed)
        {
            base.Move(moveSpeed);
        }
    }



}
