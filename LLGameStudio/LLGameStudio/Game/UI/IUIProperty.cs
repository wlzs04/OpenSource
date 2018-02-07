using LLGameStudio.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI节点的属性类，有需要与XML文件交互或
    /// 需要与图形化界面关联的属性可以继承此类。
    /// </summary>
    public abstract class IUIProperty
    {
        protected string name;
        protected Type type;
        protected UIPropertyEnum propertyEnum;
        protected string helpText;
        protected string defaultValue;
        protected dynamic value;

        public string Name { get => name; }
        public Type Type { get => type; }
        public UIPropertyEnum PropertyEnum { get => propertyEnum; }
        public string HelpText { get => helpText;}
        public string DefaultValue { get => defaultValue;}
        public dynamic Value { get => value; set => this.value = LLConvert.ChangeType(value, type); }
        public bool IsDefault { get => defaultValue == Value.ToString(); }

        public IUIProperty(string name, Type type , UIPropertyEnum propertyEnum = UIPropertyEnum.Other, string helpText="此属性意义不明。",string defaultValue="0")
        {
            this.name = name;
            this.type = type;
            this.propertyEnum = propertyEnum;
            this.helpText = helpText;
            this.defaultValue = defaultValue;
            Value= defaultValue;
        }
    }

    /// <summary>
    /// 实现此接口的类或结构体可以使用LLConvert类的ChangeType方法
    /// 与字符串类型进行相互转换。
    /// </summary>
    interface IConvertStringClass
    {
        void FromString(string s);
        string ToString();
    }

    /// <summary>
    /// UI属性枚举，用于描述属性属于哪一分类。
    /// </summary>
    public enum UIPropertyEnum
    {
        Transform,//变换
        Common,//通用
        Other,//其他
    }
}
