using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.Evaluation
{
    public abstract class Measure:IName
    {
        public Measure() { }
        public Set Set { get; set; }
        public string Name { get; set; }
        public Structuring Structuring { get; set; }
        public Structuring RealPartition { get; set; }
        public Attribute ObjetiveAttribute { get; set; }
        public abstract double EvaluatePartition();
    }

    public abstract class External : Measure
    {
        public External() { }
    }
    
    public abstract class Internal : Measure
    {
        public Internal() { }
    }
   
}
