using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble.Graphics;
using ClusterEnsemble.Proximities;
using System.Diagnostics;

namespace ClusterEnsemble.Clusters
{
    public abstract class ClusterAlgorithm : IName,INeedProgressBar
    {
        public Set Set { get; set; }// not allow null
        public Proximity Proximity { get; set; }
        public ProximityType ProximityType { get; set; }
        public TimeSpan Time { get; set; }

        public ClusterAlgorithm() { }
        public ClusterAlgorithm(Set set, Proximity diss)
        {
            if (set == null || diss == null)
                throw new ArgumentNullException("Parametro Incorrecto en el constructor de la clase ClusterAlgorithm");
            this.Set = set;
            this.Proximity = diss;
        }

        public abstract Structuring BuildStructuring();
        public Structuring BuildStructuringWithTime()
        {
            try
            {
                Stopwatch _Stopwatch = new Stopwatch();
                _Stopwatch.Start();
                Structuring _temp = this.BuildStructuring();
                _Stopwatch.Stop();
                Time = _Stopwatch.Elapsed;

                return _temp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Out]
        public Structuring Structuring { get; set; }

        [Out]
        public string Output { get { return "Cluster algorithm"; } set { } }

        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }


        #region IName Members

        public string Name{get;set;}

        #endregion

        #region INeedProgressBar Members

        public IContainerProgressBar IContainerProgressBar { get; set; }

        #endregion
    }

    public abstract class NonHierarchical : ClusterAlgorithm
    {
        public NonHierarchical(Set set, Proximity diss)
            : base(set, diss)
        { Name = "Non Hierarchical"; }
        public NonHierarchical() : base() { Name = "Non Hierarchical"; }
    }

    public abstract class Hierarchical : ClusterAlgorithm
    {
        public Hierarchical(Set set, Proximity diss)
            : base(set, diss)
        { Name = "Hierarchical"; }
        public Hierarchical() : base() { Name = "Hierarchical"; }
    }

    // Hierarchical dividirlo en Aglomerativos y Divisivos 
    public abstract class Agglomerative : Hierarchical 
    {
        public Agglomerative(Set set, Proximity diss)
            : base(set, diss)
        { Name = "Agglomerative"; }
        public Agglomerative() : base() { Name = "Agglomerative"; }
    }
    public abstract class AgglomerativeWithLifetime : Hierarchical
    {
        public AgglomerativeWithLifetime(Set set, Proximity diss)
            : base(set, diss)
        { Name = "Agglomerative with Lifetime"; }
        public AgglomerativeWithLifetime() : base() { Name = "Agglomerative with Lifetime"; }

        public new int ClustersCount { get; set; }
    }
    public abstract class Divisive : Hierarchical
    {
        public Divisive(Set set, Proximity diss)
            : base(set, diss)
        { Name = "Divisive"; }
        public Divisive() : base() { Name = "Divisive"; }
    }
    public abstract class DivisiveWithLifetime : Hierarchical
    {
        public DivisiveWithLifetime(Set set, Proximity diss)
            : base(set, diss)
        { Name = "Divisive with Lifetime"; }
        public DivisiveWithLifetime() : base() { Name = "Divisive with Lifetime"; }

        public new int ClustersCount { get; set; }
    }

}