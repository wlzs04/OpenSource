using LLGameStudio.Common.DataType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LLGameStudio.Game.Particle
{
    enum ParticleType
    {
        Point,
        Star,
        Image,
        Sequence
    }
    
    struct Particle
    {
        public double x;
        public double y;
        public double radius;
        public double leftTime;
        Vector2 particleDirection;

        public Particle(double x, double y, double radius , Vector2 particleDirection)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.particleDirection = particleDirection;
            leftTime = Double.MaxValue;
        }

        public Particle(double x, double y, double radius, double directionX, double directionY)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            particleDirection = new Vector2(directionX, directionY);
            leftTime = Double.MaxValue;
        }

        public Particle(double x, double y, double radius,double leftTime ,double directionX, double directionY)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            particleDirection = new Vector2(directionX, directionY);
            this.leftTime = leftTime;
        }

        public bool MoveByTime(double time)
        {
            x += particleDirection.X * time;
            y += particleDirection.Y * time;
            leftTime -= time;
            return leftTime <= 0;
        }
    }

    class ParticleEmitter
    {
        bool enable = true;
        bool play = true;
        bool isLoop = true;
        ParticleType particleType = ParticleType.Point;//粒子类型
        string particleImagePath = "";//粒子图片
        int row = 1;//粒子类别为序列图时拥有的行数
        int column = 1;//粒子类别为序列图时拥有的行数
        int particleMaxNumber = 10;//粒子最大数量
        int particleStartNumber = 1;//粒子开始时的粒子数量
        int particleCreateNumberBySecond = 2;//粒子每秒创建数量
        double particleLoopTime = 0;//粒子动画每次循环的时间，0代表不循环
        Vector2 particlePositionError = new Vector2(0, 0);//粒子起点位置误差
        System.Windows.Media.Color particleColor = Colors.White;//粒子颜色
        double radius = 3;//粒子半径
        double radiusError = 3;//粒子半径误差
        double particleVelocity = 10;//粒子速度
        double particleVelocityError = 2;//粒子速度误差
        Vector2 particleDirection = new Vector2(0, -1);//粒子发射方向
        double angleRange = 30;//粒子发射角度范围，0~180
        List<Particle> particleList = new List<Particle>();
        Canvas canvas;
        Random random = new Random();

        public ParticleEmitter(Canvas canvas)
        {
            this.canvas = canvas;
        }

        void InitParticle()
        {
            particleList.Clear();
            canvas.Children.Clear();
            switch (particleType)
            {
                case ParticleType.Point:
                    for (int i = 0; i < particleStartNumber; i++)
                    {
                        AddParticle();
                    }
                    for (int i = 0; i < particleMaxNumber; i++)
                    {
                        Ellipse ellipse = new Ellipse();
                        ellipse.Fill = new SolidColorBrush(particleColor);
                        ellipse.Width = 0;
                        ellipse.Height = 0;
                        canvas.Children.Add(ellipse);
                    }
                    break;
                case ParticleType.Star:
                    break;
                case ParticleType.Image:
                    break;
                case ParticleType.Sequence:
                    break;
                default:
                    break;
            }
        }

        void AddParticle()
        {
            double x = random.NextDouble() * particlePositionError.X;
            double y = random.NextDouble() * particlePositionError.Y;
            double radius = random.NextDouble() * this.radius;
            double angle = (random.NextDouble() * angleRange / 180);
            double vx = particleDirection.X * Math.Cos(angle) - particleDirection.Y * Math.Sin(angle);
            double vy = particleDirection.X * Math.Sin(angle) + particleDirection.Y * Math.Cos(angle);
            Particle particle = new Particle(x, y, radius, vx, vy);
            particleList.Add(new Particle());
        }

        public void StartPlay()
        {
            InitParticle();
            
            play = true;
        }

        public void StopPlay()
        {
            play = false;
        }

        public void Update()
        {
            if(!enable||!play)
            {
                return;
            }
            double thisTickTime = 0.016;
            double asd = thisTickTime * particleCreateNumberBySecond;
            int addNumber = random.NextDouble();

            //粒子移动和移除。
            for (int i=0;i< particleList.Count;i++)
            {
                if(particleList[i].MoveByTime(0))
                {
                    particleList.RemoveAt(i);
                }
            }
        }

        public void Render()
        {
            if (!enable)
            {
                return;
            }
            foreach (var item in particleList)
            {
                
            }
        }
    }
}
