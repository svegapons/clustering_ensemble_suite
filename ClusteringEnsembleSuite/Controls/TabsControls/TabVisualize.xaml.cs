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
using ClusteringEnsembleSuite.Code;
using ClusterEnsemble;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.Graphics;

namespace ClusteringEnsembleSuite.Controls.TabsControls
{
    /// <summary>
    /// Interaction logic for TabVisualize.xaml
    /// </summary>
    public partial class TabVisualize : UserControl, INeedProgressBar
    {
        ClusterEnsemble.Attribute Att_objetive { get; set; }
        public event NewStructuringEventHandler NewStructuringEventHandler;

        public TabVisualize()
        {
            try
            {
                InitializeComponent();

                //Cuando se le de click de Visualize a uno de los botones de las particiones
                this.uctrl_ListClusterAlgVisualizer.NewStructuringEventHandler += new NewStructuringEventHandler(this.ctrl_partvisualize.NewStructuring);
                //Cuando se le de Visualize a la Particion Real
                this.NewStructuringEventHandler += new NewStructuringEventHandler(this.ctrl_partvisualize.NewStructuring);

                uctrl_ListClusterAlgVisualizer.GroupBoxHeader = "Algorithms";
                uctrl_ListClusterAlgVisualizer.TabVisualize = true;
                //uctrl_ListClusterAlgVisualizer.ListViewSelectionMode = SelectionMode.Single;

                ctrl_partvisualize.TabVisualize = true;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void NewStructuring(object asender, NewStructuringEventArgs e)
        {
            try
            {
                this.uctrl_ListClusterAlgVisualizer.AddPartitionInfo(e.NewStructuringInfo);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void cb_Objetive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_objetive.SelectedIndex != -1)
                {
                    Att_objetive = (ClusterEnsemble.Attribute)cb_objetive.SelectedItem;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        /// <summary>
        /// Este metodo se ejecuta cuando el Loader lanza el evento de que hay un nuevo Set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewSet(object sender, NewSetEventArgs e)
        {
            try
            {
                this.uctrl_ListClusterAlgVisualizer.ResetPartitionInfo();

                cb_objetive.ItemsSource = null;
                if (e.NewSet != null)
                {
                    cb_objetive.ItemsSource = e.NewSet.Attributes.Values;
                    cb_objetive.DisplayMemberPath = "Name";
                    if (e.NewSet.Attributes.Values.Count > 0)
                        cb_objetive.SelectedIndex = cb_objetive.Items.Count - 1;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NewStructuringEventHandler != null)
                {
                    if (Enviroment.Set != null)
                    {
                        Structuring s = RealPartitionBuilder.BuildRealPartition(Enviroment.Set, Att_objetive);
                        NewStructuringEventHandler(this, new NewStructuringEventArgs(new PartitionInfo() { AlgorithmName = "Real Partition", Partition = s }));
                    }
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
    }
}
