using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Clusters;

namespace ClusterEnsemble.ReductionFunctions
{
    public abstract class ReductionFunction
    {
        public abstract List<Structuring> GetReduction(Set aSet, List<Structuring> aStructurings, out Set anewSet);
        public abstract Structuring TurnOffReduction(Set aSet, Structuring aStructuring);

        private static double[,] bellTriangle;

        public static double BellNumber(int n)
        {
            if (bellTriangle == null)
            {
                bellTriangle = new double[n + 1, n + 1];
                bellTriangle[0, 0] = 1;
                // Calculamos 
                CalulateBellNumber(1);
            }
            else if (bellTriangle.GetLength(0) < n+1)
            {
                double[,] tempTriangle = bellTriangle;
                bellTriangle = new double[n + 1, n + 1];

                // Copiamos
                for (int i = 0; i < tempTriangle.GetLength(0) - 1; i++)
                    for (int j = i; j < tempTriangle.GetLength(1); j++)
                        bellTriangle[i, j] = tempTriangle[i, j];
                    
                // Calculamos lo que falta     
                CalulateBellNumber(tempTriangle.GetLength(1) - 1);
            }

            return bellTriangle[0, n];
        }

        private static void CalulateBellNumber(int startRow)
        {
            int n = bellTriangle.GetLength(1);

            for (int j = startRow; j < bellTriangle.GetLength(1); j++)
                for (int i = 0; i <= j; i++)
                    if (i == 0)
                        bellTriangle[i, j] = bellTriangle[j - 1, j - 1];
                    else
                        bellTriangle[i, j] = bellTriangle[i - 1, j - 1] + bellTriangle[i - 1, j];
        }
    }

    public class NoReduction : ReductionFunction
    {
        public override List<Structuring> GetReduction(Set aSet, List<Structuring> aStructurings, out Set anewSet)
        {
            if(aSet == null||aStructurings==null)
                throw new ArgumentNullException();
            
            anewSet = aSet;
            return aStructurings;
        }

        public override Structuring TurnOffReduction(Set aSet, Structuring aStructuring)
        {
            return aStructuring;
        }
    }

    public class FragmentClusterReduction : ReductionFunction
    {
        Dictionary<string, List<object>> fragments;

        public override List<Structuring> GetReduction(Set dataSet, List<Structuring> realPartitions, out Set anewSet)
        {
            if (dataSet == null || realPartitions == null)
                throw new ArgumentNullException();

            string[] objCodes = new string[dataSet.ElementsCount];
            for (int i = 0; i < dataSet.ElementsCount; i++)            
                objCodes[i] = "";

            fragments = new Dictionary<string, List<object>>();

            foreach (var partition in realPartitions)            
                for (int j = 0; j < dataSet.ElementsCount; j++)                
                    objCodes[j] += partition.GetCluster(dataSet.Elements[j])[0].Name;

            for (int j = 0; j < dataSet.ElementsCount; j++)
            {
                if (!fragments.ContainsKey(objCodes[j]))
                {
                    List<object> list = new List<object>();
                    list.Add(dataSet.Elements[j]);

                    fragments.Add(objCodes[j], list);
                }
                else
                    fragments[objCodes[j]].Add(dataSet.Elements[j]);
            }

            //
            List<Element> _elements = new List<Element>();
            Set _newSet = new Set(dataSet.RelationName + "(Reduction-Set)", _elements,dataSet.Attributes);
            foreach (var item in fragments)
            {
                Element _e = new Element(_newSet, item.Value);
                _e.Name = item.Key;
                _elements.Add(_e);
            }
            anewSet = _newSet;
            List<Structuring> partitions = new List<Structuring>();
            realPartitions.ForEach(rp => partitions.Add(new ReductionPartition(rp, _newSet, fragments)));

            return partitions;
        }

