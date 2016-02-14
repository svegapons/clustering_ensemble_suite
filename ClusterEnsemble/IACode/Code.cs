using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble;
using System.Windows;
using System.ComponentModel;
using ClusterEnsemble.Clusters;
using System.Windows.Controls;
using System.Windows.Media.Animation;



namespace ClusterEnsemble
{

    public partial class Attribute
    {
        public string GetValues
        {
            get
            {
                StringBuilder result = new StringBuilder();

                switch (AttributeType)
                {
                    case AttributeType.Nominal:
                        result.Append("{");
                        for (int i = 0; i < Values.Count; i++)
                        {
                            if (i > 0)
                                result.Append(", ");
                            result.Append(Values[i].ToString());
                        }
                        result.Append("}");
                        break;
                    case AttributeType.Numeric:
                        result.Append(Utils.Numeric);
                        break;
                    case AttributeType.String:
                        result.Append(Utils.String);
                        break;
                    case AttributeType.Date:
                        result.Append(Utils.Date);
                        break;
                    default:
                        throw new Exception("Tipo Incorrecto de AttributeType en el metodo: ARFF_ToString de la clase Attribute");
                }

                return result.ToString();

            }
        }
    }

    public class EuclideanDistanceWithNomial : IDissimilarity
    {
        public double[,] matriz{get; private set;}
        bool CalculateMatrix;
        public EuclideanDistanceWithNomial(Set set, List<string> AttributesToCalculateDissimilarity,bool CalculateMatrix) 
        {

            this.AttributesToCalculateDissimilarity = AttributesToCalculateDissimilarity;
            this.CalculateMatrix = CalculateMatrix;

            if (CalculateMatrix)
            {
                matriz = new double[set.Elements.Count, set.Elements.Count];
                for (int i = 0; i < set.Elements.Count; i++)
                {
                    matriz[i, i] = 0;
                    for (int j = i + 1; j < set.Elements.Count; j++)
                    {
                        matriz[i, j] = CalculateDissimilarityPrivate(set.Elements[i], set.Elements[j]);
                        matriz[j, i] = matriz[i, j];
                    }
                }
            }
        }
        public double CalculateDissimilarity(Element ei, Element ej)
        {
            if (CalculateMatrix)
                return matriz[ei.Index, ej.Index];
            else
                return CalculateDissimilarityPrivate(ei, ej);
        }
        private double CalculateDissimilarityPrivate(Element ei, Element ej)
        {
            double result = 0;
            for (int i = 0; i < ei.Attributes.ValuesCount; i++)
            {
                if (AttributesToCalculateDissimilarity.Contains(ei.Attributes[i].Name))
                {
                    if (ei.Attributes[i].AttributeType == AttributeType.Numeric)
                    {
                        result += Math.Abs(((double)ei[i] - (double)ej[i])); //Math.Pow(((double)ei[i] - (double)ej[i]), 2);
                    }
                    else
                    {
                        result += (ei[i] == ej[i]) ? 0 : 1;
                    }
                }
            }

            return result; //Math.Sqrt(result);
        }

        public Property Properties { get; set; }
        public List<string> AttributesToCalculateDissimilarity { get; set; }

        public override string ToString()
        {
            return "Euclidean Distance";
        }

    }

    public class Tupla<T>
    {
        public T I { get; set; }
        public T J { get; set; }
        public Tupla(T i, T j)
        {
            I = i;
            J = j;
        }
        public Tupla<T> Clone()
        {
            return new Tupla<T>(I, J);
        }

        public int SetDifference_I_minus_J()
        {
            Cluster ci = I as Cluster;
            Cluster cj = J as Cluster;
            int result = 0;

            if (cj != null && ci != null)
            {
                foreach (Element e in cj.Elements)
                    if (!ci.Elements.Contains(e))
                        result++;
            }
            return result;
        }
        public int SetEqualsElement_I_J()
        {
            Cluster ci = I as Cluster;
            Cluster cj = J as Cluster;
            int result = 0;
            if (cj != null && ci != null)
            {
                foreach (Element e in cj.Elements)
                    if (ci.Elements.Contains(e))
                        result++;
            }
            return result;
        }

    }

