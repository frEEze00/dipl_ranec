using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace dipl_ranec {
    class TravellingSalesmanProblem {
        public int[,] Data { get; set; }
        public int CountOfVertex { get; set; }
        public bool[] DesiredCity { get; set; }
        public void GenerateData(int chanceExistenceOfPath) {
            Data = new int[CountOfVertex, CountOfVertex];
            Random rnd = new Random();
            Random chance = new Random();
            for (int i = 0; i < CountOfVertex; i++) {
                for (int j = 0; j < CountOfVertex; j++) {
                    if (i == j) {
                        Data[i, j] = 0;
                    }
                    else if (i < j) {
                        if (chance.Next(1,10) <= chanceExistenceOfPath) {
                            Data[i, j] = rnd.Next(1, 6);
                        }                       
                    }
                    else {
                        Data[i, j] = Data[j, i];
                    }
                }                
            }
        }
        public void GenerateDesiredCity() {
            DesiredCity = new bool[CountOfVertex];
            Random rnd = new Random();
            for (int i = 0; i < CountOfVertex; i++) {
                if (rnd.Next(1,3) == 1) {
                    DesiredCity[i] = true;
                }
            }
        }
        public void PrintData() {
            for (int i = 0; i < CountOfVertex; i++) {
                for (int j = 0; j < CountOfVertex; j++) {
                    Console.Write("{0}\t", Data[i, j]);
                    //Console.Write("{0}x{1}\t", i, j);
                }
                Console.WriteLine();
            }
        }
        public int[,] GreedyResult;
        public void GreedyAlg() {
            GreedyResult = new int[CountOfVertex, CountOfVertex];

        }
    }
}
