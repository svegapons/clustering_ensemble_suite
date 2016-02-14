using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using ClusterEnsemble;
using ClusterEnsemble.DataStructures;
using ClusteringEnsembleSuite.Controls.TabsControls;
using ClusteringEnsembleSuite.Code.DataStructures;

namespace ClusteringEnsembleSuite.Code
{
    /// <summary>
    /// Parámetro del evento NewSetEventHandler
    /// </summary>
    public class NewSetEventArgs : EventArgs
    {
        private Set fNewSet;

        public NewSetEventArgs(Set aNewSet)
        {
            this.fNewSet = aNewSet;
        }
        
        public Set NewSet
        {
            get { return fNewSet; }
        }
    }
    /// <summary>
    /// Evento que lanza Loader indicando que se ha cargado un nuevo conjunto
    /// </summary>
    /// <param name="aLoader"></param>
    /// <param name="e"></param>
    public delegate void NewSetEventHandler(object aLoader, NewSetEventArgs e);


    /// <summary>
    /// Parámetro del evento NewClusterAlgorithmEventHandler
    /// </summary>
    public class NewClusterAlgorithmEventArgs : EventArgs
    {
        Tree fNewClusterAlgorithm;
        public NewClusterAlgorithmEventArgs(Tree aNewClusterAlgorithm)
        {
            this.fNewClusterAlgorithm = aNewClusterAlgorithm;
        }

        public Tree NewClusterAlgorithm
        {
            get { return fNewClusterAlgorithm; }
        }
    }
    /// <summary>
    /// Evento que lanza OneClusterAlgVisualizer indicando que se ha seleccionado un nuevo Algoritmo de Clustering
    /// </summary>
    /// <param name="aOneClusterAlgVisualizer"></param>
    /// <param name="e"></param>
    public delegate void NewClusterAlgorithmEventHandler(object aOneClusterAlgVisualizer, NewClusterAlgorithmEventArgs e);

    /// <summary>
    /// Parámetro del evento NewEnsembleAlgorithmEventHandler
    /// </summary>
    public class NewEnsembleAlgorithmEventArgs : EventArgs
    {
        Tree fNewEnsembleAlgorithm;
        public NewEnsembleAlgorithmEventArgs(Tree aNewEnsembleAlgorithm)
        {
            this.fNewEnsembleAlgorithm = aNewEnsembleAlgorithm;
        }

        public Tree NewEnsembleAlgorithm
        {
            get { return fNewEnsembleAlgorithm; }
        }
    }
    /// <summary>
    /// Evento que lanza OneEnsembleAlgVisualizer indicando que se ha seleccionado un nuevo Algoritmo de Ensemble
    /// </summary>
    /// <param name="aOneEnsembleAlgVisualizer"></param>
    /// <param name="e"></param>
    public delegate void NewEnsembleAlgorithmEventHandler(object aOneEnsembleAlgVisualizer, NewEnsembleAlgorithmEventArgs e);


    /// <summary>
    /// Parámetro del evento NewStructuringEventHandler
    /// </summary>
    public class NewStructuringEventArgs : EventArgs
    {
        PartitionInfo fStructuring;
        public NewStructuringEventArgs(PartitionInfo aNewStructuring)
        {
            this.fStructuring = aNewStructuring;
        }

        public Structuring NewStructuring
        {
            get
            {
                return fStructuring.Partition;
            }
        }       
        public PartitionInfo NewStructuringInfo
        {
            get
            {
                return fStructuring;
            }
        }
        public AlgorithmType StrcturingType
        {
            get
            {
                return fStructuring.AlgorithmType;
            }
        }
    }
    /// <summary>
    /// Evento que lanza TabEnsemble y TabClustering indicando que hay una nueva partición creada,
    /// se enteran TabEnsemble y TabEvaluation
    /// </summary>
    /// <param name="asender"></param>
    /// <param name="e"></param>
    public delegate void NewStructuringEventHandler(object asender, NewStructuringEventArgs e);

    
    /// <summary>
    /// Parámetro del evento NewMeasuresEventHandler
    /// </summary>
    public class NewMeasuresEnventArgs : EventArgs
    {
        List<MeasureInfo> fSelectedMeasures { get; set; }

        public NewMeasuresEnventArgs(List<MeasureInfo> aSelectedMeasures)
        {
            this.fSelectedMeasures = aSelectedMeasures;
        }

        public List<MeasureInfo> NewMeasures
        {
            get
            {
                return fSelectedMeasures;
            }
        }
    }
    /// <summary>
    /// Evento que lanza MeasuresVisualizer indicando que se han seleccionado nuevas medidas de evaluación
    /// </summary>
    /// <param name="asender"></param>
    /// <param name="e"></param>
    public delegate void NewMeasuresEventHandler(object asender, NewMeasuresEnventArgs e);

    /// <summary>
    /// Parámetro del evento CheckAttributeEventHandler
    /// </summary>
    public class CheckAttributeEnventArgs : EventArgs
    {
        List<ClusterEnsemble.Attribute> fSelectedAttributes { get; set; }

        public CheckAttributeEnventArgs(List<ClusterEnsemble.Attribute> aSelectedAttributes)
        {
            this.fSelectedAttributes = aSelectedAttributes;
        }

        public List<ClusterEnsemble.Attribute> SelectedAttributes
        {
            get
            {
                return fSelectedAttributes;
            }
        }
    }
    /// <summary>
    /// Evento que lanza AttributesVisualizer indicando que se ha chequeado o deschequeado algun atributo
    /// </summary>
    /// <param name="asender"></param>
    /// <param name="e"></param>
    public delegate void CheckAttributeEventHandler(object asender, CheckAttributeEnventArgs e);
 
}
