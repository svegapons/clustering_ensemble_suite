using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using System.Reflection;
using ClusterEnsemble.Graphics;
using ClusterEnsemble.ClusterEnsemble;
using ClusterEnsemble;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Evaluation;
using ClusterEnsemble.Proximities;

namespace ClusterEnsemble.Reflection
{
    public class ReflectionTools
    {
        Assembly assembly;
        List<Type> clustering;
        List<Type> ensemble;
        List<Type> dissimilarities;
        List<Type> measure;

        public ReflectionTools()
        {
            assembly = Assembly.Load("ClusterEnsemble");
            //assembly = Assembly.LoadFile(@"C:\Users\jean\Desktop\NOW\Tesis Code\ClusteringEnsembleSuite\ClusterEnsemble\bin\Debug\ClusterEnsemble.dll");
            Type[] temp = assembly.GetTypes();

            clustering = new List<Type>();
            ensemble = new List<Type>();
            dissimilarities = new List<Type>();
            measure = new List<Type>();

            foreach (Type item in temp)
            {
                VerifyCustomAttributes(item);

                if (item.GetConstructor(new Type[] { }) != null)
                {
                    if (item.Namespace == "ClusterEnsemble.Clusters" && item.IsSubclassOf(typeof(ClusterAlgorithm)) && item.IsPublic)
                        clustering.Add(item);
                    else if (item.Namespace == "ClusterEnsemble.ClusterEnsemble" && item.IsSubclassOf(typeof(ConsensusFunction)) && item.IsPublic)
                        ensemble.Add(item);
                    else if (item.Namespace == "ClusterEnsemble.Evaluation" && item.IsSubclassOf(typeof(Measure)) && item.IsPublic)
                        measure.Add(item);
                    else if (item.Namespace == "ClusterEnsemble.Proximities" && (item.IsSubclassOf(typeof(Dissimilarity)) || item.IsSubclassOf(typeof(Similarity))) && item.IsPublic)
                        dissimilarities.Add(item);
                }
            }
        }

        public Type this[string fullname]
        {
            get
            {
                if (fullname == null || !clustering.Exists(e => e.FullName == fullname))
                    throw new ArgumentException();
                return assembly.GetType(fullname);
            }
        }

        public Tree GetClusterAlgorithms()
        {
            if (clustering == null)
                return null;

            return GetTree("ClusterEnsemble.Clusters.ClusterAlgorithm", "ClusterAlgorithm");
        }

        public Tree GetClusterEnsembleAlgorithms()
        {
            if (ensemble == null)
                return null;

            return GetTree("ClusterEnsemble.ClusterEnsemble.ConsensusFunction", "ConsensusFunction");
        }

        public Tree GetMeasures()
        {
            if (measure == null)
                return null;

            return GetTree("ClusterEnsemble.Evaluation.Measure", "Measure");
        }

        Tree GetTree(string fullname, string name)
        {
            List<Type> final = new List<Type>();
            switch (name)
            {
                case "ClusterAlgorithm":
                    foreach (Type t in clustering)
                        final.Add(t);
                    break;
                case "ConsensusFunction":
                    foreach (Type t in ensemble)
                        final.Add(t);
                    break;
                case "Measure":
                    foreach (Type t in measure)
                        final.Add(t);
                    break;
                default:
                    return null;
            }

            ClassFields c = new ClassFields() { FullName = fullname, Name = name, IsAbstract = true };
            Tree root = new Tree(c);
            Queue<Tree> queue = new Queue<Tree>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Tree top = queue.Dequeue();

                Type type = assembly.GetType(top.Value.FullName);

                List<DataStructures.Property> _properties = new List<DataStructures.Property>();
                List<DataStructures.Property> _propertiesIN = new List<DataStructures.Property>();
                List<DataStructures.Property> _propertiesOUT = new List<DataStructures.Property>();
                PropertyInfo[] p = type.GetProperties();
                foreach (PropertyInfo item in p)
                {
                    DataStructures.Property p1 = new DataStructures.Property() { ClassName = top.Value.Name, Name = item.Name };

                    foreach (var item1 in item.GetCustomAttributes(true))
                    {
                        In _attIN = item1 as In;
                        Out _attOUT = item1 as Out;
                        if (_attIN != null)
                        {
                            //p1.ParameterConverterType = _attIN.ParameterConverterType;
                            //p1.Converter = GetConverter(_attIN.ParameterConverterType.FullName);
                            p1.Converter = _attIN.Converter;
                            p1.IsMultipleSelection = _attIN.IsMultipleSelection;
                            p1.Value_Converters = _attIN.Value_Converters;
                        

                            _propertiesIN.Add(p1);

                            break;
                        }
                        else if (_attOUT != null)
                        {
                            _propertiesOUT.Add(p1);

                            break;
                        }
                    }

                    _properties.Add(p1);
                }
                top.Value.Properties = _properties;
                top.Value.InProperties = _propertiesIN;
                top.Value.OutProperties = _propertiesOUT;

                List<Tree> lnc = new List<Tree>();

                List<int> indexs = new List<int>();
                for (int i = 0; i < final.Count; i++)
                {
                    if (final[i].BaseType == type)
                    {
                        
                        Tree ncf = new Tree(new ClassFields());

                        if (!final[i].IsAbstract)
                        {
                            IName instance = GetInstance<IName>(final[i].FullName);
                            ncf.Value.Name = instance.Name;
                        }
                        else
                            ncf.Value.Name = final[i].Name;

                        ncf.Value.FullName = final[i].FullName;
                        ncf.Value.IsAbstract = final[i].IsAbstract ? true : false;

                        lnc.Add(ncf);
                        indexs.Add(i);

                        queue.Enqueue(ncf);
                    }
                }
                top.Childs = lnc;

                for (int i = indexs.Count - 1; i >= 0; i--)
                {
                    final.RemoveAt(indexs[i]);
                }

            }

