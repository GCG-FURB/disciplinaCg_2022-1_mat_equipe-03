using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria {

        double raio;
        Ponto4D ponto, centro;

        int numeroPontos;



        public Circulo(char rotulo, Objeto paiRef, Ponto4D ptoCentro, double radius, int numeroPontos) : base(rotulo, paiRef)
        {
            base.PontosAdicionar(ptoCentro);
            if(numeroPontos > 360){
                numeroPontos = 360;
            }
            
            this.numeroPontos = numeroPontos;
            this.centro = ptoCentro;
            this.raio = radius;
        }

        protected override void DesenharObjeto()
        {

             int grau = 360/numeroPontos;

            // GL.Enable(EnableCap.Blend);
            // GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(base.PrimitivaTipo);

            for (int i = 0; i < 360; i += grau)
            {
                ponto = Matematica.GerarPtosCirculo(i, this.raio);
                GL.Vertex2(ponto.X + centro.X, ponto.Y + centro.Y);
                
            }
            GL.End();
        }
    }
}