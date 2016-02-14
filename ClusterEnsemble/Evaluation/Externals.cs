using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Added
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.Evaluation
{
    public class MeasureF : External
    {
        [In(typeof(DoubleConverter))]
        public double Beta { get; set; }

        public MeasureF()
        {
            this.Name = "F Measure [0,1] max";
        }

        //Separar similares elementos a veces es peor que poner en un mismo cluster 2 elementos disimilares
        //Esta medida lo que hace es penalizar más los FN con Beta > 1 que las FP

        public override double EvaluatePartition()
        {
            if (RealPartition == null || Structuring == null)
                throw new Exception("Realpartition o Structuring son null en la clase MeasureF.");
            double _result = 0;

            //Penaliza mas los FN que los FP
            int _TP = 0, _TN = 0, _FP = 0, _FN = 0;

            for (int i = 0; i < this.Set.ElementsCount; i++)
                for (int j = i + 1; j < this.Set.ElementsCount; j++)
                {
                    bool sameClass = this.RealPartition.BeSameCluster(this.Set[i], this.Set[j]);
                    bool sameCluster = this.Structuring.BeSameCluster(this.Set[i], this.Set[j]);

                    if (sameClass && sameCluster)
                        _TP++;
                    if (!sameClass && !sameCluster)
                        _TN++;
                    if (!sameClass && sameCluster)
                        _FP++;
                    if (sameClass && !sameCluster)
                        _FN++;
                }
            double _P = ((double)_TP) / (_TP + _FP);
            double _R = ((double)_TP) / (_TP + _FN);

            //Significa que todos los cluster tienen un solo elemento tanto en la RealPartition como en Structuring, ya que si hay al menos un cluster
            //que tenga mas de un elemento entonces se cumple o sameClas o sameCluster
            if (_TN == (Set.ElementsCount * (Set.ElementsCount - 1)) / 2)
                _result = 1;
            else if (_P == 0 || double.IsNaN(_P) || _R == 0 || double.IsNaN(_R))
                _result = 0;
            else
                _result = ((Beta * Beta + 1) * _P * _R) / (Beta * Beta * _P + _R);

            return _result;
        }
    }
    public class MeasurePurity : External
    {
        public MeasurePurity()
        {
            this.Name = "Purity [0,1] max";
        }

        public override double EvaluatePartition()
        {
            if (RealPartition == null || Structuring == null)
                throw new Exception("Realpartition o Structuring son null en la clase MeasurePurity.");
            double _result = 0;

            Dictionary<string, int> _classs = new Dictionary<string, int>();
            Dictionary<string, int> _clusters = new Dictionary<string, int>();

            int _count = 0;
            //Mapeando el nombre de la clase a un entero
            foreach (var s in this.RealPartition.Clusters.Keys)
                _classs.Add(s, _count++);

            //Mapeando el nombre del cluster a un entero
            _count = 0;
            foreach (var s in this.Structuring.Clusters.Keys)
                _clusters.Add(s, _count++);

            int[,] _ClassCluster = new int[_classs.Count, _clusters.Count];

            foreach (var c in this.RealPartition.Clusters.Values)
                foreach (var e in c.Elements)
                    if (!(this.Structuring.HaveUnassignedElements() && this.Structuring.IsUnassigned(e)))
                    _ClassCluster[_classs[c.Name], _clusters[this.Structuring.GetCluster(e)[0].Name]]++;

            int _goodAssigned = 0;
            for (int j = 0; j < _ClassCluster.GetLength(1); j++)
            {
                int _max = int.MinValue;
                //int index = -1;
                for (int i = 0; i < _ClassCluster.GetLength(0); i++)
                    if (_ClassCluster[i, j] > _max)
                    {
                        _max = _ClassCluster[i, j];
                        //index = i;
                    }
                _goodAssigned += _max;
            }

            int _UnassignedCount = (RealPartition.HaveUnassignedElements()) ? RealPartition.UnassignedElements.Count : 0;
            _result = _goodAssigned / (double)(this.Set.ElementsCount - _UnassignedCount);

            return _result;
        }
    }
    public class MeasureRandIndex : External
    {
        public MeasureRandIndex()
        {
            this.Name = "Rand Index [0,1] max";
        }

        //              |   sameCluster   |   diffCluster |
        //______________|_________________|_______________|
        // sameClass    |      TP         |      FN       |
        // diffClass    |      FP         |      TN       |
        //______________|_________________|_______________|
        
        public override double EvaluatePartition()
        {
            if (RealPartition == null || Structuring == null)
                throw new Exception("Realpartition o Structuring son null en la clase MeasureRandIndex.");
            double _result = 0;
            
            int _TP = 0, _TN = 0, _FP = 0, _FN = 0;

            for (int i = 0; i < this.Set.ElementsCount; i++)
                for (int j = i + 1; j < this.Set.ElementsCount; j++)
                {
                    bool sameClass = this.RealPartition.BeSameCluster(this.Set[i], this.Set[j]);
                    bool sameCluster = this.Structuring.BeSameCluster(this.Set[i], this.Set[j]);

                    if (sameClass && sameCluster)
                        _TP++;
                    if (!sameClass && !sameCluster)
                        _TN++;
                    if (!sameClass && sameCluster)
                        _FP++;
                    if (sameClass && !sameCluster)
                        _FN++;
                }
            int _TP_TN = _TP + _TN;
            _result = ((double)(_TP_TN)) / (_TP_TN + _FP + _FN);

            return _result;
        }
    }
    public class MeasureMutualInformation : External
    {
        public MeasureMutualInformation()
        {
            this.Name = "Normalized Mutual Information [0, 1] max";
        }
        public override double EvaluatePartition()
        {
            Structuring _A = Structuring;
            Structuring _B = RealPartition;

            double _result = MI(_A, _B, Set.ElementsCount) / ((Entropy(_A) + Entropy(_B)) / 2);

            return _result;
        }

        private double MI(Structuring aA, Structuring aB, int aElementsCount)
        {
            double _result = 0;
            foreach (var _cA in aA.Clusters.Values)
            {
                foreach (var _cB in aB.Clusters.Values)
                {
                    int _nab = CalculateIntersection(_cA, _cB);
                    int _na = _cA.ElementsCount;
                    int _nb = _cB.ElementsCount;
                    int _n = aElementsCount;

                    double _probability = _nab / ((double)_n);

                    if (_probability != 0)
                        _result += (_probability) * Math.Log10((_nab * _n) / ((double)_na * _nb));
                }
            }
            return _result;
        }
        private int CalculateIntersection(Cluster acA, Cluster acB)
        {
            int _result = 0;
            foreach (Element _e in acA.Elements)
                if (acB.Elements.Contains(_e))
                    _result++;
            return _result;
        }
        private double Entropy(Structuring aStructuring)
        {
            double _result = 0;
            double _elementsCount = aStructuring.Elements.Count;//(aStructuring.HaveUnassignedElements())?aStructuring.Elements.Count-aStructuring.UnassignedElements.Count:aStructuring.Elements.Count;
            foreach (Cluster _cluster in aStructuring.Clusters.Values)
            {
                //esta probabilidad siempre es > 0 ya que cada cluster tiene al menos un elemento
                double _probability = _cluster.ElementsCount / ((double)_elementsCount);
                double _log = Math.Log10(_probability);

                _result += _probability * _log;
            }
            return -(_result);
        }
    }

    class ErrorRate: External
    {
        public ErrorRate()
        {
            Name = "Error Rate Percent [0,100] min";
        }

        public override double EvaluatePartition()
        {
            if (RealPartition == null || Structuring == null)
                throw new Exception("Realpartition o Structuring son null en la clase MeasurePurity.");
            double _badAssigned = 0;

            Dictionary<string, int> _classs = new Dictionary<string, int>();
            Dictionary<string, int> _clusters = new Dictionary<string, int>();

            int _count = 0;
            //Mapeando el nombre de la clase a un entero
            foreach (var s in this.RealPartition.Clusters.Keys)
                _classs.Add(s, _count++);

            //Mapeando el nombre del cluster a un entero
            _count = 0;
            foreach (var s in this.Structuring.Clusters.Keys)
                _clusters.Add(s, _count++);

            int[,] _ClassCluster = new int[_classs.Count, _clusters.Count];

            foreach (var c in this.RealPartition.Clusters.Values)
                foreach (var e in c.Elements)
                    //if (!(this.Structuring.HaveUnassignedElements() && this.Structuring.IsUnassigned(e)))
                    _ClassCluster[_classs[c.Name], _clusters[this.Structuring.GetCluster(e)[0].Name]]++;

            int _goodAssigned = 0;
            for (int j = 0; j < _ClassCluster.GetLength(1); j++)
            {
                int _max = int.MinValue;
                //int index = -1;
                for (int i = 0; i < _ClassCluster.GetLength(0); i++)
                    if (_ClassCluster[i, j] > _max)
                    {
                        _max = _ClassCluster[i, j];
                        //index = i;
                    }
                _goodAssigned += _max;
            }

            _badAssigned = Set.ElementsCount - _goodAssigned;

            return _badAssigned * 100 / Set.ElementsCount;
        }
    }
}
