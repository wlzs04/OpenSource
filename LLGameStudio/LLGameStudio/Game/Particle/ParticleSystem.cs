using LLGameStudio.Common.DataType;
using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace LLGameStudio.Game.Particle
{
    public class ParticleSystem : IXMLClass
    {
        Canvas canvas;
        bool enable = true;
        string name = "";
        bool isLoop = true;
        public List<ParticleEmitter> paticleEmitters = new List<ParticleEmitter>();
        Vector2 postion=new Vector2();

        public ParticleSystem(string name,Canvas canvas)
        {
            this.name = name;
            this.canvas = canvas;
        }

        public void SetPosition(double x,double y)
        {
            postion.X = x;
            postion.Y = y;
        }

        public Vector2 GetPosition()
        {
            return postion;
        }

        public void AddEmitter(ParticleEmitter emitter)
        {
            paticleEmitters.Add(emitter);
        }

        public void Update(double thisTickTime)
        {
            if(!enable)
            {
                return;
            }
            foreach (var item in paticleEmitters)
            {
                item.Update(thisTickTime);
            }
        }

        public void Render()
        {
            if(!enable)
            {
                return;
            }
            foreach (var item in paticleEmitters)
            {
                item.Render();
            }
        }

        public void LoadContentFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                if(item.Name.ToString()=="ParticleEmitter")
                {
                    ParticleEmitter particleEmitter = new ParticleEmitter(this,null);
                    particleEmitter.LoadContentFromXML(item);
                    AddEmitter(particleEmitter);
                }
            }
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("ParticleSystem");
            ExportAttrbuteToXML(element);
            foreach (var item in paticleEmitters)
            {
                element.Add(item.ExportContentToXML());
            }
            return element;
        }

        void ExportAttrbuteToXML(XElement element)
        {
            
        }
    }
}
