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
using Telerik.Windows.Controls;
using ClusteringEnsembleSuite.Code;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.DataStructures;
using System.Collections.ObjectModel;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for MeasuresVisualizer.xaml
    /// </summary>
    public partial class MeasuresVisualizer : Window
    {
        List<Tree> SelectedMeasures { get; set; }
        public event NewMeasuresEventHandler NewMeasuresEventHandler;

        public MeasuresVisualizer()
        {
            try
            {
                InitializeComponent();
                this.tv_Measures.ItemsSource = new ListMeasureTree();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void bt_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool _Ok;
                string _errors = this.uctrl_AllParMeasVis.VerifyErrors(out _Ok);

                if (!_Ok)
                {
                    MessageBox.Show(_errors, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {

                    SelectedMeasures = DeleteAbstract(this.tv_Measures.CheckedItems);

                    List<MeasureInfo> _measuresInfo = new List<MeasureInfo>();
                    for (int i = 0; i < SelectedMeasures.Count; i++)
                    {
                        _measuresInfo.Add(new MeasureInfo()
                        {
                            Tree = SelectedMeasures[i],
                            MeasureName = SelectedMeasures[i].Value.Name
                        });
                    }

                    if (NewMeasuresEventHandler != null)
                    {
                        NewMeasuresEventHandler(this, new NewMeasuresEnventArgs(_measuresInfo));
                    }
                    this.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private List<Tree> DeleteAbstract(ObservableCollection<object> aTrees)
        {
            try
            {
                List<Tree> _result = new List<Tree>();
                foreach (RadTreeViewItem _tree in aTrees)
                    if (!((Tree)_tree.Item).Value.IsAbstract)
                        _result.Add(((Tree)_tree.Item));

                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<Tree>();
            }
        }

        private void tv_Measures_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                this.uctrl_AllParMeasVis.Trees = DeleteAbstract(this.tv_Measures.CheckedItems);
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
