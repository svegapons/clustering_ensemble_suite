using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Reflection;
using ClusterEnsemble.Clusters;

namespace ClusteringEnsembleSuite.Code.DataStructures
{

    public class ListClusterTree : List<Tree>
    {
        public ListClusterTree()
        {
            UpdateRoot();
        }

        public void UpdateRoot()
        {
            ReflectionTools _rct = new ReflectionTools();

            Tree _root = _rct.GetClusterAlgorithms();

            Add(_root);
        }
    }

    public class ListClusterEnsembleTree : List<Tree>
    {
        public ListClusterEnsembleTree()
        {
            UpdateRoot();
        }
        public void UpdateRoot()
        {
            ReflectionTools _rct = new ReflectionTools();

            Tree _root = _rct.GetClusterEnsembleAlgorithms();

            Add(_root);
        }
    }

    public class ListMeasureTree : List<Tree>
    {
        public ListMeasureTree()
        {
            UpdateRoot();
        }
        public void UpdateRoot()
        {
            ReflectionTools _rct = new ReflectionTools();

            Tree _root = _rct.GetMeasures();

            Add(_root);
        }
    }

}
