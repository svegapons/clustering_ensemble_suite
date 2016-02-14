using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.Clusters
{
    public class SingleLink : Agglomerative
    {
        public SingleLink(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Single Link";
            ProximityType = ProximityType.Dissimilarity;
        }
        public SingleLink()
            : base()
        {
            Name = "Single Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }
        double AlfaI(double i, double j)
        {
            return 0.5;
        }
        double AlfaJ(double i, double j)
        {
            return 0.5;
        }
        double Beta(double i, double j)
        {
            return 0.0;
        }
        double Gamma(double i, double j)
        {
            return -0.5;
        }

    }

    public class CompleteLink : Agglomerative
    {

        public CompleteLink(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Complete Link";
            ProximityType = ProximityType.Dissimilarity;
        }
        public CompleteLink()
            : base()
        {
            Name = "Complete Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }
        double AlfaI(double i, double j)
        {
            return 0.5;
        }
        double AlfaJ(double i, double j)
        {
            return 0.5;
        }
        double Beta(double i, double j)
        {
            return 0.0;
        }
        double Gamma(double i, double j)
        {
            return 0.5;
        }

    }

    public class GroupAverage : Agglomerative
    {
        public GroupAverage(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Group Average Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public GroupAverage()
            : base()
        {
            Name = "Group Average Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }

        double AlfaI(double i, double j)
        {
            return i / (i + j);
        }
        double AlfaJ(double i, double j)
        {
            return j / (i + j);
        }
        double Beta(double i, double j)
        {
            return 0.0;
        }
        double Gamma(double i, double j)
        {
            return 0.0;
        }
    }

    public class WeightedAverage : Agglomerative
    {

        public WeightedAverage(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Weighted Average Link";
            ProximityType = ProximityType.Dissimilarity;
        }
        public WeightedAverage()
            : base()
        {
            Name = "Weighted Average Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }

        double AlfaI(double i, double j)
        {
            return 0.5;
        }
        double AlfaJ(double i, double j)
        {
            return 0.5;
        }
        double Beta(double i, double j)
        {
            return 0.0;
        }
        double Gamma(double i, double j)
        {
            return 0.0;
        }

    }

    public class MedianLink : Agglomerative
    {

        public MedianLink(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Median Link";
            ProximityType = ProximityType.Dissimilarity;
        }
        public MedianLink()
            : base()
        {
            Name = "Median Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }
        double AlfaI(double i, double j)
        {
            return 0.5;
        }
        double AlfaJ(double i, double j)
        {
            return 0.5;
        }
        double Beta(double i, double j)
        {
            return -0.25;
        }
        double Gamma(double i, double j)
        {
            return 0.0;
        }

    }

    public class CentroidLink : Agglomerative
    {
        public CentroidLink(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Centroid Link";
            ProximityType = ProximityType.Dissimilarity;
        }
        public CentroidLink()
            : base()
        {
            Name = "Centroid Link";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }
        double AlfaI(double i, double j)
        {
            return i / (i + j);
        }
        double AlfaJ(double i, double j)
        {
            return j / (i + j);
        }
        double Beta(double i, double j)
        {
            return (-i * j) / Math.Pow(i + j, 2);
        }
        double Gamma(double i, double j)
        {
            return 0.0;
        }
    }

    public class Flexible_LanceAndWilliams : Agglomerative
    {
        [In(typeof(DoubleLTOneConverter))]
        public double Beta { get; set; }

        public Flexible_LanceAndWilliams(Set set, Proximity diss)
            : base(set, diss)
        {
            Beta = 0.5;
            Name = "Flexible (Lance and Williams) ";
            ProximityType = ProximityType.Dissimilarity;
        }
        public Flexible_LanceAndWilliams()
            : base()
        {
            Beta = 0.5; 
            Name = "Flexible (Lance and Williams) ";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            if (!(Beta < 1))
                throw new ArgumentException("El parámetro Beta del Método Flexible_LanceAndWilliams deber ser menor estricto que 1.");

            SAHN sahn = new SAHN(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = BetaUpdate;
            sahn.UpdateGamma = Gamma;
            sahn.ClustersCount = ClustersCount;
            Structuring = sahn.BuildStructuring();
            return Structuring;
        }

        double AlfaI(double i, double j)
        {
            return (1 - Beta) / 2;
        }
        double AlfaJ(double i, double j)
        {
            return (1 - Beta) / 2;
        }
        double BetaUpdate(double i, double j)
        {
            return Beta;
        }
        double Gamma(double i, double j)
        {
            return 0.0;
        }
    }
}