    public partial class Element
    {
        public override string ToString()
        {
            return Values[0].ToString();
        }

        public int CurrentDiff { get; set; }

        public int CalculateDiff(Element centroid)
        {
            int result = 0;
            for (int i = 1; i < Attributes.ValuesCount - 1; i++)
            {
                if (Attributes[i].AttributeType == AttributeType.Numeric)
                    result += ((double)this[i] == (double)centroid[i]) ? 0 : 1;
                else
                    result += (this[i] == centroid[i]) ? 0 : 1;
            }

            return result;
        }
    }

    public partial class Cluster
    {
        public double ClusterDistanceAverage(Cluster oper, IDissimilarity diss)
        {
            double sum = 0;
            int cant = 0;
            foreach (Element element in oper.Elements)
                foreach (Element operElement in this.Elements)
                {
                    sum += diss.CalculateDissimilarity(element, operElement);
                    cant++;
                }
            return (double)sum /(double) cant;
        }
        public double ClusterDistanceSingle(Cluster oper, IDissimilarity diss)
        {
            double min = double.MaxValue;
            for (int i = 0; i < Elements.Count; i++)
            {
                for (int j = 0; j < oper.Elements.Count; j++)
                {
                    double dissValue = diss.CalculateDissimilarity(Elements[i], oper.Elements[j]);
                    if (dissValue < min)
                    {
                        min = dissValue;
                    }
                }
            }
            return min;
        }
        public double ClusterDistanceComplete(Cluster oper, IDissimilarity diss)
        {
            double max = double.MinValue;
            for (int i = 0; i < Elements.Count; i++)
            {
                for (int j = 0; j < oper.Elements.Count; j++)
                {
                    double dissValue = diss.CalculateDissimilarity(Elements[i], oper.Elements[j]);
                    if (dissValue > max)
                    {
                        max = dissValue;
                    }
                }
            }
            return max;
        }

        public Cluster Join(Cluster cluster)
        {
            List<Element> ClusterElements = new List<Element>();
            foreach (Element e in Elements)
                if (!ClusterElements.Contains(e))
                    ClusterElements.Add(e);
            foreach (Element e in cluster.Elements)
                if (!ClusterElements.Contains(e))
                    ClusterElements.Add(e);
            return new Cluster(Name + "-" + cluster.Name, ClusterElements);
        }
    }

    public class RealPartition : Partition
    {

        public Attribute ObjetiveAttr { get; set; }

        public RealPartition(Set set, Attribute ObjetiveAttr)
        {
            if (!(ObjetiveAttr.AttributeType == AttributeType.Nominal))
                throw new ArgumentException("El atributo objetivo tiene que ser de tipo NOMINAL obligatoriamente");

            Dictionary<string, Cluster> clusters = new Dictionary<string, Cluster>();

            for (int i = 0; i < ObjetiveAttr.ValuesCount; i++)
            {
                clusters.Add(ObjetiveAttr[i].ToString(), new Cluster(ObjetiveAttr[i].ToString()));
            }

            foreach (Element element in set.Elements)
            {
                clusters[element[ObjetiveAttr].ToString()].AddElement(element);
            }

            foreach (var c in clusters.Values)
                c.UpdateCentroid();

            Clusters = clusters;
            this.ObjetiveAttr = ObjetiveAttr;

        }

        public double MeasurePurity(Partition partition)
        {

            return PurityInMating(PerfectMatingWithPurityAssignment(partition));

            //Dictionary<string, int> classs = new Dictionary<string, int>();
            //Dictionary<string, int> clusters = new Dictionary<string, int>();

            //int count = 0;
            ////Mapeando el nombre de la clase a un entero
            //foreach (var s in Clusters.Keys)
            //    classs.Add(s, count++);

            ////Mapeando el nombre del cluster a un entero
            //count = 0;
            //foreach (var c in partition.Clusters.Values)
            //    clusters.Add(c.Name, count++);

            //int[,] ClassCluster = new int[classs.Count, clusters.Count];

            //foreach (var c in Clusters.Values)
            //    foreach (var e in c.Elements)
            //        ClassCluster[classs[c.Name], clusters[partition.GetCluster(e)[0].Name]]++;

            //int numerador = 0;
            //for (int j = 0; j < ClassCluster.GetLength(1); j++)
            //{
            //    int max = int.MinValue;
            //    for (int i = 0; i < ClassCluster.GetLength(0); i++)
            //        if (ClassCluster[i, j] > max)
            //            max = ClassCluster[i, j];

            //    numerador += max;
            //}

            //return ((double)numerador) / Elements.Keys.Count;

        }

