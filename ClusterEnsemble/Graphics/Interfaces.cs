using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace ClusterEnsemble.Graphics
{
    interface IName
    {
        string Name { get; set; }
    }

    public interface INeedProgressBar
    {
        IContainerProgressBar IContainerProgressBar { get; set; }
    }

    public interface IContainerProgressBar
    {
        RadProgressBar ProgressBar { get;}
        void ResetProgressBar(int amin, int amax, bool aIsIndeterminate);
        void ResetPB(int amin, int amax, bool aIsIndeterminate);
        void FinishProgressBar();
        void FinishPB();
        void UpdateProgressBar(int acurrent, string amessage, bool aInderterminate);
        void UpdatePB(int acurrent, string amessage, bool aInderterminate);
        void ShowError(string amessage);
        void ShowE(string amessage);
    }
}
