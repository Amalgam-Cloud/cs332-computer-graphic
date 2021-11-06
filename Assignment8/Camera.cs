using System;

namespace Assignment8
{
    class Camera
    {
        public Dot pos { get; set; }
        public double phi { get; set; }
        public double theta { get; set; }
        public double[,] Projection { get; set; }
        public Dot Forward()
        { 
             return new Dot(-Math.Sin(phi), Math.Sin(theta), -Math.Cos(phi));
        }
        public Dot Left ()
        { 
                return new Dot(-Math.Sin(phi + Math.PI / 2), 0, -Math.Cos(phi + Math.PI / 2));
 }
        public Dot Up() 
        { 
            return Dot.VectorMult(Forward(), Left());  
        }
        public Dot Right() 
        { 
            return -1 *Left(); 
        }
        public Dot Back() 
        {  return -1 * Forward();  
        }
        public Dot Down()
        {
            return -1 * Up();
        }

        public double[,] TrueProjection()
        {
                return Transformer.MatrMult(Transformer.MatrMult(Transformer.MatrMult(Transformer.Translate(-1 * pos), Transformer.RotateY(-phi)),Transformer.RotateX(-theta))
                    ,Projection);
        }

        public Camera(Dot position, double phi, double theta, double[,] projection)
        {
            pos = position;
            phi = phi;
            theta = theta;
            Projection = projection;
        }
    }
}
