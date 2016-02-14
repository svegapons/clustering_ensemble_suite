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
using Telerik.Windows.Controls;
using System.Windows.Forms;
using ClusterEnsemble.LoadersAndSavers;
using ClusterEnsemble;
using ClusteringEnsembleSuite.Controls.Visualizers;
using ClusterEnsemble.Graphics;
using Telerik.Windows.Controls.Charting;

namespace ClusteringEnsembleSuite
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window, IContainerProgressBar
    {
        public Main()
        {
            try
            {
                InitializeComponent();

                this.tbitem_TabClustering.NewStructuringEventHandler += new NewStructuringEventHandler(this.tbitem_TabEnsemble.NewStructuring);
                this.tbitem_TabClustering.NewStructuringEventHandler += new NewStructuringEventHandler(this.tbitem_TabEvaluation.NewStructuring);
                this.tbitem_TabEnsemble.NewStructuringEventHandler += new NewStructuringEventHandler(this.tbitem_TabEvaluation.NewStructuring);
                this.tbitem_TabClustering.NewStructuringEventHandler += new NewStructuringEventHandler(this.tbitem_TabVisualize.NewStructuring);
                this.tbitem_TabEnsemble.NewStructuringEventHandler += new NewStructuringEventHandler(this.tbitem_TabVisualize.NewStructuring);

                this.tbitem_TabPreprocess.uctrl_Loader.NewSetEventHandler += new NewSetEventHandler(this.tbitem_TabEvaluation.NewSet);
                this.tbitem_TabPreprocess.uctrl_Loader.NewSetEventHandler += new NewSetEventHandler(this.tbitem_TabClustering.NewSet);
                this.tbitem_TabPreprocess.uctrl_Loader.NewSetEventHandler += new NewSetEventHandler(this.tbitem_TabEnsemble.NewSet);
                this.tbitem_TabPreprocess.uctrl_Loader.NewSetEventHandler += new NewSetEventHandler(this.tbitem_TabVisualize.ctrl_partvisualize.NewSet);
                this.tbitem_TabPreprocess.uctrl_Loader.NewSetEventHandler += new NewSetEventHandler(this.tbitem_TabVisualize.NewSet);

                this.tbitem_TabPreprocess.uctrl_Attributes.CheckAttributeEventHandler += new CheckAttributeEventHandler(this.tbitem_TabClustering.this_ClusterAlgVisualizer.CheckAttributes);

                this.tbitem_TabClustering.IContainerProgressBar = this;
                this.tbitem_TabEnsemble.IContainerProgressBar = this;
                this.tbitem_TabEvaluation.IContainerProgressBar = this;
                this.tbitem_TabPreprocess.ctrl_graphic.IContainerProgressBar = this;
                this.tbitem_TabVisualize.IContainerProgressBar = this;
                this.tbitem_TabPreprocess.uctrl_Loader.IContainerProgressBar = this;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void ButtonTool_Click_Open(object sender, RoutedEventArgs e)
        {
            try
            {
                //throw new Exception("HOLA");
               tbitem_TabPreprocess.uctrl_Loader.LoadDataBase();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void ButtonTool_Click_Close(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void ButtonTool_Click_Visualize(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Set> sets = LoadStructurings();
                if (sets != null)
                {
                    PartitionsVisualizerWindow pv = new PartitionsVisualizerWindow(sets);
                    pv.Show();
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private List<Set> LoadStructurings()
        {
            OpenFileDialog _dlg = new OpenFileDialog();
            //_dlg.InitialDirectory = GetContentFolder();
            _dlg.Filter = "ARFF Documents (*.arff)|*.arff";
            _dlg.FilterIndex = 1;
            _dlg.Multiselect = true;

            List<Set> sets = new List<Set>();

            if (_dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {                    

                    foreach (string filename in _dlg.FileNames)
                    {
                        ArffLoader _arffLoader = new ArffLoader(filename);

                        _arffLoader.SetSource(filename);
                        Set s = _arffLoader.Load();

                        sets.Add(s);                        
                    }

                }
                catch (Exception _ex)
                {

                    System.Windows.MessageBox.Show("Corrupt file, try with another.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return null;
                }

                return sets;
            }
            else
                return null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        #region IContainerProgressBar Members

        public RadProgressBar ProgressBar
        {
            get
            {
                return pb_Progress;
            }

        }

        delegate void Reseter(int amin, int amax, bool aIsIndeterminate);
        public void ResetProgressBar(int amin, int amax, bool aIsIndeterminate)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Reseter(ResetPB), amin, amax, aIsIndeterminate);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        public void ResetPB(int amin, int amax, bool aIsIndeterminate)
        {
            try
            {
                grid_gray.Visibility = Visibility.Visible;
                pb_Progress.IsIndeterminate = aIsIndeterminate;
                pb_Progress.Minimum = amin;
                pb_Progress.Maximum = amax;
                pb_Progress.Value = amin;

                tb_state.Text = "Ready";

                tblk_Progress.Text = "";
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        delegate void Finisher();
        public void FinishProgressBar()
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Finisher(FinishPB));
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        public void FinishPB()
        {
            try
            {
                if (!pb_Progress.IsIndeterminate)
                    pb_Progress.Value = pb_Progress.Maximum;

                tb_state.Text = "Ready";
                grid_gray.Visibility = Visibility.Hidden;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        delegate void Updater(int acurrent, string amessage, bool aInderterminate);
        public void UpdateProgressBar(int acurrent, string amessage, bool aInderterminate)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Updater(UpdatePB), acurrent, amessage, aInderterminate);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        public void UpdatePB(int acurrent, string amessage, bool aInderterminate)
        {
            try
            {
                pb_Progress.IsIndeterminate = aInderterminate;
                if (!pb_Progress.IsIndeterminate)
                    pb_Progress.Value = acurrent;
                tb_state.Text = amessage;
                tblk_Progress.Text = "Running... (" + (int)(100 * acurrent / pb_Progress.Maximum) + " %)";
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        delegate void Error(string amessage);
        public void ShowError(string amessage)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Error(ShowE), amessage);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        public void ShowE(string amessage)
        {
            try
            {
                FinishPB();
                System.Windows.MessageBox.Show(amessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }


        #endregion
    }
}
