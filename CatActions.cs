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
    public class CatActions
    {
        public Rectangle Cat { get; set; }
        public Canvas CatArea { get; set; }
        public Sprite CatSprite { get; set; }
        public bool MouseTaken { get; private set; }
        public bool CatFaceLeft { get; private set; }
        public Point MouseTakenLocation {
            get
            {
                if (CatFaceLeft)
                {
                    return new Point(Canvas.GetLeft(Cat) + Cat.Width * 0.155, Canvas.GetTop(Cat) + Cat.Height * 0.35); 
                }
                else
                {
                    return new Point(Canvas.GetLeft(Cat) + Cat.Width * 0.755, Canvas.GetTop(Cat) + Cat.Height * 0.39);
                }
            }
        }

        private DispatcherTimer CatChaseMouseTimer = new DispatcherTimer();
        private DispatcherTimer CatTakeMouseTimer = new DispatcherTimer();
        private DispatcherTimer CatWanderTimer = new DispatcherTimer();
        private DispatcherTimer MoveMouseTimer = new DispatcherTimer();


        Random random = new Random();
        Point WanderDestination;

        public CatActions(Rectangle Cat, Canvas CatArea, double MovementTickInterval, Sprite CatSprite)
        {
            this.Cat = Cat;
            this.CatArea = CatArea;
            this.CatSprite = CatSprite;
            MouseTaken = false;

            SetupTimers(MovementTickInterval, 0.000000000000000000001);
        }

        private void SetupTimers(double MovementTickInterval, double MoveMouseTickInterval)
        {
            CatChaseMouseTimer.Interval = TimeSpan.FromMilliseconds(MovementTickInterval);
            CatChaseMouseTimer.Tick += ChaseTimerTick;

            CatWanderTimer.Interval = TimeSpan.FromMilliseconds(MovementTickInterval);
            CatWanderTimer.Tick += WanderTimerTick;

            CatTakeMouseTimer.Interval = TimeSpan.FromMilliseconds(MovementTickInterval);
            CatTakeMouseTimer.Tick += TakeTimerTick;

            MoveMouseTimer.Interval = TimeSpan.FromMilliseconds(MoveMouseTickInterval);
            MoveMouseTimer.Tick += MouseMoveTick;
        }

        private void ChaseTimerTick(object sender, EventArgs e)
        {
            ChaseMouse(2.5);
        }

        private void WanderTimerTick(object sender, EventArgs e)
        {
            WanderRound(2.5);
        }

        private void TakeTimerTick(object sender, EventArgs e)
        {
            TakeMouse(2.5);
        }

        private void MouseMoveTick(object sender, EventArgs e)
        {
            MouseHandler.SetMousePosition(MouseTakenLocation);
        }

        public void CatChaseMouseStart()
        {
            CatCurrentActionStop();
            CatSprite.RunStart();
            CatChaseMouseTimer.Start();
        }

        public void CatWanderRoundStart()
        {
            CatCurrentActionStop();
            CatSprite.WalkStart();
            ResetWanderDestination();
            CatWanderTimer.Start();
        }

        public void CatTakeMouseStart()
        {
            StopAllTimer();
            CatSprite.StopAll();

            MouseTaken = true;
            CatSprite.WalkStart();
            ResetWanderDestination();
            CatTakeMouseTimer.Start();
            MoveMouseTimer.Start();
        }

        public void CatCurrentActionStop()
        {
            CatSprite.StopAll();
            StopAllTimer();
            MouseTaken = false;
        }

        private void StopAllTimer()
        {
            if (CatChaseMouseTimer.IsEnabled)
                CatChaseMouseTimer.Stop();

            if (CatWanderTimer.IsEnabled)
                CatWanderTimer.Stop();

            if (CatTakeMouseTimer.IsEnabled)
                CatTakeMouseTimer.Stop();

            if(MoveMouseTimer.IsEnabled)
                MoveMouseTimer.Stop();
        }

        private void ResetWanderDestination()
        {
            var height = SystemParameters.PrimaryScreenHeight;
            var width = SystemParameters.PrimaryScreenWidth;
            WanderDestination = new Point(random.Next(0, Convert.ToInt32(width)), random.Next(0, Convert.ToInt32(height)));
        }

        private Vector GetDirection(Point position, double CatLeft, double CatTop)
        {
            var direction = new Vector();

            direction.X = (position.X - Cat.Width / 2) - CatLeft;
            direction.Y = (position.Y - Cat.Height / 2) - CatTop;

            return direction;
        }

        private double GetDistance(Vector direction)
        {
            return Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
        }

        public double GetDistanceFromCatToMouse()
        {
            return GetDistance(GetDirection(MouseHandler.GetMousePosition(), Canvas.GetLeft(Cat), Canvas.GetTop(Cat)));
        }

        private void MoveCatSprite(Vector direction, double distance, double CatLeft, double CatTop, double catSpeed, Point position)
        {
            direction.Y /= distance;
            direction.X /= distance;

            Canvas.SetLeft(Cat, CatLeft + (direction.X * catSpeed));
            Canvas.SetTop(Cat, CatTop + (direction.Y * catSpeed));

            TurnLeftOrRight((position.X - Cat.Width / 2) - CatLeft);
        }

        private void TurnLeftOrRight(double nowX)
        {
            if (0 > nowX)
            {
                CatSprite.WalkLeft();
                CatSprite.StationaryLeft();
                CatSprite.RunLeft();
                CatFaceLeft = true;
            }
            else if (0 < nowX)
            {
                CatSprite.WalkRight();
                CatSprite.StationaryRight();
                CatSprite.RunRight();
                CatFaceLeft = false;
            }
        }

        private void ChaseMouse(double catSpeed)
        {
            Point position = MouseHandler.GetMousePosition();
            double CatLeft = Canvas.GetLeft(Cat);
            double CatTop = Canvas.GetTop(Cat);
            double threshold = 5;

            var direction = GetDirection(position, CatLeft, CatTop);
            double distance = GetDistance(direction);

            if(distance > threshold)
            {
                MoveCatSprite(direction, distance, CatLeft, CatTop, catSpeed, position);
            }
            else
            {
                CatTakeMouseStart();
            }
        }

        private void WanderRound(double catSpeed)
        {
            Point position = WanderDestination;
            double CatLeft = Canvas.GetLeft(Cat);
            double CatTop = Canvas.GetTop(Cat);
            double threshold = Cat.Width/2;

            var direction = GetDirection(position, CatLeft, CatTop);
            double distance = GetDistance(direction);

            if (distance > threshold)
            {
                MoveCatSprite(direction, distance, CatLeft, CatTop, catSpeed, position);
            }
            else
            {
                ResetWanderDestination();
            }
        }

        private void TakeMouse(double catSpeed)
        {
            Point position = WanderDestination;
            double CatLeft = Canvas.GetLeft(Cat);
            double CatTop = Canvas.GetTop(Cat);
            double threshold = Cat.Width / 2;

            var direction = GetDirection(position, CatLeft, CatTop);
            double distance = GetDistance(direction);

            if (distance > threshold)
            {
                MoveCatSprite(direction, distance, CatLeft, CatTop, catSpeed, position);
            }
            else
            { 
                MouseTaken = false;
                CatSprite.WalkStop();
                CatSprite.StationaryStart();
                MoveMouseTimer.Stop();
            }
        }
    }
}
