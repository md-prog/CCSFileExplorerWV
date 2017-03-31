using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSFileExplorerWV.CCSF
{
    public class Tristrip
    {
        public Vector3f V1 { get; set; }
        public Vector3f V2 { get; set; }
        public Vector3f V3 { get; set; }
        private Vector3f Centroid {
            get {
                return V1.Add(V2).Add(V3).Div(3.0F);
            }
        }
        private Vector3f Normal {
            get {
                return V2.Sub(V1).Cross(V3.Sub(V1)).Normalize();
            }
        }

        public Tristrip(Tristrip other)
            : this(other.V1, other.V2, other.V3)
        {
        }

        public Tristrip(Vector3f V1, Vector3f V2, Vector3f V3)
        {
            this.V1 = new Vector3f(V1);
            this.V2 = new Vector3f(V2);
            this.V3 = new Vector3f(V3);
        }

        public bool Contains(Vector3f v) { return (v.Equals(V1)) || (v.Equals(V2)) || (v.Equals(V3)); }

        override public String ToString()
        {
            return "f " + V1.ID + "/" + V1.ID + " " + V2.ID + "/" + V2.ID + " " + V3.ID + "/" + V3.ID;
        }

        public bool Equals(Tristrip other) { return Normal.Equals(other.Normal); }
    }
}
