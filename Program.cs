using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace indexedObjFileToTrisArray
{
    public static class Extensions
    {
        public static String ToFixedString(this float value)
        {
            if (value < 0)
                return String.Format(" {0:F6}", value);
            else
                return String.Format("  {0:F6}", value);
        }
    }


    class Program
    {
        struct Position
        {
            public float X;
            public float Y;
            public float Z;
            public Position( float x, float y, float z )
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        struct UV
        {
            public float U;
            public float V;
            public UV( float u, float v )
            {
                U = u;
                V = v;
            }
        }
        
        struct Triangle
        {
            public int Pos0;
            public int UV0;

            public int Pos1;
            public int UV1;

            public int Pos2;
            public int UV2;

            public Triangle(int pos0, int uv0, int pos1, int uv1, int pos2, int uv2)
            {
                Pos0 = pos0;
                UV0 = uv0;
                Pos1 = pos1;
                UV1 = uv1;
                Pos2 = pos2;
                UV2 = uv2;
            }
        }

        static void Main(string[] args)
        {
            string line;

            List<Position> positions = new List<Position>();
            List<UV> uvs = new List<UV>();
            List<Triangle> triangles = new List<Triangle>();

            System.IO.StreamReader objFile = new System.IO.StreamReader(@"sphere.obj");
            while ( (line = objFile.ReadLine()) != null )
            {
                String[] parts = line.Split(new char[]{' ', '/'});

                if (parts[0] == @"v")
                {
                    positions.Add(new Position(
                        float.Parse(parts[1]),
                        float.Parse(parts[2]),
                        float.Parse(parts[3])));
                }
                else if(parts[0]== @"vt")
                {
                    uvs.Add(new UV(
                        float.Parse(parts[1]),
                        float.Parse(parts[2])));
                }
                else if (parts[0] == @"f")
                {
                    triangles.Add(new Triangle(
                        int.Parse(parts[1]),
                        int.Parse(parts[2]),
                        int.Parse(parts[3]),
                        int.Parse(parts[4]),
                        int.Parse(parts[5]),
                        int.Parse(parts[6])));
                }                
            }
            objFile.Close();






            // Example #3: Write only some strings in an array to a file. 
            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            using (System.IO.StreamWriter cppFile = new System.IO.StreamWriter(@"vertexData.cpp"))
            {
                cppFile.WriteLine("float vertexData[] =                      ");
                cppFile.WriteLine("{    //    POSITION             TEXCOORD  ");

                foreach (Triangle triangle in triangles)
                {
                    string line0 = "    "
                        + positions.ElementAt(triangle.Pos0-1).X.ToFixedString() + ", "
                        + positions.ElementAt(triangle.Pos0-1).Y.ToFixedString() + ", "
                        + positions.ElementAt(triangle.Pos0-1).Z.ToFixedString() + ", "
                        + uvs.ElementAt(triangle.UV0-1).U.ToFixedString() + ", "
                        + uvs.ElementAt(triangle.UV0-1).V.ToFixedString() + ", ";
                    cppFile.WriteLine(line0);

                    string line1 = "    "
                        + positions.ElementAt(triangle.Pos1-1).X.ToFixedString() + ", "
                        + positions.ElementAt(triangle.Pos1-1).Y.ToFixedString() + ", "
                        + positions.ElementAt(triangle.Pos1-1).Z.ToFixedString() + ", "
                        + uvs.ElementAt(triangle.UV1-1).U.ToFixedString() + ", "
                        + uvs.ElementAt(triangle.UV1-1).V.ToFixedString() + ", ";
                    cppFile.WriteLine(line1);

                    string line2 = "    "
                        + positions.ElementAt(triangle.Pos2-1).X.ToFixedString() + ", "
                        + positions.ElementAt(triangle.Pos2-1).Y.ToFixedString() + ", "
                        + positions.ElementAt(triangle.Pos2-1).Z.ToFixedString() + ", "
                        + uvs.ElementAt(triangle.UV2-1).U.ToFixedString() + ", "
                        + uvs.ElementAt(triangle.UV2-1).V.ToFixedString() + ", ";
                    cppFile.WriteLine(line2);
                }

                cppFile.WriteLine("};                                        ");
            }


        }
    }
}
