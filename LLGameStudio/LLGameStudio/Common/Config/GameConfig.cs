﻿using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.Config
{
    /// <summary>
    /// 游戏使用的图形API
    /// </summary>
    enum GraphicsApiType
    {
        Direct2D,//Direct2D
        LL2D,//未实现
    }

    /// <summary>
    /// 游戏配置
    /// </summary>
    public class GameConfig : IXMLClass
    {
        //int width=800;
        //int height=600;
        //string gameName="游戏";
        //bool fullScreen=false;
        //bool canMultiGame = false;
        
        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();
        Property.GameName gameName = new Property.GameName();
        Property.Width width = new Property.Width();
        Property.Height height = new Property.Height();
        Property.FullScreen fullScreen = new Property.FullScreen();
        Property.CanMultiGame canMultiGame = new Property.CanMultiGame();
        Property.StartScene startScene = new Property.StartScene();
        Property.GraphicsApi graphicsApi = new Property.GraphicsApi();
        Property.OpenNetClient openNetClient = new Property.OpenNetClient();
        Property.OpenPhysics openPhysics = new Property.OpenPhysics();
        Property.Icon icon = new Property.Icon();
        Property.DefaultCursor defaultCursor = new Property.DefaultCursor();
        
        public int Width { get => width.Value; set => width.Value = value; }
        public int Height { get => height.Value; set => height.Value = value; }
        public string GameName { get => gameName.Value; set => gameName.Value = value; }
        public bool FullScreen { get => fullScreen.Value; set => fullScreen.Value = value; }
        public bool CanMultiGame { get => canMultiGame.Value; set => canMultiGame.Value = value; }
        
        public GameConfig()
        {
            AddProperty(gameName);
            AddProperty(width);
            AddProperty(height);
            AddProperty(fullScreen);
            AddProperty(canMultiGame);
            AddProperty(startScene);
            AddProperty(graphicsApi);
            AddProperty(openNetClient);
            AddProperty(openPhysics);
            AddProperty(icon);
            AddProperty(defaultCursor);
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property"></param>
        void AddProperty(IUIProperty property)
        {
            propertyDictionary.Add(property.Name, property);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name,string value)
        {
            propertyDictionary[name].Value = value;
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Game");
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
    }

    namespace Property
    {
        public class GameName : IUIProperty
        {
            public GameName() : base("gameName", typeof(string), UIPropertyEnum.Common, "游戏名称。", "游戏") { }
        }

        public class Width : IUIProperty
        {
            public Width() : base("width", typeof(int), UIPropertyEnum.Transform, "游戏窗体宽度。", "800") { }
        }

        public class Height : IUIProperty
        {
            public Height() : base("height", typeof(int), UIPropertyEnum.Transform, "游戏窗体高度。", "600") { }
        }

        public class FullScreen : IUIProperty
        {
            public FullScreen() : base("fullScreen", typeof(bool), UIPropertyEnum.Common, "游戏窗体是否全屏。", "False") { }
        }

        public class CanMultiGame : IUIProperty
        {
            public CanMultiGame() : base("canMultiGame", typeof(bool), UIPropertyEnum.Common, "是否可以同时开启多个游戏。", "False") { }
        }

        public class StartScene : IUIProperty
        {
            public StartScene() : base("startScene", typeof(string), UIPropertyEnum.Common, "游戏开始时加载的场景。", "layout/StartScene.scene") { }
        }

        public class GraphicsApi : IUIProperty
        {
            public GraphicsApi() : base("graphicsApi", typeof(GraphicsApiType), UIPropertyEnum.Common, "游戏使用的图形API。", "Direct2D") { }
        }

        public class OpenNetClient : IUIProperty
        {
            public OpenNetClient() : base("openNetClient", typeof(bool), UIPropertyEnum.Common, "是否开启网络传输功能。", "False") { }
        }

        public class OpenPhysics : IUIProperty
        {
            public OpenPhysics() : base("openPhysics", typeof(bool), UIPropertyEnum.Common, "是否开启物理模拟功能。", "False") { }
        }

        public class Icon : IUIProperty
        {
            public Icon() : base("icon", typeof(string), UIPropertyEnum.Common, "游戏使用的图标。", "") { }
        }

        public class DefaultCursor : IUIProperty
        {
            public DefaultCursor() : base("defaultCursor", typeof(string), UIPropertyEnum.Common, "游戏默认的光标。", "") { }
        }
    }
}
