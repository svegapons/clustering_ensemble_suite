using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.Clusters
{
    class SAHN : Agglomerative
    {
        public UpdateParameters UpdateAlfaI { get; set; }
        public UpdateParameters UpdateAlfaJ { get; set; }
        public UpdateParameters UpdateBeta { get; set; }
        public UpdateParameters UpdateGamma { get; set; }      

        public SAHN(Set set, Proximity diss)
            : base(set, diss)
        { }
        public SAHN() : base() { }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null)
                    throw new NullReferenceException();

                int _current = 1;
                int _max = Set.ElementsCount;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running Hierarchical agglomerative algorithm...", true);
                }

                if (ClustersCount > Set.ElementsCount)
                    ClustersCount = Set.ElementsCount;

                if (ClustersCount <= 0)
                    throw new Exception("La cantidad de clusters debe ser mayor que cero");
                if (ClustersCount == 1)
                {
                    Dictionary<string, Cluster> dic_clus = new Dictionary<string, Cluster>();
                    string name = "C-0";
                    List<Element> temp = new List<Element>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                    {
                        if (IContainerProgressBar != null)
                            IContainerProgressBar.UpdateProgressBar(_current++, "Running Hierarchical agglomerative algorithm...", false);

                        temp.Add(Set[i]);
                    }

                    dic_clus.Add(name, new Cluster(name) { Elements = temp });

                    Structuring = new Partition() { Clusters = dic_clus, Proximity = Proximity };

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;
                }

                double AlfaI = 0, AlfaJ = 0, Beta = 0, Gamma = 0;

                //Al inicio cada elemento es un cluster
                double[,] DMatrix = new double[Set.Elements.Count, Set.Elements.Count];
                List<Cluster> clusters = new List<Cluster>();
                bool[] des = new bool[Set.Elements.Count];//true si ese elemento ya no es representativo

                for (int i = 0; i < Set.Elements.Count; i++)
                {
                    List<Element> l = new List<Element>();
                    l.Add(Set[i]);
                    clusters.Add(new Cluster("C-" + i, l));
                }

                //Construir para cada cluster una cola con prioridad
                //con las disimilitudes de el con el resto de los clusters
                //y la matriz de todas las disimilitudes O(n2*log n)

                List<HeapArray<Container>> lh = new List<HeapArray<Container>>();

                for (int i = 0; i < Set.Elements.Count; i++)
                {
                    lh.Add(new HeapArray<Container>(Set.Elements.Count - 1));//No se pone la diss de un elemento con el mismo
                }
                for (int i = 0; i < Set.Elements.Count; i++)
                {
                    for (int j = i + 1; j < Set.Elements.Count; j++)
                    {

                        double temp_diss = Proximity.CalculateProximity(Set.Elements[i], Set.Elements[j]);

                        DMatrix[i, j] = temp_diss;
                        DMatrix[j, i] = temp_diss;
                        lh[i].Add(new Container { Rank = temp_diss, Name = i, Cluster = j });
                        lh[j].Add(new Container { Rank = temp_diss, Name = j, Cluster = i });
                    }
                }

                //Algoritmo O(n2*log n)
                for (int i = Set.Elements.Count; i > ClustersCount; i--)
                {
                    if (IContainerProgressBar != null)
                        IContainerProgressBar.UpdateProgressBar(_current++, "Running Hierarchical agglomerative algorithm...", false);

                    //Seleccionar los 2 clusters mas similares O(n)
                    double min = double.MaxValue;
                    int cluster_i = 0, cluster_j = 0;
                    int pos_cluster_i = 0, pos_cluster_j = 0;

                    for (int j = lh.Count - 1; j >= 0; j--)
                    {
                        if (lh[j].First.Rank < min)
                        {
                            min = lh[j].First.Rank;
                            cluster_i = lh[j].First.Name;
                            cluster_j = lh[j].First.Cluster;
                            pos_cluster_i = j;
                        }
                    }
                    for (int j = 0; j < lh.Count; j++)
                    {
                        if (lh[j].First != null && lh[j].First.Name == cluster_j)
                        {
                            pos_cluster_j = j;
                            break;
                        }
                    }

                    //Calcular posiciones para borrar y guardar               

                    int erase_pos = 0, final_pos = 0;

                    erase_pos = pos_cluster_i > pos_cluster_j ? pos_cluster_i : pos_cluster_j;
                    final_pos = pos_cluster_i < pos_cluster_j ? pos_cluster_i : pos_cluster_j;

                    lh.RemoveAt(erase_pos);

                    //Actualizar los parametros AlfaI, AlfaJ, Beta y Gamma
                    double cluster_i_count = clusters[erase_pos].ElementsCount;
                    double cluster_j_count = clusters[final_pos].ElementsCount;

                    AlfaI = UpdateAlfaI(cluster_i_count, cluster_j_count);
                    AlfaJ = UpdateAlfaJ(cluster_i_count, cluster_j_count);
                    Beta = UpdateBeta(cluster_i_count, cluster_j_count);
                    Gamma = UpdateGamma(cluster_i_count, cluster_j_count);

                    //Unir los clusters

                    foreach (Element item in clusters[erase_pos].Elements)
                    {
                        clusters[final_pos].AddElement(item);
                    }
                    clusters.RemoveAt(erase_pos);

                    //Actualizar DMatrix con la disimilitud del nuevo cluster
                    //y el resto de los clusters O(n)
                    //Formula que se usa segun el algoritmo

                    //La posicion del cluster i en Dmatrix es cluster_i
                    //La posicion del cluster j en Dmatrix es cluster_j

                    int pos_h = -1;
                    int pos_i = cluster_i;
                    int pos_j = cluster_j;

                    if (erase_pos == pos_cluster_i)
                    {
                        des[cluster_i] = true;
                        pos_h = cluster_j;
                    }
                    else
                    {
                        des[cluster_j] = true;
                        pos_h = cluster_i;
                    }

                    for (int k = 0; k < DMatrix.GetLength(0); k++)
                    {
                        DMatrix[pos_h, k] = AlfaI * DMatrix[pos_i, k] + AlfaJ * DMatrix[pos_j, k] + Beta * DMatrix[pos_i, pos_j] + Gamma * Math.Abs(DMatrix[pos_i, k] - DMatrix[pos_j, k]);
                        DMatrix[k, pos_h] = DMatrix[pos_h, k];
                    }

                    //Actualizar array de Heaps con la disimilitud del nuevo cluster
                    //y el resto de los clusters O(n*log n)

                    lh[final_pos] = new HeapArray<Container>(Set.Elements.Count - 1);

                    for (int j = 0; j < lh.Count; j++)
                    {

                        Container[] lc = lh[j].ToArray;
                        lh[j] = new HeapArray<Container>(Set.Elements.Count - 1);
                        for (int k = 1; k < lc.Length; k++)
                        {
                            if (lc[k] == null)
                                break;
                            if (lc[k].Cluster != cluster_i && lc[k].Cluster != cluster_j)
                                lh[j].Add(lc[k]);

                        }
                        if (j != final_pos && lh[j].First != null)
                            lh[j].Add(new Container { Rank = DMatrix[pos_h, lh[j].First.Name], Name = lh[j].First.Name, Cluster = pos_h });

                    }

                    for (int j = 0; j < DMatrix.GetLength(0); j++)
                    {
                        if (pos_h != j && pos_j != j && pos_i != j && !des[j])
                            lh[final_pos].Add(new Container { Rank = DMatrix[pos_h, j], Name = pos_h, Cluster = j });

                    }


                }

                //Crear Dictionary<string,Cluster> para construir la particion
                Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                int cont = 0;
                for (int i = 0; i < clusters.Count; i++)
                {
                    if (clusters[i] != null)
                    {
                        clusters[i].Name = "C-" + cont;
                        dic_clusters.Add(clusters[i].Name, clusters[i]);
                        cont++;
                    }
                }

                Structuring = new Partition() { Clusters = dic_clusters, Proximity = Proximity };

                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();

                return Structuring;
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in Hierarchical agglomerative algorithm.");
                return null;
            }
        }
        
    }
    delegate double UpdateParameters(double i, double j);
}

