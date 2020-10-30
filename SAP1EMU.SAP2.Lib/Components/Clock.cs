using System;
using System.Collections.Generic;

namespace SAP1EMU.Lib.Components
{
    public class Clock : IObservable<TicTok>
    {
        public bool IsEnabled { get; set; }

        private readonly List<IObserver<TicTok>> observers;

        public Clock()
        {
            observers = new List<IObserver<TicTok>>();
        }

        public void SendTicTok(Nullable<TicTok> tictok)
        {
            foreach (var observer in observers)
            {
                if (!tictok.HasValue)
                    observer.OnError(new ClockException());
                else
                    observer.OnNext(tictok.Value);
            }
        }

        public IDisposable Subscribe(IObserver<TicTok> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<TicTok>> _observers;
            private readonly IObserver<TicTok> _observer;

            public Unsubscriber(List<IObserver<TicTok>> observers, IObserver<TicTok> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
    }
}