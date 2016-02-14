using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Reflection;
using ClusterEnsemble.ReductionFunctions;
using ClusterEnsemble.ClusterEnsemble;



namespace ClusterEnsemble.Graphics
{
    public interface IParameterCoverter
    {
        bool TryConvert(string aparameter, out object aresult);
        object Convert(string aparameter);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class In : System.Attribute
    {
        public bool IsMultipleSelection { get; set; }        
        public IParameterCoverter Converter { get; set; }
        public List<Value_Converter> Value_Converters { get; set; }

        public In(Type parameterConverterType)
        {
            Type interf = parameterConverterType.GetInterface("IParameterCoverter");
            if (interf == null || !parameterConverterType.IsPublic)
                throw new ArgumentException("El parámetro del Atributo IN no implementa la interface IParameterCoverter o el tipo no es PUBLIC.");

            Converter = ReflectionTools.GetInstance<IParameterCoverter>(parameterConverterType.FullName);

            IsMultipleSelection = false;

        }
        public In(string[] aValues, Type[] aParameterConverterTypeArr)
        {
            Value_Converters = new List<Value_Converter>();
            if (aValues == null || aParameterConverterTypeArr == null || aValues.Length != aParameterConverterTypeArr.Length)
                throw new ArgumentException("Parámetros incorrectos en el Atributo IN, posible causa : los arreglos son NULL o no tienen la misma cantidad de elementos");

            for (int i = 0; i < aParameterConverterTypeArr.Length; i++)
            {
                Type _interf = aParameterConverterTypeArr[i].GetInterface("IParameterCoverter");
                if (_interf == null || !aParameterConverterTypeArr[i].IsPublic)
                    throw new ArgumentException("El parámetro del Atributo IN no implementa la interface IParameterCoverter o el tipo no es PUBLIC.");
                else
                    Value_Converters.Add(new Value_Converter() { Converter = ReflectionTools.GetInstance<IParameterCoverter>( aParameterConverterTypeArr[i].FullName), Value = aValues[i] });
            }

            IsMultipleSelection = true;
        }
    }
    public class Value_Converter
    {
        public IParameterCoverter Converter { get; set; }
        public string Value { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Out : System.Attribute
    { }

    public class IntConverter : IParameterCoverter
    {
        #region IParameterCoverter Members
        
        public bool TryConvert(string aparameter, out object aresult)
        {
            int _temp;
            bool _result = int.TryParse(aparameter, out _temp);
            aresult = _temp;
            return _result;
        }
        public object Convert(string parameter)
        {
            object _result;
            if (TryConvert(parameter, out _result))
                return _result;
            else return null;
        }

        #endregion
    }
    public class IntGTZeroConverter : IParameterCoverter
    {
        #region IParameterCoverter Members
        
        public bool TryConvert(string aparameter, out object aresult)
        {
            int _temp;
            bool _result = int.TryParse(aparameter, out _temp);
            aresult = _temp;
            if (!(_temp > 0))
            {
                _result = false;
                aresult = -1;
            }

            return _result;
        }
        public object Convert(string parameter)
        {
            object _result;
            if (TryConvert(parameter, out _result))
                return _result;
            else return null;
        }

        #endregion
    }
    public class DoubleGTZeroConverter : IParameterCoverter
    {
        #region IParameterCoverter Members

        public bool TryConvert(string aparameter, out object aresult)
        {
            double _temp;
            bool _result = double.TryParse(aparameter, out _temp);
            aresult = _temp;
            if (!(_temp > 0))
            {
                _result = false;
                aresult = -1;
            }

            return _result;
        }
        public object Convert(string parameter)
        {
            object _result;
            if (TryConvert(parameter, out _result))
                return _result;
            else return null;
        }

        #endregion
    }
    public class DoubleLTOneConverter : IParameterCoverter
    {
        #region IParameterCoverter Members

        public bool TryConvert(string aparameter, out object aresult)
        {
            double _temp;
            bool _result = double.TryParse(aparameter, out _temp);
            aresult = _temp;
            if (!(_temp < 1))
            {
                _result = false;
                aresult = 1;
            }

            return _result;
        }
        public object Convert(string parameter)
        {
            object _result;
            if (TryConvert(parameter, out _result))
                return _result;
            else return null;
        }

        #endregion
    }
    public class DoubleConverter : IParameterCoverter
    {
        #region IParameterCoverter Members
       
        public bool TryConvert(string aparameter, out object aresult)
        {
            double _temp;
            bool _result = double.TryParse(aparameter, out _temp);
            aresult = _temp;
            return _result;
        }
        public object Convert(string parameter)
        {
            object _result;
            if (TryConvert(parameter, out _result))
                return _result;
            else return null;
        }
        
        #endregion
    }
    public class ClusterAlgorithmConverter : IParameterCoverter
    {

        #region IParameterCoverter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar, esta clase debe heredar de ClusterAlgorithm</param>
        /// <param name="aresult"></param>
        /// <returns></returns>
        public bool TryConvert(string aparameter, out object aresult)
        {
            ClusterAlgorithm _temp = ReflectionTools.GetInstance<ClusterAlgorithm>("ClusterEnsemble.Clusters."+aparameter);

            if (_temp != null)
            {
                aresult = _temp;
                return true;
            }
            else
            {
                aresult = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar, esta clase debe heredar de ClusterAlgorithm</param>
        /// <returns></returns>
        public object Convert(string aparameter)
        {
            object _result;
            if (TryConvert(aparameter, out _result))
                return _result;
            else
                return null;
        }

        #endregion
    }

    public class ReductionFunctionConverter : IParameterCoverter
    {

        #region IParameterCoverter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar, esta clase debe heredar de ClusterAlgorithm</param>
        /// <param name="aresult"></param>
        /// <returns></returns>
        public bool TryConvert(string aparameter, out object aresult)
        {
            ReductionFunction _temp = ReflectionTools.GetInstance<ReductionFunction>("ClusterEnsemble.ReductionFunctions." + aparameter);

            if (_temp != null)
            {
                aresult = _temp;
                return true;
            }
            else
            {
                aresult = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar, esta clase debe heredar de ReductionFunction</param>
        /// <returns></returns>
        public object Convert(string aparameter)
        {
            object _result;
            if (TryConvert(aparameter, out _result))
                return _result;
            else
                return null;
        }

        #endregion
    }

    public class GenericDistancesConverter : IParameterCoverter
    {

        #region IParameterCoverter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar</param>
        /// <param name="aresult"></param>
        /// <returns></returns>
        public bool TryConvert(string aparameter, out object aresult)
        {
            GenericDistances _temp = ReflectionTools.GetInstance<GenericDistances>("ClusterEnsemble.ClusterEnsemble." + aparameter);

            if (_temp != null)
            {
                aresult = _temp;
                return true;
            }
            else
            {
                aresult = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar, esta clase debe heredar de ReductionFunction</param>
        /// <returns></returns>
        public object Convert(string aparameter)
        {
            object _result;
            if (TryConvert(aparameter, out _result))
                return _result;
            else
                return null;
        }

        #endregion
    }

    public class BooleanConverter : IParameterCoverter
    {
        #region IParameterCoverter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">Nombre de la clase a instanciar</param>
        /// <param name="aresult"></param>
        /// <returns></returns>
        public bool TryConvert(string aparameter, out object aresult)
        {
            switch (aparameter)
            {
                case "True":
                    aresult = true;
                    return true;
                case "False":
                    aresult = false;
                    return true;
                default:
                    aresult = true;
                    return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparameter">True or False, to return the value</param>
        /// <returns></returns>
        public object Convert(string aparameter)
        {
            object _result;
            if (TryConvert(aparameter, out _result))
                return _result;
            else
                return null;
        }

        #endregion
    }

}
