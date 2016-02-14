using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using Telerik.Windows.Controls.Charting;
using ClusterEnsemble.DataStructures;
using Telerik.Windows.Controls;
using System.Windows;
using System.Windows.Controls;
using ClusterEnsemble;

namespace ClusteringEnsembleSuite.Code
{
    public class TelerikUtils
    {

        #region Chart de Barras

        public static SeriesMapping InitChart(ClusterEnsemble.Attribute att_objetive, RadChart rc_graphic, CheckBox dim)
        {
            bool isChecked = (bool)dim.IsChecked;
            if (isChecked)
                return InitChart3D(att_objetive, rc_graphic);
            else
                return InitChart2D(att_objetive, rc_graphic);
        }

        public static SeriesMapping InitChart3D(ClusterEnsemble.Attribute att_objetive, RadChart rc_graphic)
        {
            SeriesMapping sm1 = new SeriesMapping();
            sm1.SeriesDefinition = new Bar3DSeriesDefinition();
            sm1.LegendLabel = "Set Values";


            return InitChartData(att_objetive, sm1, rc_graphic);

        }

        public static SeriesMapping InitChart2D(ClusterEnsemble.Attribute att_objetive, RadChart rc_graphic)
        {
            SeriesMapping sm1 = new SeriesMapping();
            sm1.SeriesDefinition = new BarSeriesDefinition();
            sm1.LegendLabel = "Set Values";

            return InitChartData(att_objetive, sm1, rc_graphic);
        }

        static SeriesMapping InitChartDataNumeric(SeriesMapping sm1)
        {
            ItemMapping imX = new ItemMapping("XValue", DataPointMember.XValue);
            ItemMapping imY = new ItemMapping("YValue", DataPointMember.YValue);
            ItemMapping imL = new ItemMapping("Label", DataPointMember.Label);

            sm1.ItemMappings.Add(imY);
            sm1.ItemMappings.Add(imX);
            sm1.ItemMappings.Add(imL);

            return sm1;
        }

        static SeriesMapping InitChartDataString(SeriesMapping sm1)
        {
            ItemMapping imX = new ItemMapping("XCategory", DataPointMember.XCategory);
            ItemMapping imY = new ItemMapping("YValue", DataPointMember.YValue);
            ItemMapping imL = new ItemMapping("Label", DataPointMember.Label);

            sm1.ItemMappings.Add(imY);
            sm1.ItemMappings.Add(imX);
            sm1.ItemMappings.Add(imL);

            return sm1;
        }

        public static SeriesMapping InitChartData(ClusterEnsemble.Attribute att_objetive, SeriesMapping sm1, RadChart rc_graphic)
        {
            SeriesMapping result = null;
            switch (att_objetive.AttributeType)
            {
                case AttributeType.String:
                case AttributeType.Nominal:
                case AttributeType.Date:
                    rc_graphic.DefaultView.ChartArea.AxisX.LabelRotationAngle = 45;
                    result = InitChartDataString(sm1);
                    break;
                case AttributeType.Numeric:
                    rc_graphic.DefaultView.ChartArea.AxisX.LabelRotationAngle = 0;
                    result = InitChartDataNumeric(sm1);
                    break;

                default:
                    break;
            }
            return result;
        }

        public static void SetAnimationsSettings(RadChart rc_graphic)
        {
            rc_graphic.AnimationSettings.ItemAnimationDuration = new TimeSpan(0, 0, 1);
            rc_graphic.AnimationSettings.ItemDelay = new TimeSpan(0);
            rc_graphic.AnimationSettings.TotalSeriesAnimationDuration = new TimeSpan(0, 0, 2);
        }

        public static void ChartAreaZoom(RadChart rc_graphic)
        {
            CameraExtension cameraExtension = new CameraExtension();
            cameraExtension.SpinAxis = SpinAxis.XY;
            cameraExtension.ZoomEnabled = true;

            rc_graphic.DefaultView.ChartArea.Extensions.Add(cameraExtension);
        }

        public static void SetAxisXVisibility(CheckBox AxisXCheckbox, RadChart RadChart)
        {
            bool isChecked = (bool)AxisXCheckbox.IsChecked;
            if (isChecked)
                RadChart.DefaultView.ChartArea.AxisX.Visibility = Visibility.Visible;
            else
                RadChart.DefaultView.ChartArea.AxisX.Visibility = Visibility.Collapsed;
        }

