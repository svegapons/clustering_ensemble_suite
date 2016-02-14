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
using ClusterEnsemble;
using ClusteringEnsembleSuite.Code;
using ClusteringEnsembleSuite.Code.DataStructures;
using System.ComponentModel;


namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for ListClusterAlgVIsualizer.xaml
    /// </summary>
    public partial class ListClusterAlgVIsualizer : UserControl
    {
        List<CopyPartitionInfo> CopyPartitionInfoList { get; set; }

        //Esto es para cuando este en el tabVisualizae que el control de visualizar los clusters se entere
        public bool TabVisualize { get; set; }
        public event NewStructuringEventHandler NewStructuringEventHandler;

        public ListClusterAlgVIsualizer()
        {
            try
            {
                InitializeComponent();
                this.CopyPartitionInfoList = new List<CopyPartitionInfo>();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        public void AddPartitionInfo(PartitionInfo aPartitionInfo)
        {
            try
            {
                CopyPartitionInfo _copy = new CopyPartitionInfo() { Index = this.CopyPartitionInfoList.Count, PartitionInfo = aPartitionInfo };
                this.CopyPartitionInfoList.Add(_copy);

                this.lv_Alg.ItemsSource = null;
                this.lv_Alg.ItemsSource = CopyPartitionInfoList;

                this.chb_selectAll.IsChecked = true;

                if (lv_Alg.SelectionMode != SelectionMode.Single)
                    this.lv_Alg.SelectAll();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        public void ResetPartitionInfo()
        {
            try
            {
                this.CopyPartitionInfoList = new List<CopyPartitionInfo>();
                this.lv_Alg.ItemsSource = null;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public string GroupBoxHeader
        {
            set
            {
                try
                {
                    this.gb.Header = value;
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }
        public SelectionMode ListViewSelectionMode
        {
            set
            {
                try
                {
                    this.lv_Alg.SelectionMode = value;

                    if (value == SelectionMode.Single)
                    {
                        this.chb_selectAll.Visibility = Visibility.Hidden;
                        this.chb_selectAll.Height = 0;
                    }
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }
        public List<PartitionInfo> GetSelected()
        {
            try
            {
                List<PartitionInfo> _result = new List<PartitionInfo>();
                foreach (CopyPartitionInfo _copypartInfo in this.lv_Alg.SelectedItems)
                    _result.Add(_copypartInfo.PartitionInfo);

                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<PartitionInfo>();
            }
        }
        public List<int> GetSelectedIndexs()
        {
            try
            {
                List<int> _result = new List<int>();

                for (int i = 0; i < this.lv_Alg.SelectedItems.Count; i++)
                    _result.Add(((CopyPartitionInfo)this.lv_Alg.SelectedItems[i]).Index);

                _result.Sort();
                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<int>();
            }
        }

        private void SetAllAttributes(bool avalue)
        {
            try
            {
                if (lv_Alg.SelectionMode != SelectionMode.Single)
                    if (avalue)
                        this.lv_Alg.SelectAll();
                    else
                        this.lv_Alg.UnselectAll();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!TabVisualize)
                {
                    List<PartitionInfo> _partInfo = new List<PartitionInfo>();
                    this.CopyPartitionInfoList.ForEach(_cpi => _partInfo.Add(_cpi.PartitionInfo));
                    ClusterDatails cd = new ClusterDatails(_partInfo, (int)((Button)sender).Tag);
                    cd.ShowDialog();
                }
                else
                {
                    if (NewStructuringEventHandler != null)
                    {
                        CopyPartitionInfo temp = CopyPartitionInfoList[(int)((Button)sender).Tag];
                        NewStructuringEventHandler(this, new NewStructuringEventArgs(temp.PartitionInfo));
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void bt_removeSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> _Indexs = GetSelectedIndexs();
                if (_Indexs.Count > 0)
                {
                    //CopyPartitionInfoList.RemoveAll(aCPI => _Indexs.Contains(aCPI.Index));
                    for (int i = 0; i < _Indexs.Count; i++)
                        CopyPartitionInfoList.RemoveAt(_Indexs[i] - i);

                    UpdateIndex();
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void UpdateIndex()
        {
            try
            {
                for (int i = 0; i < CopyPartitionInfoList.Count; i++)
                    CopyPartitionInfoList[i].Index = i;

                this.lv_Alg.ItemsSource = null;
                this.lv_Alg.ItemsSource = CopyPartitionInfoList;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        GridViewColumnHeader fLastHeaderClicked = null;
        ListSortDirection fLastDirection = ListSortDirection.Ascending;
        private void lv_Alg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
                ListSortDirection direction;

                if (headerClicked != null)
                {
                    if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                    {
                        if (headerClicked != fLastHeaderClicked)
                            direction = ListSortDirection.Ascending;
                        else
                            if (fLastDirection == ListSortDirection.Ascending)
                                direction = ListSortDirection.Descending;
                            else
                                direction = ListSortDirection.Ascending;

                        string header = headerClicked.Column.Header as string;

                        if (header != "State" && header != "Visualize" && header != "Index")
                        {
                            SortElements(header, direction);

                            if (direction == ListSortDirection.Ascending)
                                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                            else
                                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;

                            // Remove arrow from previously sorted header
                            if (fLastHeaderClicked != null && fLastHeaderClicked != headerClicked)
                                fLastHeaderClicked.Column.HeaderTemplate = null;

                            fLastHeaderClicked = headerClicked;
                            fLastDirection = direction;
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void SortElements(string sortBy, ListSortDirection direction)
        {
            try
            {
                if (CopyPartitionInfoList != null && sortBy != "State" && sortBy != "Visualize" && sortBy != "Index")
                {
                    IOrderedEnumerable<CopyPartitionInfo> _CopyPartitionInfoList = null;

                    if (direction == ListSortDirection.Ascending)
                    {
                        switch (sortBy)
                        {
                            //case "Index":
                            //    _CopyPartitionInfoList = CopyPartitionInfoList.OrderBy(_cpi => _cpi.Index);
                            //    break;
                            case "Algorithm Type":
                                _CopyPartitionInfoList = CopyPartitionInfoList.OrderBy(_cpi => _cpi.PartitionInfo.AlgorithmType);
                                break;
                            case "Algorithm":
                                _CopyPartitionInfoList = CopyPartitionInfoList.OrderBy(_cpi => _cpi.PartitionInfo.AlgorithmName);
                                break;
                        }
                    }
                    else if (direction == ListSortDirection.Descending)
                    {
                        switch (sortBy)
                        {
                            //case "Index":
                            //    _CopyPartitionInfoList = CopyPartitionInfoList.OrderByDescending(_cpi => _cpi.Index);
                            //    break;
                            case "Algorithm Type":
                                _CopyPartitionInfoList = CopyPartitionInfoList.OrderByDescending(_cpi => _cpi.PartitionInfo.AlgorithmType);
                                break;
                            case "Algorithm":
                                _CopyPartitionInfoList = CopyPartitionInfoList.OrderByDescending(_cpi => _cpi.PartitionInfo.AlgorithmName);
                                break;
                        }
                    }

                    //Actualizar CopyPartitionInfoList con la nueva lista ordenada
                    CopyPartitionInfoList = new List<CopyPartitionInfo>();
                    foreach (CopyPartitionInfo _cpi in _CopyPartitionInfoList)
                        CopyPartitionInfoList.Add(_cpi);
                    UpdateIndex();

                    this.lv_Alg.ItemsSource = null;
                    this.lv_Alg.ItemsSource = CopyPartitionInfoList;
                    if (CopyPartitionInfoList.Count > 0)
                        this.chb_selectAll.IsChecked = true;
                    this.lv_Alg.SelectAll();
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

    }
}
