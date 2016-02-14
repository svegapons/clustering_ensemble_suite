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
using System.Windows.Shapes;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusteringEnsembleSuite.Code;
using Telerik.Windows.Controls;
using ClusterEnsemble.DataStructures;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for EnsembleAlgorithmVisualizer.xaml
    /// </summary>
    public partial class EnsembleAlgorithmVisualizer : Window
    {
        public event NewEnsembleAlgorithmEventHandler NewEnsembleAlgorithm;
        public EnsembleAlgorithmVisualizer()
        {
            try
            {
                InitializeComponent();

                this.tv_EnsembleAlgs.ItemsSource = new ListClusterEnsembleTree();
                this.uctrl_OneEnsembleAlgVis.NewEnsembleAlgorithm += new NewEnsembleAlgorithmEventHandler(NewEnsembleAlg);
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
                if (NewEnsembleAlgorithm != null)
                    NewEnsembleAlgorithm(asender, aNewEnsembleAlg);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void tv_EnsembleAlgs_SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((RadTreeView)sender).SelectedItem != null)
                {
                    Tree _tree = (Tree)((RadTreeView)sender).SelectedItem;
                    this.uctrl_OneEnsembleAlgVis.Tree = _tree;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
                e.Cancel = true;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
    }
}
