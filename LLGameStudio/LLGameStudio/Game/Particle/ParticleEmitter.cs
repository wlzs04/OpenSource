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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace LLGameStudio.Game.Particle
{
    enum ParticleType
    {
        Point,//点
        Star,//星
        Image,//图片
        Sequence//序列图
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
        public Property.LoopTime loopTime = new Property.LoopTime();
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
        public Property.ImagePath imagePath = new Property.ImagePath();
        public Property.Row row = new Property.Row();
        public Property.Column column = new Property.Column();

        bool enable = true;
        bool play = true;
        double currentPlayTime = 0;//粒子当前已经播放的时间，每循环一次重新计时。
        int currentImageIndex = 0;//粒子类型为序列图时，当前播放的是第几帧。
        ImageBrush imageBrush;
        List<ImageSource> imageSequenceList = new List<ImageSource>();

        List<Particle> particleList = new List<Particle>();
        Canvas canvas;
        Random random = new Random();

        public ParticleEmitter(ParticleSystem particleSystem,Canvas canvas)
        {
            this.particleSystem = particleSystem;
            this.canvas = canvas;
            
            AddProperty(isLoop);
            AddProperty(loopTime);
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
            AddProperty(imagePath);
            AddProperty(row);
            AddProperty(column);
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }

        void InitParticle()
        {
            ResetParticle();
            canvas.Children.Clear();
            switch (particleType.Value)
            {
                case ParticleType.Point:
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
                    for (int i = 0; i < maxNumber.Value; i++)
                    {
                        Polygon polygon = new Polygon();
                        polygon.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color.Value));
                        polygon.FillRule = FillRule.Nonzero;
                        PointCollection myPointCollection = new PointCollection();
                        myPointCollection.Add(new System.Windows.Point(10, 100));
                        myPointCollection.Add(new System.Windows.Point(50, 0));
                        myPointCollection.Add(new System.Windows.Point(90, 100));
                        myPointCollection.Add(new System.Windows.Point(0, 35));
                        myPointCollection.Add(new System.Windows.Point(100, 35));
                        polygon.Points = myPointCollection;
                        polygon.Width = 100;
                        polygon.Height = 100;
                        polygon.RenderTransform = new ScaleTransform(0, 0);
                        canvas.Children.Add(polygon);
                    }
                    break;
                case ParticleType.Image:
                    imageBrush = new ImageBrush();
                    if (imagePath.Value != "")
                    {
                        imageBrush.ImageSource = new BitmapImage(new Uri(imagePath.Value, UriKind.Absolute));
                    }
                    else
                    {
                        imageBrush.ImageSource = null;
                    }
                    for (int i = 0; i < maxNumber.Value; i++)
                    {
                        System.Windows.Controls.Image ellipse = new System.Windows.Controls.Image();
                        ellipse.Source = imageBrush.ImageSource;
                        ellipse.Width = 0;
                        ellipse.Height = 0;
                        canvas.Children.Add(ellipse);
                    }
                    break;
                case ParticleType.Sequence:
                    imageSequenceList.Clear();
                    BitmapSource bitmap = new BitmapImage(new Uri(imagePath.Value, UriKind.Absolute));
                    double imageWidth = bitmap.PixelWidth;
                    int everyWidth = (int)(imageWidth / column.Value);
                    double imageHeight = bitmap.PixelHeight;
                    int everyHeight = (int)(imageHeight / row.Value);
                    for (int i = 0; i < row.Value; i++)
                    {
                        for (int j = 0; j < column.Value; j++)
                        {
                            ////定义切割矩形
                            //var cut = new Int32Rect(j* everyWidth, i* everyHeight, everyWidth, everyHeight);
                            ////计算Stride
                            //var stride = bitmap.Format.BitsPerPixel * cut.Width / 8;
                            ////声明字节数组
                            //int[] data = new int[cut.Height * stride];
                            ////调用CopyPixels
                            //bitmap.CopyPixels(cut, data, stride, 0);
                            ImageBrush imageBrush = new ImageBrush(); 
                            imageSequenceList.Add(new CroppedBitmap(bitmap, new Int32Rect(j * everyWidth, i * everyHeight, everyWidth, everyHeight)));
                        }
                    }
                    for (int i = 0; i < maxNumber.Value; i++)
                    {
                        System.Windows.Controls.Image ellipse = new System.Windows.Controls.Image();
                        ellipse.Width = 0;
                        ellipse.Height = 0;
                        canvas.Children.Add(ellipse);
                    }
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
            if(name=="color"
                || name=="maxNumber"
                ||name== "particleType"
                || name == "imagePath"
                || name == "row"
                || name == "column"
                )
            {
                InitParticle();
                currentPlayTime = 0;
            }
        }

        void AddParticle()
        {
            if(particleList.Count>maxNumber.Value)
            {
                return;
            }
            double x = position.Value.X + random.NextDouble() * positionError.Value.X;
            double y = position.Value.Y + random.NextDouble() * positionError.Value.Y;
            double radius = (radiusError.Value-2*random.NextDouble()* radiusError.Value) + this.radius.Value;
            double angle =((1 - 2 * random.NextDouble()) * angleRange.Value / 180 * Math.PI);
            double dx = direction.Value.X * Math.Cos(angle) - direction.Value.Y * Math.Sin(angle);
            double dy = direction.Value.X * Math.Sin(angle) + direction.Value.Y * Math.Cos(angle);
            double vx = dx * (velocity.Value + random.NextDouble() * velocityError.Value);
            double vy = dy * (velocity.Value + random.NextDouble() * velocityError.Value);
            Particle particle = new Particle(x, y, radius, vx, vy);
            particleList.Add(particle);
        }

        /// <summary>
        /// 开始播放粒子动画
        /// </summary>
        public void StartPlay()
        {
            InitParticle();
            play = true;
            currentPlayTime = 0;
        }

        /// <summary>
        /// 暂停粒子动画
        /// </summary>
        public void PausePlay()
        {
            play = false;
        }

        /// <summary>
        /// 停止粒子动画
        /// </summary>
        public void StopPlay()
        {
            play = false;
            currentPlayTime = 0;
        }

        /// <summary>
        /// 将所有粒子重置为最初状态。
        /// </summary>
        void ResetParticle()
        {
            particleList.Clear();
            for (int i = 0; i < startNumber.Value; i++)
            {
                AddParticle();
            }
        }

        /// <summary>
        /// 更新位置等信息
        /// </summary>
        /// <param name="thisTickTime"></param>
        public void Update(double thisTickTime)
        {
            if(!enable||!play)
            {
                return;
            }

            if (isLoop.Value && currentPlayTime > loopTime.Value)
            {
                ResetParticle();
                currentPlayTime = 0;
                currentImageIndex = 0;
            }

            currentPlayTime += thisTickTime;
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

            if(particleType.Value==ParticleType.Sequence)
            {
                currentImageIndex = (int)((currentPlayTime / loopTime.Value) * row.Value * column.Value);

                if (currentImageIndex>=row.Value*column.Value)
                {
                    currentImageIndex= row.Value * column.Value-1;
                }
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
            int y = 0;
            int particleCount = particleList.Count;
            switch (particleType.Value)
            {
                case ParticleType.Point:
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
                    foreach (Polygon item in canvas.Children)
                    {
                        if (y < particleCount)
                        {
                            Particle particle = particleList[y];
                            Vector2 parentPosition = particleSystem.GetPosition();
                            item.Margin = new Thickness(parentPosition.X + particle.x, parentPosition.Y + particle.y, 0, 0);
                            item.RenderTransform = new ScaleTransform(particle.radius * 2/100, particle.radius * 2 / 100);
                        }
                        else
                        {
                            item.RenderTransform = new ScaleTransform(0, 0);
                        }
                        y++;
                    }
                    break;
                case ParticleType.Image:
                    foreach (System.Windows.Controls.Image item in canvas.Children)
                    {
                        if (y < particleCount)
                        {
                            Particle particle = particleList[y];
                            Vector2 parentPosition = particleSystem.GetPosition();
                            item.Margin = new Thickness(parentPosition.X + particle.x, parentPosition.Y + particle.y, 0, 0);
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
                case ParticleType.Sequence:
                    foreach (System.Windows.Controls.Image item in canvas.Children)
                    {
                        if (y < particleCount)
                        {
                            Particle particle = particleList[y];
                            Vector2 parentPosition = particleSystem.GetPosition();
                            item.Margin = new Thickness(parentPosition.X + particle.x, parentPosition.Y + particle.y, 0, 0);
                            item.Width = particle.radius * 2;
                            item.Height = particle.radius * 2;
                            item.Source = imageSequenceList[currentImageIndex];
                        }
                        else
                        {
                            item.Width = 0;
                            item.Height = 0;
                        }
                        y++;
                    }
                    break;
                default:
                    break;
            }
        }

        public void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
        }

        void LoadAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            foreach (var item in propertyDictionary)
            {
                xAttribute = element.Attribute(item.Key);
                if (xAttribute != null) { item.Value.Value = xAttribute.Value; xAttribute.Remove(); }
            }
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("ParticleEmitter");
            ExportAttrbuteToXML(element);
            return element;
        }

        void ExportAttrbuteToXML(XElement element)
        {
            foreach (var item in propertyDictionary)
            {
                if (!item.Value.IsDefault)
                {
                    element.Add(new XAttribute(item.Value.Name, item.Value.Value));
                }
            }
        }
    }

    namespace Property
    {
        public class IsLoop : IUIProperty
        {
            public IsLoop() : base("isLoop", typeof(bool), UIPropertyEnum.Common, "是否循环。", "True") { }
        }

        public class LoopTime : IUIProperty
        {
            public LoopTime() : base("loopTime", typeof(double), UIPropertyEnum.Common, "粒子动画每次循环的时间。", "5") { }
        }

        public class ParticleType : IUIProperty
        {
            public ParticleType() : base("particleType", typeof(Game.Particle.ParticleType), UIPropertyEnum.Common, "粒子类型。", "Point") { }
        }

        public class MaxNumber : IUIProperty
        {
            public MaxNumber() : base("maxNumber", typeof(int), UIPropertyEnum.Common, "粒子最大数量。", "10") { }
        }

        public class StartNumber : IUIProperty
        {
            public StartNumber() : base("startNumber", typeof(int), UIPropertyEnum.Common, "粒子开始时的数量。", "0") { }
        }

        public class CreateNumberBySecond : IUIProperty
        {
            public CreateNumberBySecond() : base("createNumberBySecond", typeof(int), UIPropertyEnum.Common, "粒子每秒创建数量。", "2") { }
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

        public class ImagePath : IUIProperty
        {
            public ImagePath() : base("imagePath", typeof(string), UIPropertyEnum.Common, "粒子图片。", "") { }
        }

        public class Row : IUIProperty
        {
            public Row() : base("row", typeof(int), UIPropertyEnum.Common, "//粒子类别为序列图时拥有的行数.", "1") { }
        }

        public class Column : IUIProperty
        {
            public Column() : base("column", typeof(int), UIPropertyEnum.Common, "粒子类别为序列图时拥有的列数。", "1") { }
        }
    }
}
