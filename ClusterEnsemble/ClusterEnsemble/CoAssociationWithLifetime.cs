using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.ClusterEnsemble
{
    #region CoAssociation
    public class SimpleCoAssociationLifetime : CoAssociationWithLifetime
    {
        [In(new string[] { "SingleLinkLifetime",
                           "CompleteLinkLifetime", 
                           "GroupAverageLifetime",
                           "WeightedAverageLifetime",
                           "MedianLinkLifetime",
                           "CentroidLinkLifetime"},
            new Type[] { typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter)})]
        public ClusterAlgorithm ClusterAlgorithm { get; set; }

        public SimpleCoAssociationLifetime() : base() { Name = "Simple CoAssociation Lifetime"; }
        public SimpleCoAssociationLifetime(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "Simple CoAssociation Lifetime"; }

        public override Structuring BuildStructuring()
        {
            if (Structurings == null || Set == null || ClusterAlgorithm == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Simple CoAssociation algorithm with Lifetime...", true);

            }

            ClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
            ClusterAlgorithm.Set = Set;
            ClusterAlgorithm.Proximity = new CoAssociationMatrixDiss(Set, Structurings, Name);

            return ClusterAlgorithm.BuildStructuring();
        }



    }

  
    #endregion

    #region Probability Association

    public class ProbabilityCoAssociationLifetime : CoAssociationWithLifetime
    {
        public ProbabilityCoAssociationLifetime(Set set, List<Structuring> estr)
            : base(set, estr) { Name = "Probability CoAssociation Lifetime"; }
        public ProbabilityCoAssociationLifetime() : base() { Name = "Probability CoAssociation Lifetime"; }

        [In(new string[] { "SingleLinkLifetime",
                           "CompleteLinkLifetime", 
                           "GroupAverageLifetime",
                           "WeightedAverageLifetime",
                           "MedianLinkLifetime",
                           "CentroidLinkLifetime"},
            new Type[] { typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter)})]
        public ClusterAlgorithm ClusterAlgorithm { get; set; }

        public override Structuring BuildStructuring()
        {
            if (Set == null || Structurings == null || ClusterAlgorithm == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Probability CoAssociation algorithm with Lifetime...", true);

            }

            ClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
            ClusterAlgorithm.Set = Set;
            ClusterAlgorithm.Proximity = new ProbabilityCoAssociationMatrixDiss(Set, Structurings);
            

            return ClusterAlgorithm.BuildStructuring();
        }


    }

    

    #endregion

    #region Weighted Association
    public class WeightedCoAssociationLifetime : CoAssociationWithLifetime
    {
        public WeightedCoAssociationLifetime(Set set, List<Structuring> estr)
            : base(set, estr) { Name = "Weighted CoAssociation Lifetime"; }
        public WeightedCoAssociationLifetime() : base() { Name = "Weighted CoAssociation Lifetime"; }

        [In(new string[] { "SingleLinkLifetime",
                           "CompleteLinkLifetime", 
                           "GroupAverageLifetime",
                           "WeightedAverageLifetime",
                           "MedianLinkLifetime",
                           "CentroidLinkLifetime"},
            new Type[] { typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter)})]
        public ClusterAlgorithm ClusterAlgorithm { get; set; }

        public override Structuring BuildStructuring()
        {
            if (Set == null || Structurings == null || ClusterAlgorithm == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Weighted CoAssociation algorithm with Lifetime...", true);

            }

            ClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
            ClusterAlgorithm.Set = Set;
            ClusterAlgorithm.Proximity = new WeightedCoAssociationMatrixDiss(Set, Structurings);            

            return ClusterAlgorithm.BuildStructuring();
        }
    }

    
    #endregion
}
