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

namespace dipl_ranec {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var bp = new BackPack();
            bp.GenerateData(25, 200, 1000);
            bp.Volume = 1500;
            bp.GreedyAlgorithm();
            bp.PrintData(bp.Items);
            bp.GetInfoOfBackPack(bp.Items);
            bp.PrintData(bp.ResultForGreedyChoice);
            bp.GetInfoOfBackPack(bp.ResultForGreedyChoice);
            MessageBox.Show("sdsa");
            bp.Calculate(30, 100);
            //Random rnd = new Random();
            //int temp;
            //for (int i = 0; i < 10; i++) {
            //    temp = rnd.Next(0, 2);
            //    Console.WriteLine(temp.ToString() + "\t" 
            //        + Convert.ToBoolean(temp).ToString());
            //}


            Console.ReadLine();
        }
    }
}
