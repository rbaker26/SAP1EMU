using System;

namespace SAP1EMU.Lib.Components
{
    /// <summary>
    /// This will prevent "RaceCases" on the board.
    /// All push functions will happen on Tic and all pull functions will happen on Tok
    /// </summary>
    public struct TicTok
    {
        public enum State
        {
            Tic,
            Tok
        };

        public State ClockState { get; private set; }

        public void ToggleClockState()
        {
            if (ClockState == State.Tic)
            {
                ClockState = State.Tok;
            }
            else
            {
                ClockState = State.Tic;
            }
        }

        public void Init()
        {
            ClockState = State.Tic;
        }

        #region Equals Override Sutff

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.ClockState == ((TicTok)obj).ClockState;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(TicTok left, TicTok right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TicTok left, TicTok right)
        {
            return !(left == right);
        }

        #endregion Equals Override Sutff
    }
}