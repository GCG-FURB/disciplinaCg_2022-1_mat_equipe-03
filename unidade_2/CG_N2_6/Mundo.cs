/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using CG_N2;

namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }


        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private char objetoId = '@';
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;

        private Ponto pontoSpline1;
        private Ponto pontoSpline2;
        private Ponto pontoSpline3;
        private Ponto pontoSpline4;
        private Ponto pontoSplineSelecionado;

        private Spline spline;

        public static Ponto4D pontoCentral = new Ponto4D(0, 0);



#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -300; camera.xmax = 300; camera.ymin = -300; camera.ymax = 300;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

           //inicio Spline;
            Ponto4D ponto1 = new Ponto4D(-200, -200);
            Ponto4D ponto2 = new Ponto4D(-200, 200);
            Ponto4D ponto3 = new Ponto4D(200, 200);
            Ponto4D ponto4 = new Ponto4D(200, -200);

            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline1 = new Ponto(objetoId, null, ponto1);
            objetosLista.Add(pontoSpline1);

            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline2 = new Ponto(objetoId, null, ponto2);
            objetosLista.Add(pontoSpline2);

            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline3 = new Ponto(objetoId, null, ponto3);
            objetosLista.Add(pontoSpline3);

            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline4 = new Ponto(objetoId, null, ponto4);
            objetosLista.Add(pontoSpline4);

            
           
            objetoId = Utilitario.charProximo(objetoId);
            SegReta segReta1 = new SegReta(objetoId, null, ponto1, ponto2);
            segReta1.PrimitivaTamanho = 5;
            
            segReta1.ObjetoCor.CorR = 224; segReta1.ObjetoCor.CorG = 255; segReta1.ObjetoCor.CorB = 255;
            objetosLista.Add(segReta1);
            
            objetoId = Utilitario.charProximo(objetoId);
            SegReta segReta2 = new SegReta(objetoId, null, ponto2, ponto3);
            segReta2.PrimitivaTamanho = 5;
            segReta2.ObjetoCor.CorR = 224; segReta2.ObjetoCor.CorG = 255; segReta2.ObjetoCor.CorB = 255;
            objetosLista.Add(segReta2);

            objetoId = Utilitario.charProximo(objetoId);
            SegReta segReta3 = new SegReta(objetoId, null, ponto3, ponto4);
            segReta3.PrimitivaTamanho = 5;
            segReta3.ObjetoCor.CorR = 224; segReta3.ObjetoCor.CorG = 255; segReta3.ObjetoCor.CorB = 255;
            objetosLista.Add(segReta3);
            
            objetoId = Utilitario.charProximo(objetoId);
            spline = new Spline(objetoId, null, pontoSpline1, pontoSpline2, pontoSpline3, pontoSpline4, 10);
            spline.PrimitivaTamanho = 5;
            spline.ObjetoCor.CorR = 255; spline.ObjetoCor.CorG = 255; spline.ObjetoCor.CorB = 0;
            objetosLista.Add(spline);
            //fim spline




#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
             

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            // spline = new Spline(objetoId, null, pontoSpline1, pontoSpline2, pontoSpline3, pontoSpline4, 10);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }
        

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();

            else if (e.Key == Key.E)
            {
                if (pontoSplineSelecionado != null)
                {
                    pontoSplineSelecionado.ponto.X--;
                    return;
                }
            }
            else if (e.Key == Key.D)
            {
                if (pontoSplineSelecionado != null)
                {
                    pontoSplineSelecionado.ponto.X++;
                    return;
                }
            }
            else if (e.Key == Key.C)
            {
                if (pontoSplineSelecionado != null)
                {
                    pontoSplineSelecionado.ponto.Y++;
                    return;
                }
            }
            else if (e.Key == Key.B)
            {
                if (pontoSplineSelecionado != null)
                {
                    pontoSplineSelecionado.ponto.Y--;
                    return;
                }
            }
            else if (e.Key == Key.KeypadPlus && spline != null)
                spline.qtdPontos++;
            else if (e.Key == Key.KeypadSubtract && spline != null)
            {
                if (spline.qtdPontos > 1)
                    spline.qtdPontos--;
            }
            else if (e.Key == Key.R && spline != null)
                spline.qtdPontos = 10;
            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (int i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            else if (e.Key == Key.Number1 || e.Key == Key.Keypad1){
                
                pontoSpline1.cor = Color.Red;
                pontoSpline2.cor = Color.Black;
                pontoSpline3.cor = Color.Black;
                pontoSpline4.cor = Color.Black;
                pontoSplineSelecionado = pontoSpline1;

                }
            else if (e.Key == Key.Number2 || e.Key == Key.Keypad2){
                pontoSpline1.cor = Color.Black;
                pontoSpline2.cor = Color.Red;
                pontoSpline3.cor = Color.Black;
                pontoSpline4.cor = Color.Black;
                pontoSplineSelecionado = pontoSpline2;
                }
            else if (e.Key == Key.Number3 || e.Key == Key.Keypad3){
                pontoSpline1.cor = Color.Black;
                pontoSpline2.cor = Color.Black;
                pontoSpline3.cor = Color.Red;
                pontoSpline4.cor = Color.Black;
                pontoSplineSelecionado = pontoSpline3;
                }
            else if (e.Key == Key.Number4 || e.Key == Key.Keypad4){
                pontoSpline1.cor = Color.Black;
                pontoSpline2.cor = Color.Black;
                pontoSpline3.cor = Color.Black;
                pontoSpline4.cor = Color.Red;
                pontoSplineSelecionado = pontoSpline4;
            }
            else if (e.Key == Key.Plus)
            {
                if (spline.qtdPontos <= 100)
                    spline.qtdPontos++;
            }
            else if (e.Key == Key.Minus)
            {
                if (spline.qtdPontos > 0)
                {
                    var aux = spline.qtdPontos - 1;
                    if (aux > 0)
                    {
                        spline.qtdPontos = aux;
                    }
                }

            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }



#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
    }
}