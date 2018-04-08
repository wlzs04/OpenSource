using LLGameStudio.Common.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LLGameStudio.Game.Particle
{
    public class ParticleSystem
    {
        Canvas canvas;
        bool enable = true;
        string name = "";
        bool isLoop = true;
        List<ParticleEmitter> paticleEmitters = new List<ParticleEmitter>();
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

        public void Update()
        {
            if(!enable)
            {
                return;
            }
            foreach (var item in paticleEmitters)
            {
                item.Update();
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
    }
}
