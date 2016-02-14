using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Graphics;
using ClusterEnsemble.Proximities;

namespace ClusterEnsemble.ClusterEnsemble
{
    #region CoAssociation
    public class SimpleCoAssociation : CoAssociation
    {
        [In(new string[] { "SingleLink",
                           "CompleteLink", 
                           "GroupAverage",
                           "WeightedAverage",
                           "MedianLink",
                           "CentroidLink",
                           "Flexible_LanceAndWilliams"},
            new   Type[] { typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter) })]
        public ClusterAlgorithm ClusterAlgorithm { get; set; }

        public SimpleCoAssociation() : base() { Name = "Simple CoAssociation"; }
        public SimpleCoAssociation(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "Simple CoAssociation"; }

        public override Structuring BuildStructuring()
        {
            if (Structurings == null || Set == null || ClusterAlgorithm == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Simple CoAssociation algorithm...", true);

            }

            ClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
            ClusterAlgorithm.ClustersCount = ClustersCount;
            ClusterAlgorithm.Set = Set;
            ClusterAlgorithm.Proximity = new CoAssociationMatrixDiss(Set, Structurings, Name);

            return ClusterAlgorithm.BuildStructuring();
        }



    }

    class CoAssociationMatrixDiss : Dissimilarity
    {
        double[,] matrix;
        string AlgorithmName { get; set; }
        public CoAssociationMatrixDiss(Set set, List<Structuring> estr, string aAlgorithmName)
        {
            AttributesToCalculateProximity = set.Attributes.Values;
            AdmissibleElementType = ElementType.Mixt;
            Name = "CoAssociationMatrixDiss";

            matrix = GetMatrix(set, estr);

            AlgorithmName = aAlgorithmName;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            return matrix[ei.Index, ej.Index];
        }

        double[,] GetMatrix(Set set, List<Structuring> estr)
        {

            if (estr == null || set == null)
                throw new NullReferenceException();
            int elementsCount = set.ElementsCount;

            double[,] matrix = new double[elementsCount, elementsCount];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i, i] = 0;
                for (int j = i + 1; j < matrix.GetLength(1); j++)
                {
                    double matched = 0;
                    foreach (var est in estr)
                        if (est.BeSameCluster(set[i], set[j]))
                            matched++;

                    double val = matched / estr.Count;

                    matrix[i, j] = 1 - val;
                    matrix[j, i] = 1 - val;
                }
            }

            return matrix;
        }

    }
    #endregion

    #region Probability Association

    public class ProbabilityCoAssociation : CoAssociation
    {
        public ProbabilityCoAssociation(Set set, List<Structuring> estr)
            : base(set, estr) { Name = "Probability CoAssociation"; }
        public ProbabilityCoAssociation() : base() { Name = "Probability CoAssociation"; }

        [In(new string[] { "SingleLink",
                           "CompleteLink", 
                           "GroupAverage",
                           "WeightedAverage",
                           "MedianLink",
                           "CentroidLink",
                           "Flexible_LanceAndWilliams"},
            new Type[] { typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter) })]
        public ClusterAlgorithm ClusterAlgorithm { get; set; }

        public override Structuring BuildStructuring()
        {
            if (Structurings == null || Set == null || ClusterAlgorithm == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Probability CoAssociation algorithm...", true);

            }

            ClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
            ClusterAlgorithm.Set = Set;
            ClusterAlgorithm.Proximity = new ProbabilityCoAssociationMatrixDiss(Set, Structurings);
            ClusterAlgorithm.ClustersCount = ClustersCount;

            return ClusterAlgorithm.BuildStructuring();
        }


    }

    class ProbabilityCoAssociationMatrixDiss : Dissimilarity
    {
        double[,] matrix;
        public ProbabilityCoAssociationMatrixDiss(Set set, List<Structuring> estr)
        {
            AttributesToCalculateProximity = set.Attributes.Values;
            AdmissibleElementType = ElementType.Mixt;
            Name = "ProbabilityCoAssociationMatrixDiss";
            matrix = GetMatrix(set, estr);
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            return matrix[ei.Index, ej.Index];
        }

        double[,] GetMatrix(Set set, List<Structuring> estr)
        {
            if (estr == null || set == null)
                throw new NullReferenceException();

            int elementsCount = set.ElementsCount;
            double[,] matrix = new double[elementsCount, elementsCount];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i, i] = 0;
                for (int j = i + 1; j < matrix.GetLength(1); j++)
                {
                    double sum = 0;
                    foreach (var est in estr)
                        if (est.BeSameCluster(set[i], set[j]))
                            sum += 1 / (1 + Math.Pow(est.GetCountElementsCluster(set[i]), (1.0 / set.Attributes.ValuesCount)));

                    double val = sum / estr.Count;
                    matrix[i, j] = 1 - val;
                    matrix[j, i] = 1 - val;
                }
            }

            return matrix;
        }
    }

    #endregion

    #region Weighted Association
    public class WeightedCoAssociation : CoAssociation
    {
        public WeightedCoAssociation(Set set, List<Structuring> estr)
            : base(set, estr) { Name = "Weighted CoAssociation"; }
        public WeightedCoAssociation() : base() { Name = "Weighted CoAssociation"; }

        [In(new string[] { "SingleLink",
                           "CompleteLink", 
                           "GroupAverage",
                           "WeightedAverage",
                           "MedianLink",
                           "CentroidLink",
                           "Flexible_LanceAndWilliams"},
            new Type[] { typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter), 
                           typeof(ClusterAlgorithmConverter) })]
        public ClusterAlgorithm ClusterAlgorithm { get; set; }

        public override Structuring BuildStructuring()
        {
            if (Structurings == null || Set == null || ClusterAlgorithm == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Weighted CoAssociation algorithm...", true);

            }

            ClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
            ClusterAlgorithm.Set = Set;
            ClusterAlgorithm.Proximity = new WeightedCoAssociationMatrixDiss(Set, Structurings);
            ClusterAlgorithm.ClustersCount = ClustersCount;

            return ClusterAlgorithm.BuildStructuring();
        }
    }

    class WeightedCoAssociationMatrixDiss : Dissimilarity
    {
        double[,] matrix;
        public WeightedCoAssociationMatrixDiss(Set set, List<Structuring> estr)
        {
            AttributesToCalculateProximity = set.Attributes.Values;
            AdmissibleElementType = ElementType.Mixt;
            Name = "WeightedCoAssociationMatrixDiss";
            matrix = GetMatrix(set, estr);
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            return matrix[ei.Index, ej.Index];
        }

        double[,] GetMatrix(Set set, List<Structuring> estr)
        {
            if (estr == null || set == null)
                throw new NullReferenceException();

            int elementsCount = set.ElementsCount;
            double[,] matrix = new double[elementsCount, elementsCount];

            double aux = 0;
            double[] den = new double[estr.Count];

            double min = double.MaxValue;
            double max1 = double.MinValue;
            for (int k = 0; k < estr.Count; k++)
            {
                min = double.MaxValue;
                max1 = double.MinValue;
                for (int i = 0; i < set.ElementsCount; i++)
                {
                    for (int j = i + 1; j < set.ElementsCount; j++)
                    {
                        aux = estr[k].Proximity.CalculateProximity(set[i], set[j]);
                        if (aux > max1)
                            max1 = aux;
                        if (aux < min)
                            min = aux;
                    }
                }
                den[k] = max1 - min;
            }

            //Codigo Sandro
            double p_max = 0;
            double c_max = 0;
            for (int i = 0; i < estr.Count; i++)
            {
                if (estr[i].ClustersCount > p_max)
                    p_max = estr[i].ClustersCount;
                foreach (Cluster clus in estr[i].Clusters.Values)
                {
                    if (clus.ElementsCount > c_max)
                        c_max = clus.ElementsCount;
                }
            }

            double max = double.MinValue;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                //matrix[i, i] = 0;
                for (int j = i; j < matrix.GetLength(1); j++)
                {
                    double sum = 0;
                    for (int k = 0; k < estr.Count; k++)
                    {
                        if (estr[k].BeSameCluster(set[i], set[j]))
                        {
                            double diss = estr[k].Proximity.CalculateProximity(set[i], set[j]);
                            if (diss != 0)
                                sum += ((double)(1 + estr[k].ClustersCount / p_max) * (2 - (double)(estr[k].GetCluster(set[i])[0].ElementsCount / c_max))) * (2 - (diss / den[k]));
                        }
                    }

                    double val = sum / estr.Count;
                    if (val > max)
                        max = val;

                    matrix[i, j] = val;
                    matrix[j, i] = val;
                }
            }

            if (max != 0)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = i + 1; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = 1 - (matrix[i, j] / max);
                        matrix[j, i] = 1 - (matrix[j, i] / max);
                    }
                }
            }
            else
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = i + 1; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = 0;
                        matrix[j, i] = 0;
                    }
                }
            }

            return matrix;
        }
    }
    #endregion
}
