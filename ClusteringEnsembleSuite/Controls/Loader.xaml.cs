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
using System.Windows.Forms;
using System.IO;
using ClusterEnsemble;
using ClusterEnsemble.LoadersAndSavers;
using ClusteringEnsembleSuite.Code;
using System.Threading;
using ClusterEnsemble.Graphics;

namespace ClusteringEnsembleSuite.Controls
{
    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Loader : System.Windows.Controls.UserControl, INeedProgressBar
    {
        
        //Evento que indica que se cargó un nuevo Set
        public event NewSetEventHandler NewSetEventHandler;
        public Loader()
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

        delegate Set Load();
        public void LoadDataBase()
        {
            try
            {
                if (ILoader == null)
                {
                    System.Windows.MessageBox.Show("Can't load any file because not contain the property ILoader.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                OpenFileDialog _dlg = new OpenFileDialog();
                //_dlg.InitialDirectory = GetContentFolder();
                _dlg.Filter = "ARFF Documents (*.arff)|*.arff";
                _dlg.FilterIndex = 1;
                if (_dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        ILoader.SetSource(_dlg.FileName);
                        
                        this.fset = ILoader.Load(_dlg.FileName);
                    }
                    catch (Exception _ex)
                    {
                        System.Windows.MessageBox.Show("Corrupt file, try with another.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //Set != NULL
                    this.tbk_DBPath.ToolTip = _dlg.FileName;
                    this.tbk_DBPath.Text = _dlg.FileName;
                    this.tbk_RelationName.Text = this.Set.RelationName;
                    this.tbk_Instances.Text = this.Set.ElementsCount.ToString();
                    this.tbk_Attributes.Text = this.Set.Attributes.ValuesCount.ToString();

                    if (NewSetEventHandler != null)
                    {
                        this.NewSetEventHandler(this, new NewSetEventArgs(fset));
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        Set fset;
        public Set Set
        {
            get
            {
                return fset;
            }
        }

        public ILoader ILoader { get; set; }

        private void bt_loader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDataBase();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        // ------------------------- GetContentFolder -------------------------
        /// <summary>
        ///   Locates and returns the path to the "Content\" folder
        ///   containing the fixed document for the sample.</summary>
        /// <returns>
        ///   The path to the fixed document "Content\" folder.</returns>
        private string GetContentFolder()
        {
            try
            {
                // Get the path to the current directory and its length.
                string _contentDir = Directory.GetCurrentDirectory();
                int _dirLength = _contentDir.Length;

                // If we're in "...\bin\debug", move up to the root.
                if (_contentDir.ToLower().EndsWith(@"\bin\debug"))
                    _contentDir = _contentDir.Remove(_dirLength - 10, 10);

                // If we're in "...\bin\release", move up to the root.
                else if (_contentDir.ToLower().EndsWith(@"\bin\release"))
                    _contentDir = _contentDir.Remove(_dirLength - 12, 12);

                // If there's a "Content" subfolder, that's what we want.
                if (Directory.Exists(_contentDir + @"\Content"))
                    _contentDir = _contentDir + @"\Content";

                // Return the "Content\" folder (or the "current"
                // directory if we're executing somewhere else).
                return _contentDir;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return "";
            }
        }



        #region INeedProgressBar Members

        public IContainerProgressBar IContainerProgressBar { get; set; }

        #endregion
    }
}
