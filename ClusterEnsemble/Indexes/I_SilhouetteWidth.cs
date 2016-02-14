using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterEnsemble
{
    /// <summary>
    /// El ancho de la silueta es calculado como el promedio de la silueta en cada objeto.
    /// </summary>
    public class I_SilhouetteWidth:Index
    {
        public I_SilhouetteWidth() : base() { }

        public override double Process(Partition p)
        {
            double result = 0;
            for (int i = 0; i < p.ElementsCount; i++)
                result += GetSilhouette(p, p.Set.Elements[i]);

            return result / p.ElementsCount;
        }

        protected double GetSilhouette(Partition p, Element e)
        {
            double outDist = double.MaxValue;
            string idCluster=p.ClustAssignation[e];
            double inDist = Set.EuclideanDistance(e, p.ClusterCentroid[idCluster]);
            double aux = 0;
            foreach (string str in p.ClusterCentroid.Keys)
            {
                if (str != idCluster)
                {
                    aux = Set.EuclideanDistance(e, p.ClusterCentroid[str]);
                    if (aux < outDist)
                        outDist = aux;
                }
            }
            return (outDist - inDist) / Math.Max(inDist, outDist);


        }
    }
}
