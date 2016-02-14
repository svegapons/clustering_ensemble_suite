using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Graphics;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Proximities;

///OJOOOOOO
///Aqui se usan todos cuando hay latticeReduction,
///para los fragmentCluster o si no hay redccion se usan los del fichero Mirkin.cs
///

namespace ClusterEnsemble.ClusterEnsemble
{
    public abstract class GenericDistancesNew
    {
        public abstract double CalculateDistance(Structuring structA, Structuring structB, Set Set);
        public abstract double CalculateDistanceWithLabels(string[] structA, string[] structB, Set Set);
    }

    public class MirkinDistanceNew : GenericDistances
    {
        public override double CalculateDistance(Structuring structA, Structuring structB, Set Set)
        {
            int _FP = 0, _FN = 0;
            for (int i = 0; i < Set.ElementsCount; i++)
                for (int j = i + 1; j < Set.ElementsCount; j++)
                {
                    bool sameClusterA = structA.BeSameCluster(Set[i], Set[j]);
                    bool sameClusterB = structB.BeSameCluster(Set[i], Set[j]);

                    if (!sameClusterA && sameClusterB)
                        _FP++;
                    if (sameClusterA && !sameClusterB)
                        _FN++;
                }
            return _FN + _FP;
        }

        public override double CalculateDistanceWithLabels(string[] structA, string[] structB,Set Set)
        {
            int _FP = 0, _FN = 0;
            for (int i = 0; i < Set.ElementsCount; i++)
                for (int j = i + 1; j < Set.ElementsCount; j++)
                {
                    bool sameClusterA = structA[i] == structA[j];
                    bool sameClusterB = structB[i] == structB[j];

                    if (!sameClusterA && sameClusterB)
                        _FP++;
                    if (sameClusterA && !sameClusterB)
                        _FN++;
                }
            return _FN + _FP;
        }
    }

    public class LatticeDistanceNew : GenericDistances
    {
        public override double CalculateDistance(Structuring structA, Structuring structB, Set dataSet)
        {
            bool[,] adjMatrix = new bool[dataSet.ElementsCount, dataSet.ElementsCount];
            int[] visited = new int[dataSet.ElementsCount];
            int ccCount = 0;

            for (int i = 0; i < dataSet.ElementsCount - 1; i++)
                for (int j = i + 1; j < dataSet.ElementsCount; j++)
                {
                    if (structA.BeSameCluster(dataSet[i], dataSet[j]) || structB.BeSameCluster(dataSet[i], dataSet[j]))
                        adjMatrix[i, j] = adjMatrix[j, i] = true;         
                }

            for (int i = 0; i < dataSet.ElementsCount; i++)
                if (visited[i] == 0)
                {
                    ccCount++;
                    DFS(i, adjMatrix, visited, ccCount);
                }

            return 2 * (dataSet.ElementsCount - ccCount) - (dataSet.ElementsCount - structA.ClustersCount) - (dataSet.ElementsCount - structB.ClustersCount);
        }

        public override double CalculateDistanceWithLabels(string[] structA, string[] structB, Set dataSet)
        {
            bool[,] adjMatrix = new bool[dataSet.ElementsCount, dataSet.ElementsCount];
            int[] visited = new int[dataSet.ElementsCount];
            int ccCount = 0;

            for (int i = 0; i < dataSet.ElementsCount - 1; i++)
                for (int j = i + 1; j < dataSet.ElementsCount; j++)
                {
                    if (structA[i] == structA[j] || structB[i] == structB[j])
                        adjMatrix[i, j] = adjMatrix[j, i] = true;
                }

            for (int i = 0; i < dataSet.ElementsCount; i++)
                if (visited[i] == 0)
                {
                    ccCount++;
                    DFS(i, adjMatrix, visited, ccCount);
                }

            return 2 * (dataSet.ElementsCount - ccCount) - (dataSet.ElementsCount - structA.Distinct().Count()) - (dataSet.ElementsCount - structB.Distinct().Count());
        }

        private void DFS(int v, bool[,] adjMatrix, int[] visited, int marker)
        {
            visited[v] = marker;
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
                if (adjMatrix[v, i] && visited[i] == 0)
                    DFS(i, adjMatrix, visited, marker);
        }
    }

