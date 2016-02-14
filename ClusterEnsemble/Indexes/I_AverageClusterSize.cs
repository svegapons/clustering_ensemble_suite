using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterEnsemble
{
    public class I_AverageClusterSize:Index
    {
        public I_AverageClusterSize() : base() { }

        public override double Process(Partition p)
        {
            double result = 0;
            foreach (string str in p.Clusters.Keys)
            {
                result += p.Clusters[str].Count;
            }
            return result / p.ClusterCount;
        }
    }
}
