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
        public string TimeFinish() {
            timeFinish = DateTime.Now;
            var result = (timeFinish.TimeOfDay - timeStart.TimeOfDay).ToString();
            return result;
        }

        private void CreateTableForResult(TextWriter writer, List<Item> data, string method) {
            if (data.Count == 0) {
                writer.WriteLine("<H1> Table \"" + method +"\" is empty </H1>");
            }
            else {
                writer.WriteLine("<table border=\"1\">");
                writer.WriteLine("<caption>" + method + "</caption>");
                writer.WriteLine("<tr><th>Cost</th><th>Mas</th></tr>");                
                foreach (var item in data) {
                    writer.WriteLine("<tr> <td>{0}</td> <td>{1}</td></tr>", item.Cost, item.Mas);
                }
                writer.WriteLine("</table>");
            }
            
        }
        private void CreateTableForInputData(TextWriter writer, List<Item> data, 
            List<Item> greedyResult, List<Item> geneticResult) {
            writer.WriteLine("<table border=\"1\">");
            writer.WriteLine("<caption>Input Data</caption>");
            writer.WriteLine("<tr><th>Cost</th><th>Mas</th><th>Use</th></tr>");
            foreach (var item in data) {
                writer.WriteLine(
                    item.Use
                        ? "<tr> <td>{0}</td> <td>{1}</td> <td><font color=\"green\">{2}</font></td></tr>"
                        : "<tr> <td>{0}</td> <td>{1}</td> <td><font color=\"red\">{2}</font></td></tr>",
                    item.Cost, item.Mas, item.Use);
            }
            writer.WriteLine("</table>");
        }
        public void CreateHtmlFile(List<Item> inputItems, List<Item> greedyResult, List<Item> geneticResult) {
            string tempFileName = "list.html";
            using (TextWriter writer = new StreamWriter(tempFileName)) {
                try {
                    writer.WriteLine("<html>");
                    writer.WriteLine("<head><meta charset=\"utf-8\"><title>Задача о ранце</title></head>");
                    writer.WriteLine("<body>");

                    writer.WriteLine("<p>");
                    writer.WriteLine("<H3> Input Cost = " + GetCostOfBackPack(inputItems) + "</H3>");
                    writer.WriteLine("<H3> Input Mas = " + GetMasOfBackPack(inputItems) + "</H3>");
                    writer.WriteLine("<H3> greedyResult Cost = " + GetCostOfBackPack(greedyResult) + "</H3>");
                    writer.WriteLine("<H3> greedyResult Mas = " + GetMasOfBackPack(greedyResult) + "</H3>");
                    writer.WriteLine("<H3> geneticResult Cost = " + GetCostOfBackPack(geneticResult) + "</H3>");
                    writer.WriteLine("<H3> geneticResult Mas = " + GetMasOfBackPack(geneticResult) + "</H3>");

                    writer.WriteLine("<H3> Точный метод Cost = " + Met(3000, inputItems) + "</H3>");

                    writer.WriteLine("<table border=\"0\">");
                    writer.WriteLine("<caption>Results</caption>");
                    writer.WriteLine("<tr> <td></td> <td></td> <td></td></tr>");

                    writer.Write("<tr> <td>");
                    CreateTableForInputData(writer, inputItems, greedyResult, geneticResult);
                    writer.Write("</td> <td>");
                    CreateTableForResult(writer, greedyResult, @"Greedy Result");
                    writer.Write("</td> <td>");
                    CreateTableForResult(writer, geneticResult, @"Genetic Result");
                    writer.Write("</td></tr>");

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
