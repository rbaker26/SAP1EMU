using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Components
{
    public struct TicTok
    {
        public enum State
        {
            Tic,
            Tok
        };


        // TODO - This will prevent racecases when one reg is pushing to wbus and another is pulling
        // All pushing to Wbus will happen on Tic
        // All pulling from Wbus will happen on Toc
        public State ClockState { get; private set; }
        public void ToggleClockState() 
        {
            if(ClockState == State.Tic)
            {
                ClockState = State.Tok;
            }
            else
            {
                ClockState = State.Tic;

            }
        }
    }
}
