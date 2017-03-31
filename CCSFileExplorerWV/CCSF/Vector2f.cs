using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSFileExplorerWV.CCSF
{
    public class Vector2f : IComparable<Vector2f>
    {
        public int ID { get; set; }
        public float U { get; set; }
        public float V { get; set; }

        public Vector2f()
        {

        }

        public Vector2f(float u, float v)
        {
            this.U = u;
            this.V = v;
        }

        public Vector2f(Vector2f v)
            :this(v.U, v.V)
        {
            ID = v.ID;
        }


        public Vector2f(byte[] data, int id)
        {
            U = (((sbyte)data[0] & 0xFF) / 256.0F + (sbyte)data[1]);
            V = (((sbyte)data[2] & 0xFF) / 256.0F + (sbyte)data[3]);

            this.ID = id;
        }

        public Vector2f Add(Vector2f other)
        {
            Vector2f result = new Vector2f(this);
            result.U += other.U;
            result.V += other.V;
            return result;
        }

        public Vector2f Sub(Vector2f other)
        {
            Vector2f result = new Vector2f();
            result.U -= other.U;
            result.V -= other.V;
            return result;
        }

        public Vector2f Mul(float scalar)
        {
            Vector2f result = new Vector2f();
            U *= scalar;
            V *= scalar;
            return result;
        }

        public Vector2f Div(float factor) { return Mul(1.0F / factor); }

        public float Dot(Vector2f other) { return U * U + V * V; }

        public Vector2f Normalize()
        {
            Vector2f result = new Vector2f(this);
            float length = this.Length();
            result.U /= length;
            result.V /= length;
            return result;
        }

        public void Negate()
        {
            U = (-U);
            V = (-V);
        }

        public float Length() { return (float)Math.Abs(Math.Sqrt(U * U + V * V)); }

        override public bool Equals(Object obj)
        {
            if (obj is Vector2f)
            {
                Vector2f other = (Vector2f)obj;
                return (this.U == other.U) && (this.U == other.V);
            }
            return base.Equals(obj);
        }

        override public String ToString() {
            return "vt " + U + " " + V;
        }

        int IComparable<Vector2f>.CompareTo(Vector2f other)
        {
            return (int)(Length() - other.Length());
        }
    }
}
