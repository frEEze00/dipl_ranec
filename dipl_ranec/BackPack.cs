using System;
using System.Collections.Generic;
using System.Linq;


namespace dipl_ranec {
    internal class BackPack {
        public List<Item> Items = new List<Item>();
        public struct Item {
            public int Cost { get; set; }
            public int Mas { get; set; }
            public bool Use { get; set; }
        }
        public int Volume;
        public void GenerateData(int count, int maxMas, int maxCost) {
            Items.Clear();
            var rnd = new Random();
            for (var i = 0; i < count; i++) {
                var el = new Item
                {
                    Cost = rnd.Next(1, maxCost),
                    Mas = rnd.Next(1, maxMas),
                    Use = false
                };
                Items.Add(el);
            }
        }
        public void PrintData(List<Item> data) {
            if (data.Count == 0) {
                Console.WriteLine(@"Data is empty");
            }
            else {
                Console.WriteLine(@"Cost: 		Mas: 		Use:");
                foreach (var item in data)
                {
                    Console.ForegroundColor = item.Use ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine(item.Cost.ToString() + @"		"
                        + item.Mas.ToString() + @"		" + item.Use.ToString());
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public long GetMasOfBackPack(List<Item> data)
        {
            return data.Aggregate<Item, long>(0, (current, item) => current + item.Mas);
        }
        public long GetCostOfBackPack(List<Item> data)
        {
            return data.Aggregate<Item, long>(0, (current, item) => current + item.Cost);
        }
        public void GetInfoOfBackPack(List<Item> data) {
            Console.WriteLine(@"mas : " + GetMasOfBackPack(data).ToString());
            Console.WriteLine(@"cost: " + GetCostOfBackPack(data).ToString());
        }

        #region
        // Жадный Алгоритм
        public List<Item> ResultForGreedyChoice = new List<Item>();
        private struct StructForGreedyChoice {
            public int Index;
            public double Ratio;
        }    
        public void GreedyAlgorithm() {
            var tempResult = new List<StructForGreedyChoice>();
            var tempStruct = new StructForGreedyChoice();
            var index = 0;
            var tempSum = 0;
            foreach (var item in Items) {
                tempStruct.Index = index;
                tempStruct.Ratio = (double)item.Cost / item.Mas;
                index++;
                tempResult.Add(tempStruct);
            }

            tempResult.Sort((strct1, strct2) => strct2.Ratio.CompareTo(strct1.Ratio));

            foreach (var item in tempResult) {
                if (Items[item.Index].Mas + tempSum <= Volume) {
                    var str = Items[item.Index];
                    str.Use = true;
                    Items[item.Index] = str;
                    ResultForGreedyChoice.Add(Items[item.Index]);
                    tempSum += Items[item.Index].Mas;
                }
                else {
                    break;
                }
            }         
        }
        #endregion
        #region
        // Генетический алгоритм
        public List<Item> ResultForGeneticAlgorithm = new List<Item>();
        public struct Chromosome {
            public bool[] Chrom;
            public double Q;
        }
        public List<Chromosome> PopulationStart = new List<Chromosome>();
        public List<Chromosome> PopulationFinish = new List<Chromosome>();
        public void CalculateMax(int countOfChromosomes) {
            CreatePopulationMax(countOfChromosomes);
            PopulationFinish = Crossing(PopulationStart, 45);
            PrintChromosomes(PopulationStart);
            Console.WriteLine(@"-----------------");
            PrintChromosomes(PopulationFinish);
        }
        public void Calculate(int countOfChromosomes, int countOfIterations) {
            CreatePopulation(countOfChromosomes);
            PopulationFinish = Crossing(PopulationStart, countOfChromosomes * 2);
            for (var i = 1; i < countOfIterations; i++) {
                PopulationFinish = Crossing(PopulationFinish, 50);
            }
            GetResult();
        }
        public void CreatePopulationMax(int countOfChromosomes) {
            //генерация максимального количества используемых предметов
            PopulationStart.Clear();
            var countOfItems = Items.Count;
            var rnd = new Random();
            
            for (var i = 0; i < countOfChromosomes; i++) {
                var tempChrom = new Chromosome {Chrom = new bool[countOfItems]};
                var tempMas = 0;
                var useCount = 0;
                for (var k = 0; k < countOfItems; k++) {
                    tempChrom.Chrom[k] = false;
                }
                int tempIndex;
                do {
                    tempIndex = rnd.Next(0, countOfItems);
                    if (tempChrom.Chrom[tempIndex]) continue;
                    tempChrom.Chrom[tempIndex] = true;
                    tempMas += Items[tempIndex].Mas;
                    useCount++;
                } while (tempMas <= Volume && useCount < countOfItems);
                if (tempMas > Volume) {
                    tempChrom.Chrom[tempIndex] = false;
                }
               //tempChrom.Q = EvaluationOfChromosomeMas(tempChrom);
                tempChrom.Q = EvaluationOfChromosomeCost(tempChrom);
                PopulationStart.Add(tempChrom);
            }
        }
        public void CreatePopulation(int countOfChromosomes) {
            //генерация некоторого количества используемых предметов
            PopulationStart.Clear();
            var countOfItems = Items.Count;
            var rnd = new Random();

            for (var i = 0; i < countOfChromosomes; i++) {
                var tempChrom = new Chromosome {Chrom = new bool[countOfItems]};
                var tempMas = 0;
                for (var k = 0; k < countOfItems; k++) {
                    tempChrom.Chrom[k] = false;
                }
                for (var j = 0; j < countOfItems / 3; j++) {
                    if (!Convert.ToBoolean(rnd.Next(0, 2))) continue;
                    var tempIndex = rnd.Next(0, countOfItems);
                    if (tempChrom.Chrom[tempIndex] || tempMas + Items[tempIndex].Mas > Volume) continue;
                    tempChrom.Chrom[tempIndex] = true;
                    tempMas += Items[tempIndex].Mas;
                }
                //tempChrom.Q = EvaluationOfChromosomeMas(tempChrom);
                tempChrom.Q = EvaluationOfChromosomeCost(tempChrom);
                PopulationStart.Add(tempChrom);
                
            }
        }
        public int GetMasOfCromosome(Chromosome chr)
        {
            return Items.Where((t, i) => chr.Chrom[i]).Sum(t => t.Mas);
        }
        public int GetCostOfCromosome(Chromosome chr)
        {
            return Items.Where((t, i) => chr.Chrom[i]).Sum(t => t.Cost);
        }
        public void PrintChromosomes(List<Chromosome> popul) {
            //popul.Sort(delegate (Chromosome strct1, Chromosome strct2) 
            //{ return strct2.q.CompareTo(strct1.q); });

            foreach (var item in popul) {
                foreach (bool t in item.Chrom) {
                    if (t) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('1');
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('0');
                    }
                }
                Console.Write(@"	Mas = " + GetMasOfCromosome(item).ToString());
                Console.Write(@"	Cost = " + GetCostOfCromosome(item).ToString());
                Console.Write(@"	Q = " + item.Q.ToString("0.0000000"));
                
                Console.WriteLine();                
            }
        }
        public void PrintChromosomesRes(List<Chromosome> popul) {
            //popul.Sort(delegate (Chromosome strct1, Chromosome strct2) 
            //{ return strct2.q.CompareTo(strct1.q); });


                foreach (bool t in popul[0].Chrom) {
                    if (t) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('1');
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('0');
                    }
                }
            Console.Write(@"	Mas = " + GetMasOfCromosome(popul[0]).ToString());
                Console.Write(@"	Cost = " + GetCostOfCromosome(popul[0]).ToString());
                Console.Write(@"	Q = " + popul[0].Q.ToString("0.0000000"));

                Console.WriteLine();
        }
        public List<Chromosome> Crossing(List<Chromosome> popul, int maxCount) {
            var result = new List<Chromosome>();
            var rnd = new Random();
            var tempChr = new Chromosome();
            for (var i = 0; i < popul.Count; i++) {
                for (var j = 0; j < popul.Count; j++) {
                    if (i == j) continue;
                    var index = rnd.Next(0, Items.Count);
                    tempChr.Chrom = Swap(popul[i].Chrom, popul[j].Chrom, index);
                    //tempChr.Q = EvaluationOfChromosomeMas(tempChr);
                    tempChr.Q = EvaluationOfChromosomeCost(tempChr);
                        if (GetMasOfCromosome(tempChr) <= Volume) {
                            result.Add(tempChr);
                        }                  
                    tempChr.Chrom = Swap(popul[j].Chrom, popul[i].Chrom, index);
                    //tempChr.Q = EvaluationOfChromosomeMas(tempChr);
                    tempChr.Q = EvaluationOfChromosomeCost(tempChr);
                    if (GetMasOfCromosome(tempChr) <= Volume) {
                        result.Add(tempChr);
                    }
                }
            }
            result = Mutation(result);

            //result.Sort(delegate (Chromosome strct1, Chromosome strct2) { return GetCostOfCromosome(strct2).CompareTo(GetCostOfCromosome(strct1)); });
            //result = result.OrderBy(x => -(x.q)).ThenBy(x => -GetCostOfCromosome(x)).ThenBy(x => GetMasOfCromosome(x)).ToList();
            //result = result.OrderBy(x => -x.Q).ThenBy(x => -GetCostOfCromosome(x)).ToList();
            //result = result.OrderBy(x => -GetCostOfCromosome(x)).ThenBy(x => -(x.q)).ToList();
            result = result.OrderBy(x => -(x.Q)).ToList();          


            //for (var i = 0; i < result.Count; i++) {
            //    if (GetMasOfCromosome(result[i]) <= Volume) continue;
            //    result.RemoveAt(i);
            //    i--;
            //}


            for (var i = result.Count - 1; i > maxCount; i--) {
                result.RemoveAt(i);
            }
            return result;
        }
        public List<Chromosome> Mutation(List<Chromosome> popul) {
            var rnd = new Random();
            for (var i = 0; i < popul.Count; i++) {
                if (rnd.Next()%2 != 0) continue;
                var index = rnd.Next(0, Items.Count);
                var tempChr = popul[i];
                tempChr.Chrom[index] = !popul[i].Chrom[index];
                //tempChr.Q = EvaluationOfChromosomeMas(tempChr);
                tempChr.Q = EvaluationOfChromosomeCost(tempChr);
                popul[i] = tempChr;
            }
            for (var i = 0; i < popul.Count; i++) {
                if (GetMasOfCromosome(popul[i]) <= Volume) continue;
                popul.RemoveAt(i);
                i--;
            }
            return popul;
        }
        public bool[] Swap(bool[] mas1, bool[] mas2, int index) {
            var result = new bool[mas1.Length];
            for (var i = 0; i < index; i++) {
                result[i] = mas1[i];
            }
            for (var i = index; i < mas2.Length; i++) {
                result[i] = mas2[i];
            }
            return result;
        }
        public double EvaluationOfChromosomeMas(Chromosome chr) {
            double q;
            double delMax = Math.Max(Volume, GetMasOfBackPack(Items) - Volume);
            if (GetMasOfCromosome(chr) <= Volume) {
                q = 1 - Math.Sqrt(Math.Abs((double)GetMasOfCromosome(chr) - Volume) / Volume);               
            }
            else {
                q = 1 - Math.Sqrt(Math.Abs((double)GetMasOfCromosome(chr) - Volume) / delMax);
            }
           // q *= GetCostOfCromosome(chr);
            return q;
        }
        public double EvaluationOfChromosomeCost(Chromosome chr) {
            double q;
            /*
            double delMax = Math.Max(GetCostOfBackPack(Items), GetCostOfBackPack(Items) - GetCostOfCromosome(chr));
            if (GetCostOfCromosome(chr) <= GetCostOfBackPack(Items)) {
                q = 1 - (Math.Sqrt((double)Math.Abs(GetCostOfCromosome(chr) - GetCostOfBackPack(Items)) / GetCostOfBackPack(Items)));
            }
            else {
                q = 1 - (Math.Sqrt(Math.Abs(GetCostOfCromosome(chr) - GetCostOfBackPack(Items)) / delMax));
            }*/

            /*
            if (GetCostOfCromosome(chr) != 0) {
                return q = (double)GetCostOfCromosome(chr) / GetMasOfCromosome(chr);
            }
            else {
                return 0;
            }*/

            double delMax = Math.Max(Volume, GetMasOfBackPack(Items) - Volume);
            if (GetMasOfCromosome(chr) <= Volume) {
                q = 1 - Math.Sqrt(Math.Abs((double)GetMasOfCromosome(chr) - Volume) / Volume);
            }
            else {
                q = 1 - Math.Sqrt(Math.Abs((double)GetMasOfCromosome(chr) - Volume) / delMax);
            }

            return q*((double)GetCostOfCromosome(chr) / GetMasOfCromosome(chr));
        }
        private void GetResult() {
            for (var i = 0; i < PopulationFinish[0].Chrom.Length; i++) {
                if (PopulationFinish[0].Chrom[i]) {
                    ResultForGeneticAlgorithm.Add(Items[i]);
                }
            }
        }
        #endregion
        #region
        //Случайный поиск
        public List<Item> ResultForRandomSearch = new List<Item>();
        public void RandomSearch(int countOfTests) {
            int countOfItems = Items.Count;
            bool[] mask = new bool[countOfItems];
            bool[] result = new bool[countOfItems];           
            long maxMas = 0;
            
            Random rnd = new Random();
            long temp;
            for (int i = 0; i < countOfTests; i++) {
                for (int j = 0; j < countOfItems; j++) {
                    if (rnd.Next() % 2 == 0) {
                        mask[j] = !mask[j];
                    }
                }
                temp = Items.Where((t, k) => mask[k]).Sum(t => t.Mas);
                if (temp > maxMas) {
                    maxMas = temp;
                    result = mask;
                }
            }
            for (int i = 0; i < countOfItems; i++) {
                if (result[i]) {
                    ResultForRandomSearch.Add(Items[i]);
                }
            }
        }
        #endregion
        #region
        //Точный метод 
        public List<Item> To4nResult = new List<Item>();
        private bool[] uses;
        public int Met(int needed, List<Item> data) {
            int n = data.Count;
            uses = new bool[n];
            
            int[,] dp = new int[needed + 1, n + 1];
            for (int j = 1; j <= n; j++) {
                for (int w = 1; w <= needed; w++) {
                    if (data[j - 1].Mas <= w) {
                        dp[w, j] = Math.Max(dp[w, j - 1], dp[w - data[j - 1].Mas, j - 1] + data[j - 1].Cost);
                    }
                    else {
                        dp[w, j] = dp[w, j - 1];
                    }
                }
            }
            return dp[needed, n];
        }

        public int CalculateMet2(List<Item> data) {
            uses = new bool[data.Count];
            Met2(data, 0, 0);
            return 0;
        }

        public int Met2(List<Item> data, int k, int max) {
            if (k < data.Count) {
                
            }
            return 0;
        }
        #endregion
    }
}
 
