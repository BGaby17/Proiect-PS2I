using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
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
        private DispatcherTimer visibilityTimer;


        private double _liftOpacity = 1.0;
        public double LiftOpacity
        {
            get => _liftOpacity;
            set
            {
                if (_liftOpacity != value)
                {
                    _liftOpacity = value;
                    OnPropertyChanged(nameof(LiftOpacity));
                }
            }
        }

        private void VisibilityTimer_Tick(object sender, EventArgs e)
        {
            LiftOpacity = 0.5; // Reduce opacity
            visibilityTimer.Stop();
        }

        public void ShowLiftElement(System.Windows.Visibility element)
        {
            element = System.Windows.Visibility.Visible;
            LiftOpacity = 1.0; // Reset opacity
            visibilityTimer.Start();
        }

        public LiftViewModel()
        {
            ProcessStart = true;
            ProcessStop = false ;
            ProcessContinue = false;
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;

        }

        public void Init()
        {
            timer.Elapsed += _timer_Elapsed;
            worker.DoWork += _worker_DoWork;
            worker.RunWorkerAsync();
            Lift_E0 = System.Windows.Visibility.Visible;
            Use_E0 = System.Windows.Visibility.Visible;
            ProcesStart = false;
            ProcessStop = true;
            ProcessContinue = true;
            ButtonEnabledFloor1 = true;
            ButtonEnabledFloor2 = true;
            ButtonEnabledFloor3 = true;
            ButtonEnabledFloor4 = true;
        }

        private ProcessState _currentStateOfTheProcess = ProcessState.Running;

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
            if (!_isChangingStateInProgress)
            {
                _isChangingStateInProgress = true;
                _nextState = NextProcessState;
                timer.Interval = TimeInterval;
                timer.Start();
            }
        }

        private ProcessState _lastState;


        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (_lastState != TheStateOfTheProcess)
                {
                    ComputeNextStateAsync(TheStateOfTheProcess);
                    _lastState = TheStateOfTheProcess;
                }
               // System.Threading.Thread.Sleep(100);
            
            }
        }
        

        private bool _floor1=false,_floor2 = false, _floor3 = false, _floor4 = false;  // cu astea verific la ce etaj sunt inainte de apasa pe s5
        private void groundFloor()
        {
            Lift_E4 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Hidden;
            Lift_E2_E1 = System.Windows.Visibility.Hidden;
            Lift_E1 = System.Windows.Visibility.Hidden;
            Lift_E1_E0 = System.Windows.Visibility.Hidden;
            Lift_E0 = System.Windows.Visibility.Visible;

            Use_E4 = System.Windows.Visibility.Hidden;
            Use_E3 = System.Windows.Visibility.Hidden;
            Use_E2 = System.Windows.Visibility.Hidden;
            Use_E1 = System.Windows.Visibility.Hidden;
            Use_E0 = System.Windows.Visibility.Visible;
        }
        private async Task floor1()
        {
            _floor1 = true;
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;
            Lift_E4 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Hidden;
            Lift_E2_E1 = System.Windows.Visibility.Hidden;
            Use_E0 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            Lift_E0 = System.Windows.Visibility.Hidden;
            if (!_Lift_E1_E0)  // Verificăm dacă nu e deja vizibilă
            {
                Lift_E1_E0 = System.Windows.Visibility.Visible;
                await Task.Delay(1000);
                Lift_E1_E0 = System.Windows.Visibility.Hidden;
            }

            Lift_E1 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Use_E4 = System.Windows.Visibility.Hidden;
            Use_E3 = System.Windows.Visibility.Hidden;
            Use_E2 = System.Windows.Visibility.Hidden;
            Use_E1 = System.Windows.Visibility.Visible;



        }

        private async Task floor2()
        {
            _floor2 = true;
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;
            Use_E1 = System.Windows.Visibility.Hidden;
            Lift_E1 = System.Windows.Visibility.Hidden;
            Lift_E2_E1 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Lift_E2_E1 = System.Windows.Visibility.Hidden;
           // await Task.Delay(1000);
            Lift_E2 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Use_E2 = System.Windows.Visibility.Visible;
            Use_E1 = System.Windows.Visibility.Hidden;
        }
          
        private async Task floor3()
        {
            _floor3 = true;
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor4 = false;
            Use_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Use_E3 = System.Windows.Visibility.Visible;
            Use_E2 = System.Windows.Visibility.Hidden;

        }


        private async Task floor4()
        {

            _floor4 = true;
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            Use_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
            Lift_E4 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Use_E4 = System.Windows.Visibility.Visible;
            Use_E3 = System.Windows.Visibility.Hidden;

        }

        private async Task floor1_down()
        {
            _floor1 = false;
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;
           
            Lift_E4 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Hidden;
            Lift_E2_E1 = System.Windows.Visibility.Hidden;
            Lift_E0 = System.Windows.Visibility.Hidden;
            Use_E1 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            Lift_E1 = System.Windows.Visibility.Hidden;
            if (!_Lift_E1_E0)  // Verificăm dacă nu e deja vizibilă
            {
                Lift_E1_E0 = System.Windows.Visibility.Visible;
                await Task.Delay(1000);
                Lift_E1_E0 = System.Windows.Visibility.Hidden;
            }

            Lift_E0 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Use_E0 = System.Windows.Visibility.Visible;

            Use_E4 = System.Windows.Visibility.Hidden;
            Use_E3 = System.Windows.Visibility.Hidden;
            Use_E2 = System.Windows.Visibility.Hidden;

        }
        private async Task floor2_down()
        {
            _floor2 = false;
            Use_E1 = System.Windows.Visibility.Hidden;
            Lift_E1 = System.Windows.Visibility.Hidden;
            Use_E2 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            Lift_E2 = System.Windows.Visibility.Hidden;
            Lift_E2_E1 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Lift_E2_E1 = System.Windows.Visibility.Hidden;
            Lift_E1 = System.Windows.Visibility.Visible;
            //await Task.Delay(1000);
           // Use_E1 = System.Windows.Visibility.Visible;
            /*await Task.Delay(1000);
            Use_E1 = System.Windows.Visibility.Hidden;*/
        }

        private async Task floor3_down()
        {
            _floor3 = false;
            Use_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Hidden;
            Use_E3 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Visible;

        }

        private async Task floor4_down()
        {
            _floor4 = false;
            Use_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Hidden;
            Use_E4 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            Lift_E4 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Visible;

        }
        public async Task running()
        {

            if (_floor4 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor4_down();
                await floor3_down();
                await floor2_down();
                await floor1_down();
            }
            else if (_floor3 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor3_down();
                await floor2_down();
                await floor1_down();
            }
            else if (_floor2 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor2_down();
                await floor1_down();
            }
            else
                   if (_floor1 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor1_down();


            }

            ProcessStart = false;
            ButtonEnabledFloor1 = true;
            ButtonEnabledFloor2 = true;
            ButtonEnabledFloor3 = true;
            ButtonEnabledFloor4 = true;
        
        }
        public async Task stopped()
        {
            ProcessStop = true;
            ProcessContinue = true;
            ButtonEnabledFloor1 = true;
            ButtonEnabledFloor2 = true;
            ButtonEnabledFloor3 = true;
            ButtonEnabledFloor4 = true;
            Lift_E4 = System.Windows.Visibility.Visible;

        }

       
        public async Task ComputeNextStateAsync(ProcessState CurrentState)
        {
            switch (CurrentState)
            {
                case ProcessState.Stopped:

                    ///aici nush cum sa gestionez situatia  ca e destul de nasta sa te bagi peste procesul in curs
                  //  ProcessStart = true;
                 await   stopped();
                    break;

                case ProcessState.Running:
                 await running();
                    break;

                case ProcessState.GroundFloor:
                    
                    groundFloor();
                  //  ButtonEnabled = true;
                    break;

                case ProcessState.Floor1:
                    
                    await floor1();
                   // ButtonEnabled = true;
                    break;

                case ProcessState.Floor2:
                   
                    await floor1();
                    await floor2();

                   // ButtonEnabled = true;
                    break;

                case ProcessState.Floor3:
                    await floor1();
                    await floor2();
                    await floor3();

                  //  ButtonEnabled = true;

                    break;

                case ProcessState.Floor4:
                    await floor1();
                    await floor2();
                    await floor3();
                    await floor4();

                  //  ButtonEnabled = true;

                    break;
            }
        }

        public void ForceNextState(ProcessState NextState)
        {
            _isChangingStateInProgress = false;
            timer.Stop();
            TheStateOfTheProcess = NextState;
        }

        public bool _ButtonEnabled, _ButtonEnabledFloor1, _ButtonEnabledFloor2, _ButtonEnabledFloor3, _ButtonEnabledFloor4,
            _ProcessStart, _ProcessStop, _ProcessContinue;

        public bool ButtonEnabled
        {
            get { return _ButtonEnabled; }
            set
            {
                _ButtonEnabled = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }
        public bool ProcessStop
        {
            get { return _ProcessStop; }
            set
            {
                _ProcessStop = value;
                OnPropertyChanged(nameof(ProcessStop));
            }
        }
        public bool ProcessContinue
        {
            get { return _ProcessContinue; }
            set
            {
                _ProcessContinue = value;
                OnPropertyChanged(nameof(ProcessContinue));
            }
        }
        public bool ProcessStart
        {
            get { return _ProcessStart; }
            set
            {
                _ProcessStart = value;
                OnPropertyChanged(nameof(ProcessStart));
            }
        }
        public bool ButtonEnabledFloor1
        {
            get { return _ButtonEnabledFloor1; }
            set
            {
                _ButtonEnabledFloor1 = value;
                OnPropertyChanged(nameof(ButtonEnabledFloor1));
            }
        }

        public bool ButtonEnabledFloor2
        {
            get { return _ButtonEnabledFloor2; }
            set
            {
                _ButtonEnabledFloor2 = value;
                OnPropertyChanged(nameof(ButtonEnabledFloor2));
            }
        }


        public bool ButtonEnabledFloor3
        {
            get { return _ButtonEnabledFloor3; }
            set
            {
                _ButtonEnabledFloor3 = value;
                OnPropertyChanged(nameof(ButtonEnabledFloor3));
            }
        }

        public bool ButtonEnabledFloor4
        {
            get { return _ButtonEnabledFloor4; }
            set
            {
                _ButtonEnabledFloor4 = value;
                OnPropertyChanged(nameof(ButtonEnabledFloor4));
            }
        }




        public bool _ProcesStart = true;

        public bool ProcesStart
        {
            get { return _ProcesStart; }
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
                if (_Lift_E1 != newValue)
                {
                    _Lift_E1 = newValue;
                    OnPropertyChanged(nameof(Lift_E1));
                }
            }
        }

       // private System.Windows.Visibility _liftE1E0;
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
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
                bool newValue = (value == System.Windows.Visibility.Visible);
                if (_Use_E0 != newValue)
                {
                    _Use_E0 = newValue;
                    OnPropertyChanged(nameof(Use_E0));
                }
            }
        }
    }
}
