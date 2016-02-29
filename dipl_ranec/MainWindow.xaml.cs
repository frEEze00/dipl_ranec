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
            var help = new Helper();
            string Time = "";

            int countOfItems = 20;
            int maxMas = 1000;
            int maxCost = 100;
            int volume = (maxMas / 3) * countOfItems;

            help.TimeStart();                  
            bp.GenerateData(countOfItems, maxMas, maxCost);
            Time += help.TimeFinish() + " Generate\n";
            bp.Volume = volume;
            help.CreateTxtFile(bp.Items, volume, maxCost, maxMas);

            help.TimeStart();
            bp.GreedyAlgorithm();
            Time += help.TimeFinish() + " Greedy\n";

            help.TimeStart();
            bp.RandomSearch(100);
            Time += help.TimeFinish() + " Random\n";

            help.TimeStart();
            //bp.Calculate(20, 5);
            Time += help.TimeFinish() + " Genetic\n";
            int temp = 0;
            help.TimeStart();
            //temp = bp.Met(bp.Volume, bp.Items);
            Time += help.TimeFinish() + " To4n\n";
            //Time += temp + "\n";

            //MessageBox.Show(Time);
            //help.CreateHtmlFile(bp.Items, bp.ResultForGreedyChoice, bp.ResultForGeneticAlgorithm, bp.To4nResult, temp, bp.ResultForRandomSearch);
            this.Close();
            //help.MathCad();
            help.CreateExcell();
            Console.ReadLine();

            
        }
    }
}