        public static void SetAxisYVisibility(CheckBox AxisYCheckbox, RadChart RadChart)
        {
            bool isChecked = (bool)AxisYCheckbox.IsChecked;
            if (isChecked)
                RadChart.DefaultView.ChartArea.AxisY.Visibility = Visibility.Visible;
            else
                RadChart.DefaultView.ChartArea.AxisY.Visibility = Visibility.Collapsed;
        }

        public static void SetAxisXGridLinesVisibility(CheckBox AxisXGridLinesCheckbox, RadChart RadChart)
        {
            bool isChecked = (bool)AxisXGridLinesCheckbox.IsChecked;
            if (isChecked)
                RadChart.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Visible;
            else
                RadChart.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Collapsed;
        }

        public static void SetAxisYGridLinesVisibility(CheckBox AxisYGridLinesCheckbox, RadChart RadChart)
        {
            bool isChecked = (bool)AxisYGridLinesCheckbox.IsChecked;
            if (isChecked)
                RadChart.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Visible;
            else
                RadChart.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Collapsed;
        }

        public static void SetAxisXStripLinesVisibility(CheckBox AxisXStripLinesCheckbox, RadChart RadChart)
        {
            bool isChecked = (bool)AxisXStripLinesCheckbox.IsChecked;
            if (isChecked)
                RadChart.DefaultView.ChartArea.AxisX.StripLinesVisibility = Visibility.Visible;
            else
                RadChart.DefaultView.ChartArea.AxisX.StripLinesVisibility = Visibility.Collapsed;
        }

        public static void SetAxisYStripLinesVisibility(CheckBox AxisYStripLinesCheckbox, RadChart RadChart)
        {
            bool isChecked = (bool)AxisYStripLinesCheckbox.IsChecked;
            if (isChecked)
                RadChart.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Visible;
            else
                RadChart.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
        }

        static List<DataPoint> FillChartNumericData(ClusterEnsemble.Attribute att_objetive)
        {
            List<DataPoint> data = new List<DataPoint>();
            Dictionary<double, double> temp_dic = new Dictionary<double, double>();

            double max = double.MinValue;
            double min = double.MaxValue;

            foreach (var item in Enviroment.Set.Elements)
            {
                if (item[att_objetive] != null)//missing values
                {
                    if (!temp_dic.ContainsKey((double)item[att_objetive]))
                    {
                        temp_dic.Add((double)item[att_objetive], 1);
                        if ((double)item[att_objetive] > max)
                            max = (double)item[att_objetive];
                        else if ((double)item[att_objetive] < min)
                            min = (double)item[att_objetive];
                    }
                    else
                        temp_dic[(double)item[att_objetive]]++;
                }

            }


            foreach (var item in temp_dic.Keys)
            {
                data.Add(new DataPoint() { Label = item.ToString(), YValue = temp_dic[item] });
            }


            data.Sort(new DataPointXComparer());

            return data;

        }

        static List<DataPoint> FillChartStringData(ClusterEnsemble.Attribute att_objetive)
        {
            List<DataPoint> data = new List<DataPoint>();
            Dictionary<string, double> temp_dic = new Dictionary<string, double>();


            foreach (var item in Enviroment.Set.Elements)
            {
                if (item[att_objetive] != null)//missing vallues
                {
                    if (!temp_dic.ContainsKey((string)item[att_objetive]))
                        temp_dic.Add((string)item[att_objetive], 1);
                    else
                        temp_dic[(string)item[att_objetive]]++;
                }

            }

            foreach (var item in temp_dic.Keys)
            {
                data.Add(new DataPoint() { XCategory = item, YValue = temp_dic[item], Label = item });
            }

            data.Sort(new DataPointCategoryComparer());

            return data;

        }

        static List<DataPoint> FillChartDateData(ClusterEnsemble.Attribute att_objetive)
        {
            List<DataPoint> data = new List<DataPoint>();
            Dictionary<DateTime, double> temp_dic = new Dictionary<DateTime, double>();


            foreach (var item in Enviroment.Set.Elements)
            {
                if (item[att_objetive] != null)//missing vallues
                {
                    if (!temp_dic.ContainsKey((DateTime)item[att_objetive]))
                        temp_dic.Add((DateTime)item[att_objetive], 1);
                    else
                        temp_dic[(DateTime)item[att_objetive]]++;
                }

            }

            foreach (var item in temp_dic.Keys)
            {
                data.Add(new DataPoint() { XCategory = item.ToString(), YValue = temp_dic[item], Label = item.ToString(), IsDateTime = true });
            }

            data.Sort(new DataPointCategoryComparer());

            return data;

        }

