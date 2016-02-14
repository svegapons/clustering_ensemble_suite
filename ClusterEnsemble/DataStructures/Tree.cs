using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.DataStructures
{
    public class Tree
    {
        public ClassFields Value { get; set; }
        public List<Tree> Childs { get; set; }

        public Tree(ClassFields value, List<Tree> childs)
        {
            Value = value;
            Childs = childs;

        }
        public Tree(ClassFields value)
        {
            Value = value;
            Childs = new List<Tree>();

        }

        public int ChildsCount
        {
            get { return Childs.Count; }
        }
    }


    public class ClassFields
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsAbstract { get; set; }
        public List<Property> Properties { get; set; }
        public List<Property> InProperties { get; set; }
        public List<Property> OutProperties { get; set; }
    }

    public class Property
    {
        public string ClassName { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public IParameterCoverter Converter { get; set; }

        public bool IsMultipleSelection { get; set; }
        public List<Value_Converter> Value_Converters { get; set; }

    }
}
