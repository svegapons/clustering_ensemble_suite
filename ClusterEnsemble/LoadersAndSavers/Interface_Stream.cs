using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ClusterEnsemble
{
    public interface ILoader
    {
        void SetSource(FileStream file);
        void SetSource(string filepath);
        void ResetSource();
        string SourceFilePath { get; set; }
        bool TryLoad();
        bool TryLoad(string filepath);
        Set Load();
        Set Load(string filepath);
    }
    public interface ISaver
    {
        void SetDestination(StreamWriter w);
        void SetDestination(string filepath);
        void ResetDestination();
        void Save(Set set);
        void Save(string filepath, Set set);
        void Save(Structuring s);
        void Save(string filepath, Structuring s);
    }

}
