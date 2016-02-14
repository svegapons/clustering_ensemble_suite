using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterEnsemble
{
    /// <summary>
    /// Este indice refleja el grado de conectividad de un cluster, evaluando el grado en qeu los objetos vecinos son colocados en un mismo cluster.
    /// </summary>
    public class I_Conectivity:Index
    {
        public I_Conectivity() : base() { }


        private int neighborsQuantity = 5;
        public int NeighborsQuantity 
        {
            get { return neighborsQuantity; }
            set { neighborsQuantity = value; }
        }

        public override double Process(Partition p)
        {
            double result = 0;
            int cont = 0;
            for (int i = 0; i < p.ElementsCount; i++)
            {
                cont = 0;
                foreach (double val in p.Set.Distances[i].Keys)
                {
                    if (cont == neighborsQuantity)
                        break;

                    if (!p.InTheSameCluster(p.Set.Elements[i], p.Set.Distances[i][val]))
                        result += 1.0 / val;

                    cont++;
                }
            }
            return result;
        }
    }
}
