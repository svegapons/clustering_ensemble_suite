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
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls;
using ClusterEnsemble;
using ClusterEnsemble.Graphics;
using System.Threading;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for AttributesGraphicsVisualizer.xaml
    /// </summary>
    public partial class AttributesGraphicsVisualizer : UserControl, INeedProgressBar
    {
        ClusterEnsemble.Attribute att_objetive;
        public AttributesGraphicsVisualizer()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void UpdateSet(Set set)
        {
            try
            {
                sp_cboxs.Visibility = Visibility.Visible;

                Cb_objetive.ItemsSource = null;
                Cb_objetive.ItemsSource = set.Attributes.Values;
                Cb_objetive.DisplayMemberPath = "Name";

                if (set.Attributes.Values.Count > 0)
                    Cb_objetive.SelectedIndex = Cb_objetive.Items.Count - 1; ;

                TelerikUtils.SetAnimationsSettings(Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void Bar3DChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                SeriesMapping _series = TelerikUtils.InitChart3D(att_objetive, Rc_att);
                Rc_att.SeriesMappings.Clear();
                Rc_att.SeriesMappings.Add(_series);

                List<DataPoint> _temp = TelerikUtils.FillChartData(att_objetive);
                Rc_att.ItemsSource = null;
                Rc_att.ItemsSource = _temp;

                TelerikUtils.ChartAreaZoom(Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void Bar3DUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                SeriesMapping _series = TelerikUtils.InitChart2D(att_objetive, Rc_att);
                Rc_att.SeriesMappings.Clear();
                Rc_att.SeriesMappings.Add(_series);

                List<DataPoint> _temp = TelerikUtils.FillChartData(att_objetive);
                Rc_att.ItemsSource = null;
                Rc_att.ItemsSource = _temp;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void ShowLabelsChecked(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void ShowToolTipsChecked(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void AxisXVisibilityChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox temp = sender as CheckBox;
                TelerikUtils.SetAxisXVisibility(temp, Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void AxisYVisibilityChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox temp = sender as CheckBox;
                TelerikUtils.SetAxisYVisibility(temp, Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void AxisXGridLinesChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox temp = sender as CheckBox;
                TelerikUtils.SetAxisXGridLinesVisibility(temp, Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void AxisYGridLinesChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox temp = sender as CheckBox;
                TelerikUtils.SetAxisYGridLinesVisibility(temp, Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void AxisXStripLinesChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox temp = sender as CheckBox;
                TelerikUtils.SetAxisXStripLinesVisibility(temp, Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void AxisYStripLinesChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox temp = sender as CheckBox;
                TelerikUtils.SetAxisYStripLinesVisibility(temp, Rc_att);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private delegate void Updater(List<DataPoint> _temp);
        private void RadComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cb_objetive.SelectedIndex != -1)
                {
                    att_objetive = (ClusterEnsemble.Attribute)Cb_objetive.SelectedItem;
                    Rc_att.DefaultView.ChartArea.AxisX.Title = att_objetive.Name;
                    SeriesMapping _series = TelerikUtils.InitChart(att_objetive, Rc_att, DCheckBox);
                    Rc_att.SeriesMappings.Clear();
                    Rc_att.SeriesMappings.Add(_series);
                    List<DataPoint> _temp = TelerikUtils.FillChartData(att_objetive);

                    if ((bool)DCheckBox.IsChecked)
                        TelerikUtils.ChartAreaZoom(Rc_att);

                    Rc_att.ItemsSource = null;

                    //if (IContainerProgressBar != null)
                    //{
                    //    IContainerProgressBar.ResetProgressBar(1, 1, true);
                    //    IContainerProgressBar.UpdateProgressBar(1, "Loading Data Series...", true);
                    //}

                    Rc_att.ItemsSource = _temp;

                    //if (IContainerProgressBar != null)
                    //    IContainerProgressBar.FinishPB();


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
