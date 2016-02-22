using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace dipl_ranec {
    class Helper : BackPack {
         DateTime timeStart;
         DateTime timeFinish;

        public void TimeStart() {
            timeStart = DateTime.Now;
        }
        public void TimeFinish() {
            timeFinish = DateTime.Now;
        }

        public string CalculateTime() {
            var result = (timeFinish.TimeOfDay - timeStart.TimeOfDay).ToString();
            MessageBox.Show(result);
            return result;
        }

        public void CreateHtmlFile(List<Item> inputItems, List<Item> outputItems) {
            string tempFileName = "list.html";
            using (TextWriter writer = new StreamWriter(tempFileName)) {
                try {
                    writer.WriteLine("<html>");
                    writer.WriteLine("<head><meta charset=\"utf-8\"><title>Задача о ранце</title></head>");
                    writer.WriteLine("<body>");

                    writer.WriteLine("<p>");

                    writer.WriteLine("<table border=\"1\">");
                    writer.WriteLine("<caption>Input Data Table</caption>");
                    writer.WriteLine("<tr><th>Cost</th><th>Mas</th><th>Use</th></tr>");
                    foreach (var item in inputItems) {
                        writer.WriteLine(
                            item.Use
                                ? "<tr> <td>{0}</td> <td>{1}</td> <td><font color=\"green\">{2}</font></td></tr>"
                                : "<tr> <td>{0}</td> <td>{1}</td> <td><font color=\"red\">{2}</font></td></tr>",
                            item.Cost, item.Mas, item.Use);
                    }
                    writer.WriteLine("</table>");
                    writer.WriteLine("</p>");

                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                }
                catch (Exception ex) {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                finally {
                    writer.Close();
                }
            }
            if (File.Exists(tempFileName)) {
                System.Diagnostics.Process.Start(tempFileName);
            }
        }
    }
}
