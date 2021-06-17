using System;
using System.Collections.Generic;
using System.DoubleNumerics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class QuadAreaLight : PotatoLight
    {
        public QuadAreaLight(Vector3 position, float intensity, Color color, LightType type) : base(position, intensity, color, type)
        {
        }

        Vector3 Normal = PotatoRaytracing.WorldCoordinate.PotatoCoordinate.VECTOR_LEFT;
        public Vector3 GetRandomPoint()
        {
            return 
        }

        public override Vector3 DirectionToLight(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public override float IntensityOverDistance(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public override bool IsInRange(Vector3 position)
        {
            throw new NotImplementedException();
        }
    }
}
