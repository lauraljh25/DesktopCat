using System;
using System.Windows.Threading;

namespace DesktopCat
{
    public class CatBehaviour
    {
        public CatActions catActions { get; set; }
        private DispatcherTimer behaviourRandomTimer = new DispatcherTimer();
        private bool wander;
        private static Random random = new Random();

        private DispatcherTimer behaviourChaseMouseWhenNearTimer = new DispatcherTimer();

        private bool gotMouse = false;
        private int catStationaryCounter = 0;

        public CatBehaviour(CatActions catActions)
        {
            this.catActions = catActions;
            SetupTimers();
        }

        private void SetupTimers()
        {
            behaviourRandomTimer.Interval = TimeSpan.FromSeconds(random.Next(10, 120));
            behaviourRandomTimer.Tick += ChangeBehaviourRandomTimerTick;

            behaviourChaseMouseWhenNearTimer.Interval = TimeSpan.FromMilliseconds(1);
            behaviourChaseMouseWhenNearTimer.Tick += BehaviourChaseMouseWhenNearTick;
        }

        #region randomBehaviour

        private void ChangeBehaviourRandomTimerTick(object sender, EventArgs e)
        {
            if (wander)
            {
                catActions.CatWanderRoundStart();
                behaviourRandomTimer.Interval = TimeSpan.FromSeconds(random.Next(10, 120));
                wander = false;
            }
            else
            {
                catActions.CatChaseMouseStart();
                behaviourRandomTimer.Interval = TimeSpan.FromSeconds(random.Next(10, 120));
                wander = true;
            }
        }

        public void StartRandomCatBehaviour()
        {
            int behaviour = random.Next(0, 3);

            if (behaviour == 1)
            {
                catActions.CatChaseMouseStart();
                wander = true;
            }
            else
            {
                catActions.CatWanderRoundStart();
                wander = false;
            }

            behaviourRandomTimer.Start();
        }

        #endregion randomBehaviour

        #region catChaseMouseWhenNearBehaviour

        public void startCatBehaviourChaseMouseWhenNear()
        {
            catActions.CatWanderRoundStart();
            behaviourChaseMouseWhenNearTimer.Start();
            wander = true;
        }

        private void BehaviourChaseMouseWhenNearTick(object sender, EventArgs e)
        {
            if(wander)
            {
                if(catActions.GetDistanceFromCatToMouse() < 400)
                {
                    catActions.CatChaseMouseStart();
                    wander = false;
                }
            }
            else
            {
                if(gotMouse)
                {
                    //cat has released mouse
                    if(gotMouse != catActions.MouseTaken)
                    {
                        if(catStationaryCounter == 1000)
                        {
                            catActions.CatWanderRoundStart();
                            wander = true;
                            catStationaryCounter = 0;
                        }
                        else
                        {
                            catStationaryCounter++;
                        }
                    }
                }
                else
                {
                    gotMouse = catActions.MouseTaken;
                }
            }
        }

        #endregion catChaseMouseWhenNearBehaviour
    }
}
