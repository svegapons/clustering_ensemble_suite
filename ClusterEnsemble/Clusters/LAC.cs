using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble.Graphics;
using ClusterEnsemble.Proximities;
using System.Threading;

namespace ClusterEnsemble.Clusters
{
    public class LAC:NonHierarchical
    {

        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        [In(typeof(DoubleConverter))]
        public double H { get; set; }

        [In(typeof(IntConverter))]
        public int Seed { get; set; }

        public LAC(Set set, Proximity diss)
            : base(set, diss)
        {
            H = 9;
            Name = "Locally Adaptive Clustering";
            ProximityType = ProximityType.None;
        }
        public LAC()
            : base()
        {
            H = 9;
            Name = "Locally Adaptive Clustering";
            ProximityType = ProximityType.None;
        }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null)
                    throw new NullReferenceException();

                int _current = 1;
                int _max = IterationsCount * (Set.ElementsCount * 4);
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running LAC algorithm...", true);
                }

                //////BORRAR////////////////////////////
                //Verificar que todos los atributos sean de tipo Numerico
                foreach (var attr in Set.Attributes.Values)
                    if (attr.AttributeType != AttributeType.Numeric)
                        throw new Exception("The LAC algorithm run only with numerics Sets.");

                if (ClustersCount > Set.ElementsCount)
                    ClustersCount = Set.ElementsCount;

                if (ClustersCount == 1)
                {
                    Dictionary<string, Cluster> dic_clus = new Dictionary<string, Cluster>();
                    string name = "C-0";
                    List<Element> temp = new List<Element>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                        temp.Add(Set[i]);

                    dic_clus.Add(name, new Cluster(name) { Elements = temp });

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    Structuring = new Partition() { Clusters = dic_clus, Proximity = Proximity };
                    return Structuring;
                }
                else
                {
                    List<Cluster> clusters = new List<Cluster>();
                    List<List<double>> W = new List<List<double>>();

                    //Seleccionar los centroides aleatorios
                    Random r = new Random(Seed);
                    int index = 0;

                    #region K centroids y W
                    for (int i = Set.ElementsCount - 1, j = 0; i >= 0; i--, j++)
                    {
                        index = r.Next(0, i + 1);

                        List<Element> l = new List<Element>();
                        l.Add(Set[index]);
                        clusters.Add(new Cluster("C-" + j, l) { Centroid = Set[index] });

                        List<double> wi = new List<double>();
                        for (int m = 0; m < Set.Attributes.ValuesCount; m++)
                        {
                            wi.Add(1.0 / Math.Sqrt(Set.Attributes.ValuesCount));
                        }

                        W.Add(wi);

                        //Element temp = Set[index];
                        //Set[index] = Set[i];
                        //Set[i] = temp;
                        Set.Swap(index, i);


                        if (clusters.Count == ClustersCount)
                            break;
                    }
                    #endregion

                    bool converged = false;
                    int numIterations = 0;
                    int[] clusterAssignments = new int[Set.ElementsCount];

                    while (!converged && numIterations < IterationsCount)
                    {
                        numIterations++;
                        converged = true;

                        //PASO 3::
                        //Asignar cada elemento a su cluster correspondiente(lo unico que se utiliza son los centroides)
                        //No tienen porque estar llenos los clusters
                        AssignElements(clusterAssignments, clusters, W, ref _current);

                        foreach (var item in clusters)
                            item.Elements.Clear();

                        //Lleno cada cluster segun la asignacion
                        for (int j = 0; j < clusterAssignments.Length; j++)
                        {
                            while (clusters.Count <= clusterAssignments[j])
                                clusters.Add(new Cluster("C-" + j));
                            clusters[clusterAssignments[j]].AddElement(Set[j]);

                            if (IContainerProgressBar != null)
                                IContainerProgressBar.UpdateProgressBar(_current++, "Running LAC algorithm...", false);
                        }

                        //PASO 4::
                        //Computar los nuevos pesos
                        for (int j = 0; j < W.Count; j++)
                        {
                            for (int i = 0; i < W[j].Count; i++)
                            {
                                double Xji = CalculateXji(i, clusters[j]);

                                double wji = CalculateWji(Xji, Set.Attributes.ValuesCount, clusters[j]);

                                W[j][i] = wji;
                            }
                        }

                        //PASO 5::
                        //Asignar nuevamente cada elemento a su cluster correspondiente
                        converged = AssignElements(clusterAssignments, clusters, W, ref _current);

                        foreach (var item in clusters)
                            item.Elements.Clear();

                        for (int j = 0; j < clusterAssignments.Length; j++)
                        {
                            while (clusters.Count <= clusterAssignments[j])
                                clusters.Add(new Cluster("C-" + j));
                            clusters[clusterAssignments[j]].AddElement(Set[j]);

                            if (IContainerProgressBar != null)
                                IContainerProgressBar.UpdateProgressBar(_current++, "Running LAC algorithm...", false);
                        }

                        //PASO 6::
                        //Computar los nuevos centroides
                        for (int i = 0; i < clusters.Count; i++)
                            clusters[i].UpdateCentroid();
                    }


                    //Crear Dictionary<string,Cluster> para poder construir la particion
                    Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                    for (int i = 0; i < clusters.Count; i++)
                    {
                        List<double> _ws = new List<double>();
                        clusters[i].Name = "C-" + i;
                        clusters[i].Weights = _ws;
                        dic_clusters.Add(clusters[i].Name, clusters[i]);

                        for (int j = 0; j < Set.Attributes.ValuesCount; j++)
                            _ws.Add((double)W[i][j]);
                    }

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    Structuring = new WeightedPartition() { Clusters = dic_clusters, Proximity = Proximity };
                    return Structuring;
                }
            }
            catch (Exception _ex)
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in LAC algorithm.\n" + _ex.Message);
                return null;
            }
        }


        private int ClusterProcessedElement(Element e, List<Cluster> l, List<List<double>> W)
        {
            try
            {
                //Esto es asi ya que estamos tratando con dissimilitud, es decir mientras mas pequeno mas cerca estan
                double minDist = double.MaxValue;
                int bestCluster = 0;
                for (int i = 0; i < ClustersCount; i++)
                {
                    double dist = CalculateDistance(e, l[i].Centroid, W[i]);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestCluster = i;
                    }
                }
                return bestCluster;
            }
            catch
            {
                return -1;
            }
        }

        private double CalculateDistance(Element e, Element centroid, List<double> wi)
        {
            try
            {
                double _result = 0;
                for (int i = 0; i < e.Attributes.ValuesCount; i++)
                    _result += (double)wi[i] * Math.Pow((double)centroid[i] - (double)e[i], 2);

                return Math.Sqrt(_result);
            }
            catch
            {
                return -1;
            }
        }

        private double CalculateXji(int i, Cluster c)
        {
            try
            {
                double _result = 0;
                for (int k = 0; k < c.ElementsCount; k++)
                    _result += Math.Pow((double)c.Centroid[i] - (double)c[k][i], 2);

                return _result / c.ElementsCount;
            }
            catch
            {
                return -1;
            }
        }

        private double CalculateWji(double Xji, int attributesCount, Cluster c)
        {
            try
            {
                double _numerator = Math.Exp(-H * Xji);
                double _denominator = 0;

                for (int k = 0; k < attributesCount; k++)
                    _denominator += Math.Exp(-2 * H * CalculateXji(k, c));

                _denominator = Math.Sqrt(_denominator);

                return _numerator / _denominator;
            }
            catch
            {
                return -1;
            }
        }

        private bool AssignElements(int[] clusterAssignments, List<Cluster> clusters, List<List<double>> W, ref int _current)
        {
            try
            {
                bool converged = true;
                //Asignar cada elemento a su cluster correspondiente
                for (int i = 0; i < Set.ElementsCount; i++)
                {
                    Element _element = Set[i];
                    int newC = ClusterProcessedElement(_element, clusters, W);

                    if (newC != clusterAssignments[i])
                    {
                        converged = false;
                        clusterAssignments[i] = newC;
                    }

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.UpdateProgressBar(_current++, "Running LAC algorithm...", false);

                }

                return converged;
            }
            catch
            {
                return true;
            }
        }
    }
}
