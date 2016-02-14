using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.Clusters
{
    class SAHN_StopRules : Agglomerative
    {
        public UpdateParameters UpdateAlfaI { get; set; }
        public UpdateParameters UpdateAlfaJ { get; set; }
        public UpdateParameters UpdateBeta { get; set; }
        public UpdateParameters UpdateGamma { get; set; }

        public SAHN_StopRules(Set set, Proximity diss)
            : base(set, diss)
        {
            ProximityType = ProximityType.Dissimilarity;
        }

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
                    IContainerProgressBar.UpdateProgressBar(1, "Running Hierarchical agglomerative algorithm with Lifetime...", true);
                }

                
                double AlfaI = 0, AlfaJ = 0, Beta = 0, Gamma = 0;

                //Al inicio cada elemento es un cluster
                double[,] DMatrix = new double[Set.Elements.Count, Set.Elements.Count];

                List<Cluster> clusters = new List<Cluster>();
                List<Cluster> best_clusters = null;
                List<double> distances = new List<double>();

                bool[] des = new bool[Set.Elements.Count];//true si ese elemento ya no es representativo

                for (int i = 0; i < Set.Elements.Count; i++)
                {
                    List<Element> l = new List<Element>();
                    l.Add(Set[i]);
                    clusters.Add(new Cluster("C-" + i, l));
                    distances.Add(0);
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

                //Variables de las stopping rules
                double step = Set.ElementsCount - 1, bestST = double.MinValue;
                double stoprule = 1;

                //Algoritmo O(n2*log n)
                for (int i = Set.Elements.Count; i > 2; i--)
                {
                    if (IContainerProgressBar != null)
                        IContainerProgressBar.UpdateProgressBar(_current++, "Running Hierarchical agglomerative algorithm with Lifetime...", false);

                    //Seleccionar los 2 clusters mas parecidos O(n)
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

                    //Actualizar los parametros AlfaI, AlfaJ y Beta
                    double cluster_i_count = clusters[erase_pos].ElementsCount;
                    double cluster_j_count = clusters[final_pos].ElementsCount;

                    AlfaI = UpdateAlfaI(cluster_i_count, cluster_j_count);
                    AlfaJ = UpdateAlfaJ(cluster_i_count, cluster_j_count);
                    Beta = UpdateBeta(cluster_i_count, cluster_j_count);
                    Gamma = UpdateGamma(cluster_i_count, cluster_j_count);

                    //Llamado al Stopping Rule
                    stoprule = LifeTimeStoppingRule(distances, final_pos, erase_pos, min);
                    //stoprule = CHStoppingRule(clusters, step, final_pos);

                    if (stoprule > bestST)
                    {
                        bestST = stoprule;

                        best_clusters = new List<Cluster>();
                        foreach (Cluster item in clusters)
                        {
                            Cluster temp = new Cluster(item.Name);
                            foreach (var e in item.Elements)
                            {
                                temp.Elements.Add(e);
                            }
                            best_clusters.Add(temp);
                        }
                    }

                    step--;

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
                for (int i = 0; i < best_clusters.Count; i++)
                {
                    if (best_clusters[i] != null)
                    {
                        best_clusters[i].Name = "C-" + cont;
                        dic_clusters.Add(best_clusters[i].Name, best_clusters[i]);
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
                    IContainerProgressBar.ShowError("Error occurred in Hierarchical agglomerative algorithm with Lifetime.");
                return null;
            }
        }

        double CHStoppingRule(List<Cluster> clusters, double step, int final_pos)
        {
            #region CH STOPPING RULE
            //Seleccionar la mejor particion
            //CH stopping rule

            double between = 0, within = 0;

            //Calcular centroide de cada cluster 
            foreach (Cluster c in clusters)
            {
                c.UpdateCentroid();
            }

            //Calcular media con peso entre los clusters
            Attributes attrs = Set.Attributes;
            List<object> values = new List<object>();
            for (int j = 0; j < attrs.ValuesCount; j++)
            {
                double temp = 0;
                for (int k = 0; k < clusters.Count; k++)
                    if (attrs[j].AttributeType == AttributeType.Numeric && clusters[k].Centroid.Values[j] != null)
                        temp += clusters[k].ElementsCount * (double)clusters[k].Centroid.Values[j];

                values.Add(temp / clusters.Count);
            }

            Element mean = new Element(values, Set.ElementType, Set.Attributes);
            mean.Index = -1;

            //Calcular Tr(Sb)---> between
            for (int j = 0; j < attrs.ValuesCount; j++)
            {
                for (int k = 0; k < clusters.Count; k++)
                {
                    if (attrs[j].AttributeType == AttributeType.Numeric && clusters[k].Centroid.Values[j] != null && mean.Values[j] != null)
                        between += clusters[k].ElementsCount * Math.Pow((double)clusters[k].Centroid.Values[j] - (double)mean.Values[j], 2);

                }
            }

            //Calcular Tr(Sw)---> within
            for (int j = 0; j < attrs.ValuesCount; j++)
            {
                for (int k = 0; k < clusters[final_pos].ElementsCount; k++)
                {
                    if (attrs[j].AttributeType == AttributeType.Numeric && clusters[final_pos].Elements[k].Values[j] != null && clusters[final_pos].Centroid.Values[j] != null)
                    {
                        within += Math.Round(Math.Pow((double)clusters[final_pos].Elements[k].Values[j] - (double)clusters[final_pos].Centroid.Values[j], 2), 15);
                    }
                }
            }

            if (within != 0)
            {

                double num = between / (step - 1);
                double den = within / (Set.ElementsCount - step);

                return num / den;
            }
            return 0;

            #endregion
        }

        double HartiganStoppingRule(List<Cluster> clusters, double step, int final_pos)
        {
            #region HARTIGAN STOPPING RULE
            
            double between = 0, within = 0;

            //Calcular centroide de cada cluster 
            foreach (Cluster c in clusters)
            {
                c.UpdateCentroid();
            }

            //Calcular media con peso entre los clusters
            Attributes attrs = Set.Attributes;
            List<object> values = new List<object>();
            for (int j = 0; j < attrs.ValuesCount; j++)
            {
                double temp = 0;
                for (int k = 0; k < clusters.Count; k++)
                    if (attrs[j].AttributeType == AttributeType.Numeric &&  clusters[k].Centroid.Values[j] != null)
                        temp += clusters[k].ElementsCount * (double)clusters[k].Centroid.Values[j];

                values.Add(temp / clusters.Count);
            }

            Element mean = new Element(values, Set.ElementType, Set.Attributes);
            mean.Index = -1;

            //Calcular Tr(Sb)---> between
            for (int j = 0; j < attrs.ValuesCount; j++)
            {
                for (int k = 0; k < clusters.Count; k++)
                {
                    if (attrs[j].AttributeType == AttributeType.Numeric && clusters[k].Centroid.Values[j] != null && mean.Values[j] != null)
                        between += clusters[k].ElementsCount * Math.Pow((double)clusters[k].Centroid.Values[j] - (double)mean.Values[j], 2);

                }
            }

            //Calcular Tr(Sw)---> within
            for (int j = 0; j < attrs.ValuesCount; j++)
            {
                for (int k = 0; k < clusters[final_pos].ElementsCount; k++)
                {
                    if (attrs[j].AttributeType == AttributeType.Numeric && clusters[final_pos].Elements[k].Values[j] != null && clusters[final_pos].Centroid.Values[j] != null)
                    {
                        within += Math.Round(Math.Pow((double)clusters[final_pos].Elements[k].Values[j] - (double)clusters[final_pos].Centroid.Values[j], 2), 15);
                    }
                }
            }

            if (within != 0)
                return Math.Log(between / within);

            return 0;

            #endregion
        }

        double LifeTimeStoppingRule(List<double> distances, int final_pos, int erase_pos, double min)
        {

            double lifetime = min - Math.Max(distances[final_pos], distances[erase_pos]);
            distances[final_pos] = min;
            distances.RemoveAt(erase_pos);
            return lifetime;

        }

    }
     


}