    //First Heuristics
    public class BOKNew : Mirkin
    {
        public BOKNew(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "Best of k (BOK) New"; }
        public BOKNew() : base() { Name = "Best of k (BOK) New"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int k = StructuringsCount;
                int _current = 1, _max = k * k;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running BOK algorithm...", true);
                }

                double _bestDistance = double.MaxValue;
                int _structuringPosition = 0;

                for (int i = 0; i < StructuringsCount; i++)
                {
                    double _temp = 0;
                    for (int j = 0; j < StructuringsCount; j++)
                    {
                        if (i != j)
                            _temp += GenericDistances.CalculateDistance(Structurings[i], Structurings[j], Set);

                        if (IContainerProgressBar != null)
                            IContainerProgressBar.UpdateProgressBar(_current++, "Running BOK algorithm...", false);
                    }
                    if (_temp < _bestDistance)
                    {
                        _bestDistance = _temp;
                        _structuringPosition = i;
                    }

                }
                Structuring = Structurings[_structuringPosition];

                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();

                return Structuring;

            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in CSPA algorithm.");
                return null;
            }
        }
    }

    //Second Heuristics
    public class SAOMNew : Mirkin
    {
        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        [In(typeof(DoubleGTZeroConverter))]
        public double Temperature { get; set; }

        [In(new string[] { "True",
                           "False"},
            new Type[] { typeof(BooleanConverter), 
                           typeof(BooleanConverter) })]
        public bool UseBOK { get; set; }

        public SAOMNew(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "Simulated Annealing One-element Move (SAOM) New"; }
        public SAOMNew() : base() { Name = "Simulated Annealing One-element Move (SAOM) New"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int _current = 1, _max = IterationsCount;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(0, "Running SAOM algorithm...", true);
                }

                int random_pos = new Random(Environment.TickCount).Next(0, StructuringsCount);
                //EuclideanDistance _eu = new EuclideanDistance();
                //_eu.AttributesToCalculateProximity = Set.Attributes.Values;
                //KMeans _kmeans = new KMeans(Set, _eu);
                //_kmeans.ClustersCount =3;
                //_kmeans.Seed = Environment.TickCount;
                //_kmeans.IterationsCount = 100;

                //Structuring initial = Structurings[random_pos];

                BOK bok = new BOK(Set, Structurings) { GenericDistances = new MirkinDistance()};
                Structuring initial = UseBOK ? bok.BuildStructuring() : Structurings[0];

                string[] initial_sol = new string[Set.ElementsCount];
                for (int i = 0; i < Set.ElementsCount; i++)
                    initial_sol[i] = initial.Elements[Set[i]][0];

                int _maxCluster = 2*initial.ClustersCount;

                List<string[]> labels_List = new List<string[]>();
                //Convertir todas las particiones a arreglo de string, donde en cada posicion
                //esta la etiqueta del cluster al que pertenece el elemento j
                for (int i = 0; i < Structurings.Count; i++)
                {
                    string[] _temp = new string[Set.ElementsCount];
                    labels_List.Add(_temp);
                    for (int j = 0; j < Set.ElementsCount; j++)
                        _temp[j] = RealStructurings[i].Elements[RealSet[j]][0];
                }
                string[] _result_labels = SANew<string[]>.RunSAOM(Set, initial_sol, labels_List, Temperature, .99, 50, 1.05, 10, .5, IterationsCount, IContainerProgressBar, _current, GenericDistances, ref _maxCluster);
                Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                for (int i = 0; i < Set.ElementsCount; i++)
                {
                    if (dic_clusters.ContainsKey(_result_labels[i]))
                        dic_clusters[_result_labels[i]].AddElement(Set[i]);
                    else
                    {
                        Cluster c = new Cluster(_result_labels[i]);
                        c.AddElement(Set[i]);
                        dic_clusters.Add(c.Name, c);
                    }
                }

                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();

                Structuring = new Partition() { Clusters = dic_clusters };
                return Structuring;
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in SAOM algorithm.");
                return null;
            }
        }
    }

    //Third Heuristics
    public class BOMNew : Mirkin
    {
        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        [In(typeof(DoubleGTZeroConverter))]
        public double Temperature { get; set; }

        [In(new string[] { "True",
                           "False"},
            new Type[] { typeof(BooleanConverter), 
                           typeof(BooleanConverter) })]
        public bool UseBOK { get; set; }

        public BOMNew(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "Greedy Simulated Annealing One-element Move (GreedySAOM) New"; }
        public BOMNew() : base() { Name = "Greedy Simulated Annealing One-element Move (GreedySAOM) New"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int _current = 1, _max = IterationsCount;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(0, "Running Greedy SAOM algorithm...", true);
                }

                int random_pos = new Random(Environment.TickCount).Next(0, StructuringsCount);
                //EuclideanDistance _eu = new EuclideanDistance();
                //_eu.AttributesToCalculateProximity = Set.Attributes.Values;
                //KMeans _kmeans = new KMeans(Set, _eu);
                //_kmeans.ClustersCount =3;
                //_kmeans.Seed = Environment.TickCount;
                //_kmeans.IterationsCount = 100;
                
                //Structuring initial = Structurings[random_pos];

                BOK bok = new BOK(Set, Structurings) { GenericDistances = new MirkinDistance() };
                Structuring initial = UseBOK ? bok.BuildStructuring() : Structurings[0];

                string[] initial_sol = new string[Set.ElementsCount];
                for (int i = 0; i < Set.ElementsCount; i++)
                    initial_sol[i] = initial.Elements[Set[i]][0];
                
                int _maxCluster = 2 * initial.ClustersCount;
                
                List<string[]> labels_List = new List<string[]>();
                //Convertir todas las particiones a arreglo de string, donde en cada posicion
                //esta la etiqueta del cluster al que pertenece el elemento j
                for (int i = 0; i < Structurings.Count; i++)
                {
                    string[] _temp = new string[Set.ElementsCount];
                    labels_List.Add(_temp);
                    for (int j = 0; j < Set.ElementsCount; j++)
                        _temp[j] = RealStructurings[i].Elements[RealSet[j]][0];
                }

                string[] _result_labels = SANew<string[]>.RunBOM(Set, initial_sol, labels_List, Temperature, .99, 50, 1.05, 10, .5, IterationsCount, IContainerProgressBar, _current, GenericDistances, ref _maxCluster);
                Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                for (int i = 0; i < Set.ElementsCount; i++)
                {
                    if (dic_clusters.ContainsKey(_result_labels[i]))
                        dic_clusters[_result_labels[i]].AddElement(Set[i]);
                    else
                    {
                        Cluster c = new Cluster(_result_labels[i]);
                        c.AddElement(Set[i]);
                        dic_clusters.Add(c.Name, c);
                    }
                }

                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();

                Structuring = new Partition() { Clusters = dic_clusters };
                return Structuring;
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in SAOM algorithm.");
                return null;
            }
        }
    }

    public class SAOMNewNeighborNew : Mirkin
    {
        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        [In(typeof(DoubleGTZeroConverter))]
        public double Temperature { get; set; }

        [In(new string[] { "True",
                           "False"},
            new Type[] { typeof(BooleanConverter), 
                           typeof(BooleanConverter) })]
        public bool UseBOK { get; set; }

        public SAOMNewNeighborNew(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "Simulated Annealing MeetJoinClusters"; }
        public SAOMNewNeighborNew() : base() { Name = "Simulated Annealing MeetJoinClusters New"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int _current = 1, _max = IterationsCount;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(0, "Running Simulated Annealing MeetJoinClusters New algorithm...", true);
                }

                int random_pos = new Random(Environment.TickCount).Next(0, StructuringsCount);
                //EuclideanDistance _eu = new EuclideanDistance();
                //_eu.AttributesToCalculateProximity = Set.Attributes.Values;
                //KMeans _kmeans = new KMeans(Set, _eu);
                //_kmeans.ClustersCount =3;
                //_kmeans.Seed = Environment.TickCount;
                //_kmeans.IterationsCount = 100;

                //Structuring initial = Structurings[random_pos];

                BOK bok = new BOK(Set, Structurings) { GenericDistances = new MirkinDistance() };
                Structuring initial = UseBOK ? bok.BuildStructuring() : Structurings[0];

                string[] initial_sol = new string[Set.ElementsCount];
                for (int i = 0; i < Set.ElementsCount; i++)
                    initial_sol[i] = initial.Elements[Set[i]][0];

                int _maxCluster = initial.ClustersCount;

                List<string[]> labels_List = new List<string[]>();
                //Convertir todas las particiones a arreglo de string, donde en cada posicion
                //esta la etiqueta del cluster al que pertenece el elemento j
                for (int i = 0; i < Structurings.Count; i++)
                {
                    string[] _temp = new string[Set.ElementsCount];
                    labels_List.Add(_temp);
                    for (int j = 0; j < Set.ElementsCount; j++)
                        _temp[j] = RealStructurings[i].Elements[RealSet[j]][0];
                }

                string[] _result_labels = SANew<string[]>.RunSAOMNewNeighbor(Set, initial_sol, labels_List, Temperature, .99, 50, 1.05, 10, .5, IterationsCount, IContainerProgressBar, _current, GenericDistances, ref _maxCluster);
                Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                for (int i = 0; i < Set.ElementsCount; i++)
                {
                    if (dic_clusters.ContainsKey(_result_labels[i]))
                        dic_clusters[_result_labels[i]].AddElement(Set[i]);
                    else
                    {
                        Cluster c = new Cluster(_result_labels[i]);
                        c.AddElement(Set[i]);
                        dic_clusters.Add(c.Name, c);
                    }
                }

                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();

                Structuring = new Partition() { Clusters = dic_clusters };
                return Structuring;
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in Simulated Annealing MeetJoinClusters New algorithm.");
                return null;
            }
        }
    }


    public class SANew<T>
    {
        /// <summary>
        /// Simulated Annealing, minimize the function.
        /// </summary>
        /// <param name="initial">Initial Solution.</param>
        /// <param name="Temp0">Initial Temperature. (Temp0=1000)</param>
        /// <param name="alpha">Update K. (alpha less than 1, alpha=.99)</param>
        /// <param name="K0">Iterations Count each step (100).</param>
        /// <param name="rho">Update K. (rho=1.05)</param>
        /// <param name="A">Acceptations Count each step. (A=10)</param>
        /// <param name="Frz">Final Temperature. (Frz=.5)</param>
        /// <param name="iterationsCount">Iterations Count. (iterationsCount=1000)</param>
        /// <param name="Neighbor">Delegate to calculate an Neighbor.</param>
        /// <param name="parametersNeighbor">Neighbor's parameters.</param>
        /// <param name="EvaluateSolution">Function to evaluate.</param>
        /// <param name="parametersEvaluateSolution">EvaluateSolution's parameters.</param>
        /// <returns>Best Solution.</returns>
        public static T Run(T initial, double Temp0, double alpha, double K0, double rho, double A, double Frz, double iterationsCount, Neighbor<T> Neighbor, object[] parametersNeighbor, EvaluateSolution<T> EvaluateSolution, object[] parametersEvaluateSolution)
        {
            T _result = initial;
            double Temp = Temp0;
            double K = K0;

            double k = 0;
            double a = 0;
            double iteration = 0;
            while (iteration < iterationsCount && A / K < Frz)
            {
                k = 0;
                a = 0;
                while (k < K && a < A)
                {
                    T nextSolution = Neighbor(_result, parametersNeighbor);
                    double diff = EvaluateSolution(nextSolution, parametersEvaluateSolution) - EvaluateSolution(_result, parametersEvaluateSolution);

                    if (diff < 0)
                    {
                        _result = nextSolution;
                        a++;
                    }
                    else
                    {
                        double r = new Random(Environment.TickCount).NextDouble();

                        if (r < Math.Exp(-diff / Temp))
                        {
                            _result = nextSolution;
                            a++;
                        }

                    }
                    k++;
                }

                //Update
                Temp = alpha * Temp;
                K = rho * K;

                iteration++;
            }

            return _result;
        }

        /// <summary>
        /// Simulated Annealing, minimize the function.
        /// </summary>
        /// <param name="initial">Initial Solution.</param>
        /// <param name="Temp0">Initial Temperature. (Temp0=1000)</param>
        /// <param name="alpha">Update K. (alpha less than 1, alpha=.99)</param>
        /// <param name="K0">Iterations Count each step (100).</param>
        /// <param name="rho">Update K. (rho=1.05)</param>
        /// <param name="A">Acceptations Count each step. (A=10)</param>
        /// <param name="Frz">Final Temperature. (Frz=.5)</param>
        /// <param name="iterationsCount">Iterations Count. (iterationsCount=1000)</param>
        /// <param name="Neighbor">Delegate to calculate an Neighbor.</param>
        /// <param name="parametersNeighbor">Neighbor's parameters.</param>
        /// <param name="EvaluateSolution">Function to evaluate.</param>
        /// <param name="parametersEvaluateSolution">EvaluateSolution's parameters.</param>ref int aMaxCluster
        /// <param name="aMaxCluster">aMaxCluster the parameter to create new cluster in OneMoveElement.</param>
        /// <returns>Best Solution.</returns>
        public static string[] RunSAOM(Set Set, string[] initial_sol, List<string[]> partitions, double Temp0, double alpha, double K0, double rho, double A, double Frz, double iterationsCount, IContainerProgressBar IContainerProgressBar, int _current, GenericDistances aGenericDistance, ref int aMaxCluster)
        {
            string[] _result = initial_sol;
            string[] _best = _result;
            double Temp = Temp0;
            double K = K0;

            double old_evaluation = 0;

            double k = 0;
            double a = 0;
            double iteration = 0;
            while (iteration < iterationsCount && A / K < Frz)
            {
                k = 0;
                a = 0;
                while (k < K && a < A)
                {

                    old_evaluation = DelegateImplementations.S(Set, _best, partitions, aGenericDistance);

                    string[] nextSolution = DelegateImplementations.MoveOneElement(Set, _result, ref aMaxCluster);

                    double new_evaluation = DelegateImplementations.S(Set, nextSolution, partitions, aGenericDistance);

                    double diff = new_evaluation - old_evaluation;

                    if (diff < 0)
                    {
                        _best = nextSolution;
                        _result = nextSolution;
                        a++;
                    }
                    else
                    {
                        double r = new Random(Environment.TickCount).NextDouble();

                        if (r < Math.Exp(-diff / Temp))
                        {
                            _result = nextSolution;
                            a++;
                        }
                        else
                            aMaxCluster = _result.Distinct<string>().Count();

                    }
                    k++;
                }

                //Update
                Temp = alpha * Temp;
                K = rho * K;

                iteration++;

                if (IContainerProgressBar != null)
                    IContainerProgressBar.UpdateProgressBar(_current++, "Running SAOM algorithm...", false);
            }

            return _best;
        }

        /// <summary>
        /// Simulated Annealing, minimize the function.
        /// </summary>
        /// <param name="initial">Initial Solution.</param>
        /// <param name="Temp0">Initial Temperature. (Temp0=1000)</param>
        /// <param name="alpha">Update K. (alpha less than 1, alpha=.99)</param>
        /// <param name="K0">Iterations Count each step (100).</param>
        /// <param name="rho">Update K. (rho=1.05)</param>
        /// <param name="A">Acceptations Count each step. (A=10)</param>
        /// <param name="Frz">Final Temperature. (Frz=.5)</param>
        /// <param name="iterationsCount">Iterations Count. (iterationsCount=1000)</param>
        /// <param name="Neighbor">Delegate to calculate an Neighbor.</param>
        /// <param name="parametersNeighbor">Neighbor's parameters.</param>
        /// <param name="EvaluateSolution">Function to evaluate.</param>
        /// <param name="parametersEvaluateSolution">EvaluateSolution's parameters.</param>ref int aMaxCluster
        /// <param name="aMaxCluster">aMaxCluster the parameter to create new cluster in OneMoveElement.</param>
        /// <returns>Best Solution.</returns>
        public static string[] RunBOM(Set Set, string[] initial_sol, List<string[]> partitions, double Temp0, double alpha, double K0, double rho, double A, double Frz, double iterationsCount, IContainerProgressBar IContainerProgressBar, int _current, GenericDistances aGenericDistance, ref int aMaxCluster)
        {
            //string[] _result = initial_sol;
            string[] _best = initial_sol;
            double Temp = Temp0;
            double K = K0;

            double old_evaluation = 0;

            double k = 0;
            double a = 0;
            double iteration = 0;
            while (iteration < iterationsCount && A / K < Frz)
            {
                k = 0;
                a = 0;
                while (k < K && a < A)
                {

                    old_evaluation = DelegateImplementations.S(Set, _best, partitions, aGenericDistance);

                    string[] nextSolution = DelegateImplementations.MoveOneElement(Set, _best, ref aMaxCluster);

                    double new_evaluation = DelegateImplementations.S(Set, nextSolution, partitions, aGenericDistance);

                    double diff = new_evaluation - old_evaluation;

                    if (diff < 0)
                    {
                        _best = nextSolution;
                        a++;
                    }
                    else
                        aMaxCluster = _best.Distinct<string>().Count();
                   
                    k++;
                }

                //Update
                Temp = alpha * Temp;
                K = rho * K;

                iteration++;

                if (IContainerProgressBar != null)
                    IContainerProgressBar.UpdateProgressBar(_current++, "Running SAOM algorithm...", false);
            }

            return _best;
        }

        public static string[] RunSAOMNewNeighbor(Set Set, string[] initial_sol, List<string[]> partitions, double Temp0, double alpha, double K0, double rho, double A, double Frz, double iterationsCount, IContainerProgressBar IContainerProgressBar, int _current, GenericDistances aGenericDistance, ref int aMaxCluster)
        {
            string[] _result = initial_sol;
            string[] _best = _result;
            double Temp = Temp0;
            double K = K0;

            double old_evaluation = 0;

            double k = 0;
            double a = 0;
            double iteration = 0;
            while (iteration < iterationsCount && A / K < Frz)
            {
                k = 0;
                a = 0;
                while (k < K && a < A)
                {

                    old_evaluation = DelegateImplementations.S(Set, _best, partitions, aGenericDistance);


                    string[] nextSolution = DelegateImplementations.MeetJoinClusters(Set, _result, ref aMaxCluster);

                    double new_evaluation = DelegateImplementations.S(Set, nextSolution, partitions, aGenericDistance);

                    double diff = new_evaluation - old_evaluation;

                    if (diff < 0)
                    {
                        _best = nextSolution;
                        _result = nextSolution;
                        a++;
                    }
                    else
                    {
                        double r = new Random(Environment.TickCount).NextDouble();

                        if (r < Math.Exp(-diff / Temp))
                        {
                            _result = nextSolution;
                            a++;
                        }
                        else
                            aMaxCluster = _result.Distinct<string>().Count();


                    }
                    k++;
                }

                //Update
                Temp = alpha * Temp;
                K = rho * K;

                iteration++;

                if (IContainerProgressBar != null)
                    IContainerProgressBar.UpdateProgressBar(_current++, "Running SAOMNewNeighbor algorithm...", false);
            }

            return _best;
        }

    }

    public class DelegateImplementationsNew
    {
        public static string[] MoveOneElement(Set Set, string[] labels, ref int aMaxCluster)
        {
            Random _rnd = new Random(Environment.TickCount);
            Random _rnd1 = new Random(Environment.TickCount + 100);
            Random _rnd2 = new Random(Environment.TickCount + 200);

            int element1 = -1;
            element1 = _rnd.Next(0, Set.ElementsCount);
            string cluster1 = labels[element1];

            int element2 = -1;
            element2 = _rnd1.Next(0, Set.ElementsCount);
            string cluster2 = labels[element2];

            string[] _result = new string[labels.Length];
            for (int i = 0; i < _result.Length; i++)
                _result[i] = labels[i];
            
            if (cluster1 != cluster2)
            {
                //Swap
                string _temp = _result[element1];
                _result[element1] = _result[element2];
                _result[element2] = _temp;
            }
            else
            {
                int _pos = _rnd2.NextDouble() > .5 ? element1 : element2;
                _result[_pos] = _result[_pos] + aMaxCluster;
                aMaxCluster++;

            }

            return _result;
        }

        public static double S(Set Set, string[] new_partition, List<string[]> Structurings, GenericDistances aGenericDistance)
        {
            string[] _tempNewPartition = new_partition;
            new_partition = new string[Structurings[0].Length];

            for (int i = 0; i < _tempNewPartition.Length; i++)
            {
                for (int j = 0; j < Set[i].Values.Count; j++)
                {
                    Element _e = (Element)Set[i].Values[j];
                    new_partition[_e.Index] = _tempNewPartition[i];
                }
            }

            double _result = 0;
            for (int i = 0; i < Structurings.Count; i++)
                _result += aGenericDistance.CalculateDistanceWithLabels(new_partition, Structurings[i], Set);

            new_partition = _tempNewPartition;

            return _result;
        }
    }
}