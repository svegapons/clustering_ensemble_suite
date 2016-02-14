using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.ClusterEnsemble
{
    public class HCE : Genetic
    {

        public HCE() : base() { Name = "HCE"; }
        public HCE(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "HCE"; }

        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        public override Structuring BuildStructuring()
        {
            try
            {
                int _current = 1;
                int _max = IterationsCount;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running HCE algorithm...", true);
                }

                int n = StructuringsCount * (StructuringsCount - 1) / 2;

                HeapArrayMax<Pair> _pairs = new HeapArrayMax<Pair>(n);
                for (int i = 0; i < StructuringsCount; i++)
                {
                    for (int j = i + 1; j < StructuringsCount; j++)
                    {
                        Pair _temp = Fitness(Structurings[i], Structurings[j]);                       

                        _pairs.Add(_temp);
                    }
                }

                for (int i = 0; i < IterationsCount; i++)
                {
                    Pair parents = _pairs.First;
                    _pairs.RemoveFirst();                    

                    Crossover(parents);

                    //calcular nuevo fitness//////////////***********//////////////
                    _pairs.Add(Fitness(parents.P1, parents.P2));

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.UpdateProgressBar(_current++, "Running HCE algorithm...", false);

                }

                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();
                
                return _pairs.First.P2;
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in HCE algorithm.");
                return null;
            }
        }

        private void Crossover(Pair parents)
        {
            parents.P1 = Utils.Clone((Structuring)parents.P1);
            parents.P2 = Utils.Clone((Structuring)parents.P2);

            Cluster new_c1 = new Cluster(parents.C1.Name);
            Cluster new_c2 = new Cluster(parents.C2.Name);

            List<Element> un_el_p1=new List<Element>();
            List<Element> un_el_p2=new List<Element>();

            foreach (Element item in parents.C1.Elements)
            {
                if (parents.C2.HaveElement(item))
                    new_c2.AddElement(item);

                //else if (!parents.P2.Clusters.Values.ToList().Exists(c => c.HaveElement(item)))
                //    new_c2.AddElement(item);
                //else
                //{
                //    //Si entra aki, el problema es que un elemento de C1, no pertenece a C2 (C2 es un de la P2), ni tampoco
                //    //pertenece a algun otro cluster de P2, por lo tanto ese elemento no esta en la particion P2, y P2 y P1
                //    //son particiones de un mismo conjunto. Este caso solamente se debe dar cuando P2 tiene elementos no asignados
                //    //y este elemento que estoy analizando puede estar no asignado, ahi en P2.
                //    throw new Exception("no puede entrara aki, siginificaria que cada particion tiene conjuntos distintos.");
                    
                //    string c_temp = parents.P2.Elements[item][0];
                //    parents.P2.Clusters[c_temp].Elements.Remove(item);
                //    parents.P2.Elements.Remove(item);
                //    un_el_p2.Add(item);
                //}       
                
            }            

            foreach (Element item in parents.C2.Elements)
            {
                if (parents.C1.HaveElement(item))
                    new_c1.AddElement(item);

                //else if (!parents.P1.Clusters.Values.ToList().Exists(c => c.HaveElement(item)))
                //    new_c1.AddElement(item);
                //else
                //{
                //    for (int i = 0; i < parents.P1.Clusters.Values.ToList().Count; i++)
                //    {
                //        Cluster _ccc = parents.P1.Clusters.Values.ToList()[i];
                //        for (int ii = 0; ii < _ccc.ElementsCount; ii++)
                //        {
                //            if (_ccc[ii].Index == item.Index)
                //            { 
                //            }
                //        }
                //    }
                //    //Si entra aki, el problema es que un elemento de C1, no pertenece a C2 (C2 es un de la P2), ni tampoco
                //    //pertenece a algun otro cluster de P2, por lo tanto ese elemento no esta en la particion P2, y P2 y P1
                //    //son particiones de un mismo conjunto. Este caso solamente se debe dar cuando P2 tiene elementos no asignados
                //    //y este elemento que estoy analizando puede estar no asignado, ahi en P2.
                //    throw new Exception("no puede entrara aki, siginificaria que cada particion tiene conjuntos distintos.");

                //    string c_temp = parents.P1.Elements[item][0];
                //    parents.P1.Clusters[c_temp].Elements.Remove(item);
                //    parents.P1.Elements.Remove(item);
                //    un_el_p1.Add(item);
                //}

            }

            foreach (var item in new_c1.Elements)
            {
                parents.P1.Elements[item] = new List<string>() { new_c1.Name };
            }

            foreach (var item in new_c2.Elements)
            {
                parents.P2.Elements[item] = new List<string>() { new_c2.Name };
            }

            parents.P1.Clusters[new_c1.Name] = new_c1;
            parents.P2.Clusters[new_c2.Name] = new_c2;

            if (parents.P1.UnassignedElements != null)
                parents.P1.UnassignedElements.AddRange(un_el_p1);
            else
                parents.P1.UnassignedElements = un_el_p1;

            if (parents.P2.UnassignedElements != null)
                parents.P2.UnassignedElements.AddRange(un_el_p2);
            else
                parents.P2.UnassignedElements = un_el_p2;

        }

        private Pair Fitness(Structuring s1, Structuring s2)
        {
            Pair p1 = FitnessOneWay(s1, s2);
            Pair p2 = FitnessOneWay(s2, s1);

            return new Pair() { Rank = p1.Rank + p2.Rank, C1 = p1.C1, C2 = p1.C2, P1 = s1, P2 = s2 };
        }

        Pair FitnessOneWay(Structuring s1, Structuring s2)
        {

            double result = 0;
            double best = double.MinValue;

            Cluster best_c1 = null;
            Cluster best_c2 = null;

            foreach (Cluster c1 in s1.Clusters.Values)
            {
                double max_overlap = double.MinValue;
                Cluster temp_c2 = null;
                foreach (Cluster c2 in s2.Clusters.Values)
                {
                    double temp = c1.Elements.FindAll(e => c2.Elements.Contains(e)).Count;

                    if (temp > max_overlap)
                    {
                        max_overlap = temp;
                        temp_c2 = c2;
                    }
                }
                if (max_overlap > best)
                {
                    best = max_overlap;
                    best_c1 = c1;
                    best_c2 = temp_c2;
                }
                result += max_overlap;
            }

            return new Pair() { Rank = result, C1 = best_c1, C2 = best_c2 };
        }

        
    }
}
