using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterEnsemble
{
    public class I_Variance:Index
    {
        public I_Variance() : base() { }



        /// <summary>
        /// Indice numero 1. Varianza (Compacidad) entre los agrupamientos de una particion. (Que tan pequenna es la suma de todas las distancias de los elementos de un cluster a su centroide.)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override double Process(Partition p)
        {
            double result = 0;
            List<Element> aux;
            foreach (string clusterId in p.Clusters.Keys)
            {
                aux = p.Clusters[clusterId];
                for (int i = 0; i < aux.Count; i++)
                {
                    result += Set.EuclideanDistance(aux[i], p.ClusterCentroid[clusterId]);
                }
            }
            return Math.Sqrt(result / p.ElementsCount);
        }

        /// <summary>
        /// Distancia euclideana entre dos elementos.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
       
    }
}
