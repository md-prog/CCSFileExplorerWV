﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCSFileExplorerWV.CCSF;

namespace CCSFileExplorerWV
{

    /**
     * MDL Chunk
     * 
     * .Hack/G.U. Version
     * 
     * 0x00 DWORD 0xCCCC0800
     * 0x04 DWORD SIZE IN DWORD, SO IN BYTES IT WILL BE * 4
     * 0x08 DWORD UNKNOWN1 : maybe mesh id
     * 0x0C DWORD UNKNOWN2
     * 0x10 DWORD UNKNOWN3
     * 0x14 DWORD UNKNOWN4
     * 0x18 DWORD UNKNOWN5 // 0x00000070 always
     *
     * now, if unknown2 and unknown3 are zero, thats all, otherwise next:
     * 
     * 0x1C DWORD UNKNOWN6 // maybe model type from xentax post?
     * 0x20 DWORD UNKNOWN7
     * 0x24 DWORD VERTEX_COUNT //important!
     * // VERTEXT DATA - VERTEX_COUNT
     * // CONNECTION DATA - VERTEX_COUNT
     * // UNKNOWN DATA - VERTEX_COUNT
     * // UV Buffer - VERTEX_COUNT
     *
     * 
     * 
     * IMOQF Version
     * 0x00 DWORD 0xCCCC0800
     * 0x04 DWORD SIZE IN DWORD, SO IN BYTES IT WILL BE * 4
     * 0x08 DWORD UNKNOWN1 : maybe mesh id
     * 
     */
    public class Block0800 : Block
    {
        public List<ModelData> models;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;


        public Block0800(uint _type, uint _id, byte[] _data)
        {
            BlockID = _type;
            ID = _id;
            Data = _data;
        }

        public Block0800(Stream s)
        {
            Size = Block.ReadUInt32(s); // 0x04
            ID = Block.ReadUInt32(s); // 0x08

            uint size = Size - 1; 
            MemoryStream m = new MemoryStream();
            uint u = 0, l = 0;
            while (!isValidBlockType(u = ReadUInt32(s)) && l++<size)
            {
                m.Write(BitConverter.GetBytes(u), 0, 4);
            }
            if(isValidBlockType(u))
                s.Seek(-4, SeekOrigin.Current);

            Data = m.ToArray();
        }

        public override TreeNode ToNode()
        {
            return new TreeNode(BlockID.ToString("X8") + " Size(bytes): 0x" + Data.Length.ToString("X"));
        }

        public override void WriteBlock(Stream s)
        {
            WriteUInt32(s, BlockID);
            WriteUInt32(s, (uint)(Data.Length / 4 + 1));
            WriteUInt32(s, ID);
            s.Write(Data, 0, Data.Length);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Found " + models.Count + " models:");
            int count = 1;
            foreach (ModelData mdl in models)
                sb.AppendLine(" Model " + (count++) + " has " + mdl.VertexCount + " vertices");
            return sb.ToString();
        }

        public void ProcessData()
        {
            models = new List<ModelData>();
            MemoryStream m = new MemoryStream();
            Unknown2 = BitConverter.ToUInt32(Data, 0); // 0x0C
            Unknown3 = BitConverter.ToUInt32(Data, 4); // 0x10
            Unknown4 = BitConverter.ToUInt32(Data, 8); // 0x14
            Unknown5 = BitConverter.ToUInt32(Data, 12); // 0x18

            if (Unknown2 != 0 && Unknown3 != 0)
            {
                if(this.CCSFile.FileVersion == CCSFile.FileVersionEnum.HACK_GU) {
                    m.Write(Data, 24, Data.Length - 24);
                }
                else if(this.CCSFile.FileVersion == CCSFile.FileVersionEnum.IMOQF)
                {
                    m.Write(Data, 16, Data.Length - 16);
                }
                m.Seek(0, 0);
                while (m.Position < m.Length)
                    models.Add(new ModelData(m));
            }
        }

        public void CopyToScene(int index)
        {
            SceneHelper.InitScene(ModelToFloats(models[index]));
        }