        public double MeasureRI(Partition partition)
        {
            int TP = 0, TN = 0, FP = 0, FN = 0;

            List<Element> elements = Elements.Keys.ToList();

            for (int i = 0; i < elements.Count; i++)
                for (int j = i + 1; j < elements.Count; j++)
                {
                    bool sameClass = this.BeSameCluster(elements[i], elements[j]);
                    bool sameCluster = partition.BeSameCluster(elements[i], elements[j]);

                    if (sameClass && sameCluster)
                        TP++;
                    if (!sameClass && !sameCluster)
                        TN++;
                    if (!sameClass && sameCluster)
                        FP++;
                    if (sameClass && !sameCluster)
                        FN++;
                }
            return ((double)(TP + TN)) / (TP + FP + TN + FN);

        }

        public double MeasureF(Partition partition, double B)
        {
            B = 5;
            //Penaliza mas los FN que los FP
            int TP = 0, TN = 0, FP = 0, FN = 0;

            List<Element> elements = Elements.Keys.ToList();
            for (int i = 0; i < elements.Count; i++)
                for (int j = i + 1; j < elements.Count; j++)
                {
                    bool sameClass = this.BeSameCluster(elements[i], elements[j]);
                    bool sameCluster = partition.BeSameCluster(elements[i], elements[j]);

                    if (sameClass && sameCluster)
                        TP++;
                    if (!sameClass && !sameCluster)
                        TN++;
                    if (!sameClass && sameCluster)
                        FP++;
                    if (sameClass && !sameCluster)
                        FN++;
                }
            double P = ((double)TP) / (TP + FP);
            double R = ((double)TP) / (TP + FN);
            double FB = ((B * B + 1) * P * R) / (B * B * P + R);
            return FB;
        }

        public List<Tupla<Cluster>> PerfectMatingWithPurityAssignment(Partition partition)
        {
            List<Tupla<Cluster>> result = new List<Tupla<Cluster>>();

            Dictionary<string, int> classs = new Dictionary<string, int>();
            Dictionary<string, int> clusters = new Dictionary<string, int>();

            int count = 0;
            //Mapeando el nombre de la clase a un entero
            foreach (var s in Clusters.Keys)
                classs.Add(s, count++);

            //Mapeando el nombre del cluster a un entero
            count = 0;
            foreach (var c in partition.Clusters.Values)
                clusters.Add(c.Name, count++);

            int[,] ClassCluster = new int[classs.Count, clusters.Count];

            foreach (var c in Clusters.Values)
                foreach (var e in c.Elements)
                    ClassCluster[classs[c.Name], clusters[partition.GetCluster(e)[0].Name]]++;

            for (int j = 0; j < ClassCluster.GetLength(1); j++)
            {
                int max = int.MinValue;
                int index = -1;
                for (int i = 0; i < ClassCluster.GetLength(0); i++)
                    if (ClassCluster[i, j] > max)
                    {
                        max = ClassCluster[i, j];
                        index = i;
                    }

                Cluster class_i = Clusters.Values.ToList()[index];
                Cluster cluster_j = partition.Clusters.Values.ToList()[j];

                result.Add(new Tupla<Cluster>(class_i, cluster_j));
            }

            return result;
        }

