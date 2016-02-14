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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//Added
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusteringEnsembleSuite.Code;
using System.Windows.Threading;
using System.Threading;

namespace ClusteringEnsembleSuite.Windows
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class Intro : Window
    {
        List<Image> Images { get; set; }
         DispatcherTimer _dt {get;set;}
         DispatcherTimer _dt1 { get; set; }
         public Intro()
         {
             InitializeComponent();
             _dt = new DispatcherTimer();
             _dt.Interval = TimeSpan.FromMilliseconds(5000);
             _dt.Tick += new EventHandler(_dt_Tick);
             _dt.Start();
         }
        void _dt_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
        
        Random _random = new Random(Environment.TickCount);

        #region Animations
        void Animation0(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation0;

            canvas.Children[0].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[0].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation1(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation1;

            canvas.Children[1].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[1].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation2(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation2;

            canvas.Children[2].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[2].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation3(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation3;

            canvas.Children[3].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[3].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation4(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation4;

            canvas.Children[4].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[4].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation5(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation5;

            canvas.Children[5].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[5].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation6(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation6;

            canvas.Children[6].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[6].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation7(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation7;

            canvas.Children[7].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[7].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation8(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation8;

            canvas.Children[8].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[8].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation9(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation9;

            canvas.Children[9].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[9].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation10(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation10;

            canvas.Children[10].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[10].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation11(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation11;

            canvas.Children[11].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[11].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation12(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation12;

            canvas.Children[12].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[12].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation13(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation13;

            canvas.Children[13].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[13].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation14(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation14;

            canvas.Children[14].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[14].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation15(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation15;

            canvas.Children[15].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[15].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation16(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation16;

            canvas.Children[16].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[16].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation17(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation17;

            canvas.Children[17].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[17].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation18(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation18;

            canvas.Children[18].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[18].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation19(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation19;

            canvas.Children[19].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[19].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation20(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation20;

            canvas.Children[20].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[20].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation21(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation21;

            canvas.Children[21].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[21].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation22(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation22;

            canvas.Children[22].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[22].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation23(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation23;

            canvas.Children[23].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[23].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation24(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation24;

            canvas.Children[24].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[24].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation25(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation25;

            canvas.Children[25].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[25].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation26(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation26;

            canvas.Children[26].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[26].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation27(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation27;

            canvas.Children[27].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[27].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation28(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation28;

            canvas.Children[28].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[28].BeginAnimation(Canvas.TopProperty, _day);
        }
        void Animation29(object sender, EventArgs e)
        {
            DoubleAnimation _dax = new DoubleAnimation(_random.NextDouble() * this.ActualWidth, TimeSpan.FromMilliseconds(1500));
            DoubleAnimation _day = new DoubleAnimation(_random.NextDouble() * this.ActualHeight, TimeSpan.FromMilliseconds(1500));
            _dax.Completed += Animation29;

            canvas.Children[29].BeginAnimation(Canvas.LeftProperty, _dax);
            canvas.Children[29].BeginAnimation(Canvas.TopProperty, _day);
        }
        #endregion

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Animation0(canvas.Children[0], null);
            Animation1(canvas.Children[1], null);
            Animation2(canvas.Children[2], null);
            Animation3(canvas.Children[3], null);
            Animation4(canvas.Children[4], null);
            Animation5(canvas.Children[5], null);
            Animation6(canvas.Children[6], null);
            Animation7(canvas.Children[7], null);
            Animation8(canvas.Children[8], null);
            Animation9(canvas.Children[9], null);

            Animation10(canvas.Children[10], null);
            Animation11(canvas.Children[11], null);
            Animation12(canvas.Children[12], null);
            Animation13(canvas.Children[13], null);
            Animation14(canvas.Children[14], null);
            Animation15(canvas.Children[15], null);
            Animation16(canvas.Children[16], null);
            Animation17(canvas.Children[17], null);
            Animation18(canvas.Children[18], null);
            Animation19(canvas.Children[19], null);

            Animation20(canvas.Children[20], null);
            Animation21(canvas.Children[21], null);
            Animation22(canvas.Children[22], null);
            Animation23(canvas.Children[23], null);
            Animation24(canvas.Children[24], null);
            Animation25(canvas.Children[25], null);
            Animation26(canvas.Children[26], null);
            Animation27(canvas.Children[27], null);
            Animation28(canvas.Children[28], null);
            Animation29(canvas.Children[29], null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //System.Windows.Application.Current.Shutdown();
            Main _main = new Main();
            _main.Focus();
            _main.Show();
        }
    }
}
