using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Proximities;

namespace ClusterEnsemble.Clusters
{
    public class SingleLinkLifetime : AgglomerativeWithLifetime
    {
        public SingleLinkLifetime(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Single Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }
        public SingleLinkLifetime()
            : base()
        {
            Name = "Single Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN_StopRules sahn = new SAHN_StopRules(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;
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

    public class CompleteLinkLifetime : AgglomerativeWithLifetime
    {

        public CompleteLinkLifetime(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Complete Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }
        public CompleteLinkLifetime()
            : base()
        {
            Name = "Complete Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN_StopRules sahn = new SAHN_StopRules(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;            
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

    public class GroupAverageLifetime : AgglomerativeWithLifetime
    {
        public GroupAverageLifetime(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Group Average Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public GroupAverageLifetime()
            : base()
        {
            Name = "Group Average Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN_StopRules sahn = new SAHN_StopRules(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;            
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

    public class WeightedAverageLifetime : AgglomerativeWithLifetime
    {

        public WeightedAverageLifetime(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Weighted Average Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }
        public WeightedAverageLifetime()
            : base()
        {
            Name = "Weighted Average Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN_StopRules sahn = new SAHN_StopRules(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;            
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

    public class MedianLinkLifetime : AgglomerativeWithLifetime
    {

        public MedianLinkLifetime(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Median Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }
        public MedianLinkLifetime()
            : base()
        {
            Name = "Median Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN_StopRules sahn = new SAHN_StopRules(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;            
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

    public class CentroidLinkLifetime : AgglomerativeWithLifetime
    {
        public CentroidLinkLifetime(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Centroid Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }
        public CentroidLinkLifetime()
            : base()
        {
            Name = "Centroid Link Lifetime";
            ProximityType = ProximityType.Dissimilarity;
        }

        public override Structuring BuildStructuring()
        {
            SAHN_StopRules sahn = new SAHN_StopRules(Set, Proximity);
            sahn.IContainerProgressBar = IContainerProgressBar;
            sahn.UpdateAlfaI = AlfaI;
            sahn.UpdateAlfaJ = AlfaJ;
            sahn.UpdateBeta = Beta;
            sahn.UpdateGamma = Gamma;            
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
}
