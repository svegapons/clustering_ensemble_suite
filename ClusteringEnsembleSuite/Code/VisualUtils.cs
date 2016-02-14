using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using ClusterEnsemble.Reflection;
using ClusterEnsemble;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.ClusterEnsemble;
using ClusterEnsemble.Proximities;

namespace ClusteringEnsembleSuite.Code
{
    class VisualUtils
    {
        public static List<string> ExpandsToMax(List<string> astrings)
        {
            List<string> result = new List<string>();
            int _maxlength = int.MinValue;
            _maxlength = astrings.Max(s=>s.Length);

            astrings.ForEach(s =>result.Add(s.PadLeft(_maxlength, ' ')));

            return result;
        }
        /// <summary>
        /// Le pone las propiedades que se don de entradas, pero que son las mismas para todos los algoritmos
        /// </summary>
        /// <param name="_rct"> Clase que le pone cierta propiedad a un algoritmo de cluster </param>
        /// <param name="_ClusterAlg"> Clase que es el algoritmo de cluster al que se la van a poner las propiedades</param>
        /// <param name="Tree"> El nodo que representa el Algoritmo de Cluster que tiene todas las propiedades, al igual que las de In y Out </param>
        public static bool SetGlobalInProperties(ReflectionTools arct, ClusterAlgorithm aClusterAlg, Tree Tree, out string aError, bool aRandAttr)
        {

            ClusterEnsemble.DataStructures.Property _Set = Tree.Value.Properties.First(p => p.Name == "Set");
            _Set.Value = Enviroment.Set;

            if (!aRandAttr)
                Enviroment.Proximity.AttributesToCalculateProximity = Enviroment.AttributesToCalculateProximity;
            else 
            {
                //el ultimo attr debe estar unchecked
                List<ClusterEnsemble.Attribute> _attr = Enviroment.AttributesToCalculateProximity;
                List<ClusterEnsemble.Attribute> _new = new List<ClusterEnsemble.Attribute>();
                Random _rnd = new Random(Environment.TickCount);
                for (int i = 0; i < _attr.Count; i++)
                {
                    double _num = _rnd.NextDouble();
                    if(_num>.5)
                        _new.Add(_attr[i]);
                }

                if (_new.Count == 0)
                {
                    _new.Add(_attr[_rnd.Next(0,_attr.Count)]);
                }
                
                Enviroment.Proximity.AttributesToCalculateProximity = _new;
            }
            ClusterEnsemble.DataStructures.Property _Proximity = Tree.Value.Properties.First(p => p.Name == "Proximity");
            _Proximity.Value = Enviroment.Proximity;

            aError = "";
            switch (aClusterAlg.ProximityType)
            {
                case ProximityType.Similarity:
                    if (!(Enviroment.Proximity is Similarity))
                    {
                        aError = "The current algorithm work only with Similarity";
                        return false;
                    }
                    break;
                case ProximityType.Dissimilarity:
                    if (!(Enviroment.Proximity is Dissimilarity))
                    {
                        aError = "The current algorithm work only with Dissimilarity";
                        return false;
                    }
                    break;
                case ProximityType.Both:
                    break;

            }

            arct.SetProperty(Tree.Value.FullName, aClusterAlg, _Set);
            arct.SetProperty(Tree.Value.FullName, aClusterAlg, _Proximity);
            return true;
        }

        public static void SetGlobalInProperties(ReflectionTools arct, ConsensusFunction aEnsembleAlg, Tree Tree, List<Structuring> aStrcs)
        {
            ClusterEnsemble.DataStructures.Property _Set = Tree.Value.Properties.First(p => p.Name == "Set");
            _Set.Value = Enviroment.Set;

            ClusterEnsemble.DataStructures.Property _Structurings = Tree.Value.Properties.First(p => p.Name == "Structurings");
            _Structurings.Value = aStrcs;

            arct.SetProperty(Tree.Value.FullName, aEnsembleAlg, _Set);
            arct.SetProperty(Tree.Value.FullName, aEnsembleAlg, _Structurings);
        }
    }
}
