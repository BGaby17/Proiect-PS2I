using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DateModel;

namespace Simulator
{
    /// <summary>
    /// Interaction logic for Lift.xaml
    /// </summary>
    public partial class Lift : UserControl
    {

        LiftViewModel _lift;
        public Lift()
        {
            InitializeComponent();
            _lift= new LiftViewModel();
            this.DataContext = _lift;
        }

        private void Button_Start(object sender, RoutedEventArgs e)
        {
            _lift.Init();
        }

        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            _lift.ForceNextState(ProcessState.Stop);
        }

        private void Button_Continu(object sender, RoutedEventArgs e)
        {
            _lift.ForceNextState(ProcessState.Pornit);
        }

        private void Button_Etajul1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Etajul2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Etajul3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Etajul4(object sender, RoutedEventArgs e)
        {

        }
    }
}
