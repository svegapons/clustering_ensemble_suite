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
using ClusterEnsemble;
using ClusteringEnsembleSuite.Code;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for ListSetsVisualizer.xaml
    /// </summary>
    public partial class ListSetsVisualizer : UserControl
    {
        List<Set> Sets;
        public event NewSetEventHandler NewSetEventHandler;

        public ListSetsVisualizer()
        {
            try
            {
                InitializeComponent();
                Sets = new List<Set>();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void AddSets(List<Set> sets)
        {
            try
            {
                Sets = sets;
                lv_Sets.ItemsSource = sets;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void SetAllAttributes(bool avalue)
        {
            try
            {
                if (lv_Sets.SelectionMode != SelectionMode.Single)
                    if (avalue)
                        this.lv_Sets.SelectAll();
                    else
                        this.lv_Sets.UnselectAll();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void chb_selectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((CheckBox)sender).IsChecked.Value)
                    SetAllAttributes(true);
                else
                    SetAllAttributes(false);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void lv_Sets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Set actual = (Set)lv_Sets.SelectedItem;
                if (NewSetEventHandler != null)
                {
                    this.NewSetEventHandler(this, new NewSetEventArgs(actual));
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }      
        

       
    }
}
