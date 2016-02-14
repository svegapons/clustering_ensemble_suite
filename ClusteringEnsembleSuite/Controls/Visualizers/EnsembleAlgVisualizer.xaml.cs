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
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Reflection;
using ClusteringEnsembleSuite.Code;
using ClusterEnsemble;

//Added
using Telerik.Windows.Controls;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for EnsembleAlgVisualizer.xaml
    /// </summary>
    public partial class EnsembleAlgVisualizer : UserControl
    {
        public event NewEnsembleAlgorithmEventHandler NewEnsembleAlgorithm;
        public EnsembleAlgVisualizer()
        {
            InitializeComponent();

            this.tv_EnsembleAlgs.ItemsSource = new ListClusterEnsembleTree();
            this.uctrl_OneEnsembleAlgVis.NewEnsembleAlgorithm += new NewEnsembleAlgorithmEventHandler(NewEnsembleAlg);
             
        }
       
        private void NewEnsembleAlg(object asender, NewEnsembleAlgorithmEventArgs aNewEnsembleAlg)
        {
            if (NewEnsembleAlgorithm != null)
                NewEnsembleAlgorithm(asender, aNewEnsembleAlg);
        }
        
        private void bt_Hidden_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void tv_EnsembleAlgs_SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((RadTreeView)sender).SelectedItem != null)
            {
                Tree _tree = (Tree)((RadTreeView)sender).SelectedItem;
                this.uctrl_OneEnsembleAlgVis.Tree = _tree;
            }
        }

    }
}
