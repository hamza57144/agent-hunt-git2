﻿using UnityEngine;

namespace CoverShooter.AI
{
    [Folder("Move")]
    [Success("Done")]
    [AllowAimAndFire]
    [AllowCrouch]
    public class Move : BaseAction
    {
        [ValueType(ValueType.RelativeDirection)]
        [ValueType(ValueType.Vector3)]
        public Value Direction = new Value(CoverShooter.Direction.Forward);

        [ValueType(ValueType.Speed)]
        public Value Speed = new Value(CharacterSpeed.Walk);

        [ValueType(ValueType.Float)]
        public Value Distance = new Value(1);

        [ValueType(ValueType.Facing)]
        [ValueType(ValueType.RelativeDirection)]
        [ValueType(ValueType.Vector3)]
        [ValueType(ValueType.GameObject)]
        public Value Facing = new Value(CharacterFacing.None);

        public override void Enter(State state, int layer, ref ActionState values)
        {
            values.Position = state.Actor.transform.position;
        }

        public override AIResult Update(State state, int layer, ref ActionState values)
        {
            var actor = state.Actor;

            var direction = state.GetDirection(ref Direction);
            values.Covered += Vector3.Dot(direction, actor.transform.position - values.Position);
            values.Position = actor.transform.position;

            float speed = 1;

            switch (state.Dereference(ref Speed).Speed)
            {
                case CharacterSpeed.Walk: speed = 0.5f; break;
                case CharacterSpeed.Run: speed = 1.0f; break;
                case CharacterSpeed.Sprint: speed = 2.0f; break;
            }

            Vector3 facing;
            if (state.GetFacing(ref Facing, direction, out facing))
                actor.InputLook(actor.transform.position + facing * 100);

            actor.InputMovement(new CharacterMovement(direction, speed));

            if (values.Covered >= state.Dereference(ref Distance).Float)
                return AIResult.Finish();
            else
                return AIResult.Hold();
        }
    }
}