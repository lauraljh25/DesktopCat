using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopCat
{
    public class Sprite
    {
        //location of actions 
        private readonly Rect stand_Left = new Rect(222, 0, 0, 0);
        private readonly Rect stand2_Left = new Rect(0, 0, 0, 0);
        private readonly Rect look_Left = new Rect(110, 0, 0, 0);
        private readonly Rect sit_Left = new Rect(325, 0, 0, 0);

        private readonly Rect stand_Right = new Rect(403,273,0,0);
        private readonly Rect look_Right = new Rect(515, 273, 0, 0);
        private readonly Rect sit_Right = new Rect(295, 273, 0, 0);

        private readonly Rect walk1_Left = new Rect(9, 89, 0, 0);
        private readonly Rect walk2_Left = new Rect(127, 89, 0, 0);
        private readonly Rect walk3_Left = new Rect(360, 89, 0, 0);
        private readonly Rect walk4_Left = new Rect(475, 88, 0, 0);

        private readonly Rect walk1_Right = new Rect(163, 365, 0, 0);
        private readonly Rect walk2_Right = new Rect(275, 366, 0, 0);
        private readonly Rect walk3_Right = new Rect(505, 366, 0, 0);
        private readonly Rect walk4_Right = new Rect(622, 366, 0, 0);

        private readonly Rect run1_Left = new Rect(15,  183, 0, 0);
        private readonly Rect run2_Left = new Rect(130, 182, 0, 0);
        private readonly Rect run3_Left = new Rect(255, 183, 0, 0);
        private readonly Rect run4_Left = new Rect(373, 183, 0, 0);
        private readonly Rect run5_Left = new Rect(495, 183, 0, 0);
        private readonly Rect run6_Left = new Rect(623, 183, 0, 0);

        private readonly Rect run1_Right = new Rect(611, 455, 0, 0);
        private readonly Rect run2_Right = new Rect(494, 455, 0, 0);
        private readonly Rect run3_Right = new Rect(366, 460, 0, 0);
        private readonly Rect run4_Right = new Rect(249, 461, 0, 0);
        private readonly Rect run5_Right = new Rect(126, 461, 0, 0);
        private readonly Rect run6_Right = new Rect(0, 461, 0, 0);

        private List<Rect> WalkList;
        private List<Rect> RunList;
        private List<Rect> StationaryList;

        private List<Rect> WalkList_Left;
        private List<Rect> RunList_Left;
        private List<Rect> StationaryList_Left;

        private List<Rect> WalkList_Right;
        private List<Rect> RunList_Right;
        private List<Rect> StationaryList_Right;

        DispatcherTimer WalkTimer;
        DispatcherTimer RunTimer;
        DispatcherTimer StationaryTimer;

        ImageBrush SpriteSheetImageBrush;

        int WalkCount = 0;
        int RunCount = 0;

        public Sprite(ImageBrush SpriteSheetImageBrush)
        {
            this.SpriteSheetImageBrush = SpriteSheetImageBrush;

            SetupSpriteLists();
            SetupWalkTimer();
            SetupRunTimer();
            SetupStationaryTimer();

            WalkRight();
        }

        private void SetupSpriteLists()
        {
            WalkList_Left = new List<Rect>()
            {
                walk1_Left, walk2_Left, walk3_Left, walk4_Left
            };

            WalkList_Right = new List<Rect>()
            {
                walk1_Right, walk2_Right, walk3_Right, walk4_Right
            };

            WalkList = WalkList_Right;

            RunList_Left = new List<Rect>()
            {
                run1_Left, run2_Left, run3_Left, run4_Left, run5_Left, run6_Left
            };

            RunList_Right = new List<Rect>()
            {
                run1_Right, run2_Right, run3_Right, run4_Right, run5_Right, run6_Right
            };

            RunList = RunList_Right;

            StationaryList_Left = new List<Rect>()
            {
                stand_Left, look_Left
            };

            StationaryList_Right = new List<Rect>()
            {
                stand_Right, look_Right
            };

            StationaryList = StationaryList_Right;
        }

        private void SetupWalkTimer()
        {
            WalkTimer = new DispatcherTimer();
            WalkTimer.Interval = TimeSpan.FromMilliseconds(250);
            WalkTimer.Tick += Walk;
        }

        private void SetupRunTimer()
        {
            RunTimer = new DispatcherTimer();
            RunTimer.Interval = TimeSpan.FromMilliseconds(100);
            RunTimer.Tick += Run;
        }

        private void SetupStationaryTimer()
        {
            StationaryTimer = new DispatcherTimer();
            StationaryTimer.Interval = TimeSpan.FromMilliseconds(2000);
            StationaryTimer.Tick += Stationary;
        }

        private void Walk(object sender, object e)
        {
            SpriteSheetImageBrush.Viewbox = WalkList[WalkCount];
            
            if(WalkCount == WalkList.Count()-1)
            {
                WalkCount = 0;
            }
            else
            {
                WalkCount++;
            }
        }

        private void Run(object sender, object e)
        {
            SpriteSheetImageBrush.Viewbox = RunList[RunCount];

            if (RunCount == RunList.Count() - 1)
            {
                RunCount = 0;
            }
            else
            {
                RunCount++;
            }
        }

        private void Stationary(object sender, object e)
        {
            Random randomNumber = new Random();
            SpriteSheetImageBrush.Viewbox = StationaryList[randomNumber.Next(0, StationaryList.Count())];
        }

        public void WalkStart()
        {
            if(!WalkTimer.IsEnabled)
            {
                WalkTimer.Start();
                SpriteSheetImageBrush.Viewbox = WalkList[WalkCount];
            }
        }

        public void WalkStop()
        {
            if(WalkTimer.IsEnabled)
            WalkTimer.Stop();
        }

        public void RunStart()
        {
            if (!RunTimer.IsEnabled)
            {
                RunTimer.Start();
                SpriteSheetImageBrush.Viewbox = RunList[WalkCount];
            }
        }

        public void RunStop()
        {
            if (RunTimer.IsEnabled)
                RunTimer.Stop();
        }

        public void StationaryStart()
        {
            if(!StationaryTimer.IsEnabled)
            {
                StationaryTimer.Start();
                SpriteSheetImageBrush.Viewbox = StationaryList[1];
            }
        }

        public void StationaryStop()
        {
            if (StationaryTimer.IsEnabled)
                StationaryTimer.Stop();
        }

        public void WalkLeft()
        {
            WalkList = WalkList_Left;
            
            if(WalkTimer.IsEnabled)
                SpriteSheetImageBrush.Viewbox = WalkList[WalkCount];
        }

        public void WalkRight()
        {
            WalkList = WalkList_Right;

            if (WalkTimer.IsEnabled)
                SpriteSheetImageBrush.Viewbox = WalkList[WalkCount];
        }

        public void StationaryLeft()
        {
            StationaryList = StationaryList_Left;
        }

        public void StationaryRight()
        {
            StationaryList = StationaryList_Right;
        }

        public void RunLeft()
        {
            RunList = RunList_Left;

            if (RunTimer.IsEnabled)
                SpriteSheetImageBrush.Viewbox = RunList[RunCount];
        }

        public void RunRight()
        {
            RunList = RunList_Right;

            if (RunTimer.IsEnabled)
                SpriteSheetImageBrush.Viewbox = RunList[RunCount];
        }

        public void StopAll()
        {
            RunStop();
            WalkStop();
            StationaryStop();
        }
    }
}
