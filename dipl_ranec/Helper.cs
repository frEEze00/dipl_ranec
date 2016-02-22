using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dipl_ranec {
    class Helper {
         DateTime timeStart;
         DateTime timeFinish;

        public void TimeStart() {
            timeStart = DateTime.Now;
        }
        public void TimeFinish() {
            timeFinish = DateTime.Now;
        }

        public double CalculateTime() {
            double result;
            MessageBox.Show(timeStart.TimeOfDay.ToString() + "\n" + timeFinish.TimeOfDay.ToString() + 
                "\n" + (timeFinish.TimeOfDay - timeStart.TimeOfDay).ToString());

            return 0;
        }
    }
}
