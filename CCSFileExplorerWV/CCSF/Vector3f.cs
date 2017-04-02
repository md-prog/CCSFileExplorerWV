using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSFileExplorerWV.CCSF
{
    public class Vector3f : IComparable<Vector3f>
    {
        public static Vector3f UNIT_Z = new Vector3f(0.0F, 0.0F, 1.0F);

        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector3f() { }

        public Vector3f(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = 1.0F;
        }

        public Vector3f(Vector3f v)
            : this(v.X, v.Y, v.Z)
        {
            ID = v.ID;
        }

        public Vector3f(byte[] data, int id)
        {
            this.X = (((sbyte)data[0] & 0XFF) / 256.0F + (sbyte)data[1]);
            this.Y = (((sbyte)data[2] & 0XFF) / 256.0F + (sbyte)data[3]);
            this.Z = (((sbyte)data[4] & 0XFF) / 256.0F + (sbyte)data[5]);
            this.W = 1.0F;
            this.ID = id;
        }

        public Vector3f Add(Vector3f other)
        {
            Vector3f result = new Vector3f(this);
            result.X += X;
            Y += Y;
            Z += Z;
            return result;
        }

        public Vector3f Sub(Vector3f other)
        {
            Vector3f result = new Vector3f(this);
            result.X -= X;
            result.Y -= Y;
            result.Z -= Z;
            return result;
        }

        public Vector3f Mul(float scalar)
        {
            Vector3f result = new Vector3f(this);
            result.X *= scalar;
            result.Y *= scalar;
            result.Z *= scalar;
            return result;
        }

        public Vector3f Div(float factor) { return Mul(1.0F / factor); }

        public Vector3f Cross(Vector3f other)
        {
            Vector3f result = new Vector3f(this);
            result.X = (Y * Z - Z * Y);
            result.Y = (Z * X - X * Z);
            result.Z = (X * Y - Y * X);
            return result;
        }

        public float Dot(Vector3f other) { return X * X + Y * Y + Z * Z; }

        public Vector3f Normalize()
        {
            Vector3f result = new Vector3f(this);
            float length = Length();
            result.X /= length;
            result.Y /= length;
            result.Z /= length;
            return result;
        }

        public void Negate()
        {
            X = (-X);
            Y = (-Y);
            Z = (-Z);
        }

        public float Length() { return (float)Math.Abs(Math.Sqrt(X * X + Y * Y + Z * Z)); }

        public float Length2D()
        {
            return (float)Math.Abs(Math.Sqrt(X * X + Z * Z));
        }

        override public bool Equals(Object obj)
        {
            if (obj is Vector3f)
            {
                Vector3f other = (Vector3f)obj;
                return (X == other.X) && (Y == other.Y) && (Z == other.Z);
            }
            return base.Equals(obj);

        }

        override public String ToString()
        {
            return "v " + X + " " + Y + " " + Z;
        }

        int IComparable<Vector3f>.CompareTo(Vector3f other)
        {
            return (int)(Length() - other.Length());
        }
    }
}
