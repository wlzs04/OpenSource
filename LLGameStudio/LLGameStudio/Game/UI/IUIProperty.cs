using LLGameStudio.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Game.UI
{
    abstract class IUIProperty
    {
        protected string name;
        protected Type type;
        protected UIPropertyEnum propertyEnum;
        protected string helpText;
        protected string defaultValue;
        protected dynamic value;

        public string Name { get => name; }
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

    enum UIPropertyEnum
    {
        Transform,
        Common,
        Other,
    }
}