        public string SaveModel(int n)
        {
            List<ModelData> mdls;
            if (n < 0 || n >= models.Count)
            {
                mdls = models;
            }
            else
            {
                mdls = new ModelData[] { models[n] }.ToList();
            }

            StringBuilder sb = new StringBuilder();
            foreach (ModelData mdl in mdls)
            {
                if (CCSFile.FileVersion == CCSFile.FileVersionEnum.IMOQF)
                {
                    List<float[]> trilist = ModelToFloats(mdl);
                    sb.Append("g Object\r\n");
                    sb.AppendFormat("\r\n# {0} vertices\r\n\r\n", trilist.Count);
                    foreach (float[] v in trilist)
                        sb.AppendFormat("v {0} {1} {2}\r\n", v[0], v[1], v[2]);

                    sb.AppendFormat("\r\n# {0} texture UVs\r\n\r\n", mdl.UVs.Count);
                    foreach (var v in mdl.UVs)
                        sb.AppendFormat("{0}\r\n", v.ToString());

                    sb.AppendFormat("\r\n# {0} faces\r\n\r\n", trilist.Count);
                    for (int i = 0; i < trilist.Count; i += 3)
                        sb.AppendFormat("f {0} {1} {2}\r\n", i + 1, i + 2, i + 3);
                }
                else if (CCSFile.FileVersion == CCSFile.FileVersionEnum.HACK_GU)
                {
                    sb.Append ("g Object\r\n");
                    sb.AppendFormat("\r\n# {0} vertices\r\n\r\n", mdl.Vertex.Count);
                    foreach (var v in mdl.Vertex)
                        sb.AppendFormat("{0}\r\n", v.ToString());

                    sb.AppendFormat("\r\n# {0} texture UVs\r\n\r\n", mdl.UVs.Count);
                    foreach (var v in mdl.UVs)
                        sb.AppendFormat("{0}\r\n", v.ToString());

                    sb.AppendFormat("\r\n# {0} faces\r\n\r\n", mdl.Tristrips.Count);
                    foreach (var v in mdl.Tristrips)
                        sb.AppendFormat("{0}\r\n", v.ToString());
                }
            }
            return sb.ToString();
        }

        public void SaveModel(int n, string filename)
        {
            File.WriteAllText(filename, SaveModel(n));
        }

        public List<float[]> ModelToFloats(ModelData mdl)
        {
            List<float[]> result = new List<float[]>();
            List<int> strip = new List<int>();
            int pos = 0;
            bool isStart = true;
            while (pos < mdl.TristripsBytes.Count - 1)
            {
                if (isStart)
                {
                    strip.Add(pos);
                    strip.Add(pos + 1);
                    isStart = false;
                    pos += 2;
                }
                else
                {
                    byte b = (byte)(mdl.TristripsBytes[pos] >> 24);
                    if (b == 1)
                    {
                        isStart = true;
                        result.AddRange(stripToList(strip, mdl.VertexBytes));
                        strip = new List<int>();
                    }
                    else
                        strip.Add(pos++);
                }
            }
            if(strip.Count != 0)
                result.AddRange(stripToList(strip, mdl.VertexBytes));
            return result;
        }

        public List<float[]> stripToList(List<int> strip, List<byte[]> vertices)
        {
            List<float[]> result = new List<float[]>(); 
            int i2, i3;
            for (int i = 0; i < strip.Count - 2; i++)
            {
                if (i % 2 == 0)
                {
                    i2 = i + 1;
                    i3 = i + 2;
                }
                else
                {
                    i2 = i + 2;
                    i3 = i + 1;
                }
                float[] v = new float[5];
                for (int j = 0; j < 3; j++)
                    v[j] = BitConverter.ToInt16(vertices[strip[i]], j * 2);
                result.Add(v);
                v = new float[5];
                for (int j = 0; j < 3; j++)
                    v[j] = BitConverter.ToInt16(vertices[strip[i2]], j * 2);
                result.Add(v);
                v = new float[5];
                for (int j = 0; j < 3; j++)
                    v[j] = BitConverter.ToInt16(vertices[strip[i3]], j * 2);
                result.Add(v);
            }
            return result;
        }