        public List<Tupla<Cluster>> PerfectMating(Partition partition, IDissimilarity diss)
        {
            double[,] distances = new double[Clusters.Count, partition.Clusters.Count];
            for (int i = 0; i < Clusters.Count; i++)
            {
                for (int j = 0; j < partition.Clusters.Count; j++)
                {
                    distances[i, j] = Clusters.Values.ToList<Cluster>()[i].ClusterDistanceAverage(partition.Clusters.Values.ToList<Cluster>()[j], diss);
                }
            }
            List<Tupla<int>> bestsolution = new List<Tupla<int>>();
            //      SearchAlgoritmNonInformedBPP(0, Clusters.Count, new bool[partition.Clusters.Count], new List<Tupla<Cluster>>(), bestsolution,this,partition);
            SearchAlgoritmInformedSimulatedAnnealing(100, 0.5, 10, 5, ref bestsolution, distances);

            List<Tupla<Cluster>> result = new List<Tupla<Cluster>>();
            foreach (var tupla in bestsolution)
            {
                result.Add(new Tupla<Cluster>(Clusters.Values.ToList<Cluster>()[tupla.I], partition.Clusters.Values.ToList<Cluster>()[tupla.J]));
            }

            return result;
        }

        public double PurityInMating(List<Tupla<Cluster>> mating)
        {
            int total = 0;
            int elementcount = 0;
            foreach (var tupla in mating)
            {
                total += tupla.SetEqualsElement_I_J();
                elementcount += tupla.J.Elements.Count;
            }
            return ((double)total) / elementcount;
        }

        #region  Algoritmos de busqueda
        void SearchAlgoritmNonInformedBPP(int pos, int limit, bool[] isMarked, List<Tupla<Cluster>> solution, List<Tupla<Cluster>> bestsolution, Partition p_i, Partition p_j)
        {
            if (pos == limit)
            {
                if (bestsolution.Count == 0)
                {
                    solution.ForEach(t => bestsolution.Add(new Tupla<Cluster>(t.I, t.J)));

                }
                else
                {
                    if (PurityInMating(solution) > PurityInMating(bestsolution))
                    {
                        bestsolution.Clear();
                        solution.ForEach(t => bestsolution.Add(new Tupla<Cluster>(t.I, t.J)));
                    }
                    #region
                    //===========================================================================

                    //double sol = 0;
                    //double bestsol = 0;
                    //foreach (Tupla<int> t in solution)
                    //{
                    //    sol += distances[t.I, t.J];
                    //}
                    //foreach (Tupla<int> t in bestsolution)
                    //{
                    //    bestsol += distances[t.I, t.J];
                    //}
                    //if (sol < bestsol)
                    //{
                    //    bestsolution.Clear();
                    //    solution.ForEach(t => bestsolution.Add(new Tupla<int>(t.I, t.J)));
                    //}
                    #endregion
                }
            }
            else
            {

                for (int i = 0; i < isMarked.Length; i++)
                {
                    if (!isMarked[i])
                    {
                        isMarked[i] = true;
                        solution.Add(new Tupla<Cluster>(p_i.Clusters.Values.ToList<Cluster>()[pos], p_j.Clusters.Values.ToList<Cluster>()[i]));
                        SearchAlgoritmNonInformedBPP(pos + 1, limit, isMarked, solution, bestsolution, p_i, p_j);
                        isMarked[i] = false;
                        solution.RemoveAt(solution.Count - 1);
                    }
                }
            }

        }
        void SearchAlgoritmInformedSimulatedAnnealing(double initialTemp, double epsilon, double K, double A, ref List<Tupla<int>> bestsolution, double[,] distances)
        {
            Random rnd = new Random();

            bestsolution = AleatorySolution(distances.GetLength(0), distances.GetLength(1));
            double bestEval = EvaluateSol(distances, bestsolution);
            double currentEval = bestEval;
            List<Tupla<int>> currentSol = bestsolution;


            int iteration = 0;
            double temperature = initialTemp;
            double alpha = 0.999;
            double ro = 1.05;
            double delta;
            double prob;

            int ite = 0;

            int a;
            int k;

            while (temperature > epsilon && A / K > epsilon)
            {
                iteration++;
                for (k = 0, a = 0; k < K && a < A; k++)
                {
                    List<Tupla<int>> nextSol = NearNeighbor(currentSol);
                    double nextEval = EvaluateSol(distances, nextSol);
                    delta = currentEval - nextEval;
                    if (delta > 0)//Es mejor la solucion nueva
                    {
                        currentSol = nextSol;
                        currentEval = nextEval;
                        a++;
                        if (currentEval > bestEval)
                        {
                            bestsolution = currentSol;
                            bestEval = currentEval;
                            ite = iteration;
                        }
                    }
                    else
                    {
                        prob = rnd.NextDouble();
                        if (prob < Math.Exp(delta / temperature))
                        {
                            currentSol = nextSol;
                            currentEval = nextEval;
                            a++;
                        }

                    }
                }
                temperature = alpha * temperature;
                K = ro * K;
            }
        }
        #endregion

