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
using System.ComponentModel;//Esto es para ListSortDirection

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for ElementsVisualizer.xaml
    /// </summary>
    public partial class ElementsVisualizer : UserControl
    {
        public ElementsVisualizer()
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
        
        Set fset;
        public Set Set
        {
            get { return fset; }
            set
            {
                try
                {
                    this.fset = value;
                    ((GridView)lv_elements.View).Columns.Clear();
                    if (value != null && value.Elements != null && value.ElementsCount > 0)
                    {
                        this.fElements = value.Elements;
                        ((GridView)lv_elements.View).Columns.Clear();
                        AddColumn("Index", "Index");
                        for (int i = 0; i < Set.Attributes.ValuesCount; i++)
                        {
                            AddColumn(Set.Attributes[i].Name, "Values[" + i + "]");
                        }
                        lv_elements.ItemsSource = Set.Elements;
                        g_default.Visibility = Visibility.Hidden;
                    }
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }

        List<Element> fElements;
        public List<Element> Elements
        {
            get { return fElements; }
            set
            {
                try
                {
                    this.fElements = value;
                    ((GridView)lv_elements.View).Columns.Clear();
                    if (Elements != null && Elements.Count > 0)
                    {
                        Attributes _attributes = Elements[0].Attributes;
                        AddColumn("Index", "Index");
                        for (int i = 0; i < _attributes.ValuesCount; i++)
                        {
                            AddColumn(_attributes[i].Name, "Values[" + i + "]");
                        }
                        lv_elements.ItemsSource = Elements;
                        g_default.Visibility = Visibility.Hidden;

                    }
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }
        private void AddColumn(string _columnName, string binding)
        {
            try
            {
                GridViewColumn gvc = new GridViewColumn();
                gvc.DisplayMemberBinding = new Binding(binding);
                gvc.Header = _columnName;
                gvc.Width = 100;

                ((GridView)lv_elements.View).Columns.Add(gvc);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        GridViewColumnHeader fLastHeaderClicked = null;
        ListSortDirection fLastDirection = ListSortDirection.Ascending;
        private void lv_elements_Click(object sender, RoutedEventArgs e)
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
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private void SortElements(string sortBy, ListSortDirection direction)
        {
            try
            {

                if (fElements != null && fElements.Count > 0)
                {
                    if (direction == ListSortDirection.Ascending)
                    {

                        if (sortBy != "Index")
                        {
                            int _AttrIndex = fElements[0].Attributes.Values.FindIndex(_a => _a.Name == sortBy);
                            lv_elements.ItemsSource = fElements.OrderBy(_e => _e[_AttrIndex]);
                        }
                        else
                            lv_elements.ItemsSource = fElements.OrderBy(_e => _e.Index);
                        //switch (fElements[0].Attributes[_AttrIndex].AttributeType)
                        //{
                        //    case AttributeType.Numeric:
                        //        lv_elements.ItemsSource = fElements.OrderBy(_e => (double)_e[_AttrIndex]);
                        //        break;
                        //    case AttributeType.Nominal:
                        //    case AttributeType.String:
                        //        lv_elements.ItemsSource = fElements.OrderBy(_e => (string)_e[_AttrIndex]);
                        //        break;
                        //    case AttributeType.Date:
                        //        lv_elements.ItemsSource = fElements.OrderBy(_e => (DateTime)_e[_AttrIndex]);
                        //        break;
                        //}

                    }
                    else if (direction == ListSortDirection.Descending)
                    {
                        if (sortBy != "Index")
                        {
                            int _AttrIndex = fElements[0].Attributes.Values.FindIndex(_a => _a.Name == sortBy);
                            lv_elements.ItemsSource = fElements.OrderByDescending(_e => _e[_AttrIndex]);
                        }
                        else
                            lv_elements.ItemsSource = fElements.OrderByDescending(_e => _e.Index);

                        //switch (fElements[0].Attributes[_AttrIndex].AttributeType)
                        //{
                        //    case AttributeType.Numeric:
                        //        lv_elements.ItemsSource = fElements.OrderByDescending(_e => (double)_e[_AttrIndex]);
                        //        break;
                        //    case AttributeType.Nominal:
                        //    case AttributeType.String:
                        //        lv_elements.ItemsSource = fElements.OrderByDescending(_e => (string)_e[_AttrIndex]);
                        //        break;
                        //    case AttributeType.Date:
                        //        lv_elements.ItemsSource = fElements.OrderByDescending(_e => (DateTime)_e[_AttrIndex]);
                        //        break;
                        //}


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
