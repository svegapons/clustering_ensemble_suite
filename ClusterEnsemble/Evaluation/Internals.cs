using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Graphics;
using ClusterEnsemble.Proximities;

namespace ClusterEnsemble.Evaluation
{
    public class AverageClusterSize : Internal
    {
        public AverageClusterSize()
        {
            Name = "Average Cluster Size";
        }
        public override double EvaluatePartition()
        {
            double result = 0;
            foreach (var item in Structuring.Clusters.Values)
            {
                result += item.ElementsCount;
            }
            return result / Structuring.ClustersCount;
        }
    }

    public class SilhouetteWidth : Internal
    {
        public SilhouetteWidth()
        {
            Name = "Silhouette Width [-1,1] max";
        }

        /// <summary>
        /// La silueta es calculada como el promedio de las siluetas de todos los elementos
        /// </summary>
        /// <returns></returns>
        public override double EvaluatePartition()
        {
            UpdatesCentroids();

            double result = 0;
            foreach (var item in Set.Elements)
            {
                result += GetSilhouette(item);
            }
            return result / Set.ElementsCount;
        }

        double GetSilhouette(Element e)
        {
            if (Structuring.HaveUnassignedElements() && Structuring.IsUnassigned(e))
                return 0;

            EuclideanDistance ed = new EuclideanDistance();
            ed.AttributesToCalculateProximity = Set.Attributes.Values;
            
            Cluster actual = Structuring.GetCluster(e)[0];

            double ai = ed.CalculateProximity(e, actual.Centroid);

            double bi = double.MaxValue;
            foreach (var cluster in Structuring.Clusters.Values)
            {
                if (actual.Name != cluster.Name)
                {
                    double aux = ed.CalculateProximity(e, cluster.Centroid);
                    if (aux < bi)
                        bi = aux;
                }
            }
            return (bi - ai) / Math.Max(bi, ai);
        }

        void UpdatesCentroids()
        {
            foreach (var cluster in Structuring.Clusters.Values)
            {
                cluster.UpdateCentroid();
            }
        }
    }

    public class Variance : Internal
    {
        public Variance()
        {
            Name = "Variance [0, +∞) min";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double EvaluatePartition()
        {
            UpdatesCentroids();

            EuclideanDistance ed = new EuclideanDistance();
            ed.AttributesToCalculateProximity = Set.Attributes.Values;

            double result = 0;
            foreach (var cluster in Structuring.Clusters.Values)
            {
                foreach (var item in cluster.Elements)
                {
                    result += ed.CalculateProximity(item, cluster.Centroid);
                }
            }

            return Math.Sqrt(result / Set.ElementsCount);
        }
        void UpdatesCentroids()
        {
            foreach (var cluster in Structuring.Clusters.Values)
            {
                cluster.UpdateCentroid();
            }
        }
    }

    public class Connectivity : Internal
    {
        List<HeapArray<Container>> lh;
        public Connectivity()
        {
            Name = "Connectivity [0, +∞) min";
        }

        [In(typeof(IntGTZeroConverter))]
        public int Neighbors { get; set; }

        public override double EvaluatePartition()
        {
            FillHeaps();            

            double result = 0;

            for (int i = 0; i < Set.ElementsCount; i++)
            {
                int cont = 0;
                while (lh[i].First != null)
                {
                    if (cont == Neighbors)
                        break;

                    double distance = lh[i].First.Rank;

                    if (!Structuring.BeSameCluster(Set.Elements[i], Set.Elements[lh[i].First.Cluster]))
                        result += 1 / distance;

                    lh[i].RemoveFirst();
                    cont++;
                }
            }
            return result;
        }
        public void FillHeaps()
        {
            if (Set == null)
                throw new NullReferenceException("Error conjunto NULL en la Medida Connectivity");

            lh = new List<HeapArray<Container>>(Set.ElementsCount);
            for (int i = 0; i < Set.Elements.Count; i++)
            {
                lh.Add(new HeapArray<Container>(Set.Elements.Count - 1));//No se pone la diss de un elemento con el mismo
            }

            EuclideanDistance ed = new EuclideanDistance();
            ed.AttributesToCalculateProximity = Set.Attributes.Values;

            for (int i = 0; i < Set.ElementsCount; i++)
            {
                for (int j = i + 1; j < Set.ElementsCount; j++)
                {
                    double distance = ed.CalculateProximity(Set.Elements[i], Set.Elements[j]);
                    lh[i].Add(new Container() { Rank = distance, Name = i, Cluster = j });
                    lh[j].Add(new Container() { Rank = distance, Name = j, Cluster = i });

                }
            }
        }
    }

    public class Dunn : Internal
    {
        List<HeapArray<Container>> lh;
        public Dunn()
        {
            Name = "Dunn [0, +∞) max";
        }

        public override double EvaluatePartition()
        {
            FillHeaps();

            double minInter = double.MaxValue;
            double maxIntra = double.MinValue;

            for (int i = 0; i < Set.ElementsCount; i++)
            {
                while (lh[i].First != null)
                {

                    double distance = lh[i].First.Rank;

                    if (distance < minInter && !Structuring.BeSameCluster(Set.Elements[i], Set.Elements[lh[i].First.Cluster]))
                        minInter = distance;
                    else if (distance > maxIntra && Structuring.BeSameCluster(Set.Elements[i], Set.Elements[lh[i].First.Cluster]))
                        maxIntra = distance;

                    lh[i].RemoveFirst();
                }

            }
            return minInter / maxIntra;
        }
        public void FillHeaps()
        {
            if (Set == null)
                throw new NullReferenceException("Error conjunto NULL en la Medida Dunn");

            lh = new List<HeapArray<Container>>(Set.ElementsCount);
            for (int i = 0; i < Set.Elements.Count; i++)
            {
                lh.Add(new HeapArray<Container>(Set.Elements.Count - 1));//No se pone la diss de un elemento con el mismo
            }

            EuclideanDistance ed = new EuclideanDistance();
            ed.AttributesToCalculateProximity = Set.Attributes.Values;

            for (int i = 0; i < Set.ElementsCount; i++)
            {
                for (int j = i + 1; j < Set.ElementsCount; j++)
                {
                    double distance = ed.CalculateProximity(Set.Elements[i], Set.Elements[j]);
                    lh[i].Add(new Container() { Rank = distance, Name = i, Cluster = j });
                    lh[j].Add(new Container() { Rank = distance, Name = j, Cluster = i });

                }
            }
        }
        
    }
}
