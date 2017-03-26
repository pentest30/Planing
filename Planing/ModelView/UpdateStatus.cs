using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Planing.ModelView
{
    public enum EtatProgression
    {
        Ok,
        Error
    }
    public class UpdateStatus : INotifyPropertyChanged
    {
        public UpdateStatus()
        {
            TempSpan =new TimeSpan();
            _fitness = 0;
            _etetHeuristic = EtatProgression.Error;
        }
        private TimeSpan     _temp  ;

        public TimeSpan TempSpan
        {
            get { return _temp; }
            set
            {
                _temp = value;
                OnPropertyChanged();
            }
        }
        private EtatProgression _etetHeuristic;

        public EtatProgression EtatHeuristique
        {
            get { return _etetHeuristic; }
            set
            {
                _etetHeuristic = value;
                OnPropertyChanged();
            }
        }
        
        private double _fitness;

        public double Fitness
        {
            get { return _fitness; }
            set
            {
                _fitness = value; 
                OnPropertyChanged();
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
