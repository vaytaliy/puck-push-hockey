using System;
using System.Collections.Generic;
using System.Text;


namespace Hockey
{
    class GameStateManager
    {
        public GameStateManager()
        {
            CurrentState = ActiveState.MainMenu;
        }

        private enum ActiveState {MainMenu, Pause, Playing }
        private ActiveState CurrentState { get; set; }

        //public 

    }

    //public gameState 

}
