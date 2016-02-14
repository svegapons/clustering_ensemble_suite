using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.ClusterEnsemble
{
    public class QMI : InformationTheoretic
    {
        public QMI() { Name = "QMI"; }
        public QMI(Set set, List<Structuring> estr) : base(set, estr) { Name = "QMI"; }

        [In(typeof(IntGTZeroConverter))]
        public int ClusterCount { get; set; }

        [In(typeof(IntGTZeroConverter))]
        public int IterationsCount { get; set; }

        [In(typeof(IntConverter))]
        public int Seed { get; set; }

        public override Structuring BuildStructuring()
        {
            if (Structurings == null || Set == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running QMI algorithm...", true);
            }

            List<Attribute> list_att = new List<Attribute>();
            int cont = 0;
            foreach (Structuring s in Structurings)
            {
                foreach (Cluster c in s.Clusters.Values)
                {
                    Attribute att = new Attribute("x" + cont, null);
                    cont++;

                    att.AttributeType = AttributeType.Numeric;
                    list_att.Add(att);
                }
            }

            Set newset = new Set("Artificial");
            newset.Attributes = new Attributes(list_att);
            newset.ElementType = ElementType.Numeric;

            
            foreach (Element e in Set.Elements)
            {
                List<object> values = new List<object>();
                foreach (Structuring s in Structurings)
                {
                    foreach (Cluster c in s.Clusters.Values)
                    {
                        double temp = c.HaveElement(e) ? 1 : 0;
                        temp = temp - ((double)c.ElementsCount / (double)Set.ElementsCount);
                        values.Add(temp);
                    }
                }
                Element newelement = new Element(newset, values);                
                newelement.Name = e.Name;
                newelement.Index = e.Index;
                
                newset.AddElement(newelement);
            }

            KMeans kms = new KMeans(newset, new EuclideanDistance() { AttributesToCalculateProximity = newset.Attributes.Values });
            kms.ClustersCount = ClusterCount;
            kms.IterationsCount = IterationsCount;
            kms.Seed = Environment.TickCount;

            kms.IContainerProgressBar = IContainerProgressBar;

            Structuring art_struct = kms.BuildStructuring();

            List<Cluster> clusters = new List<Cluster>();
            cont = 0;
            foreach (Cluster c in art_struct.Clusters.Values)
            {
                Cluster temp = new Cluster("C-" + cont);
                cont++;
                foreach (Element item in c.Elements)
                {
                    temp.AddElement(Set[item.Index]);
                }
                clusters.Add(temp);
            }

            Dictionary<string, Cluster> temp_dic=new Dictionary<string,Cluster>();
            foreach (Cluster item in clusters)
            {
                temp_dic.Add(item.Name, item); 
            }

            Structuring real_struct = new Partition() { Clusters = temp_dic };
            return real_struct;            
        }
    }

    public class AverageQMI : InformationTheoretic
    {
        public AverageQMI() { Name = "Average QMI"; }
        public AverageQMI(Set set, List<Structuring> estr) : base(set, estr) { Name = "Average QMI"; }        

        public override Structuring BuildStructuring()
        {
            if (Structurings == null || Set == null)
                throw new NullReferenceException();

            if (IContainerProgressBar != null)
            {
                IContainerProgressBar.ResetProgressBar(1, 1, true);
                IContainerProgressBar.UpdateProgressBar(0, "Running Average QMI algorithm...", true);

            }

            List<Attribute> list_att = new List<Attribute>();
            int cont = 0;
            foreach (Structuring s in Structurings)
            {
                foreach (Cluster c in s.Clusters.Values)
                {
                    Attribute att = new Attribute("x" + cont, null);
                    cont++;

                    att.AttributeType = AttributeType.Numeric;
                    list_att.Add(att);
                }
            }

            Set newset = new Set("Artificial");
            newset.Attributes = new Attributes(list_att);
            newset.ElementType = ElementType.Numeric;

            
            foreach (Element e in Set.Elements)
            {
                List<object> values = new List<object>();
                foreach (Structuring s in Structurings)
                {
                    foreach (Cluster c in s.Clusters.Values)
                    {
                        double temp = c.HaveElement(e) ? 1 : 0;
                        temp = temp - ((double)c.ElementsCount / (double)Set.ElementsCount);
                        values.Add(temp);
                    }
                }
                Element newelement = new Element(newset, values);
                newelement.Name = e.Name;
                newelement.Index = e.Index;
                
                newset.AddElement(newelement);
            }

            GroupAverageLifetime sg = new GroupAverageLifetime(newset, new EuclideanDistance() { AttributesToCalculateProximity = newset.Attributes.Values });

            sg.IContainerProgressBar = IContainerProgressBar;

            Structuring art_struct = sg.BuildStructuring();

            List<Cluster> clusters = new List<Cluster>();
            cont = 0;
            foreach (Cluster c in art_struct.Clusters.Values)
            {
                Cluster temp = new Cluster("C-" + cont);
                cont++;
                foreach (Element item in c.Elements)
                {
                    temp.AddElement(Set[item.Index]);
                }
                clusters.Add(temp);
            }

            Dictionary<string, Cluster> temp_dic = new Dictionary<string, Cluster>();
            foreach (Cluster item in clusters)
            {
                temp_dic.Add(item.Name, item);
            }

            Structuring real_struct = new Partition() { Clusters = temp_dic };
            return real_struct;
        }
    }
}
