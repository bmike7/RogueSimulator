using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using RogueSimulator.Classes.Level;
using RogueSimulator.Interfaces;

namespace RogueSimulator.Classes.Mechanics.State
{
    public class PlayingState : IState
    {
        private const int PAUSE_BUTTON_HEIGHT = 20;
        private const int PAUSE_BUTTON_OFFSET = 40;
        private BaseLevel _currentLevel;
        private Game1 _game;
        private LevelFactory _levelFactory;
        private Button _pauseButton;
        private MouseState _prevMouseState;
        private LevelType _prevLevel;

        public PlayingState(Game1 game)
        {
            _game = game;
            _prevLevel = game.CurrentPlayingState.SelectedLevel;

            _levelFactory = new LevelFactory();
            _levelFactory.RegisterLevel(LevelType.LEVEL1, () => new Level1(game));
            _levelFactory.RegisterLevel(LevelType.LEVEL2, () => new Level2(game));
        }

        public void LoadContent()
        {
            if (_game.CurrentPlayingState.SelectedLevel != _prevLevel)
                _game.CurrentPlayingState.ResetMovement();

            _currentLevel = _levelFactory.LoadLevel(_game.CurrentPlayingState.SelectedLevel);
            _pauseButton = new Button(
                onClickAction: () =>
                {
                    _game.CurrentPlayingState.Movement = _currentLevel.Player.GetMovement();
                    _game.ChangeGameState(GameState.PAUSED);
                },
                buttonTexture: Utility.LoadTexture(_game, "SpriteSheets/Buttons/PauseButton"),
                position: new Vector2(_game.GraphicsDevice.Viewport.Width - PAUSE_BUTTON_OFFSET, PAUSE_BUTTON_OFFSET - PAUSE_BUTTON_HEIGHT),
                buttonSpriteRectangle: new Rectangle(3, 2, 10, 10),
                height: 20
            );

            _currentLevel.Create();
        }

        public void Update(GameTime gameTime)
        {
            checkMouseClick();
            _currentLevel.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentLevel.Draw(spriteBatch);

            spriteBatch.Begin();
            //Here will come other stuff that needs to be displayed always in the same spot (healthbar for instance)
            // _currentLevel.Player.Healthbar.Draw(spriteBatch)
            _pauseButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void checkMouseClick()
        {
            MouseState mouseState = Mouse.GetState();
            if (Utility.isMouseLeftButtonClicked(mouseState, _prevMouseState))
                Utility.ClickButtonIfMouseclickIntersects(mouseState, new Button[] { _pauseButton });
            _prevMouseState = mouseState;
        }
    }
}
