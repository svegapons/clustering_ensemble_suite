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
using System.Threading;
//Added
using ClusteringEnsembleSuite.Code;
using CEDS = ClusterEnsemble.DataStructures;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Reflection;
using ClusterEnsemble;
using ClusteringEnsembleSuite.Controls.Visualizers;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.Graphics;

namespace ClusteringEnsembleSuite.Controls.TabsControls
{
    /// <summary>
    /// Interaction logic for TabClustering.xaml
    /// </summary>
    public partial class TabClustering : UserControl,INeedProgressBar
    {
        public event NewStructuringEventHandler NewStructuringEventHandler;
        public ClusterAlgorithmVisualizer this_ClusterAlgVisualizer{get;set;}
        public TabClustering()
        {
            try
            {
                InitializeComponent();
                this_ClusterAlgVisualizer = new ClusterAlgorithmVisualizer();
                this.this_ClusterAlgVisualizer.NewClusterAlgorithm += new NewClusterAlgorithmEventHandler(NewClusterAlg);

                this.uctrl_ListClusterAlgVisualizer.GroupBoxHeader = "Clustering Algorithms";
                //this.uctrl_ListClusterAlgVisualizer.ListViewSelectionMode = SelectionMode.Single;
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
                this.tb_SelectClusterAlg.Text = "Select Clustering Algorithm ...";
                this.tb_output.Text = "";
                this.HasSelectedAlgorithm = false;
                this.uctrl_ListClusterAlgVisualizer.ResetPartitionInfo();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private bool HasSelectedAlgorithm { get; set; }
        private void NewClusterAlg(object asender, NewClusterAlgorithmEventArgs aNewClusterAlg)
        {
            try
            {
                Tree = aNewClusterAlg.NewClusterAlgorithm;
                string _ClusterAlg = "";

                _ClusterAlg += Tree.Value.Name + "( " + "Set-" + Enviroment.Set.RelationName + ", Diss-" + Enviroment.Proximity.Name;

                List<CEDS.Property> _parameters = Tree.Value.InProperties;

                for (int i = 0; i < _parameters.Count; i++)
                    _ClusterAlg += ", " + _parameters[i].Name + "-" + _parameters[i].Value;
                _ClusterAlg += ")";

                this.tb_SelectClusterAlg.Text = _ClusterAlg;

                this.HasSelectedAlgorithm = true;

                this.this_ClusterAlgVisualizer.Visibility = Visibility.Hidden;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private CEDS.Tree Tree { get; set; }

        private void tb_SelectClusterAlg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this_ClusterAlgVisualizer.ShowDialog();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }


        //Todos estos Metodos son para que funcione el ProgressBar
        delegate Structuring Run(ClusterAlgorithm aClusterAlgorithm);
        private void bt_Run_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HasSelectedAlgorithm)
                {
                    string _Error = "";
                    if (!Enviroment.CanRunAlgorithm(out _Error, AlgorithmType.Clustering))
                    {
                        MessageBox.Show(_Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ReflectionTools _rct = new ReflectionTools();
                    ClusterAlgorithm _ClusterAlg = ReflectionTools.GetInstance<ClusterAlgorithm>(Tree.Value.FullName);

                    foreach (CEDS.Property _p in Tree.Value.InProperties)
                    {
                        _rct.SetProperty(Tree.Value.FullName, _ClusterAlg, _p);
                    }


                    if (!VisualUtils.SetGlobalInProperties(_rct, _ClusterAlg, Tree, out _Error, this.chbx_AttrRnd.IsChecked.Value))
                    {
                        MessageBox.Show(_Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Run _run = RunMethod;
                    
                    _run.BeginInvoke(_ClusterAlg, RunFinish, new DataThread() { Run = _run, ClusterAlgorithm = _ClusterAlg });

                }
                else
                    MessageBox.Show("You must first select a Clustering algorithm.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private Structuring RunMethod(ClusterAlgorithm aClusterAlgorithm)
        {
            try
            {
                aClusterAlgorithm.IContainerProgressBar = IContainerProgressBar;
                //return aClusterAlgorithm.BuildStructuring();
                return aClusterAlgorithm.BuildStructuringWithTime();
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
            public ClusterAlgorithm ClusterAlgorithm { get; set; }
        }
        delegate void RunF(Structuring aStructuring, ClusterAlgorithm aClusterAlgorithm);
        private void RunFinish(IAsyncResult aIAsyncResult)
        {
            try
            {
                DataThread aDataThread = (DataThread)aIAsyncResult.AsyncState;
                this.Dispatcher.BeginInvoke(new RunF(RunFinish), aDataThread.Run.EndInvoke(aIAsyncResult), aDataThread.ClusterAlgorithm);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void RunFinish(Structuring aStructuring, ClusterAlgorithm aClusterAlgorithm)
        {
            try
            {
                ClusterAlgorithm _ClusterAlg = aClusterAlgorithm;
                Structuring _structuring = aStructuring;

                if (_structuring != null)
                {
                    TimeSpan ts = _ClusterAlg.Time;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);

                    PartitionInfo _partinfo = new PartitionInfo()
                    {
                        AlgorithmName = this.tb_SelectClusterAlg.Text,
                        ClusterAlgorithm = _ClusterAlg,
                        Partition = _structuring,
                        AlgorithmType = AlgorithmType.Clustering,
                        Time = elapsedTime,
                        Index = -1
                    };

                    this.uctrl_ListClusterAlgVisualizer.AddPartitionInfo(_partinfo);
                    this.tb_output.Text = _ClusterAlg.Output;

                    if (NewStructuringEventHandler != null)
                    {
                        NewStructuringEventHandler(this, new NewStructuringEventArgs(_partinfo));
                    }

                    //Experimental Mode
                    this.ExperimentalMode = false;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }


        #region INeedProgressBar Members

        public IContainerProgressBar IContainerProgressBar { get; set; }

        #endregion

        private void bt_RunAll_Click(object sender, RoutedEventArgs e)
        {
           
        }

        public bool ExperimentalMode { get; set; }
    }



}
