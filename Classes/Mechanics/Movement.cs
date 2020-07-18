using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RogueSimulator.Classes.Mechanics
{
    public class Movement
    {
        public Movement(Vector2 pos, CharacterAction action = CharacterAction.IDLE, CharacterDirection direction = CharacterDirection.RIGHT)
        {
            Position = new Position(pos.X, pos.Y);
            Action = action;
            Direction = direction;
        }

        public CharacterAction Action { get; private set; }
        public CharacterDirection Direction { get; private set; }
        public Position Position { get; private set; }

        public void Update(GameTime gameTime, CollisionBlock[] collisionBlocks)
        {
            Vector2 nextPos = Position.GetNextPosition(gameTime, Direction, Action, collisionBlocks);

            // The Direction and Action needs to be updated before the position
            // Because they depend on the current and the next one.
            updateDirection(nextPos);
            updateAction(nextPos);

            Position.Update(nextPos);
        }

        private void updateDirection(Vector2 newPos)
        {
            bool isLeft = newPos.X < Position.X;
            bool isRight = newPos.X > Position.X;

            Direction = isRight
                ? CharacterDirection.RIGHT
                : isLeft
                    ? CharacterDirection.LEFT
                    : Direction;
        }

        private void updateAction(Vector2 newPos)
        {
            if (newPos.Y != Position.Y)
            {
                Action = newPos.Y < Position.Y ? CharacterAction.JUMP : CharacterAction.FALL;
                return;
            }

            Action = newPos.X != Position.X ? CharacterAction.RUN : CharacterAction.IDLE;
        }
    }
}