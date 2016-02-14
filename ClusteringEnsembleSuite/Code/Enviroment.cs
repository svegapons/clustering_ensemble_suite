using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using ClusterEnsemble;
using ClusterEnsemble.Clusters;
using System.Windows.Media;
using ClusterEnsemble.ClusterEnsemble;
using Telerik.Windows.Controls.Charting;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.Proximities;
using System.Windows;
using Telerik.Windows.Controls;

namespace ClusteringEnsembleSuite.Code
{
    public class Enviroment
    {

        public static Set Set { get; set; }
        public static List<ClusterEnsemble.Attribute> AttributesToCalculateProximity { get; set; }
        public static Proximity Proximity { get; set; }
        public static List<Color> Colors { get; set; }
        

        static Enviroment()
        {
           
        }
        
        public static void Reset()
        {
            Set = null;
            AttributesToCalculateProximity = null;
            //Proximity = null; //Hay que tener cuidado porque cuando se pone un nuevo conjunto, esta Proximity se hace null
                                    //entonces lo que hay que hacer es no ponerla null.
            Colors = null;
        }

        public static void NewSet(object sender, NewSetEventArgs e)
        {
            Reset();
            Set = e.NewSet;
        }

        public static void CheckAttribute(object sender, CheckAttributeEnventArgs e)
        {
            AttributesToCalculateProximity = e.SelectedAttributes;
            if (e.SelectedAttributes.Count == 0)
                Proximity = null;
        }

        public static bool CanRunAlgorithm(out string aError,AlgorithmType aAlgType)
        {
            bool _result = true;
            string _algType = aAlgType.ToString();
            aError = "";
            
            if (Set == null)
            {
                _result = false;
                aError = "You must load a DataSet to apply a " + _algType + " algorithm. \r\n";
                return _result;
            }
            if (aAlgType == AlgorithmType.Clustering && (AttributesToCalculateProximity == null || AttributesToCalculateProximity.Count == 0))
            {
                _result = false;
                aError += "You must select at least one attribute to apply a " + _algType + " algorithm. \r\n";
            }
            if (aAlgType == AlgorithmType.Clustering && Proximity == null)
            {
                _result = false;
                aError += "You must select a Proximity to apply a " + _algType + " algorithm. \r\n";
            }


            return _result;
        }


    }

    //public class MessagesErrors
    //{
    //    public static string 
    //}

    



}
