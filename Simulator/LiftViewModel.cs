using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Communicator;
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
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool IsContinuePressed = false;
        private Sender _aplicatieMonitorizareRetea;
        private bool first = true;
        public void Init()
        {
            _aplicatieMonitorizareRetea = new Sender("127.0.0.1", 3000);
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
        public async Task CancelTasks()
        {
            _cancellationTokenSource.Cancel();
            if (_floor4 == true  ) { 
                await Task.Delay(1000);
            Use_E4 = System.Windows.Visibility.Visible;
            }
           else if(_floor3 == true  && _floor43==false)
            {
                await Task.Delay(1000);
                Use_E3 = System.Windows.Visibility.Visible;
            }
            else if (_floor2 == true && _floor32==false)
            {
                await Task.Delay(1000);
                Use_E2 = System.Windows.Visibility.Visible;
            }
            else if (_floor1 == true && _floor21==false)
            {
                await Task.Delay(1000);
                Use_E1 = System.Windows.Visibility.Visible;
            }
            else  if(_floor0 == true && _floor10 == false)
            {
                await Task.Delay(1000);
                Use_E0 = System.Windows.Visibility.Visible;
            }
        }

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

      

       

        public ProcessState TheStateOfTheProcess
        {
            get => _currentStateOfTheProcess;
            set
            {
                _currentStateOfTheProcess = value;
                _aplicatieMonitorizareRetea.Send(Convert.ToByte(_currentStateOfTheProcess));
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

        bool continueProcessing = true; // Variabilă de control
        private ProcessState _currentStateOfTheProcess = ProcessState.Wait;
        private async void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
           

            while (true)
            {
                await ComputeNextState(TheStateOfTheProcess);
                
            }
        }



        private bool _floor0=false, _floor1 = false, _floor2 = false, _floor3 = false, _floor4 = false, _floor21 = false, _floor32 = false, _floor10 = false, _floor43 = false; // cu astea verific la ce etaj sunt inainte de apasa pe s5
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
        private async Task floor1(CancellationToken cancellationToken)
        {
            try
            {
                _floor0 = false;
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

                // Start moving from 0 to between 0 and 1
                await Task.Delay(1000, cancellationToken);
                ChangeProcessState(ProcessState.GoingUp, 10);
                Lift_E0 = System.Windows.Visibility.Hidden;

                if (!_Lift_E1_E0)
                {
                    Lift_E1_E0 = System.Windows.Visibility.Visible;
                    _floor10 = true;
                   
                    await Task.Delay(1000, cancellationToken);
                    ChangeProcessState(ProcessState.GoingUp, 10);
                    Lift_E1_E0 = System.Windows.Visibility.Hidden;
                    _floor10 = false;
                    _floor1= true;
                }



                Lift_E1 = System.Windows.Visibility.Visible;
                await Task.Delay(1000, cancellationToken);
                Use_E4 = System.Windows.Visibility.Hidden;
                Use_E3 = System.Windows.Visibility.Hidden;
                Use_E2 = System.Windows.Visibility.Hidden;
                Use_E1 = System.Windows.Visibility.Visible;

                if (etaj == ProcessState.Floor1)
                    ChangeProcessState(ProcessState.Wait, 1000);
            }
            catch (TaskCanceledException)
            {
                if (ProcessStop)
                {
                    // Preserve intermediate shaft if stopped between floors
                    if (_floor10)
                        Lift_E1_E0 = System.Windows.Visibility.Visible;

                    // Force state to Stopped to prevent any further transitions
                   // await stopped();
                    ForceNextState(ProcessState.Stopped);
                }
            }

        }

        private async Task floor2(CancellationToken cancellationToken)
        {
            try
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
                Use_E1 = System.Windows.Visibility.Hidden;

                await Task.Delay(1000, cancellationToken);
                ChangeProcessState(ProcessState.GoingUp, 10);
                Lift_E1 = System.Windows.Visibility.Hidden;

                if (!_Lift_E2_E1)
                {
                    Lift_E2_E1 = System.Windows.Visibility.Visible;
                    _floor21 = true;
                    await Task.Delay(1000, cancellationToken);
                    ChangeProcessState(ProcessState.GoingUp, 10);
                    Lift_E2_E1 = System.Windows.Visibility.Hidden;
                    _floor21 = false;
                    _floor2 = true;
                }

                Lift_E2 = System.Windows.Visibility.Visible;
                await Task.Delay(1000, cancellationToken);
                Use_E4 = System.Windows.Visibility.Hidden;
                Use_E3 = System.Windows.Visibility.Hidden;
                Use_E2 = System.Windows.Visibility.Visible;

                if (etaj == ProcessState.Floor2)
                    ChangeProcessState(ProcessState.Wait, 1000);
            }
            catch (TaskCanceledException)
            {



                if (ProcessStop)
                {
                    // Preserve intermediate shaft if stopped between floors
                    if (_floor21)
                        Lift_E2_E1 = System.Windows.Visibility.Visible;

                    // Force state to Stopped to prevent any further transitions
                    // await stopped();
                    ForceNextState(ProcessState.Stopped);
                }

            }
        }

        private async Task floor3(CancellationToken cancellationToken)
        {
            try { 
            _floor2 = false;   ///
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;

            Lift_E4 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
          //  Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Use_E2 = System.Windows.Visibility.Hidden;

            await Task.Delay(1000, cancellationToken);
            ChangeProcessState(ProcessState.GoingUp, 10);
            Lift_E2 = System.Windows.Visibility.Hidden;

            if (!_Lift_E3_E2)
            {
                Lift_E3_E2 = System.Windows.Visibility.Visible;
                _floor32 = true;
                await Task.Delay(1000, cancellationToken);
                    ChangeProcessState(ProcessState.GoingUp, 10);
                    Lift_E3_E2 = System.Windows.Visibility.Hidden;
                _floor32 = false;
                _floor3 = true;
            }

            Lift_E3 = System.Windows.Visibility.Visible;
            await Task.Delay(1000, cancellationToken);
            Use_E4 = System.Windows.Visibility.Hidden;
            Use_E3 = System.Windows.Visibility.Visible;

            if (etaj == ProcessState.Floor3)
                ChangeProcessState(ProcessState.Wait, 1000);
                }
            catch(TaskCanceledException)
            {



                if (ProcessStop)
                {
                    // Preserve intermediate shaft if stopped between floors
                    if (_floor32)
                        Lift_E3_E2 = System.Windows.Visibility.Visible;

                    // Force state to Stopped to prevent any further transitions
                    // await stopped();
                    ForceNextState(ProcessState.Stopped);
                }



            }
        }



        private async Task floor4(CancellationToken cancellationToken)
        {
            try
            {
                _floor3 = false;
                ButtonEnabledFloor1 = false;
                ButtonEnabledFloor2 = false;
                ButtonEnabledFloor3 = false;
                ButtonEnabledFloor4 = false;

                Lift_E4 = System.Windows.Visibility.Hidden;
                Lift_E4_E3 = System.Windows.Visibility.Hidden;
               // Lift_E3 = System.Windows.Visibility.Hidden;   ///AICI FU PPRBL
                Use_E3 = System.Windows.Visibility.Hidden;

                await Task.Delay(1000, cancellationToken);
                ChangeProcessState(ProcessState.GoingUp, 10);
                Lift_E3 = System.Windows.Visibility.Hidden;

                if (!_Lift_E4_E3)
                {
                    Lift_E4_E3 = System.Windows.Visibility.Visible;
                    _floor43 = true;
                    await Task.Delay(1000, cancellationToken);
                    ChangeProcessState(ProcessState.GoingUp, 10);
                    Lift_E4_E3 = System.Windows.Visibility.Hidden;
                    _floor43 = false;
                    _floor4 = true;
                }

                Lift_E4 = System.Windows.Visibility.Visible;
                await Task.Delay(1000, cancellationToken);
                Use_E4 = System.Windows.Visibility.Visible;

                if (etaj == ProcessState.Floor4)
                    ChangeProcessState(ProcessState.Wait, 1000);
            }
            catch(TaskCanceledException)
            {


                if (ProcessStop)
                {
                    // Preserve intermediate shaft if stopped between floors
                    if (_floor43)
                        Lift_E4_E3 = System.Windows.Visibility.Visible;

                    // Force state to Stopped to prevent any further transitions
                    // await stopped();
                    ForceNextState(ProcessState.Stopped);
                }

            }
        }

        ProcessState etaj;
        private async Task floor1_down()
        {

           

             ButtonEnabledFloor1 = false;
             ButtonEnabledFloor2 = false;
             ButtonEnabledFloor3 = false;
             ButtonEnabledFloor4 = false;
           
             _floor1 = false;


               Lift_E4 = System.Windows.Visibility.Hidden;
               Lift_E4_E3 = System.Windows.Visibility.Hidden;
               Lift_E3 = System.Windows.Visibility.Hidden;
               Lift_E3_E2 = System.Windows.Visibility.Hidden;
               Lift_E2 = System.Windows.Visibility.Hidden;
               Lift_E2_E1 = System.Windows.Visibility.Hidden;
               Lift_E0 = System.Windows.Visibility.Hidden;
               Use_E1 = System.Windows.Visibility.Hidden;
               await Task.Delay(1000);
            ChangeProcessState(ProcessState.GoingDown, 10);

            Lift_E1 = System.Windows.Visibility.Hidden;
              if (!_Lift_E1_E0)  // Verificăm dacă nu e deja vizibilă
               {
                   Lift_E1_E0 = System.Windows.Visibility.Visible;
                   await Task.Delay(1000);
                ChangeProcessState(ProcessState.GoingDown, 10);
                Lift_E1_E0 = System.Windows.Visibility.Hidden;
               }
            Lift_E1_E0 = System.Windows.Visibility.Hidden;
            Lift_E0 = System.Windows.Visibility.Visible;

            if (first)
            {
                await Task.Delay(1000);
                ChangeProcessState(ProcessState.GoingDown, 10);
                first = false;
            }

            else
            {
                await Task.Delay(1000);
                ChangeProcessState(ProcessState.GoingDown, 10);

            }


            Use_E0 = System.Windows.Visibility.Visible;
           // ChangeProcessState(ProcessState.GroundFloor, 1000);
            ForceNextState(ProcessState.GroundFloor);
            Use_E4 = System.Windows.Visibility.Hidden;
               Use_E3 = System.Windows.Visibility.Hidden;
               Use_E2 = System.Windows.Visibility.Hidden;
            ProcessStop = true;

        }
        private async Task floor2_down()
        {
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;
           
            _floor2 = false;
            Use_E1 = System.Windows.Visibility.Hidden;
            Lift_E1 = System.Windows.Visibility.Hidden;
            Use_E2 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            ChangeProcessState(ProcessState.GoingDown, 10);

            Lift_E2 = System.Windows.Visibility.Hidden;
            if (!_Lift_E2_E1)  // Verificăm dacă nu e deja vizibilă
            {
                Lift_E2_E1 = System.Windows.Visibility.Visible;
                await Task.Delay(1000);
                ChangeProcessState(ProcessState.GoingDown, 10);
                Lift_E2_E1 = System.Windows.Visibility.Hidden;
            }

            Lift_E2_E1 = System.Windows.Visibility.Hidden;
            Lift_E1 = System.Windows.Visibility.Visible;

            await Task.Delay(1000);
            //if(etaj==ProcessState.Floor2)
            ChangeProcessState(ProcessState.GoingDown, 1000);
            
          //  await Task.Delay(1000);

        }

        private async Task floor3_down()
        {
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;
          
            _floor3 = false;
            Use_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Hidden;
            Use_E3 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            ChangeProcessState(ProcessState.GoingDown, 10);
            Lift_E3 = System.Windows.Visibility.Hidden;
            Lift_E3_E2 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
           // if (etaj == ProcessState.Floor3)
                ChangeProcessState(ProcessState.GoingDown, 10);
            Lift_E3_E2 = System.Windows.Visibility.Hidden;
            Lift_E2 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);

        }

        private async Task floor4_down()
        {
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;
            ProcessStop = true;
         //   ChangeProcessState(ProcessState.GoingDown, 1000);
            _floor4 = false;
            Use_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Hidden;
            Use_E4 = System.Windows.Visibility.Hidden;
            await Task.Delay(1000);
            ChangeProcessState(ProcessState.GoingDown, 10);
            Lift_E4 = System.Windows.Visibility.Hidden;
            Lift_E4_E3 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);
           // if (etaj == ProcessState.Floor3)
                ChangeProcessState(ProcessState.GoingDown, 10);
            Lift_E4_E3 = System.Windows.Visibility.Hidden;
            Lift_E3 = System.Windows.Visibility.Visible;
            await Task.Delay(1000);

        }

       
        public async Task running()
        {
          
            if (Use_E4==Visibility.Visible)
            {
                await floor4_down();
                await floor3_down();
                await floor2_down();
                await floor1_down();
            }
            else if (Use_E3 == Visibility.Visible)
            {
                await floor3_down();
                await floor2_down();
                await floor1_down();
            }
            else if (Use_E2 == Visibility.Visible)
            {
                await floor2_down();
                await floor1_down();
            }
            else if (Use_E1 == Visibility.Visible)
            {
                await floor1_down();
            }

            if (_floor4 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor4_down();
                await floor3_down();
                await floor2_down();
                await floor1_down();
                
            }
            
          else if (_floor43 == true && ProcessStart == false && ProcessContinue == true)
            {
                _floor43 = false;

                Lift_E4_E3 = System.Windows.Visibility.Hidden;
                Lift_E3 = System.Windows.Visibility.Visible;
                ChangeProcessState(ProcessState.GoingDown, 10);
                // await Task.Delay(1000);
                await floor3_down();
                await floor2_down();
                await floor1_down();
                //TheStateOfTheProcess = ProcessState.Running;
            }
            else if (_floor3 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor3_down();
                await floor2_down();
                await floor1_down();
               // TheStateOfTheProcess = ProcessState.Running;
            }
            else if (_floor32 == true && ProcessStart == false && ProcessContinue == true)
            {
                _floor32 = false;
                Lift_E3_E2 = System.Windows.Visibility.Hidden;
                Lift_E2 = System.Windows.Visibility.Visible;
                ChangeProcessState(ProcessState.GoingDown, 10);
                // await Task.Delay(1000);
                await floor2_down();
                await floor1_down();
            //    TheStateOfTheProcess = ProcessState.Running;
            }
            else if (_floor2 == true && ProcessStart == false && ProcessContinue == true)
            {
                await floor2_down();
                await floor1_down();
               // TheStateOfTheProcess = ProcessState.Running;
            }
            else if (_floor21 == true && ProcessStart == false && ProcessContinue == true)
            {
                _floor21 = false;
                Lift_E2_E1 = System.Windows.Visibility.Hidden;
                Lift_E1 = System.Windows.Visibility.Visible;
                ChangeProcessState(ProcessState.GoingDown, 10);
                // await Task.Delay(1000);
                //  await floor2_down();
                await floor1_down();
               // TheStateOfTheProcess = ProcessState.Running;
            }
            else if (_floor10 == true && ProcessStart == false && ProcessContinue == true)
            {
                _floor10 = false;
               // Lift_E1_E0 = System.Windows.Visibility.Hidden;
                //Lift_E0 = System.Windows.Visibility.Visible;
              //   await Task.Delay(1000);
                //  await floor2_down();
                await floor1_down();
                //TheStateOfTheProcess = ProcessState.Running;
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
          //  ProcessStop = true;
            ProcessContinue = true; // Dezactivăm Continue până la apăsare
            ButtonEnabledFloor1 = false;
            ButtonEnabledFloor2 = false;
            ButtonEnabledFloor3 = false;
            ButtonEnabledFloor4 = false;

            // Așteaptă până când butonul Continue este apăsat
            while (!ProcessContinue) ;
           


            // După apăsarea butonului Continue
            IsContinuePressed = false;
            ProcessContinue = true;
            ProcessStop = false;

           

            ChangeProcessState(ProcessState.Wait, 1000);
        }
        public void OnContinueButtonPressed()
        {
            IsContinuePressed = true;
        }
       

        public async Task ComputeNextState(ProcessState CurrentState)
        {

            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            switch (CurrentState)
            {

                case ProcessState.Wait:
                   ////// deci aici nush cum sa fac , ar trebui sa ramane in starea de asteptare pana la urmatoare comanda,
                   ///nu stiu cum sa foloses changeprocessstate uul
                   ///
                    break;
                case ProcessState.Stopped:

                    // aici nush cum sa gestionez situatia  ca e destul de nasta sa te bagi peste procesul in curs
                    //  ProcessStart = true;
                  // ChangeProcessState(ProcessState.Stopped, 1000);
                    await   stopped();
                    break;

                case ProcessState.Running:
                    //  ChangeProcessState(ProcessState.Running, 2000);
                    
                    await running();
                    break;

                case ProcessState.GroundFloor:
                   
                   groundFloor();
                  //  ButtonEnabled = true;await _continueTaskCompletionSource.Task;
                    break;

                case ProcessState.Floor1:
                      etaj=ProcessState.Floor1;
                    await floor1(cancellationToken);
                   // ButtonEnabled = true;
                    break;

                case ProcessState.Floor2:
                    etaj = ProcessState.Floor2;
                    await floor1(cancellationToken);
                    await floor2(cancellationToken);

                   // ButtonEnabled = true;
                    break;

                case ProcessState.Floor3:
                   etaj=ProcessState.Floor3;
                    await floor1(cancellationToken);
                    await floor2(cancellationToken);
                    await floor3(cancellationToken);

                  //  ButtonEnabled = true;

                    break;

                case ProcessState.Floor4:
                    etaj = ProcessState.Floor4;
                    await floor1(cancellationToken);
                    await floor2(cancellationToken);
                    await floor3(cancellationToken);
                    await floor4(cancellationToken);

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
