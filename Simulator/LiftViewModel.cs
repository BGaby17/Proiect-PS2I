using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateModel;

namespace Simulator
{

    //LiftViewModel implementează INotifyPropertyChanged,
    //ceea ce înseamnă că notifică UI-ul (interfața grafică)
    //atunci când valorile proprietăților sale se schimbă.
    internal class LiftViewModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        //Metoda care se activaza cand o propetate se modifica
        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }


        //BackgroundWorker rulează în fundal pentru a calcula starea următoare a procesului.
        private BackgroundWorker worker = new BackgroundWorker();

        //System.Timers.Timer este utilizat pentru a temporiza tranzițiile între stările liftului.
        private System.Timers.Timer timer = new System.Timers.Timer();
        // private Sender _aplicatieMonitorizareRetea;

        public LiftViewModel() { }

        //Initializarea ViewModel-ului (MVVM)
        public void Init()
        {
            //Se apleaza metoada _timer_Elapsed
            timer.Elapsed += _timer_Elapsed;
            //Se apleaza metoada _worker_DoWork
            worker.DoWork += _worker_DoWork;
            worker.RunWorkerAsync();//Porneste lucratorul in fundal
            ProcesStart = false;
        }

        //varibila in care intialize prima stare a procesului
        private ProcessState _currentStateOfTheProcess = ProcessState.Pornit;

        //metote de citire scriere 
        public ProcessState TheStateOfTheProcess
        {
            get => _currentStateOfTheProcess;
            set
            {
                _currentStateOfTheProcess = value;

            }
        }

        private void _timer_Elapsed(Object sender, System.Timers.ElapsedEventArgs e)
        {
            _isChangingStateInProgress = false;
            TheStateOfTheProcess = _nextState;
            timer.Stop();
        }

        private bool _isChangingStateInProgress = false;
        private ProcessState _nextState;

        private void ChangeProcessState(ProcessState NextProcessState, int TimeInterval)
        {
            // un eveniment de tranzitie odata ridicat, trebuie sa fie consumat
            // nu se poate ridica un alt eveniment de tranzitie pana cand tranzitia curenta este efectuata
            if (!_isChangingStateInProgress)
            {
                _isChangingStateInProgress = true;
                _nextState = NextProcessState;
                timer.Interval = TimeInterval;
                timer.Start();
            }
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                ComputeNextState(TheStateOfTheProcess);
                System.Threading.Thread.Sleep(100);
            }
        }

        public void ComputeNextState(ProcessState CurrentState)
        {
            switch(CurrentState)
            {
                case ProcessState.Stop:
                    ButtonEnabled = false;
                    Lift_E4 = System.Windows.Visibility.Visible;


                    break;

                case ProcessState.Pornit:
                    ButtonEnabled =true;

                    Lift_E4 = System.Windows.Visibility.Hidden;
                    Lift_E4_E3 = System.Windows.Visibility.Hidden;
                    Lift_E3= System.Windows.Visibility.Hidden;
                    Lift_E3_E2= System.Windows.Visibility.Hidden;
                    Lift_E2= System.Windows.Visibility.Hidden;
                    Lift_E2_E1 = System.Windows.Visibility.Hidden;
                    Lift_E1 = System.Windows.Visibility.Hidden;
                    Lift_E1_E0 = System.Windows.Visibility.Hidden;
                    Lift_E0= System.Windows.Visibility.Visible; 

                    Use_E4= System.Windows.Visibility.Hidden;
                    Use_E3= System.Windows.Visibility.Hidden;
                    Use_E2 = System.Windows.Visibility.Hidden;
                    Use_E1 = System.Windows.Visibility.Hidden;
                    Use_E0 = System.Windows.Visibility.Visible;

                    break;

                case ProcessState.Parter:

                    break;

                case ProcessState.Etaji1:

                    break;
                case ProcessState.Etaji2:

                    break;
                case ProcessState.Etaji3:

                    break;
                case ProcessState.Etaji4:

                    break;
            }
        }

        //Metoda care schimba starea procesului fara sa astepte finalizarea tiner-ului
        public void ForceNextState(ProcessState NextState)
        {
            _isChangingStateInProgress = false;
            timer.Stop();
            TheStateOfTheProcess = NextState;
        }

        public bool _ButtonEnabled;

        public bool ButtonEnabled
        {
            get
            { 
                return _ButtonEnabled; 
            }
            set
            {
                _ButtonEnabled = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }

        public bool _ProcesStart=true;

        public bool ProcesStart
        {
            get
            {
                return _ProcesStart;
            }
            set
            {
                _ProcesStart = value;
                OnPropertyChanged(nameof(ProcesStart));
            }
        }

        public bool _Lift_E4;
        public System.Windows.Visibility Lift_E4
        {
            get => _Lift_E4 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E4 != newValue)
                {
                    _Lift_E4 = newValue;
                    OnPropertyChanged(nameof(Lift_E4));
                }
            }
        }

        public bool _Lift_E4_E3;
        public System.Windows.Visibility Lift_E4_E3
        {
            get => _Lift_E4_E3 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E4_E3 != newValue)
                {
                    _Lift_E4_E3 = newValue;
                    OnPropertyChanged(nameof(Lift_E4_E3));
                }
            }
        }

        public bool _Lift_E3;
        public System.Windows.Visibility Lift_E3
        {
            get => _Lift_E3 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E3 != newValue)
                {
                    _Lift_E3 = newValue;
                    OnPropertyChanged(nameof(Lift_E3));
                }
            }
        }

        public bool _Lift_E3_E2;
        public System.Windows.Visibility Lift_E3_E2
        {
            get => _Lift_E3_E2 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E3_E2 != newValue)
                {
                    _Lift_E3_E2 = newValue;
                    OnPropertyChanged(nameof(Lift_E3_E2));
                }
            }
        }

        public bool _Lift_E2;
        public System.Windows.Visibility Lift_E2
        {
            get => _Lift_E2 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E2 != newValue)
                {
                    _Lift_E2 = newValue;
                    OnPropertyChanged(nameof(Lift_E2));
                }
            }
        }

        public bool _Lift_E2_E1;
        public System.Windows.Visibility Lift_E2_E1
        {
            get => _Lift_E2_E1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E2_E1 != newValue)
                {
                    _Lift_E2_E1 = newValue;
                    OnPropertyChanged(nameof(Lift_E2_E1));
                }
            }
        }

        public bool _Lift_E1;
        public System.Windows.Visibility Lift_E1
        {
            get => _Lift_E1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E1 != newValue)
                {
                    _Lift_E1 = newValue;
                    OnPropertyChanged(nameof(Lift_E1));
                }
            }
        }

        public bool _Lift_E1_E0;
        public System.Windows.Visibility Lift_E1_E0
        {
            get => _Lift_E1_E0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E1_E0 != newValue)
                {
                    _Lift_E1_E0 = newValue;
                    OnPropertyChanged(nameof(Lift_E1_E0));
                }
            }
        }

        public bool _Lift_E0;
        public System.Windows.Visibility Lift_E0
        {
            get => _Lift_E0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Lift_E0 != newValue)
                {
                    _Lift_E0 = newValue;
                    OnPropertyChanged(nameof(Lift_E0));
                }
            }
        }

        public bool _Use_E4;
        public System.Windows.Visibility Use_E4
        {
            get => _Use_E4 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Use_E4 != newValue)
                {
                    _Use_E4 = newValue;
                    OnPropertyChanged(nameof(Use_E4));
                }
            }
        }

        public bool _Use_E3;
        public System.Windows.Visibility Use_E3
        {
            get => _Use_E3 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Use_E3 != newValue)
                {
                    _Use_E3 = newValue;
                    OnPropertyChanged(nameof(Use_E3));
                }
            }
        }

        public bool _Use_E2;
        public System.Windows.Visibility Use_E2
        {
            get => _Use_E2 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Use_E2 != newValue)
                {
                    _Use_E2 = newValue;
                    OnPropertyChanged(nameof(Use_E2));
                }
            }
        }

        public bool _Use_E1;
        public System.Windows.Visibility Use_E1
        {
            get => _Use_E1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Use_E1 != newValue)
                {
                    _Use_E1 = newValue;
                    OnPropertyChanged(nameof(Use_E1));
                }
            }
        }

        public bool _Use_E0;
        public System.Windows.Visibility Use_E0
        {
            get => _Use_E0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            set
            {
                bool newValue = (value == System.Windows.Visibility.Visible); // Convertim Visibility în bool
                if (_Use_E0 != newValue)
                {
                    _Use_E0 = newValue;
                    OnPropertyChanged(nameof(Use_E0));
                }
            }
        }
    }
}