        public static List<DataPoint> FillChartData(ClusterEnsemble.Attribute att_objetive)
        {
            List<DataPoint> result = null;
            switch (att_objetive.AttributeType)
            {
                case AttributeType.Nominal:
                    result = FillChartStringData(att_objetive);
                    break;
                case AttributeType.Numeric:
                    result = FillChartNumericData(att_objetive);
                    break;
                case AttributeType.String:
                    result = FillChartStringData(att_objetive);
                    break;
                case AttributeType.Date:
                    result = FillChartDateData(att_objetive);
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion

        #region Chart de Burbujas

        public static List<SeriesMapping> InitBubbleChart(Structuring s, ClusterEnsemble.Attribute att_x, ClusterEnsemble.Attribute att_y)
        {
            List<SeriesMapping> result = new List<SeriesMapping>();

            int i = 0;
            foreach (Cluster item in s.Clusters.Values)
            {
                SeriesMapping sm = new SeriesMapping();
                sm.SeriesDefinition = new BubbleSeriesDefinition();
                sm.LegendLabel = item.Name;
                sm.CollectionIndex = i;
                i++;

                ItemMapping bubblesize = new ItemMapping("BubbleSize", DataPointMember.BubbleSize);
                ItemMapping imXCat = new ItemMapping("XCategory", DataPointMember.XCategory);
                ItemMapping imX = new ItemMapping("XValue", DataPointMember.XValue);
                ItemMapping imY = new ItemMapping("YValue", DataPointMember.YValue);
                sm.ItemMappings.Add(bubblesize);
                sm.ItemMappings.Add(imXCat);
                sm.ItemMappings.Add(imX);
                sm.ItemMappings.Add(imY);

                result.Add(sm);
            }

            if (s.HaveUnassignedElements())
            {
                SeriesMapping sm = new SeriesMapping();
                sm.SeriesDefinition = new BubbleSeriesDefinition();
                sm.LegendLabel = "Unassigned Elements";
                sm.CollectionIndex = i;
                i++;

                ItemMapping bubblesize = new ItemMapping("BubbleSize", DataPointMember.BubbleSize);
                ItemMapping imXCat = new ItemMapping("XCategory", DataPointMember.XCategory);
                ItemMapping imX = new ItemMapping("XValue", DataPointMember.XValue);
                ItemMapping imY = new ItemMapping("YValue", DataPointMember.YValue);
                sm.ItemMappings.Add(bubblesize);
                sm.ItemMappings.Add(imXCat);
                sm.ItemMappings.Add(imX);
                sm.ItemMappings.Add(imY);

                result.Add(sm);
            }

            return result;
        }

        public static List<List<DataPoint>> FillBubbleChartData(Structuring s, ClusterEnsemble.Attribute att_x, ClusterEnsemble.Attribute att_y)
        {
            List<List<DataPoint>> result = new List<List<DataPoint>>();

            foreach (Cluster item in s.Clusters.Values)
            {
                List<DataPoint> temp = FillOneClusterData(item, att_x, att_y);
                result.Add(temp);
            }
            if (s.HaveUnassignedElements())
            {
                List<DataPoint> temp = FillUnassignedData(s.UnassignedElements, att_x, att_y);
                result.Add(temp);
            }

            ResetCategory();
            return result;
        }

        private static List<DataPoint> FillUnassignedData(List<Element> list, ClusterEnsemble.Attribute att_x, ClusterEnsemble.Attribute att_y)
        {
            List<DataPoint> result = new List<DataPoint>();

            foreach (Element item in list)
            {
                DataPoint dp = new DataPoint();
                if (item[att_x] != null && item[att_y] != null)//missing values
                {

                    if (att_x.AttributeType == AttributeType.Numeric)
                        dp.XValue = (double)item[att_x] + 10;//valores cercanos a cero se funde el control
                    else if (att_x.AttributeType != AttributeType.Date)
                    {
                        if (xcat.ContainsKey((string)item[att_x]))
                            dp.XValue = xcat[(string)item[att_x]];
                        else
                        {
                            xcat.Add((string)item[att_x], ix);
                            dp.XValue = ix;
                            ix++;

                        }
                    }
                    else
                    {
                        if (xcat.ContainsKey(((DateTime)item[att_x]).ToString()))
                            dp.XValue = xcat[(string)item[att_x]];
                        else
                        {
                            xcat.Add(((DateTime)item[att_x]).ToString(), ix);
                            dp.XValue = ix;
                            ix++;

                        }
                    }

                    if (att_y.AttributeType == AttributeType.Numeric)
                        dp.YValue = (double)item[att_y] + 10;//valores cercanos a cero se funde el control
                    else if (att_y.AttributeType != AttributeType.Date)
                    {
                        if (ycat.ContainsKey((string)item[att_y]))
                            dp.YValue = ycat[(string)item[att_y]];
                        else
                        {
                            ycat.Add((string)item[att_y], iy);
                            dp.YValue = iy;
                            iy++;

                        }
                    }
                    else
                    {
                        if (ycat.ContainsKey(((DateTime)item[att_y]).ToString()))
                            dp.YValue = ycat[(string)item[att_y]];
                        else
                        {
                            ycat.Add(((DateTime)item[att_y]).ToString(), iy);
                            dp.YValue = iy;
                            iy++;

                        }
                    }

                    //dp.BubbleSize = 20;
                    result.Add(dp);
                }
            }

            result.Sort(new DataPointXComparer());

            return result;
        }

        static Dictionary<string, double> xcat = new Dictionary<string, double>();
        static Dictionary<string, double> ycat = new Dictionary<string, double>();
        static double ix = 1, iy = 1;

        public static void ResetCategory()
        {
            xcat = new Dictionary<string, double>();
            ycat = new Dictionary<string, double>();
            ix = 1;
            iy = 1;
        }

        static List<DataPoint> FillOneClusterData(Cluster cluster, ClusterEnsemble.Attribute att_x, ClusterEnsemble.Attribute att_y)
        {
            List<DataPoint> result = new List<DataPoint>();

            foreach (Element item in cluster.Elements)
            {
                DataPoint dp = new DataPoint();
                if (item[att_x] != null && item[att_y] != null)//missing values
                {

                    if (att_x.AttributeType == AttributeType.Numeric)
                        dp.XValue = (double)item[att_x] + 10;//valores cercanos a cero se funde el control
                    else if (att_x.AttributeType != AttributeType.Date)
                    {
                        if (xcat.ContainsKey((string)item[att_x]))
                            dp.XValue = xcat[(string)item[att_x]];
                        else
                        {
                            xcat.Add((string)item[att_x], ix);
                            dp.XValue = ix;
                            ix++;

                        }
                    }
                    else
                    {
                        if (xcat.ContainsKey(((DateTime)item[att_x]).ToString()))
                            dp.XValue = xcat[(string)item[att_x]];
                        else
                        {
                            xcat.Add(((DateTime)item[att_x]).ToString(), ix);
                            dp.XValue = ix;
                            ix++;

                        }
                    }

                    if (att_y.AttributeType == AttributeType.Numeric)
                        dp.YValue = (double)item[att_y] + 10;//valores cercanos a cero se funde el control
                    else if (att_y.AttributeType != AttributeType.Date)
                    {
                        if (ycat.ContainsKey((string)item[att_y]))
                            dp.YValue = ycat[(string)item[att_y]];
                        else
                        {
                            ycat.Add((string)item[att_y], iy);
                            dp.YValue = iy;
                            iy++;

                        }
                    }
                    else
                    {
                        if (ycat.ContainsKey(((DateTime)item[att_y]).ToString()))
                            dp.YValue = ycat[(string)item[att_y]];
                        else
                        {
                            ycat.Add(((DateTime)item[att_y]).ToString(), iy);
                            dp.YValue = iy;
                            iy++;

                        }
                    }

                    //dp.BubbleSize = 20;
                    result.Add(dp);
                }
            }

            result.Sort(new DataPointXComparer());

            return result;
        }

        #endregion
    }

    class DataPointXComparer : IComparer<DataPoint>
    {
        #region IComparer<DataPoint> Members

        public int Compare(DataPoint x, DataPoint y)
        {
            return x.XValue.CompareTo(y.XValue);
        }

        #endregion
    }

    class DataPointYComparer : IComparer<DataPoint>
    {
        #region IComparer<DataPoint> Members

        public int Compare(DataPoint x, DataPoint y)
        {
            return x.YValue.CompareTo(y.YValue);
        }

        #endregion
    }

    class DataPointCategoryComparer : IComparer<DataPoint>
    {
        #region IComparer<DataPoint> Members

        public int Compare(DataPoint x, DataPoint y)
        {
            return x.XCategory.CompareTo(y.XCategory);
        }

        #endregion
    }
}
