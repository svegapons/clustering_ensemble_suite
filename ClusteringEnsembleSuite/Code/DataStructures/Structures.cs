using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using ClusterEnsemble;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.ClusterEnsemble;
using ClusterEnsemble.Evaluation;
using ClusterEnsemble.DataStructures;
using System.Windows.Controls;
using Telerik.Windows.Controls;


namespace ClusteringEnsembleSuite.Code.DataStructures
{

    /// <summary>
    /// Esta clase se utiliza para visualizar las propiedades de Entrada, en dependencia si la propiedad
    /// es de multiples valores o de uno simple
    /// </summary>
    public class InControl
    {
        public ControlType ControlType { get; set; }
        public TextBox TextBox { get; set; }
        public RadComboBox ComboBox { get; set; }
    }

    /// <summary>
    /// Este enum es para saber en la clase InControl cual es el control que se sta utilizando
    /// </summary>
    public enum ControlType
    {
        TextBox,
        ComboBox
    }

    /// <summary>
    /// Se utiliza en la clase ListClusterAlgVIsualizer, para hacerle una copia a cada PartitionInfo que se annade,
    ///que nos e afecte el indice de ese PartitionInfo segun el control en que este..
    /// </summary>
    public class CopyPartitionInfo
    {
        public PartitionInfo PartitionInfo { get; set; }
        public int Index { get; set; }
    }
    public class PartitionInfo
    {
        public string AlgorithmName { get; set; }
        public Structuring Partition { get; set; }
        public ClusterAlgorithm ClusterAlgorithm { get; set; }
        public ConsensusFunction ConsensusFunction { get; set; }
        public AlgorithmType AlgorithmType { get; set; }
        public string Time { get; set; }
        public int ElementCount { get; set; }
        public double SearchSpace { get; set; }

        //Esto es solamente para el control LisClusterALgVisualizer, para poder
        //poner un boton y bindearle la propiedad Tag con este indice para
        //poder instanciar ClusterDetails
        public int Index { get; set; }

    }
    public class MeasureInfo
    {
        public Tree Tree { get; set; }
        public string MeasureName { get; set; }
    }
    public class MeasureOutput
    {
        public string AlgorithmName { get; set; }
        public Structuring Partition { get; set; }
        public List<MeasureInfo> Measures { get; set; }
        public double[] Values { get; set; }
    }

    public enum AlgorithmType
    {
        Clustering,
        Ensemble
    }
}