            return root;
        }

        /// <summary>
        /// Retorna una instancia
        /// </summary>
        /// <typeparam name="T">Es el tipo generico</typeparam>
        /// <param name="aFullname"></param>
        /// <returns></returns>
        public static T GetInstance<T>(string aFullname)
        {
            Assembly assembly = Assembly.Load("ClusterEnsemble");
            //Assembly assembly = Assembly.LoadFile(@"C:\Users\jean\Desktop\NOW\Tesis Code\ClusteringEnsembleSuite\ClusterEnsemble\bin\Debug\ClusterEnsemble.dll");

            return (T)assembly.CreateInstance(aFullname);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        IParameterCoverter GetConverter(string fullname)
        {
            return (IParameterCoverter)assembly.CreateInstance(fullname);
        }

        /// <summary>
        /// Este metodo lo que hace es ponerle cierto valor a una propiedad determinada del objecto T _generic
        /// </summary>
        /// <typeparam name="T">Posibles tipos: Measure, ConsensusFunction, ClusterAlgorithm</typeparam>
        /// <param name="fullname"></param>
        /// <param name="_generic"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public T SetProperty<T>(string fullname, T _generic, DataStructures.Property p)
        {
            Type t = assembly.GetType(fullname);

            PropertyInfo pi = t.GetProperty(p.Name);
            pi.SetValue(_generic, p.Value, null);

            return _generic;
        }
        
        public List<DataStructures.Property> GetInProperties(string fullname)
        {
            Type t = assembly.GetType(fullname);

            List<DataStructures.Property> temp = new List<DataStructures.Property>();
            PropertyInfo[] p = t.GetProperties();
            foreach (PropertyInfo item in p)
            {
                foreach (var att in item.GetCustomAttributes(true))
                {
                    if (att.GetType() == typeof(In))
                    {
                        In _in = ((In)att);

                        DataStructures.Property p1 = new DataStructures.Property() { ClassName = t.Name, Name = item.Name };
                        //p1.ParameterConverterType = _in.ParameterConverterType;
                        //p1.Converter = GetConverter(_in.ParameterConverterType.FullName);
                        p1.Converter = _in.Converter;
                        p1.IsMultipleSelection = _in.IsMultipleSelection;
                        p1.Value_Converters = _in.Value_Converters;
                        
                        temp.Add(p1);
                        break;
                    }
                }

            }
            return temp;
        }

        public List<DataStructures.Property> GetOutProperties(string fullname)
        {
            Type t = assembly.GetType(fullname);

            List<DataStructures.Property> temp = new List<DataStructures.Property>();
            PropertyInfo[] p = t.GetProperties();
            foreach (PropertyInfo item in p)
            {
                foreach (var att in item.GetCustomAttributes(true))
                {
                    if (att.GetType() == typeof(Out))
                    {
                        DataStructures.Property p1 = new DataStructures.Property() { ClassName = t.Name, Name = item.Name };
                        temp.Add(p1);
                        break;
                    }
                }

            }
            return temp;
        }

        public List<DataStructures.Property> GetProperties(string fullname)
        {
            Type t = assembly.GetType(fullname);

            List<DataStructures.Property> temp = new List<DataStructures.Property>();
            PropertyInfo[] p = t.GetProperties();
            foreach (PropertyInfo item in p)
            {
                DataStructures.Property p1 = new DataStructures.Property() { ClassName = t.Name, Name = item.Name };
                foreach (var att in item.GetCustomAttributes(true))
                {
                    if (att.GetType() == typeof(In))
                    {
                        //p1.ParameterConverterType = ((In)att).ParameterConverterType;
                        //p1.Converter = GetConverter(((In)att).ParameterConverterType.FullName);
                        p1.Converter = ((In)att).Converter;
                        break;
                    }
                }
                temp.Add(p1);


            }
            return temp;
        }

        public List<Proximity> GetProximities()
        {
            List<Proximity> result = new List<Proximity>();
            foreach (var item in dissimilarities)
            {
                Proximity id = (Proximity)assembly.CreateInstance(item.FullName);
                result.Add(id);
            }
            return result;
        }
        public void VerifyCustomAttributes(Type type)
        {
            PropertyInfo[] p = type.GetProperties();

            foreach (PropertyInfo item in p)
            {
                bool in_att = false, out_att = false;

                foreach (var att in item.GetCustomAttributes(true))
                {
                    if (att.GetType() == typeof(In) && out_att)
                    {
                        throw new Exception("Los atributos de la Propiedad " + item.Name + " estan mal definidos");
                    }
                    else if(att.GetType() == typeof(In) && in_att)
                    {
                        throw new Exception("Los atributos de la Propiedad " + item.Name + " estan mal definidos");
                    }
                    else if (att.GetType() == typeof(In))
                        in_att = true;

                    if (att.GetType() == typeof(Out) && in_att)
                    {
                        throw new Exception("Los atributos de la Propiedad " + item.Name + " estan mal definidos");
                    }
                    else if (att.GetType() == typeof(Out) && out_att)
                    {
                        throw new Exception("Los atributos de la Propiedad " + item.Name + " estan mal definidos");
                    }
                    else if (att.GetType() == typeof(Out))
                        out_att = true;
                }
                
            }

        }
    }
}