        #region Metodos privados de los algoritmos de busqueda
        List<Tupla<int>> AleatorySolution(int cantI, int cantJ)
        {
            List<int> elem = new List<int>();
            List<Tupla<int>> solution = new List<Tupla<int>>();
            for (int i = 0; i < cantJ; i++)
            {
                elem.Add(i);
            }
            for (int i = 0; i < cantI; i++)
            {
                Random r = new Random(Environment.TickCount);
                solution.Add(new Tupla<int>(i, elem[r.Next(elem.Count)]));
            }
            return solution;
        }
        List<Tupla<int>> NearNeighbor(List<Tupla<int>> elem)
        {
            Random r = new Random(Environment.TickCount);

            List<Tupla<int>> result = new List<Tupla<int>>();
            foreach (var t in elem)
                result.Add(t.Clone());

            int pos1 = r.Next(result.Count);
            int pos2 = r.Next(result.Count);
            int temp = result[pos1].I;
            result[pos1].I = result[pos2].I;
            result[pos2].I = temp;

            return result;
        }
        double EvaluateSol(double[,] distances, List<Tupla<int>> elem)
        {
            double result = 0;
            foreach (Tupla<int> t in elem)
                result += distances[t.I, t.J];
            return result;
        }
        #endregion
    }

    public partial class CoAssociationMatrixDiss
    {
        public override string ToString()
        {
            return "Coasociation matrix";
        }
    }

    public enum CalculateClusterDistance
    {
        SingleLink, CompleteLink, AverageLink
    }

    public class AgglomerativeLinks : Agglomerative
    {
        public double CurrentValue { get; set; }
        public CalculateClusterDistance CalculateClusterDistance { get; private set; }

        public AgglomerativeLinks(Set set, IDissimilarity diss, CalculateClusterDistance CalculateClusterDistance)
            : base(set, diss)
        {
            this.CalculateClusterDistance = CalculateClusterDistance;
        }
        public override Structuring BuildStructuring()
        {


            List<Cluster> clusters = new List<Cluster>();
            int cont = 0;
            foreach (Element e in Set.Elements)
            {
                clusters.Add(new Cluster("c-" + cont, new List<Element>() { e }));
                cont++;
            }
            while (clusters.Count > ClustersCount)
            {
                Cluster c1 = null;
                Cluster c2 = null;
                double min = double.MaxValue;
                for (int i = 0; i < clusters.Count; i++)
                {
                    for (int j = i + 1; j < clusters.Count; j++)
                    {
                        double disstance = 0;
                        switch (CalculateClusterDistance)
                        {
                            case CalculateClusterDistance.SingleLink:
                                disstance = clusters[i].ClusterDistanceSingle(clusters[j], Dissimilarity);
                                break;
                            case CalculateClusterDistance.CompleteLink:
                                disstance = clusters[i].ClusterDistanceComplete(clusters[j], Dissimilarity);
                                break;
                            case CalculateClusterDistance.AverageLink:
                                disstance = clusters[i].ClusterDistanceAverage(clusters[j], Dissimilarity);
                                break;
                        }
                        if (disstance < min)
                        {
                            c1 = clusters[i];
                            c2 = clusters[j];
                            min = disstance;
                        }
                    }

                }
                clusters.Remove(c1);
                clusters.Remove(c2);
                clusters.Add(c1.Join(c2));

                CurrentValue++;

            }

            Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
            foreach (Cluster c in clusters)
            {
                dic_clusters.Add(c.Name, c);
            }
            Structuring = new Partition() { Clusters = dic_clusters, Dissimilarity = Dissimilarity };
            return Structuring;
        }
    }
}
