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
using ClusterEnsemble;
using ClusteringEnsembleSuite.Code;
using ClusteringEnsembleSuite.Code.DataStructures;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for PartitionsVisualizerWindow.xaml
    /// </summary>
    public partial class PartitionsVisualizerWindow : Window
    {
        List<Set> Sets;
        Set Actual_set;
        ClusterEnsemble.Attribute att_obj;

        public event NewStructuringEventHandler NewStructuringEventHandler;

        public PartitionsVisualizerWindow()
        {
            try
            {
                InitializeComponent();
                this.uctrl_ListSetsVisualizer.NewSetEventHandler += new NewSetEventHandler(this.NewSet);
                this.uctrl_ListSetsVisualizer.NewSetEventHandler += new NewSetEventHandler(this.ctrl_partvisualize.NewSet);

                this.NewStructuringEventHandler += new NewStructuringEventHandler(this.ctrl_partvisualize.NewStructuring);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public PartitionsVisualizerWindow(List<Set> sets)
            : this()
        {
            try
            {
                Sets = sets;
                uctrl_ListSetsVisualizer.AddSets(sets);
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
                    att_obj = (ClusterEnsemble.Attribute)cb_objetive.SelectedItem;
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
                if (Actual_set != null)
                {
                    Structuring s = RealPartitionBuilder.BuildRealPartition(Actual_set, att_obj);
                    if (NewStructuringEventHandler != null)
                        NewStructuringEventHandler(this, new NewStructuringEventArgs(new PartitionInfo() { AlgorithmName = Actual_set.RelationName, Partition = s }));
                }
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
                cb_objetive.ItemsSource = null;
                Actual_set = null;
                att_obj = null;

                if (e.NewSet != null)
                {
                    Actual_set = e.NewSet;
                    cb_objetive.DisplayMemberPath = "Name";
                    cb_objetive.ItemsSource = null;
                    cb_objetive.ItemsSource = Actual_set.Attributes.Values;

                    if (e.NewSet.Attributes.Values.Count > 0)
                        cb_objetive.SelectedIndex = cb_objetive.Items.Count - 1;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
    }
}
