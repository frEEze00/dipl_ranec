﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using Mathcad;
using Application = Mathcad.Application;

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
            int id = 0;
            if (data.Count == 0) {
                writer.WriteLine("<H4> Table \"" + method +"\" is empty </H4>");
            }
            else {
                writer.WriteLine("<table border=\"1\">");
                writer.WriteLine("<caption>" + method + "</caption>");
                writer.WriteLine("<tr><th>ID</th><th>Cost</th><th>Mas</th></tr>");                
                foreach (var item in data) {
                    writer.WriteLine("<tr> <td>{0}</td> <td>{1}</td> <td>{2}</td></tr>", id++, item.Cost, item.Mas);
                }
                writer.WriteLine("</table>");
            }
            
        }
        private void CreateTableForInputData(TextWriter writer, List<Item> data, 
            List<Item> greedyResult, List<Item> geneticResult, List<Item> randomResult) {
            int id = 0;
            writer.WriteLine("<table border=\"1\">");
            writer.WriteLine("<caption>Input Data</caption>");
            writer.WriteLine("<tr><th>ID</th><th>Cost</th><th>Mas</th><th>Greedy</th><th>Random</th><th>Greedy</th></tr>");
            foreach (var item in data) {
                writer.Write("<tr> <td>{0}</td> <td>{1}</td>  <td>{2}</td>",id++, item.Cost, item.Mas, item.Use);
                writer.Write(greedyResult.Contains(item)
                    ? "<td><font color=\"green\">Use</font></td>"
                    : "<td><font color=\"red\">Not Use</font></td>");
                writer.WriteLine(randomResult.Contains(item)
                    ? "<td><font color=\"green\">Use</font></td>"
                    : "<td><font color=\"red\">Not Use</font></td>");
                writer.WriteLine(geneticResult.Contains(item)
                    ? "<td><font color=\"green\">Use</font></td></tr>"
                    : "<td><font color=\"red\">Not Use</font></td></tr>");
            }

            writer.WriteLine("</table>");
        }
        public void CreateHtmlFile(List<Item> inputItems, List<Item> greedyResult, 
            List<Item> geneticResult, List<Item> to4nResult, int to4nCost, List<Item> randomResult) {

            string tempFileName = "html\\" + DateTime.Now.ToString("d").Replace('.', '-') + " " + DateTime.Now.ToString("T").Replace(':', '-') + ".html";
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
                    writer.WriteLine("<H3> randomResult Cost = " + GetCostOfBackPack(randomResult) + "</H3>");
                    writer.WriteLine("<H3> randomResult Mas = " + GetMasOfBackPack(randomResult) + "</H3>");
                    writer.WriteLine("<H3> geneticResult Cost = " + GetCostOfBackPack(geneticResult) + "</H3>");
                    writer.WriteLine("<H3> geneticResult Mas = " + GetMasOfBackPack(geneticResult) + "</H3>");                    

                    writer.WriteLine("<H3> Точный метод Cost = " + to4nCost + "</H3>");

                    writer.WriteLine("<table border=\"0\">");
                    writer.WriteLine("<caption>Results</caption>");
                    writer.WriteLine("<tr> <td></td> <td></td> <td></td> <td></td> </tr>");

                    writer.Write("<tr> <td valign=\"top\">");
                    CreateTableForInputData(writer, inputItems, greedyResult, geneticResult, randomResult);
                    writer.Write("</td> <td valign=\"top\">");
                    CreateTableForResult(writer, greedyResult, @"GreedyResult");
                    writer.Write("</td> <td valign=\"top\">");
                    CreateTableForResult(writer, randomResult, @"RandomResult");
                    writer.Write("</td> <td valign=\"top\">");
                    CreateTableForResult(writer, geneticResult, @"GeneticResult");
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
        public void CreateTxtFile(List<Item> data, int volume, int maxCost, int maxMas) {
            string path = "data\\" + DateTime.Now.ToString("d").Replace('.', '-') + " " + DateTime.Now.ToString("T").Replace(':', '-') + ".txt";           
            StreamWriter file = new StreamWriter(path);
            foreach (var item in data) {
                file.WriteLine(item.Cost.ToString() + " " + item.Mas.ToString());
            }
            file.Close();
            GetDataFromTxt(path);
        }
        public List<Item> GetDataFromTxt(string path) {
            List<Item> data = new List<Item>();
            StreamReader file = new StreamReader(path);
            string str;
            string[] values = new string[2];
            Item tempItem = new Item();
            while ((str = file.ReadLine()) != null) {
                values = str.Split(' ');
                tempItem.Cost = Convert.ToInt32(values[0]);
                tempItem.Mas = Convert.ToInt32(values[1]);
                data.Add(tempItem);
            }
            return data;
        }

        public void CreateExcell() {
            
        }

        public void MathCad() {
            IMathcadApplication mc = new Application();
            IMathcadWorksheets mwk = mc.Worksheets;
            IMathcadWorksheet ws = mwk.Open("h:\\test2.xmcd");
            ws.SetValue("A", 2);
            Console.WriteLine("A = 2");

            ws.SetValue("B", 2);
            Console.WriteLine("B = 2");

            ws.Recalculate();

            Console.WriteLine("C = A + B = {0}", (ws.GetValue("C") as INumericValue).Real);

            mc.Visible = true;

            mc.ActiveWorksheet.Close(Mathcad.MCSaveOption.mcDiscardChanges);
            mc.Quit(Mathcad.MCSaveOption.mcDiscardChanges);
        }
    }
}
