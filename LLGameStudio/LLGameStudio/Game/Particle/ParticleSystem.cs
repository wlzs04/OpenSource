using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Game.Particle
{
    class ParticleSystem
    {
        bool enable = true;
        string name = "";
        List<ParticleEmitter> paticleEmitters = new List<ParticleEmitter>();

        public void AddEmitter(ParticleEmitter emitter)
        {
            paticleEmitters.Add(emitter);
        }

    }
}
