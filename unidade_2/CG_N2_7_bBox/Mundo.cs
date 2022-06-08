/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo

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
        private bool mouseMoverPto = false;
        private Retangulo obj_Retangulo;
        private int indexListaTipos = 0;

    
        public static Ponto4D ponto0 = new Ponto4D(0, 0);
        public static Ponto4D direitoCima = Matematica.GerarPtosCirculo(45, 300);    
        public static Ponto4D direitoBaixo = Matematica.GerarPtosCirculo(315, 300);
        public static Ponto4D esquerdoCima = Matematica.GerarPtosCirculo(135, 300);
        public static Ponto4D esquerdoBaixo = Matematica.GerarPtosCirculo(225, 300);
        public static Cor corRetangulo = new Cor(252, 15, 192);
        public static bool aux = false;

        private List<PrimitiveType> listaTipos = new List<PrimitiveType>()
        {
            PrimitiveType.Points,
            PrimitiveType.Lines,
            PrimitiveType.LineLoop,
            PrimitiveType.LineStrip,
            PrimitiveType.Triangles,
            PrimitiveType.TriangleStrip,
            PrimitiveType.TriangleFan,
            PrimitiveType.Quads,
            PrimitiveType.QuadStrip,
            PrimitiveType.Polygon
        };
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

            camera.xmax = 600;
            camera.ymax = 600;
            camera.xmin = -600;
            camera.ymin = -600;

            //  Não usar o mesmo ponto central

            Retangulo retanguloMenor = new Retangulo(Convert.ToChar("M"), null, esquerdoBaixo, direitoCima);
            retanguloMenor.ObjetoCor = corRetangulo;
            objetosLista.Add(retanguloMenor);

            Ponto pontoCentral = new Ponto(Convert.ToChar("B"), null, Mundo.ponto0);
            pontoCentral.PrimitivaTamanho = 5;
            pontoCentral.ObjetoCor = new Cor(1, 1, 1);
            objetosLista.Add(pontoCentral);

            Circulo circulo = new Circulo(Convert.ToChar("C"), null, new Ponto4D(0, 0), 300, 720 , PrimitiveType.LineLoop);
            circulo.ObjetoCor.CorR = Color.Black.R; circulo.ObjetoCor.CorG = Color.Black.G; circulo.ObjetoCor.CorB = Color.Black.B;
            circulo.PrimitivaTamanho = 2;
            objetosLista.Add(circulo); 

            Circulo circulo2 = new Circulo(Convert.ToChar("C"), null, Mundo.ponto0, 50, 72, PrimitiveType.LineLoop);
            circulo2.ObjetoCor.CorR = Color.Black.R; circulo2.ObjetoCor.CorG = Color.Black.G; circulo2.ObjetoCor.CorB = Color.Black.B;
            circulo2.PrimitivaTamanho = 2;
            objetosLista.Add(circulo2);


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
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

      
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (!e.Mouse.IsButtonDown(MouseButton.Left) || aux == false)
            {
                Mundo.ponto0.X = 0;
                Mundo.ponto0.Y = 0;

               return;
            }

            int x = (e.X /*- 300) * 2*/);  
            int y = (e.Y /* - 300*/) * -1; 

            if (Mundo.direitoBaixo.X > x && Mundo.direitoBaixo.Y < y && Mundo.esquerdoCima.X < x && Mundo.esquerdoCima.Y > y)            {
                
                Mundo.ponto0.X = x;
                Mundo.ponto0.Y = y;

                corRetangulo.CorR = 252;
                corRetangulo.CorG = 15;
                corRetangulo.CorB = 192;
            }
            else
            {
                corRetangulo.CorR = 25;
                corRetangulo.CorG = 140;
                corRetangulo.CorB = 255;

              if ((x * x) + (y * y) < 300 * 300)
                {
                    Mundo.ponto0.X = x;
                    Mundo.ponto0.Y = y;
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Mundo.aux = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.IsPressed)
            {
                int x = (e.X /*- 300) * 2*/);
                int y = (e.Y /* - 300*/) * -1; // Talvez mudar pra -2

                Mundo.aux = false;

                if (x > (ponto0.X - 50) && x < (ponto0.X + 50) && y < (ponto0.Y + 50) && y > (ponto0.Y - 50))
                {
                    Mundo.aux = true;
                }
            }
            base.OnMouseDown(e);
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