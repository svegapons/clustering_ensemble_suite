using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


//Added
using ClusteringEnsembleSuite.Code;
using CEDS = ClusterEnsemble.DataStructures;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Reflection;
using ClusterEnsemble;
using ClusterEnsemble.ClusterEnsemble;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusteringEnsembleSuite.Controls.Visualizers;
using ClusterEnsemble.Graphics;
using ClusterEnsemble.ReductionFunctions;


namespace ClusteringEnsembleSuite.Controls.TabsControls
{
    /// <summary>
    /// Interaction logic for TabEnsemble.xaml
    /// </summary>
    public partial class TabEnsemble : UserControl,INeedProgressBar
    {
        private bool HasSelectedAlgorithm { get; set; }
        private CEDS.Tree Tree { get; set; }

        public event NewStructuringEventHandler NewStructuringEventHandler;
        public EnsembleAlgorithmVisualizer this_EnsembleAlgVisualizer { get; set; }

        public TabEnsemble()
        {
            try
            {
                InitializeComponent();

                this_EnsembleAlgVisualizer = new EnsembleAlgorithmVisualizer();
                this.this_EnsembleAlgVisualizer.NewEnsembleAlgorithm += new NewEnsembleAlgorithmEventHandler(NewEnsembleAlg);

                this.uctrl_ListClusterAlgVisualizerClustering.GroupBoxHeader = "Clustering Algorithms";
                //this.uctrl_ListClusterAlgVisualizerClustering.ListViewSelectionMode = SelectionMode.Single;
                this.uctrl_ListClusterAlgVisualizerEnsemble.GroupBoxHeader = "Ensemble Clustering Algorithms";
                //this.uctrl_ListClusterAlgVisualizerEnsemble.ListViewSelectionMode = SelectionMode.Single;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void NewSet(object sender, NewSetEventArgs e)
        {
            try
            {
                this.tb_SelectEnsembleAlg.Text = "Select Ensemble Clustering Algorithm ...";
                this.tb_output.Text = "";
                this.HasSelectedAlgorithm = false;
                this.uctrl_ListClusterAlgVisualizerClustering.ResetPartitionInfo();
                this.uctrl_ListClusterAlgVisualizerEnsemble.ResetPartitionInfo();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void NewEnsembleAlg(object asender, NewEnsembleAlgorithmEventArgs aNewEnsembleAlg)
        {
            try
            {
                this.Tree = aNewEnsembleAlg.NewEnsembleAlgorithm;
                string _EnsembleAlg = "";

                _EnsembleAlg += Tree.Value.Name + "( " + "Set-" + Enviroment.Set.RelationName;

                List<CEDS.Property> _parameters = Tree.Value.InProperties;
                for (int i = 0; i < _parameters.Count; i++)
                    _EnsembleAlg += ", " + _parameters[i].Name + "-" + _parameters[i].Value;
                _EnsembleAlg += ")";

                this.tb_SelectEnsembleAlg.Text = _EnsembleAlg;

                this.HasSelectedAlgorithm = true;

                this.this_EnsembleAlgVisualizer.Visibility = Visibility.Hidden;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void tb_SelectEnsembleAlg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.this_EnsembleAlgVisualizer.ShowDialog();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        //Todos estos Metodos son para que funcione el ProgressBar
        delegate Structuring Run(ConsensusFunction aConsensusFunction);
        private void bt_Run_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HasSelectedAlgorithm)
                {
                    string _Error = "";
                    if (!Enviroment.CanRunAlgorithm(out _Error, AlgorithmType.Ensemble))
                    {
                        MessageBox.Show(_Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ReflectionTools _rcet = new ReflectionTools();
                    ConsensusFunction _EnsembleAlg = ReflectionTools.GetInstance<ConsensusFunction>(Tree.Value.FullName);

                    foreach (CEDS.Property _p in Tree.Value.InProperties)
                    {
                        _rcet.SetProperty(Tree.Value.FullName, _EnsembleAlg, _p);
                    }

                    List<Structuring> _structuringsParams = GetSelectedStructurings();
                    if (_structuringsParams == null || _structuringsParams.Count == 0)
                    {
                        MessageBox.Show("You must select at least one Structuring to apply an Ensemble algorithm.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    VisualUtils.SetGlobalInProperties(_rcet, _EnsembleAlg, Tree, _structuringsParams);

                    #region OLD CODE
                    //Structuring _structuring = _EnsembleAlg.BuildStructuring();

                    //PartitionInfo _partInfo = new PartitionInfo()
                    //{
                    //    AlgorithmName = this.tb_SelectEnsembleAlg.Text,
                    //    ConsensusFunction = _EnsembleAlg,
                    //    Partition = _structuring,
                    //    AlgorithmType = AlgorithmType.Ensemble,
                    //    Index = -1
                    //};
                    //this.uctrl_ListClusterAlgVisualizerEnsemble.AddPartitionInfo(_partInfo);


                    //if (NewStructuringEventHandler != null)
                    //{
                    //    NewStructuringEventHandler(this, new NewStructuringEventArgs(_partInfo));
                    //}
                    #endregion

                    Run _run = RunMethod;
                    _run.BeginInvoke(_EnsembleAlg, RunFinish, new DataThread() { ConsensusFunction = _EnsembleAlg, Run = _run });
                }
                else
                    MessageBox.Show("You must first select an Ensemble algorithm.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private Structuring RunMethod(ConsensusFunction aConsensusFunction)
        {
            try
            {
                aConsensusFunction.IContainerProgressBar = IContainerProgressBar;
                //return aConsensusFunction.BuildStructuring();
                return aConsensusFunction.BuildStructuringWithReduction();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return null;
            }
        }
        class DataThread
        {
            public Run Run { get; set; }
            public ConsensusFunction ConsensusFunction { get; set; }
        }
        delegate void RunF(Structuring aStructuring, ConsensusFunction aConsensusFunction);
        private void RunFinish(IAsyncResult aIAsyncResult)
        {
            try
            {
                DataThread aDataThread = (DataThread)aIAsyncResult.AsyncState;
                this.Dispatcher.BeginInvoke(new RunF(RunFinish), aDataThread.Run.EndInvoke(aIAsyncResult), aDataThread.ConsensusFunction);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void RunFinish(Structuring aStructuring, ConsensusFunction aConsensusFunction)
        {
            try
            {
                ConsensusFunction _EnsembleAlg = aConsensusFunction;
                Structuring _structuring = aStructuring;

                if (_structuring != null)
                {
                    TimeSpan ts = _EnsembleAlg.Time;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                    PartitionInfo _partInfo = new PartitionInfo()
                    {
                        AlgorithmName = this.tb_SelectEnsembleAlg.Text,
                        ConsensusFunction = _EnsembleAlg,
                        Partition = _structuring,
                        AlgorithmType = AlgorithmType.Ensemble,
                        Time = elapsedTime,
                        ElementCount = aConsensusFunction.ReductionElementCount,
                        SearchSpace = ReductionFunction.BellNumber(aConsensusFunction.ReductionElementCount),
                        Index = -1
                    };
                    this.uctrl_ListClusterAlgVisualizerEnsemble.AddPartitionInfo(_partInfo);
                    this.tb_output.Text = _EnsembleAlg.Output;


                    if (NewStructuringEventHandler != null)
                    {
                        NewStructuringEventHandler(this, new NewStructuringEventArgs(_partInfo));
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        //=========================================================



        private List<Structuring> GetSelectedStructurings()
        {
            try
            {
                List<PartitionInfo> _partitionsInfo = this.uctrl_ListClusterAlgVisualizerClustering.GetSelected();
                List<Structuring> _result = new List<Structuring>();
                if (_partitionsInfo != null)
                    _partitionsInfo.ForEach(_pi => _result.Add(_pi.Partition));

                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<Structuring>();
            }
        }

        /// <summary>
        /// Este metodo se ejecuta cuando el TabClustering lanza el evento de que hay una nueva Particion 
        /// </summary>
        /// <param name="asender"></param>
        /// <param name="e"></param>
        public void NewStructuring(object asender, NewStructuringEventArgs e)
        {
            try
            {
                this.uctrl_ListClusterAlgVisualizerClustering.AddPartitionInfo(e.NewStructuringInfo);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }


        #region INeedProgressBar Members

        public IContainerProgressBar IContainerProgressBar { get; set; }

        #endregion
    }

}
