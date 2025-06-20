using System;
using Communicator;
using DateModel;


namespace ConsoleConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Aplicatia consumator a pornit si asteapta date ...");

            ProcessState? lastState = null;

            while (true)
            {
                var data = HttpHelper.GetDataFromWebAPI();
                Console.Clear();

                if (data != null && data.Count > 0)
                {
                    var last = data.Last();

                    switch (last.State)
                    {
                        case ProcessState.GoingUp:
                           Console.WriteLine("Liftul urca.");
                              break;
                        case ProcessState.GoingDown:
                                Console.WriteLine("Liftul coboara.");
                                break;
                        case ProcessState.GroundFloor:
                                Console.WriteLine("Liftul este la parter.");
                                break;
                            case ProcessState.Stopped:
                                Console.WriteLine("Liftul este oprit.");
                                break;
                            case ProcessState.Running:
                                Console.WriteLine("Liftul este în miscare.");
                                break;
                            case ProcessState.Wait:
                                Console.WriteLine("Liftul este în asteptare.");
                                break;
                            default:
                                Console.WriteLine($"Stare necunoscuta: {last.State}");
                                break;
                    }

                        Console.WriteLine($"Data modificarii: {last.StateChangedDate:dd.MM.yyyy HH:mm:ss}");
                        lastState = last.State;
                    
                }
                else
                {
                    Console.WriteLine("Nu s-au primit date sau lista e goală.");
                }

                Thread.Sleep(2500);
            }


        }
    }
}
