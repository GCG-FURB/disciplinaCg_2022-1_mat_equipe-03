using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using gcgcg;

namespace CG_N2
{
    internal class Spline : ObjetoGeometria
    {
        public double qtdPontos { get; set; }

        public Spline(char rotulo, Objeto paiRef, Ponto ponto1, Ponto ponto2, Ponto ponto3, Ponto ponto4, int qtdPontos) : base(rotulo, paiRef)
        {

            Ponto4D pA = ponto1.GetPonto4D();
            base.PontosAdicionar(pA);

            Ponto4D pB = ponto2.GetPonto4D();
            base.PontosAdicionar(pB);

            Ponto4D pC = ponto3.GetPonto4D();
            base.PontosAdicionar(pC);

            Ponto4D pD = ponto4.GetPonto4D();
            base.PontosAdicionar(pD);

            primitiva = PrimitiveType.LineStrip;
            this.qtdPontos = qtdPontos;
        }


        private Ponto4D calcularSpline(Ponto4D pontoA, Ponto4D pontoB, double qtd)
        {
            double pX = pontoA.X + (pontoB.X - pontoA.X) * qtd;
            double pY = pontoA.Y + (pontoB.Y - pontoA.Y) * qtd;

            return new Ponto4D(pX, pY);
        }


        protected override void DesenharObjeto()
        {
         /*   Ponto4D pA = pontosLista[0];
            Ponto4D pB = pontosLista[1];
            Ponto4D pC = pontosLista[2];
            Ponto4D pD = pontosLista[3]; */

            GL.Begin(primitiva);

            GL.Vertex2(pontosLista[0].X, pontosLista[0].Y);
            for (int i = 0; i < qtdPontos; i++)
            {
                // double qtd = i / qtdPontos;
                Ponto4D pontoAb = calcularSpline(pontosLista[0], pontosLista[1], i / qtdPontos);
                Ponto4D pontoBc = calcularSpline(pontosLista[1], pontosLista[2], i / qtdPontos);
                Ponto4D pontoCd = calcularSpline(pontosLista[2], pontosLista[3], i / qtdPontos);
                Ponto4D pontoAbc = calcularSpline(pontoAb, pontoBc, i / qtdPontos);
                Ponto4D pontoBcd = calcularSpline(pontoBc, pontoCd, i / qtdPontos);
                Ponto4D pontoAbcd = calcularSpline(pontoAbc, pontoBcd, i / qtdPontos);

                GL.Vertex2(pontoAbcd.X, pontoAbcd.Y);
            }
            GL.Vertex2(pontosLista[3].X, pontosLista[3].Y);

            GL.End();
        }      
    }
}