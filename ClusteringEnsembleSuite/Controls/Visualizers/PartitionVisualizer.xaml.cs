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
using Telerik.Windows.Controls.Charting;
using ClusterEnsemble.LoadersAndSavers;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for PartitionVisualizer.xaml
    /// </summary>
    public partial class PartitionVisualizer : UserControl
    {
        ClusterEnsemble.Attribute att_x, att_y;
        Structuring Struct;
        public bool TabVisualize { get; set; }

        public PartitionVisualizer()
        {
            try
            {
                InitializeComponent();
                //rc_graphic.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Visible;
                //rc_graphic.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Visible;
                rc_graphic.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
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
                tb_alg_name.Text = "";

                Struct = null;
                rc_graphic.SeriesMappings.Reset();
                rc_graphic.ItemsSource = null;

                cb_objetiveX.ItemsSource = null;
                cb_objetiveY.ItemsSource = null;

                if (e.NewSet != null)
                {
                    cb_objetiveX.ItemsSource = e.NewSet.Attributes.Values;
                    cb_objetiveX.DisplayMemberPath = "Name";
                    cb_objetiveY.ItemsSource = e.NewSet.Attributes.Values;
                    cb_objetiveY.DisplayMemberPath = "Name";

                    if (e.NewSet.Attributes.Values.Count > 0)
                    {
                        cb_objetiveX.SelectedIndex = 0;
                        att_x = (ClusterEnsemble.Attribute)cb_objetiveX.SelectedItem;
                        cb_objetiveY.SelectedIndex = 0;
                        att_y = (ClusterEnsemble.Attribute)cb_objetiveY.SelectedItem;
                    }
                    if (att_x != null && att_y != null)
                    {
                        rc_graphic.DefaultView.ChartArea.AxisX.Title = att_x.Name;
                        rc_graphic.DefaultView.ChartArea.AxisY.Title = att_y.Name;
                    }
                    TelerikUtils.SetAnimationsSettings(rc_graphic);
                }

            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void NewStructuring(object sender, NewStructuringEventArgs e)
        {
            try
            {
                Struct = null;
                Struct = e.NewStructuring;
                tb_alg_name.Text = e.NewStructuringInfo.AlgorithmName;

                rc_graphic.SeriesMappings = new SeriesMappingCollection();

                List<SeriesMapping> sm = TelerikUtils.InitBubbleChart(Struct, att_x, att_y);

                foreach (SeriesMapping item in sm)
                {
                    rc_graphic.SeriesMappings.Add(item);
                }

                rc_graphic.ItemsSource = null;
                rc_graphic.ItemsSource = TelerikUtils.FillBubbleChartData(Struct, att_x, att_y);

            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void cb_objetiveX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                att_x = (ClusterEnsemble.Attribute)cb_objetiveX.SelectedItem;

                if (att_x == null || att_y == null)
                    return;
                else
                {
                    rc_graphic.DefaultView.ChartArea.AxisX.Title = att_x.Name;

                    if (Struct != null)
                    {
                        rc_graphic.ItemsSource = null;
                        rc_graphic.ItemsSource = TelerikUtils.FillBubbleChartData(Struct, att_x, att_y);
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void cb_objetiveY_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                att_y = (ClusterEnsemble.Attribute)cb_objetiveY.SelectedItem;

                if (att_x == null || att_y == null)
                    return;
                else
                {
                    rc_graphic.DefaultView.ChartArea.AxisY.Title = att_y.Name;

                    if (Struct != null)
                    {
                        rc_graphic.ItemsSource = null;
                        rc_graphic.ItemsSource = TelerikUtils.FillBubbleChartData(Struct, att_x, att_y);
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Struct != null)
                {
                    System.Windows.Forms.SaveFileDialog _dlg = new System.Windows.Forms.SaveFileDialog();
                    _dlg.Filter = "ARFF Documents (*.arff)|*.arff";
                    _dlg.FilterIndex = 1;


                    string[] names = tb_alg_name.Text.Split(' ');
                    string final_name = "";
                    foreach (string item in names)
                    {
                        final_name += item;
                    }

                    if (TabVisualize)
                        _dlg.FileName = Enviroment.Set.RelationName + "_" + final_name + "_Clusters" + Struct.ClustersCount;
                    else
                        _dlg.FileName = final_name + "_Clusters" + Struct.ClustersCount;

                    if (_dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string dir = _dlg.FileName;
                        ArffSaver saver = new ArffSaver(dir);
                        saver.Save(Struct);
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
    }
}
