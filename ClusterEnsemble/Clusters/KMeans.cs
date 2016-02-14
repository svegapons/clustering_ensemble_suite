using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble.Graphics;
using ClusterEnsemble.Proximities;

namespace ClusterEnsemble.Clusters
{
    public class KMeans : NonHierarchical
    {
        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        [In(typeof(IntConverter))]
        public int Seed { get; set; }

        public KMeans(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "K-Means";
            ProximityType = ProximityType.Dissimilarity;
        }
        public KMeans()
            : base()
        {
            Name = "K-Means";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null)
                    throw new NullReferenceException();

                IterationsCount = 100;

                int _current = 1;
                int _max = IterationsCount * (Set.ElementsCount * 2);
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running K-Means algorithm...", false);
                }
                
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

                    //Seleccionar los centroides aleatorios
                    Random r = new Random(Seed + Environment.TickCount);
                    int index = 0;

                    for (int i = Set.ElementsCount - 1, j = 0; i >= 0; i--, j++)
                    {
                        index = r.Next(0, i + 1);

                        List<Element> l = new List<Element>();
                        l.Add(Set[index]);
                        clusters.Add(new Cluster("C-" + j, l) { Centroid = Set[index] });

                        //Element temp = Set[index];
                        //Set[index] = Set[i];
                        //Set[i] = temp;
                        Set.Swap(index, i);


                        if (clusters.Count == ClustersCount)
                            break;
                    }                    

                    //Algoritmo
                    bool converged = false;
                    int emptyClustersCount = 0;
                    int numIterations = 0;
                    int[] clusterAssignments = new int[Set.ElementsCount];

                    while (!converged && numIterations < IterationsCount)
                    {
                        converged = true;
                        emptyClustersCount = 0;
                        numIterations++;

                        //colocar en cada centroide el elemento mas similar
                        for (int i = 0; i < Set.ElementsCount; i++)
                        {
                            Element toCluster = Set[i];
                            int newC = ClusterProcessedElement(toCluster, clusters);

                            if (newC != clusterAssignments[i])
                            {
                                converged = false;
                                clusterAssignments[i] = newC;
                            }


                            if (IContainerProgressBar != null)
                                IContainerProgressBar.UpdateProgressBar(_current++, "Running K-Means algorithm...", false);


                        }

                        foreach (var item in clusters)
                            item.Elements.Clear();

                        for (int j = 0; j < clusterAssignments.Length; j++)
                        {
                            while (clusters.Count <= clusterAssignments[j])
                                clusters.Add(new Cluster("C-" + j));
                            clusters[clusterAssignments[j]].AddElement(Set[j]);


                            if (IContainerProgressBar != null)
                                IContainerProgressBar.UpdateProgressBar(_current++, "Running K-Means algorithm...", false);


                        }

                        List<Cluster> lc = new List<Cluster>();
                        foreach (var item in clusters)
                        {
                            if (item.ElementsCount == 0)
                            {
                                emptyClustersCount++;
                                lc.Add(item);
                            }
                            else
                                item.UpdateCentroid();
                        }
                        foreach (var item in lc)
                            clusters.Remove(item);

                        if (emptyClustersCount > 0)
                            ClustersCount -= emptyClustersCount;                        

                    }

                    //Crear Dictionary<string,Cluster> para poder construir la particion
                    Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                    for (int i = 0; i < clusters.Count; i++)
                    {
                        clusters[i].Name = "C-" + i;
                        dic_clusters.Add(clusters[i].Name, clusters[i]);
                    }


                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();


                    Structuring = new Partition() { Clusters = dic_clusters, Proximity = Proximity };
                    return Structuring;
                }
            }
            catch 
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in K-Means algorithm.");
                return null;
            }
        }

        //Dado un elemento y una lista de clusters devuelve la posicion del cluster mas similar
        private int ClusterProcessedElement(Element e, List<Cluster> l)
        {
            //Esto es asi ya que estamos tratando con dissimilitud, es decir mientras mas pequeno mas cerca estan
            double minDist = double.MaxValue;
            int bestCluster = 0;
            for (int i = 0; i < ClustersCount; i++)
            {
                double dist = Proximity.CalculateProximity(e, l[i].Centroid);
                if (dist < minDist)
                {
                    minDist = dist;
                    bestCluster = i;
                }
            }
            return bestCluster;
        }

    }
}
