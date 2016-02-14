using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterEnsemble
{
    /// <summary>
    /// Mide la proporcion entre la menor distancia inter-cluster y la mayor distancia intracluster.
    /// </summary>
    public class I_Dunn:Index
    {
        public I_Dunn() : base() { }

        public override double Process(Partition p)
        {
            double minInter = double.MaxValue;
            double maxIntra = 0;
          //  double aux = 0;
            for (int i = 0; i < p.Set.Distances.Length; i++)
			{        
                foreach (double val in p.Set.Distances[i].Keys)
                {
                    if (val < minInter && !p.InTheSameCluster(p.Set.Elements[i], p.Set.Distances[i][val]))
                        minInter = val;
                    else if (val > maxIntra && p.InTheSameCluster(p.Set.Elements[i], p.Set.Distances[i][val]))
                        maxIntra = val;
                }
            }
            return  minInter/maxIntra ;
        }
    }
}
