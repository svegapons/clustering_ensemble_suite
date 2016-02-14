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
using CEDS = ClusterEnsemble.DataStructures;
using ClusterEnsemble.Clusters;
using ClusterEnsemble.Reflection;
using ClusterEnsemble;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.Evaluation;
using System.ComponentModel;
using ClusteringEnsembleSuite.Controls.Visualizers;
using ClusterEnsemble.Graphics;
using System.IO;

namespace ClusteringEnsembleSuite.Controls.TabsControls
{
    /// <summary>
    /// Interaction logic for TabEvaluation.xaml
    /// </summary>
    public partial class TabEvaluation : UserControl,INeedProgressBar
    {
        List<MeasureInfo> MeasuresSelected { get; set; }
        ClusterEnsemble.Attribute Att_objetive { get; set; }
        private bool HasSelectedMeasures { get; set; }
        public MeasuresVisualizer this_MeasureVisualizer { get; set; }

        public TabEvaluation()
        {
            try
            {
                InitializeComponent();

                this_MeasureVisualizer = new MeasuresVisualizer();
                this.this_MeasureVisualizer.NewMeasuresEventHandler += new NewMeasuresEventHandler(NewMeasuresSelected);
                this.MeasuresSelected = new List<MeasureInfo>();

                this.uctrl_ListClusterAlgVisualizerClustEnsemble.GroupBoxHeader = "Algorithms";
                //this.uctrl_ListClusterAlgVisualizerClustEnsemble.ListViewSelectionMode = SelectionMode.Single;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        /// <summary>
        /// Este metodo se ejecuta cuando el TabClustering o el TabEnsemble lanza el evento de que hay una nueva particion
        /// </summary>
        /// <param name="asender"></param>
        /// <param name="e"></param>
        public void NewStructuring(object asender, NewStructuringEventArgs e)
        {
            try
            {
                this.uctrl_ListClusterAlgVisualizerClustEnsemble.AddPartitionInfo(e.NewStructuringInfo);
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
                this.uctrl_ListClusterAlgVisualizerClustEnsemble.ResetPartitionInfo();
                this.lv_Measures.ItemsSource = null;
                this.lv_MeasuresOutput.ItemsSource = null;
                ((GridView)lv_MeasuresOutput.View).Columns.Clear();
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

        private void NewMeasuresSelected(object asender, NewMeasuresEnventArgs e)
        {
            try
            {
                this.MeasuresSelected = e.NewMeasures;
                Update_ListViewMeasures();
                this.this_MeasureVisualizer.Visibility = Visibility.Hidden;

                //Actualizar HasSelectedMeasures
                this.HasSelectedMeasures = (e.NewMeasures.Count > 0) ? true : false;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void Update_ListViewMeasures()
        {
            try
            {
                this.lv_Measures.ItemsSource = null;
                this.lv_Measures.ItemsSource = MeasuresSelected;

                this.chb_selectAll.IsChecked = true;
                this.lv_Measures.SelectAll();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void tb_SelectMeasureAlg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.this_MeasureVisualizer.ShowDialog();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }


        private void bt_Run_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HasSelectedMeasures)
                {
                    List<PartitionInfo> _partitionsInfo = this.uctrl_ListClusterAlgVisualizerClustEnsemble.GetSelected();
                    string _Error = "";
                    List<MeasureInfo> _MeasuresChecked = GetMeasuresChecked();
                    if (_MeasuresChecked == null || _MeasuresChecked.Count == 0)
                    {
                        _Error = "You must select at least one Validation Index.";
                        MessageBox.Show(_Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (_partitionsInfo == null || _partitionsInfo.Count == 0)
                    {
                        _Error = "You must select at least one Structuring to apply an Validation Index.";
                        MessageBox.Show(_Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Structuring _realpartition = RealPartitionBuilder.BuildRealPartition(Enviroment.Set, Att_objetive);
                    List<MeasureOutput> _measuresOutput = new List<MeasureOutput>();
                    List<Measure> _measures = new List<Measure>();

                    ReflectionTools _rct = new ReflectionTools();
                    for (int i = 0; i < _MeasuresChecked.Count; i++)
                    {
                        Measure _TempMeasure = ReflectionTools.GetInstance<Measure>(_MeasuresChecked[i].Tree.Value.FullName);
                        _measures.Add(_TempMeasure);
                        foreach (CEDS.Property _p in _MeasuresChecked[i].Tree.Value.InProperties)
                        {
                            _rct.SetProperty(_MeasuresChecked[i].Tree.Value.FullName, _TempMeasure, _p);
                        }


                    }

                    Run _run = RunMethod;
                    _run.BeginInvoke(_partitionsInfo, _measures, _realpartition, _measuresOutput, _MeasuresChecked, RunFinish, _run);
                }
                else
                    MessageBox.Show("You must first select a Validation Index.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        //Todos estos Metodos son para que funcione el ProgressBar
        private delegate List<MeasureOutput> Run(List<PartitionInfo> _partitionsInfo, List<Measure> _measures, Structuring _realpartition, List<MeasureOutput> _measuresOutput, List<MeasureInfo> _MeasuresChecked);
        private List<MeasureOutput> RunMethod(List<PartitionInfo> _partitionsInfo, List<Measure> _measures, Structuring _realpartition, List<MeasureOutput> _measuresOutput, List<MeasureInfo> _MeasuresChecked)
        {
            try
            {
                int _current=1;
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ResetProgressBar(1, _partitionsInfo.Count * _measures.Count + _measures.Count, false);

                for (int i = 0; i < _partitionsInfo.Count; i++)
                {
                    double[] _values = new double[_measures.Count];
                    //Evaluar cada Medida según la partición correspondiente
                    for (int k = 0; k < _measures.Count; k++)
                    {
                        if (IContainerProgressBar != null)
                            IContainerProgressBar.UpdateProgressBar(_current++, "Computing Measure " + _measures[k].Name + " in Partition " + _partitionsInfo[i].AlgorithmName, false);

                        Measure _measureTemp = _measures[k];
                        _measureTemp.Set = Enviroment.Set;
                        _measureTemp.Structuring = _partitionsInfo[i].Partition;
                        _measureTemp.RealPartition = _realpartition;
                        _measureTemp.ObjetiveAttribute = Att_objetive;

                        _values[k] = _measureTemp.EvaluatePartition();

                        _values[k] = Math.Round(_values[k], 4);
                    }

                    _measuresOutput.Add(new MeasureOutput()
                    {
                        AlgorithmName = _partitionsInfo[i].AlgorithmName,
                        Measures = _MeasuresChecked,
                        Partition = _partitionsInfo[i].Partition,
                        Values = _values
                    });
                }

                {
                    //Evaluar la particion real, para poder comparar los valores de las demas particiones con respecto a los valores de la real
                    double[] _valuesRealPartition = new double[_measures.Count];
                    //Evaluar cada Medida según la partición correspondiente
                    for (int k = 0; k < _measures.Count; k++)
                    {
                        if (IContainerProgressBar != null)
                            IContainerProgressBar.UpdateProgressBar(_current++, "Computing Measure " + _measures[k].Name + " in Real Partition", false);

                        Measure _measureTemp = _measures[k];
                        _measureTemp.Set = Enviroment.Set;
                        _measureTemp.Structuring = _realpartition;
                        _measureTemp.RealPartition = _realpartition;
                        _measureTemp.ObjetiveAttribute = Att_objetive;

                        _valuesRealPartition[k] = _measureTemp.EvaluatePartition();

                        _valuesRealPartition[k] = Math.Round(_valuesRealPartition[k], 4);
                    }

                    _measuresOutput.Add(new MeasureOutput()
                    {
                        AlgorithmName = "Real Partition",
                        Measures = _MeasuresChecked,
                        Partition = _realpartition,
                        Values = _valuesRealPartition
                    });
                }
                if (IContainerProgressBar != null)
                    IContainerProgressBar.FinishProgressBar();

                return _measuresOutput;
            }
            catch(Exception ex)
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in Measures");

                return null;

            }
        }
        private delegate void RunF(List<MeasureOutput> _measuresOutput);
        private void RunFinish(IAsyncResult aIAsyncResult)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new RunF(RunFinish), ((Run)aIAsyncResult.AsyncState).EndInvoke(aIAsyncResult));
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void RunFinish(List<MeasureOutput> _measuresOutput)
        {
            try
            {
                if (_measuresOutput != null)
                    Update_ListViewMeasuresOutput(_measuresOutput);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        //=========================================================

        private List<MeasureInfo> GetMeasuresChecked()
        {
            try
            {
                List<MeasureInfo> _result = new List<MeasureInfo>();

                foreach (MeasureInfo _mi in this.lv_Measures.SelectedItems)
                    _result.Add(_mi);

                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<MeasureInfo>();
            }
        }
        private void Update_ListViewMeasuresOutput(List<MeasureOutput> aMeasuresOutput)
        {
            try
            {
                ((GridView)lv_MeasuresOutput.View).Columns.Clear();

                if (aMeasuresOutput.Count > 0)
                {
                    MeasureOutput _measureOutput = aMeasuresOutput[0];
                    //Esta es la columna que muestra cual es el algoritmo que se le aplico a la particion en cuestion
                    AddColumn("Algorithm", "AlgorithmName");
                    for (int i = 0; i < _measureOutput.Measures.Count; i++)
                    {
                        AddColumn(_measureOutput.Measures[i].MeasureName, "Values[" + i + "]");
                    }
                    this.lv_MeasuresOutput.ItemsSource = aMeasuresOutput;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void AddColumn(string aColumnHeader, string aBinding)
        {
            try
            {
                GridViewColumn gvc = new GridViewColumn();
                gvc.DisplayMemberBinding = new Binding(aBinding);
                gvc.Header = aColumnHeader;
                gvc.Width = 100;

                ((GridView)lv_MeasuresOutput.View).Columns.Add(gvc);
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
                if (avalue)
                    lv_Measures.SelectAll();
                else
                    lv_Measures.UnselectAll();
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

        GridViewColumnHeader fLastHeaderClicked_Measures = null;
        ListSortDirection fLastDirection_Measures = ListSortDirection.Ascending;
        private void lv_Measures_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
                ListSortDirection direction;

                if (headerClicked != null)
                {
                    if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                    {
                        if (headerClicked != fLastHeaderClicked_Measures)
                            direction = ListSortDirection.Ascending;
                        else
                            if (fLastDirection_Measures == ListSortDirection.Ascending)
                                direction = ListSortDirection.Descending;
                            else
                                direction = ListSortDirection.Ascending;

                        string header = headerClicked.Column.Header as string;

                        if (header != "State")
                        {
                            SortElements_Measures(header, direction);

                            if (direction == ListSortDirection.Ascending)
                                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                            else
                                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;

                            // Remove arrow from previously sorted header
                            if (fLastHeaderClicked_Measures != null && fLastHeaderClicked_Measures != headerClicked)
                                fLastHeaderClicked_Measures.Column.HeaderTemplate = null;

                            fLastHeaderClicked_Measures = headerClicked;
                            fLastDirection_Measures = direction;
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void SortElements_Measures(string sortBy, ListSortDirection direction)
        {
            try
            {
                if (MeasuresSelected != null && sortBy != "State")
                {
                    IOrderedEnumerable<MeasureInfo> _MeasuresSelected = null;
                    if (direction == ListSortDirection.Ascending)
                    {
                        switch (sortBy)
                        {
                            case "Measure":
                                _MeasuresSelected = MeasuresSelected.OrderBy(_mi => _mi.MeasureName);
                                break;
                        }
                    }
                    else if (direction == ListSortDirection.Descending)
                    {
                        switch (sortBy)
                        {
                            case "Measure":
                                _MeasuresSelected = MeasuresSelected.OrderByDescending(_mi => _mi.MeasureName);
                                break;
                        }
                    }
                    //Actualizar MeasuresSelected con la nueva lista ordenada
                    MeasuresSelected = new List<MeasureInfo>();
                    foreach (MeasureInfo _mi in _MeasuresSelected)
                        MeasuresSelected.Add(_mi);

                    this.lv_Measures.ItemsSource = null;
                    this.lv_Measures.ItemsSource = MeasuresSelected;
                    if (MeasuresSelected.Count > 0)
                        this.chb_selectAll.IsChecked = true;
                    this.lv_Measures.SelectAll();
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        GridViewColumnHeader fLastHeaderClicked_MeasuresOutput = null;
        ListSortDirection fLastDirection_MeasuresOutput = ListSortDirection.Ascending;
        private void lv_MeasuresOutput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
                ListSortDirection direction;

                if (headerClicked != null)
                {
                    if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                    {
                        if (headerClicked != fLastHeaderClicked_MeasuresOutput)
                            direction = ListSortDirection.Ascending;
                        else
                            if (fLastDirection_MeasuresOutput == ListSortDirection.Ascending)
                                direction = ListSortDirection.Descending;
                            else
                                direction = ListSortDirection.Ascending;

                        string header = headerClicked.Column.Header as string;

                        if (header != "State")
                        {
                            SortElements_MeasuresOutput(header, direction);

                            if (direction == ListSortDirection.Ascending)
                                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                            else
                                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;

                            // Remove arrow from previously sorted header
                            if (fLastHeaderClicked_MeasuresOutput != null && fLastHeaderClicked_MeasuresOutput != headerClicked)
                                fLastHeaderClicked_MeasuresOutput.Column.HeaderTemplate = null;

                            fLastHeaderClicked_MeasuresOutput = headerClicked;
                            fLastDirection_MeasuresOutput = direction;
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void SortElements_MeasuresOutput(string sortBy, ListSortDirection direction)
        {
            try
            {
                List<MeasureOutput> _Temp_MeasuresOutput = (List<MeasureOutput>)this.lv_MeasuresOutput.ItemsSource;
                IOrderedEnumerable<MeasureOutput> _MO = null;

                if (_Temp_MeasuresOutput != null && _Temp_MeasuresOutput.Count > 0)
                {
                    if (sortBy == "Algorithm")
                    {
                        if (direction == ListSortDirection.Ascending)
                        {
                            _MO = _Temp_MeasuresOutput.OrderBy(_mo => _mo.AlgorithmName);
                        }
                        else if (direction == ListSortDirection.Descending)
                        {
                            _MO = _Temp_MeasuresOutput.OrderByDescending(_mo => _mo.AlgorithmName);
                        }
                    }
                    else//se hizo click sobre una columna que ya es una medida, es una columna de valores
                    {
                        int _index = _Temp_MeasuresOutput[0].Measures.FindIndex(_mi => _mi.MeasureName == sortBy);

                        if (direction == ListSortDirection.Ascending)
                        {
                            _MO = _Temp_MeasuresOutput.OrderBy(_mo => _mo.Values[_index]);
                        }
                        else if (direction == ListSortDirection.Descending)
                        {
                            _MO = _Temp_MeasuresOutput.OrderByDescending(_mo => _mo.Values[_index]);
                        }
                    }

                    List<MeasureOutput> _MeasuresOutput = new List<MeasureOutput>();
                    foreach (var item in _MO)
                        _MeasuresOutput.Add(item);

                    this.lv_MeasuresOutput.ItemsSource = null;
                    this.lv_MeasuresOutput.ItemsSource = _MeasuresOutput;
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


        //ADDED
        /// <summary>
        /// Handles the Click event of the bt_Save control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void bt_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog _ofd = new System.Windows.Forms.SaveFileDialog() { Filter = "Tabla|*.csv" };
                if (_ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string _path = _ofd.FileName;
                    FileStream _fs = new FileStream(_path, FileMode.Create, FileAccess.Write);
                    StreamWriter _sw = new StreamWriter(_fs);

                    try
                    {
                        //Columns
                        for (int i = 0; i < ((GridView)lv_MeasuresOutput.View).Columns.Count; i++)
                        {
                            _sw.Write(((GridView)lv_MeasuresOutput.View).Columns[i].Header.ToString().Replace(',', '-') + ", ");
                        }
                        _sw.Write("Time, ElementsCount, Bell Number");
                        _sw.WriteLine();

                        List<PartitionInfo> _pinfo = this.uctrl_ListClusterAlgVisualizerClustEnsemble.GetSelected();

                        //Fill
                        for (int i = 0; i < lv_MeasuresOutput.Items.Count; i++)
                        {
                            MeasureOutput _mo = (MeasureOutput)lv_MeasuresOutput.Items[i];
                            string _line = "";
                            _line += _mo.AlgorithmName.Replace(',', ';') + ", ";
                            for (int j = 0; j < _mo.Values.Length; j++)
                            {
                                _line += _mo.Values[j].ToString() + ", ";
                            }
                            _line += i == lv_MeasuresOutput.Items.Count - 1 ? " , ," : _pinfo[i].Time + ", " + _pinfo[i].ElementCount + ", " + _pinfo[i].SearchSpace;
                           _sw.WriteLine(_line);
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
 
                        _sw.Close();
                        _fs.Close();
                    }
                   

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
