using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Game.Particle
{
    enum ParticleType
    {
        Point,
        Sequence
    }

    class ParticleEmitter
    {
        ParticleType particleType = ParticleType.Point;
        string particleImagePath = "";
        int particleNumber = 10;
        Color particleColor = Color.White;
        double radius = 3;
        double width = 1;
        double height = 1;
    }
}