        public class ModelData
        {
            public uint Unk1;
            public uint Unk2;
            public uint VertexCount;
            public List<byte[]> VertexBytes;
            public List<uint> TristripsBytes;
            public List<uint> UnknownData;
            public List<byte[]> UVBytes;

            public List<Vector3f> Vertex;
            public List<Vector2f> UVs;
            public List<Tristrip> Tristrips;


            public ModelData(Stream s)
            {
                Unk1 = ReadUInt32(s); // 0x1c
                Unk2 = ReadUInt32(s); // 0x20
                VertexCount = ReadUInt32(s); //0x24
                VertexBytes = new List<byte[]>();
                Vertex = new List<Vector3f>();
                byte[] buff;
                long len = s.Length;

                // read vertex
                for (int i = 0; i < VertexCount; i++)
                {
                    buff = new byte[6];
                    s.Read(buff, 0, 6);
                    VertexBytes.Add(buff);
                    Vertex.Add(new Vector3f(buff, i + 1));

                    if (s.Position >= len)
                    {
                        VertexCount = 0;
                        VertexBytes = new List<byte[]>();
                        TristripsBytes = new List<uint>();
                        UnknownData = new List<uint>();
                        UVBytes = new List<byte[]>();

                        Vertex = new List<Vector3f>();
                        UVs = new List<Vector2f>();
                        Tristrips = new List<Tristrip>();
                        return;
                    }
                }
                if (6 * VertexCount % 4 > 0) // padding after vertex data to match the dword addresses
                    s.Seek(6 * VertexCount % 4, SeekOrigin.Current);

                // read faces
                TristripsBytes = new List<uint>();
                Tristrips = new List<Tristrip>();
                List<VertexConnection> ft = new List<VertexConnection>();
                for (int i = 0; i < VertexCount; i++) {
                    uint v = ReadUInt32(s);
                    TristripsBytes.Add(v);
                    ft.Add(new VertexConnection(v));
                }

                bool started = false;
                for (int i = 0, size = 0; i < ft.Count; i++) {
                    if ((started) && (ft[i].Connect == 0)) {
                        size++;
                    } else if ((started) && (ft[i].Connect != 0)) {
                        started = false;
        
                        resolveTristrip(i - size, size, ft[(i - size)].Connect);
                        size = 0;
                    }
                    if ((ft[i].Connect != 0) && (!started)) {
                        started = true;
                        size += 2;
                        i++;
                    }
                    if (i == ft.Count- 1) {
                        resolveTristrip(i - size + 1, size, ft[(i - size + 1)].Connect);
                    }
                }

                // read unknown data block (size of VertextCount)
                UnknownData = new List<uint>();
                for (int i = 0; i < VertexCount; i++)
                    UnknownData.Add(ReadUInt32(s));

                // read texture block
                UVBytes = new List<byte[]>();
                UVs = new List<Vector2f>();
                for (int i = 0; i < VertexCount; i++)
                {
                    buff = new byte[4];
                    s.Read(buff, 0, 4);
                    UVBytes.Add(buff);
                    UVs.Add(new Vector2f(buff, i + 1));
                }
            }

            private void resolveTristrip(int start, int size, int type)
            {
                for (int i = 0; i<size - 2; i++) {
                    switch (type) {
                    case 1: 
                        if (i % 2 == 1) {
                            Tristrips.Add(new Tristrip(Vertex[(start + i + 1)], Vertex[(start + i)], Vertex[(start + i + 2)]));
                        } else {
                            Tristrips.Add(new Tristrip(Vertex[(start + i)], Vertex[(start + i + 1)], Vertex[(start + i + 2)]));
                        }
                    break;
                    case 2: 
                        if (i % 2 == 1) {
                            Tristrips.Add(new Tristrip(Vertex[(start + i)], Vertex[(start + i + 1)], Vertex[(start + i + 2)]));
                        } else {
                            Tristrips.Add(new Tristrip(Vertex[(start + i + 1)], Vertex[(start + i)], Vertex[(start + i + 2)]));
                        }
                    break;
                    default:
                            return;
                    }
                }
            }
        }
    }
}