        public override Structuring TurnOffReduction(Set dataSet, Structuring partition)
        {
            if (partition is ReductionPartition)
            {
                return ((ReductionPartition)partition).Partition;
            }
            else
            {
                Dictionary<string, Cluster> _dic = new Dictionary<string, Cluster>();

                foreach (var item in partition.Clusters)
                {
                    Cluster _cluster = item.Value;
                    Cluster _newCluster = new Cluster(_cluster.Name, new List<Element>());
                    foreach (var _e in _cluster.Elements)
                        foreach (var _object in _e.Values)
                            _newCluster.AddElement((Element)_object);
                    _dic.Add(_newCluster.Name, _newCluster);
                }


                return new Partition() { Clusters = _dic, Proximity = partition.Proximity};
            }
        }
    }

    public class OurReduction : ReductionFunction
    {
        Dictionary<string, List<object>> fragments;

        public override List<Structuring> GetReduction(Set dataSet, List<Structuring> realPartitions, out Set anewSet)
        {
            if (dataSet == null || realPartitions == null)
                throw new ArgumentNullException();

            // Calculamos la matriz de adyacencia
            bool[,] adjMatrix = new bool[dataSet.ElementsCount, dataSet.ElementsCount];
            int[] visited = new int[dataSet.ElementsCount];
            int ccCount = 0;

            for (int i = 0; i < dataSet.ElementsCount - 1; i++)            
                for (int j = i + 1; j < dataSet.ElementsCount; j++)
                {
                    int jointCount = 0;
                    int halfPartitions = (int)Math.Floor(realPartitions.Count / 2.0);
                    foreach (var partition in realPartitions)
                    {
                        if (partition.BeSameCluster(dataSet[i], dataSet[j]))
                            jointCount++;
                        if (jointCount > halfPartitions)
                        {
                            adjMatrix[i, j] = adjMatrix[j, i] = true;
                            break;
                        }
                    }
                }

            for (int i = 0; i < dataSet.ElementsCount; i++)            
                if (visited[i] == 0)
                {
                    ccCount++;
                    DFS(i, adjMatrix, visited, ccCount);
                }    
        
            fragments = new Dictionary<string, List<object>>();

            for (int i = 1; i <= ccCount; i++)            
                fragments.Add("OurFragment-" + i, new List<object>());

            for (int i = 0; i < dataSet.ElementsCount; i++)            
                fragments["OurFragment-" + visited[i]].Add(dataSet[i]);
            

            // La transformación
            List<Element> _elements = new List<Element>();
            Set _newSet = new Set(dataSet.RelationName + "(Reduction-Set)", _elements, dataSet.Attributes);
            foreach (var item in fragments)
            {
                Element _e = new Element(_newSet, item.Value);
                _e.Name = item.Key;
                _elements.Add(_e);
            }
            anewSet = _newSet;
            List<Structuring> partitions = new List<Structuring>();

            //Build partitions
            //First Partition
            Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
            for (int i = 0; i < anewSet.ElementsCount; i++)
            {
                Cluster _c = new Cluster("C-"+i);
                _c.AddElement(anewSet[i]);
                dic_clusters.Add(_c.Name, _c);
            }
            partitions.Add(new Partition() { Clusters = dic_clusters });
                        
            //realPartitions.ForEach(rp => partitions.Add(new ReductionPartition(rp, _newSet, fragments)));

            return partitions;
        }

        private void DFS(int v, bool[,] adjMatrix, int[] visited, int marker)
        {
            visited[v] = marker;
            for (int i = 0; i < adjMatrix.GetLength(0); i++)            
                if (adjMatrix[v, i] && visited[i] == 0)
                    DFS(i, adjMatrix, visited, marker);            
        }

        public override Structuring TurnOffReduction(Set dataSet, Structuring partition)
        {
            if (partition is ReductionPartition)
            {
                return ((ReductionPartition)partition).Partition;
            }
            else
            {
                Dictionary<string, Cluster> _dic = new Dictionary<string, Cluster>();

                foreach (var item in partition.Clusters)
                {
                    Cluster _cluster = item.Value;
                    Cluster _newCluster = new Cluster(_cluster.Name, new List<Element>());
                    foreach (var _e in _cluster.Elements)
                        foreach (var _object in _e.Values)
                            _newCluster.AddElement((Element)_object);
                    _dic.Add(_newCluster.Name, _newCluster);
                }
                
                return new Partition() { Clusters = _dic, Proximity = partition.Proximity };
            }
        }
    }
}
