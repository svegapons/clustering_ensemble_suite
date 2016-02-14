using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using System.Threading;
using ClusterEnsemble.Proximities;

namespace ClusterEnsemble.Clusters
{
    public class Metis : NonHierarchical
    {
        public Metis(Set set, Proximity diss)
            : base(set, diss)
        {
            Name = "Metis";
            //ProximityType = ProximityType.Both;
            ProximityType = ProximityType.Similarity;
        }
        public Metis()
            : base()
        {
            Name = "Metis";
            //ProximityType = ProximityType.Both;
            ProximityType = ProximityType.Similarity;
        }

        public override Structuring BuildStructuring()
        {
            try
            {
                int _current = 1;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, 1, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running Metis algorithm...", true);
                }


                if (ClustersCount <= 0)
                    throw new Exception("La cantidad de clusters debe ser mayor que cero");
                else if (ClustersCount == 1)
                {
                    Dictionary<string, Cluster> dic_clus = new Dictionary<string, Cluster>();
                    string name = "C-0";
                    List<Element> temp = new List<Element>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                        temp.Add(Set[i]);

                    dic_clus.Add(name, new Cluster(name) { Elements = temp });

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    Structuring = new Partition() { Clusters = dic_clus, Proximity = Proximity };
                    return Structuring;
                }
                else
                {
                    string filename = BuildInputFile(ref _current);
                    string parameters = filename + " " + ClustersCount;
                    Utils.ExecuteMetisPackage(Utils.MetisExecutableName, parameters);
                    Structuring = Utils.BuildStructuringFromOutputFile(filename, Set, ClustersCount, Proximity);

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();
                    
                    return Structuring;
                }
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Has Occurred an error in METIS algorithm.");
                return null;
            }
        }

        #region Private Members 
        private string BuildInputFile(ref int acurrent)
        {
            string folderpath = Utils.ExesFolderPath;
            string filename = "inputgraph" + Utils.GetUniqueID + ".config";
            string filepath = folderpath + Path.DirectorySeparatorChar + filename;

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            FileStream fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);

            //First Line #vertex , #edges , #code
            int vertices = Set.ElementsCount;
            int edges = (vertices * (vertices - 1)) / 2;
            int code = 1;

            string firstLine = vertices + " " + edges + " " + code;
            sw.WriteLine(firstLine);

            int edgesZero = 0;//Este es un numero par ya que si (i,j)=0 entonces tbn sera cero (j,i)=0

            //n-lines, where n is the amount of vertices, the vertices start in 1
            StringBuilder line;
            for (int i = 0; i < Set.ElementsCount; i++)
            {
                line = new StringBuilder();
                for (int j = 0; j < Set.ElementsCount; j++)
                {
                    if (i != j)
                    {
                        int adj = j + 1;
                        double edgeweight = Proximity.CalculateProximity(Set[i], Set[j]);

                        //Codigo Chiki
                        string number = (edgeweight * 100000).ToString();
                        string[] parts = number.Split('.');

                        int temp = 0;
                        if (!int.TryParse(parts[0], out temp))
                            temp = int.MaxValue;
                        //hasta aqui

                        if (temp != 0)
                            line.Append(adj + " " + temp + " ");
                        else
                            edgesZero++;
                    }                    

                }
                //Write the line for the vertex i+1
                sw.WriteLine(line);
            }

            sw.Flush();

            //Actualizar la cantidad de aristas ya que pudo haber aristas con costo cero
            //las cuales no pertenecen al grafo ya que segun la especificacion del fichero de entrada
            //el costo de las aristas debe ser mayor estricto que cero
            if (edgesZero > 0)
            {
                fs.Seek(0, SeekOrigin.Begin);
                string newFirstLine = vertices + " " + (edges - edgesZero / 2) + " " + code;
                int lengthNewFirstLine = newFirstLine.Length;
                int lengthOldFirstLine = firstLine.Length;
                byte[] arr = new byte[lengthOldFirstLine];
                for (int i = 0; i < lengthNewFirstLine; i++)
                    arr[i] = (byte)newFirstLine[i];
                if (lengthNewFirstLine < lengthOldFirstLine)
                {
                    byte white = (byte)' ';
                    for (int i = lengthNewFirstLine; i < lengthOldFirstLine; i++)
                        arr[i] = white;
                }
                fs.Write(arr, 0, lengthOldFirstLine);
                fs.Flush();


            }
            sw.Close();
            fs.Close();

            return filename;
        }
        #endregion
    }
}
