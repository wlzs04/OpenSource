using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LLGameStudio.Game.Particle
{
    class ParticleSystem
    {
        Canvas canvas;
        bool enable = true;
        string name = "";
        bool isLoop = true;
        List<ParticleEmitter> paticleEmitters = new List<ParticleEmitter>();

        public ParticleSystem(string name,Canvas canvas)
        {
            this.name = name;
            this.canvas = canvas;
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
