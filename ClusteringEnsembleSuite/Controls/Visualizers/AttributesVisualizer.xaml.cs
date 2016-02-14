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
using System.ComponentModel;//Esto es para ListSortDirection

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for AttributesVisualizer.xaml
    /// </summary>
    public partial class AttributesVisualizer : UserControl
    {
        Attributes fattributes;
        public event CheckAttributeEventHandler CheckAttributeEventHandler;

        public AttributesVisualizer()
        {
            try
            {
                InitializeComponent();
                this.CheckAttributeEventHandler += new CheckAttributeEventHandler(Enviroment.CheckAttribute);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public AttributesVisualizer(Attributes aattributes)
            : this()
        {
            try
            {
                this.Attributes = aattributes;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public Attributes Attributes
        {
            get
            {
                return fattributes;
            }
            set
            {
                try
                {
                    this.fattributes = value;

                    List<AttributeInfo> _attributesinfo = new List<AttributeInfo>();

                    foreach (ClusterEnsemble.Attribute _attr in fattributes.Values)
                        _attributesinfo.Add(new AttributeInfo() { Attribute = _attr });

                    this.lv_attributes.ItemsSource = null;
                    this.lv_attributes.ItemsSource = _attributesinfo;
                    this.chb_selectAll.IsChecked = true;
                    this.lv_attributes.SelectAll();
                    g_default.Visibility = Visibility.Hidden;
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }

        public List<ClusterEnsemble.Attribute> GetSelectedAttributes()
        {
            try
            {
                if (this.lv_attributes.ItemsSource == null)
                    return new List<ClusterEnsemble.Attribute>();

                List<ClusterEnsemble.Attribute> result = new List<ClusterEnsemble.Attribute>();
                foreach (AttributeInfo _attrInfo in lv_attributes.SelectedItems)
                    result.Add(_attrInfo.Attribute);
                return result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<ClusterEnsemble.Attribute>();
            }
        }

        private void SetAllAttributes(bool avalue)
        {
            try
            {
                if (avalue)
                    lv_attributes.SelectAll();
                else
                    lv_attributes.UnselectAll();
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

        private void lv_attributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //No puedo preguntar por el SelectedIndex porque cuando mando a seleccionar todos el selected index == -1
                //y entonces no me entra en el IF y no se actualiza AttributesToCalculateProximity en la clase Environment
                //if (this.lv_attributes.SelectedIndex != -1)
                //{
                if (CheckAttributeEventHandler != null)
                    CheckAttributeEventHandler(this, new CheckAttributeEnventArgs(GetSelectedAttributes()));
                //}

                //Esto es para actualizar el texto, de name, missing,distinct, unique
                if (this.lv_attributes.SelectedItem != null)
                {
                    ClusterEnsemble.Attribute _attr = (this.lv_attributes.SelectedItem as AttributeInfo).Attribute;

                    this.tblk_SelectedAttrName.Text = _attr.Name;
                    this.tblk_SelectedAttrDistinct.Text = _attr.Distinct.ToString();
                    this.tblk_SelectedAttrMissing.Text = _attr.Missing.ToString() + "(" + _attr.MissingPercent + "%)";
                    this.tblk_SelectedAttrUnique.Text = _attr.Unique.ToString() + "(" + _attr.UniquePercent + "%)";
                }
                else
                {
                    this.tblk_SelectedAttrName.Text = "---";
                    this.tblk_SelectedAttrUnique.Text = "---";
                    this.tblk_SelectedAttrMissing.Text = "---";
                    this.tblk_SelectedAttrDistinct.Text = "---";
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        GridViewColumnHeader fLastHeaderClicked = null;
        ListSortDirection fLastDirection = ListSortDirection.Ascending;
        private void lv_attributes_Click(object sender, RoutedEventArgs e)
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
                        if (header != "State")
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
                if (fattributes != null && sortBy != "State")
                {
                    IOrderedEnumerable<AttributeInfo> _attributesinfo = null;
                    List<AttributeInfo> _attributesinfoTemp = new List<AttributeInfo>();

                    foreach (ClusterEnsemble.Attribute _attr in fattributes.Values)
                        _attributesinfoTemp.Add(new AttributeInfo() { Attribute = _attr });



                    if (direction == ListSortDirection.Ascending)
                    {
                        switch (sortBy)
                        {
                            case "Name":
                                _attributesinfo = _attributesinfoTemp.OrderBy(_ai => _ai.Name);
                                break;
                            case "Type":
                                _attributesinfo = _attributesinfoTemp.OrderBy(_ai => _ai.Type);
                                break;
                            case "Values count":
                                _attributesinfo = _attributesinfoTemp.OrderBy(_ai => _ai.ValuesCount);
                                break;
                            case "Values":
                                _attributesinfo = _attributesinfoTemp.OrderBy(_ai => _ai.GetValues);
                                break;
                        }
                    }
                    else if (direction == ListSortDirection.Descending)
                    {
                        switch (sortBy)
                        {
                            case "Name":
                                _attributesinfo = _attributesinfoTemp.OrderByDescending(_ai => _ai.Name);
                                break;
                            case "Type":
                                _attributesinfo = _attributesinfoTemp.OrderByDescending(_ai => _ai.Type);
                                break;
                            case "Values count":
                                _attributesinfo = _attributesinfoTemp.OrderByDescending(_ai => _ai.ValuesCount);
                                break;
                            case "Values":
                                _attributesinfo = _attributesinfoTemp.OrderByDescending(_ai => _ai.GetValues);
                                break;
                        }
                    }

                    this.lv_attributes.ItemsSource = null;
                    this.lv_attributes.ItemsSource = _attributesinfo;
                    this.chb_selectAll.IsChecked = true;
                    this.lv_attributes.SelectAll();
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
    }

    class AttributeInfo
    {
        public ClusterEnsemble.Attribute Attribute { get; set; }
        
        public string Name 
        {
            get 
            {
                if (Attribute != null)
                    return Attribute.Name;
                return "";
            }
        }

        public string Type
        {
            get
            {
                if (Attribute != null)
                    return Attribute.AttributeType.ToString();
                return "";
            }
        }

        public int ValuesCount
        {
            get
            {
                if (Attribute != null)
                    return Attribute.ValuesCount;
                return -1;
            }
        }

        public string GetValues
        {
            get
            {
                try
                {
                    if (Attribute != null)
                    {
                        StringBuilder result = new StringBuilder();

                        switch (Attribute.AttributeType)
                        {
                            case AttributeType.Nominal:
                                result.Append("{");
                                for (int i = 0; i < Attribute.Values.Count; i++)
                                {
                                    if (i > 0)
                                        result.Append(", ");
                                    result.Append(Attribute.Values[i].ToString());
                                }
                                result.Append("}");
                                break;
                            case AttributeType.Numeric:
                                result.Append(Utils.Numeric);
                                break;
                            case AttributeType.String:
                                result.Append(Utils.String);
                                break;
                            case AttributeType.Date:
                                result.Append(Utils.Date);
                                break;
                            default:
                                throw new Exception("Tipo Incorrecto de AttributeType en el metodo: GetValues de la clase AttributeInfo");
                        }

                        return result.ToString();
                    }
                    else
                        return "";
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                    return "";
                }
            }
        }
    }
}
