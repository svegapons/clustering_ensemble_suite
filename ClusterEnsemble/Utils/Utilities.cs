using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble
{
    public static class Utils
    {
        public static string Integer = "integer";
        public static string String = "string";
        public static string Numeric = "numeric";
        public static string Real = "real";
        public static string Date = "date";

        public static string Attribute = "@attribute";
        public static string Relation = "@relation";
        public static string Data = "@data";

        public static string Enter = "\r\n";

        //Nombre de los ejecutable Metis y Hmetis respectivamente
        public static string MetisExecutableName = "pmetis.exe";//Tbn se puede utilizar "kmetis.exe"
        public static string HMetisExecutableName = "shmetis.exe";//Tbn se puede utilizar "khmetis.exe" lo que recibe mas parametros

        private static int uniqueId = 0;


        /// <summary>
        /// Retorna un identificador unico.
        /// </summary>
        public static int GetUniqueID
        {
            get
            {
                return uniqueId++;
            }
        }

        /// <summary>
        /// Retorna la carpeta donde estan los ejecutables del Metis y el Hmetis.
        /// </summary>
        public static string ExesFolderPath
        {
            get
            {
                return CreateFolder("Exes");
            }
        }

        //OJO Excepciones porque dice que no puede acceder al fichero porque esta siendo usado por otro proceso
        //tbn da Excepcion porque dice que no existe el fichero. 
        //Parece que el metis se demora un poco creandolo, debe ser algo de eso, y lo crea pero despues parece que empieza
        //a escribir sobre el y entonces es que me da la otra excepcion de que esta siendo usado por otra persona, es que todo
        //ocurre muy rapido, NUNCA ME HA DADO EXCEPCION EN EL MODO DEBUG
        public static Structuring BuildStructuringFromOutputFile(string inputfilename, Set Set, int ClustersCount, Proximity Proximity)
        {
            try
            {
                string folderpath = Utils.ExesFolderPath;
                string filename = inputfilename + ".part." + ClustersCount;
                string filepath = folderpath + Path.DirectorySeparatorChar + filename;

                //Dar tiempo a que el algoritmo Metis cree el fichero
                while (!File.Exists(filepath))
                {
                }

                FileStream fs = null;
                bool ok = false;
                //Dar tiempo a que el algoritmo Metis termine de escribir sobre el fichero de salida
                while (!ok)
                {
                    try
                    {
                        fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                        ok = true;
                    }
                    catch
                    { }
                }

                StreamReader sr = new StreamReader(fs);
                Dictionary<string, Cluster> dic_clusters;
                try
                {
                    //Crear Dictionary<string,Cluster> para poder construir la particion
                    dic_clusters = new Dictionary<string, Cluster>();
                    string line;
                    int vertex = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int cluster = int.Parse(line);
                        string nameOfCluster = "C-" + cluster;
                        if (dic_clusters.ContainsKey(nameOfCluster))
                        {
                            dic_clusters[nameOfCluster].AddElement(Set[vertex]);
                        }
                        else
                        {
                            Cluster c = new Cluster(nameOfCluster);
                            c.AddElement(Set[vertex]);
                            dic_clusters.Add(nameOfCluster, c);
                        }
                        vertex++;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Formato incorrecto del fichero de salida del algoritmo METIS \r\nInformacion Adicional: " + ex.Message);
                }
                finally
                {
                    sr.Close();
                    fs.Close();

                    //Eliminar el fichero de entrada
                    File.Delete(ExesFolderPath + Path.DirectorySeparatorChar + inputfilename);

                    //Eliminar el fichero de salida
                    File.Delete(filepath);
                }

                //Los numeros de las particiones comienzan con cero hasta el numero de particiones menos uno[0,...,P-1]
                return new Partition() { Clusters = dic_clusters, Proximity = Proximity };
            }
            catch
            {
                return null;
            }
        }

        public static void ExecuteMetisPackage(string executablename, string parameters)
        {
            ProcessStartInfo psinf = new ProcessStartInfo();
            psinf.WorkingDirectory = Utils.ExesFolderPath;
            psinf.FileName = executablename;
            psinf.Arguments = parameters;
            psinf.CreateNoWindow = true;
            psinf.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psinf);
        }

        public static Structuring Clone(Structuring s)
        {
            ReductionPartition _rp = s as ReductionPartition;
            Partition _p = s as Partition;
            Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();

            foreach (var item in s.Clusters.Values)
                dic_clusters.Add(item.Name, new Cluster(item.Name, new List<Element>(item.Elements)));

            List<Element> temp = s.UnassignedElements == null ? new List<Element>() : s.UnassignedElements;

            if (_p != null)
                return new Partition() { Clusters = dic_clusters, UnassignedElements = new List<Element>(temp) };
            else if (_rp != null)
                return new ReductionPartition(_rp.Partition, _rp.NewSet, _rp.Map);
            else
                throw new Exception("Super mal, revisalo");

        }

        #region Private Members
        private static string CreateFolder(string foldername)
        {
            StringBuilder sb = new StringBuilder(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            sb.Append(foldername).Append(Path.DirectorySeparatorChar);

            string path = sb.ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
        #endregion
    }

    public class ElementsUtilities
    {
        public static double Norm(Element vect)
        {
            if (vect.ElementType != ElementType.Numeric || vect.HasMissing)
                throw new ArgumentNullException("Parametro Incorrecto en el metodo Norm de la clase Vector");
            double norm = 0;
            foreach (var item in vect.Values)
            {
                if (item != null)
                {
                    double c = (double)item;
                    norm += c * c;
                }
            }

            return Math.Sqrt(norm);
        }
        public static double EuclideanDistance(Element ei, Element ej)
        {
            if (ei.Equals(ej) || ei.ElementType != ElementType.Numeric || ei.HasMissing || ej.HasMissing)//Redefinir el Equals de la clase elemento
                throw new ArgumentException();

            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
                result += Math.Pow(((double)ei[i] - (double)ej[i]), 2);

            return Math.Sqrt(result);
        }
        public static double EscalarProduct(Element ei, Element ej)
        {
            if (ei.Equals(ej) || ei.ElementType != ElementType.Numeric || ei.HasMissing || ej.HasMissing)//Redefinir el Equals de la clase elemento
                throw new ArgumentException();
            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
            {
                result += ((double)ei[i]) * ((double)ej[i]);
            }
            return result;
        }

    }
}
