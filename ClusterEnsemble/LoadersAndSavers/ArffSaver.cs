using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ClusterEnsemble.LoadersAndSavers
{
    public class ArffSaver : ISaver
    {
        StreamWriter filestream;

        public ArffSaver(string filepath)
        {
            filestream = new StreamWriter(filepath, false, Encoding.Default);
        }

        #region ISaver Members

        public void SetDestination(StreamWriter w)
        {
            ResetDestination();
            filestream = w;
        }

        public void SetDestination(string filename)
        {
            ResetDestination();
            filestream = new StreamWriter(filename, false, Encoding.Default);
        }

        public void ResetDestination()
        {
            filestream = null;
        }

        public void Save(Set set)
        {
            string result = set.ARFF_ToString();
            filestream.Write(result);
            filestream.Close();
        }

        public void Save(string filename, Set set)
        {
            ResetDestination();
            filestream = new StreamWriter(filename, false, Encoding.Default);
            Save(set);
        }

        public void Save(Structuring s)
        {            
            string result = s.ARFF_ToString();
            filestream.Write(result);
            filestream.Close();
        }

        public void Save(string filename, Structuring s)
        {
            ResetDestination();
            filestream = new StreamWriter(filename, false, Encoding.Default);
            Save(s);
        }
       

        #endregion
    }
}
