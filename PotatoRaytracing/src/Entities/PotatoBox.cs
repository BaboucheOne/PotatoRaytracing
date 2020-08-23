using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoBox : PotatoEntity
    {
        public Vector3 Max = new Vector3();
        public Vector3 Min = new Vector3();

        public PotatoBox()
        {
            SetMinAndMaxBounds();
        }

        public PotatoBox(Vector3 position) : base(position)
        {
            SetMinAndMaxBounds();
        }

        public PotatoBox(Vector3 position, Quaternion rotation) : base(position, rotation)
        {
            SetMinAndMaxBounds();
        }

        public PotatoBox(Vector3 position, Quaternion rotation, Vector3 scale) : base(position, rotation, scale)
        {
            SetMinAndMaxBounds();
        }

        public PotatoBox(Vector3 position, Vector3 min, Vector3 max)
        {
            Position = position;
            Min = min;
            Max = max;
        }

        private void SetMinAndMaxBounds()
        { 
            Max = Position + Scale;
            Min = Position - Scale;
        }
    } //TODO: Definir les limites de la boite pour un mesh.
}
