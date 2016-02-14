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
using ClusterEnsemble.LoadersAndSavers;
using Telerik.Windows.Controls.Charting;
using ClusterEnsemble;
using ClusterEnsemble.Graphics;

namespace ClusteringEnsembleSuite.Controls.TabsControls
{
    /// <summary>
    /// Interaction logic for TabPreprocess.xaml
    /// </summary>
    public partial class TabPreprocess : UserControl
    {


        public TabPreprocess()
        {
            try
            {
                InitializeComponent();

                this.uctrl_Loader.ILoader = new ArffLoader();
                this.uctrl_Loader.NewSetEventHandler += new NewSetEventHandler(NewSet);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void NewSet(object sender, NewSetEventArgs e)
        {
            try
            {
                if (e.NewSet != null)
                {
                    //Es lo primero que se tiene que hacer ya que esto resetea el conjunto y pone las variables en null
                    //y AttributeVisualizer le dice al Environment los attributos seleccionados, por lo tanto si seteo al Environment despues
                    //de haberle seteado los atributos al control, finalmente ele environment no va a tener atributos seleccionados, que son los que
                    //se utilizan para calcular la disimilitud, esto es solo la primera vez, ya que despues el usuario puede ir seleccionando los atributos
                    //y el Environment si se va actualizando.
                    Enviroment.Reset();
                    Enviroment.Set = e.NewSet;

                    this.uctrl_Attributes.Attributes = e.NewSet.Attributes;
                    this.uctrl_Elements.Set = e.NewSet;

                    ctrl_graphic.UpdateSet(e.NewSet);
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }       
    }
}
