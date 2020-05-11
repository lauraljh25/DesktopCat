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
    public partial class MainWindow : Window
    {
        readonly CatActions catActions;

        public MainWindow()
        {
            InitializeComponent();

            SetupCloseHotKey();
            Sprite catSprite = new Sprite(SpriteSheetImageBrush);
            catActions = new CatActions(PlaceHolderCat, CatArea, 5, catSprite);
            
            CatBehaviour catBehaviour = new CatBehaviour(catActions);

            catBehaviour.startCatBehaviourChaseMouseWhenNear();           
        }

        private void SetupCloseHotKey()
        {
            RoutedCommand CloseHotKeys = new RoutedCommand();
            CloseHotKeys.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(CloseHotKeys, CloseWindow));
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            window.Close();
        }

        private void CatArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (catActions.MouseTaken)
            {
                MouseHandler.SetMousePosition(catActions.MouseTakenLocation);
            }
        }
    }
}
