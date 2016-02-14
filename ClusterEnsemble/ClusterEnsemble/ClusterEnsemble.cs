using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Graphics;
using ClusterEnsemble.ReductionFunctions;
using System.Diagnostics;

namespace ClusterEnsemble.ClusterEnsemble
{
    public class CusterEnsemble
    {
        public ConsensusFunction ConsensusFunction { get; set; }
        public Structuring Structuring { get; set; }


        public Structuring BuildStructuring()
        {
            if (ConsensusFunction == null || ConsensusFunction.Set == null || ConsensusFunction.Structurings == null)
                throw new NullReferenceException();

            Structuring result = ConsensusFunction.BuildStructuring();
            Structuring = result;

            return result;
        }

    }

    public abstract class ConsensusFunction : IName, INeedProgressBar
    {
        public Set Set { get; set; }
        public List<Structuring> Structurings { get; set; }
        public TimeSpan Time { get; set; }
        public int ReductionElementCount { get; set; }


        public List<Structuring> RealStructurings { get; set; }
        public Set RealSet { get; set; }

        [Out]
        public Structuring Structuring { get; set; }

        [Out]
        public string Output { get { return "Ensemble algorithm"; } set { } }

        public int StructuringsCount
        {
            get
            {
                if (Structurings == null)
                    throw new NullReferenceException();
                return Structurings.Count;
            }
        }

        public ConsensusFunction() { Name = "Consensus Function"; }
        public ConsensusFunction(Set set, List<Structuring> estr)
        {
            if (set == null || estr == null || estr.Count == 0)
                throw new ArgumentException("");
            Name = "Consensus Function";
            this.Structurings = estr;
            this.Set = set;
        }

        public Structuring BuildStructuringWithReduction()
        {
            try
            {
                if (ReductionFunction == null)
                    throw new ArgumentNullException();

                List<Structuring> _tempStructurings = Structurings;
                Set _tempSet = Set;
                Set _newSet = null;
                
                Stopwatch _Stopwatch = new Stopwatch();
                _Stopwatch.Start();
                List<Structuring> _reduction = ReductionFunction.GetReduction(Set, Structurings, out _newSet);
                
                Set = _newSet;
                Structurings = _reduction;

                RealSet = _tempSet;
                RealStructurings = _tempStructurings;

                ReductionElementCount = _newSet.ElementsCount;
                Structuring _temp = this.BuildStructuring();
                Structuring = ReductionFunction.TurnOffReduction(Set, _temp);
                _Stopwatch.Stop();
                Time = _Stopwatch.Elapsed;

                Structurings = _tempStructurings;
                Set = _tempSet;

                RealStructurings = null;
                RealSet = null;

                return Structuring;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public abstract Structuring BuildStructuring();

        [In(new string[] { "NoReduction",
                           "FragmentClusterReduction",
                           "OurReduction"},
            new Type[] { typeof(ReductionFunctionConverter), 
                         typeof(ReductionFunctionConverter),
                         typeof(ReductionFunctionConverter)})]
        public ReductionFunction ReductionFunction { get; set; }

        #region IName Members

        public string Name { get; set; }

        #endregion

        #region INeedProgressBar Members

        public IContainerProgressBar IContainerProgressBar { get; set; }

        #endregion
    }
    public abstract class CoAssociation : ConsensusFunction
    {
        public CoAssociation() { Name = "CoAssociation"; }
        public CoAssociation(Set set, List<Structuring> estr) : base(set, estr) { Name = "CoAssociation"; }

        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }

    }
    public abstract class CoAssociationWithLifetime : ConsensusFunction
    {
        public CoAssociationWithLifetime() { Name = "CoAssociation with Lifetime"; }
        public CoAssociationWithLifetime(Set set, List<Structuring> estr) : base(set, estr) { Name = "CoAssociation with Lifetime"; }

    }
    public abstract class Graphs : ConsensusFunction
    {
        public Graphs() { Name = "Graphs"; }
        public Graphs(Set set, List<Structuring> estr) : base(set, estr) { Name = "Graphs"; }

    }
    public abstract class Mirkin : ConsensusFunction
    {
        [In(new string[] { "MirkinDistance",
                            "LatticeDistance"},
           new Type[] { typeof(GenericDistancesConverter),
                         typeof(GenericDistancesConverter)})]
        public GenericDistances GenericDistances { get; set; }

        public Mirkin() { Name = "Mirkin Distance Based Methods"; }
        public Mirkin(Set set, List<Structuring> estr) : base(set, estr) { Name = "Mirkin Distance Based Methods"; }

    }
    public abstract class InformationTheoretic : ConsensusFunction
    {
        public InformationTheoretic() { Name = "Information Theoretic"; }
        public InformationTheoretic(Set set, List<Structuring> estr) : base(set, estr) { Name = "Information Theoretic"; }
    }
    public abstract class Genetic : ConsensusFunction
    {
        public Genetic() { Name = "Genetic"; }
        public Genetic(Set set, List<Structuring> estr) : base(set, estr) { Name = "Genetic"; }
    }
}    
