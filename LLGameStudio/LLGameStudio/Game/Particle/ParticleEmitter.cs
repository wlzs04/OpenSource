using LLGameStudio.Common.DataType;
using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace LLGameStudio.Game.Particle
{
    enum ParticleType
    {
        Point,
        Star,
        Image,
        Sequence
    }
    
    class Particle
    {
        public double x;
        public double y;
        public double radius;
        public double leftTime;
        Vector2 particleDirection;

        public Particle(UIElement uIElement, double x, double y, double radius , Vector2 particleDirection)
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

    public class ParticleEmitter: IXMLClass
    {
        ParticleSystem particleSystem;

        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();
        public Property.IsLoop isLoop = new Property.IsLoop();
        public Property.ParticleType particleType = new Property.ParticleType();
        public Property.MaxNumber maxNumber = new Property.MaxNumber();
        public Property.StartNumber startNumber = new Property.StartNumber();
        public Property.CreateNumberBySecond createNumberBySecond = new Property.CreateNumberBySecond();
        public Property.Radius radius = new Property.Radius();
        public Property.RadiusError radiusError = new Property.RadiusError();
        public Property.Color color = new Property.Color();
        public Property.Velocity velocity = new Property.Velocity();
        public Property.VelocityError velocityError = new Property.VelocityError();
        public Property.Direction direction = new Property.Direction();
        public Property.AngleRange angleRange = new Property.AngleRange();
        public Property.Position position = new Property.Position();
        public Property.PositionError positionError = new Property.PositionError();
        
        bool enable = true;
        bool play = true;
        
        string particleImagePath = "";//粒子图片
        int row = 1;//粒子类别为序列图时拥有的行数
        int column = 1;//粒子类别为序列图时拥有的行数
        double particleLoopTime = 0;//粒子动画每次循环的时间，0代表不循环
        
        List<Particle> particleList = new List<Particle>();
        Canvas canvas;
        Random random = new Random();

        public ParticleEmitter(ParticleSystem particleSystem,Canvas canvas)
        {
            this.particleSystem = particleSystem;
            this.canvas = canvas;

            AddProperty(isLoop);
            AddProperty(particleType);
            AddProperty(maxNumber);
            AddProperty(startNumber);
            AddProperty(createNumberBySecond);
            AddProperty(radius);
            AddProperty(radiusError);
            AddProperty(color);
            AddProperty(velocity);
            AddProperty(velocityError);
            AddProperty(direction);
            AddProperty(angleRange);
            AddProperty(position);
            AddProperty(positionError);
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        void InitParticle()
        {
            particleList.Clear();
            canvas.Children.Clear();
            switch (particleType.Value)
            {
                case ParticleType.Point:
                    for (int i = 0; i < startNumber.Value; i++)
                    {
                        AddParticle();
                    }
                    for (int i = 0; i < maxNumber.Value; i++)
                    {
                        Ellipse ellipse = new Ellipse();
                        ellipse.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color.Value));
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

        void AddProperty(IUIProperty property)
        {
            propertyDictionary.Add(property.Name, property);
        }

        public void SetProperty(string name,string value)
        {
            propertyDictionary[name].Value = value;
        }

        void AddParticle()
        {
            if(particleList.Count>maxNumber.Value)
            {
                return;
            }
            double x = random.NextDouble() * positionError.Value.X;
            double y = random.NextDouble() * positionError.Value.Y;
            double radius = (radiusError.Value-2*random.NextDouble()* radiusError.Value) + this.radius.Value;
            double angle = (random.NextDouble() * angleRange.Value / 180);
            double dx = direction.Value.X * Math.Cos(angle) - direction.Value.Y * Math.Sin(angle);
            double dy = direction.Value.X * Math.Sin(angle) + direction.Value.Y * Math.Cos(angle);
            double vx = dx * (velocity.Value + random.NextDouble() * velocityError.Value);
            double vy = dy * (velocity.Value + random.NextDouble() * velocityError.Value);
            Particle particle = new Particle(x, y, radius, vx, vy);
            particleList.Add(particle);
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
            double particleCreateNumberThisTickTime = thisTickTime * createNumberBySecond.Value;
            int addNumber = (int)particleCreateNumberThisTickTime;
            if (random.NextDouble() < particleCreateNumberThisTickTime - addNumber)
            {
                addNumber++;
            }

            for (int i = 0; i < addNumber; i++)
            {
                AddParticle();
            }

            //粒子移动和移除。
            for (int i=0;i< particleList.Count;)
            {
                if(particleList[i].MoveByTime(thisTickTime))
                {
                    particleList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void Render()
        {
            if (!enable)
            {
                return;
            }
            switch (particleType.Value)
            {
                case ParticleType.Point:
                    int y = 0;
                    int particleCount = particleList.Count;
                    foreach (Ellipse item in canvas.Children)
                    {
                        if(y<particleCount)
                        {
                            Particle particle = particleList[y];
                            Vector2 parentPosition= particleSystem.GetPosition();
                            item.Margin = new Thickness(parentPosition.X+particle.x, parentPosition.Y + particle.y, 0, 0);
                            item.Width = particle.radius * 2;
                            item.Height = particle.radius * 2;
                        }
                        else
                        {
                            item.Width = 0;
                            item.Height = 0;
                        }
                        y++;
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

        public void LoadContentFromXML(XElement element)
        {
            throw new NotImplementedException();
        }

        public XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }
    }

    namespace Property
    {
        public class IsLoop : IUIProperty
        {
            public IsLoop() : base("isLoop", typeof(bool), UIPropertyEnum.Common, "是否循环。", "True") { }
        }

        public class ParticleType : IUIProperty
        {
            public ParticleType() : base("particleType", typeof(Game.Particle.ParticleType), UIPropertyEnum.Common, "粒子类型。", "Point") { }
        }

        public class MaxNumber : IUIProperty
        {
            public MaxNumber() : base("MaxNumber", typeof(int), UIPropertyEnum.Common, "粒子最大数量。", "10") { }
        }
        

        public class StartNumber : IUIProperty
        {
            public StartNumber() : base("StartNumber", typeof(int), UIPropertyEnum.Common, "粒子开始时的数量。", "1") { }
        }

        public class CreateNumberBySecond : IUIProperty
        {
            public CreateNumberBySecond() : base("createNumberBySecond", typeof(int), UIPropertyEnum.Common, "粒子每秒创建数量。", "1") { }
        }

        public class Radius : IUIProperty
        {
            public Radius() : base("radius", typeof(double), UIPropertyEnum.Common, "粒子半径。", "3") { }
        }

        public class RadiusError : IUIProperty
        {
            public RadiusError() : base("radiusError", typeof(double), UIPropertyEnum.Common, "粒子半径误差。", "1") { }
        }

        public class Color : IUIProperty
        {
            public Color() : base("color", typeof(string), UIPropertyEnum.Common, "粒子颜色。", "#FFFFFFFF") { }
        }
        
        public class Velocity : IUIProperty
        {
            public Velocity() : base("velocity", typeof(double), UIPropertyEnum.Common, "粒子速度。", "20") { }
        }
        
        public class VelocityError : IUIProperty
        {
            public VelocityError() : base("velocityError", typeof(double), UIPropertyEnum.Common, "粒子速度误差。", "2") { }
        }
        
        public class Direction : IUIProperty
        {
            public Direction() : base("direction", typeof(Vector2), UIPropertyEnum.Common, "粒子发射方向。", "{0,-1}") { }
        }
        
        public class AngleRange : IUIProperty
        {
            public AngleRange() : base("angleRange", typeof(double), UIPropertyEnum.Common, "粒子发射角度范围，0~180。", "30") { }
        }

        public class Position : IUIProperty
        {
            public Position() : base("position", typeof(Vector2), UIPropertyEnum.Transform, "粒子发射位置。", "{0,0}") { }
        }

        public class PositionError : IUIProperty
        {
            public PositionError() : base("positionError", typeof(Vector2), UIPropertyEnum.Transform, "粒子发射位置误差。", "{0,0}") { }
        }
    }
}
