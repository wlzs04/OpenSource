using LLGameStudio.Game.Particle;
using LLGameStudio.Studio.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LLGameStudio.Studio.Window
{
    /// <summary>
    /// ParticleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParticleWindow : System.Windows.Window
    {
        string filePath = "";
        ParticleSystem particleSystem;
        List<ParticleEmitter> particleEmitters=new List<ParticleEmitter>();

        public ParticleWindow(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            
            textBoxParticleName.Text=System.IO.Path.GetFileNameWithoutExtension(filePath);
            particleSystem = new ParticleSystem(textBoxParticleName.Text, canvas);

            ContextMenu gridContextMenu = new ContextMenu();
            MenuItem addParticleEmitter = new MenuItem();
            addParticleEmitter.Header = "添加发射器";
            addParticleEmitter.Click += AddParticleEmitter;
            gridContextMenu.Items.Add(addParticleEmitter);
            gridEmitters.ContextMenu = gridContextMenu;
        }

        /// <summary>
        /// 移动窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageMinimizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageMaximizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState==WindowState.Maximized? WindowState.Normal: WindowState.Maximized;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageExitWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 添加粒子发射器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddParticleEmitter(object sender, RoutedEventArgs e)
        {
            ParticleEmitter emitter = new ParticleEmitter(particleSystem,canvas);
            particleEmitters.Add(emitter);
            particleSystem.AddEmitter(emitter);
            emitter.StartPlay();

            LLStudioParticleEmitterEdit emitterEdit = new LLStudioParticleEmitterEdit(emitter);
            stackPanelEmitters.Children.Add(emitterEdit);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            particleSystem.Update();
            particleSystem.Render();
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            particleSystem.SetPosition(canvas.Margin.Left+canvas.ActualWidth/2, canvas.Margin.Top + canvas.ActualHeight / 2);
        }
    }
}
