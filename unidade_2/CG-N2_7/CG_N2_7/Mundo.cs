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
    private SegReta obj_Retangulo;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif



    private Circulo circulo_obj;


  private Circulo circulo_obj2;
  private Circulo circulo_obj3;

  private SegReta obj_SegReta;

    private SegReta obj_SegReta2;
  private SegReta obj_SegReta3;
  private SegReta obj_SegReta4;




    Ponto4D ponto0;    
    Ponto4D ponto1;    
    Ponto4D ponto2;    
    Ponto4D ponto3;    
    Ponto4D ponto4;    
    Ponto4D pontoCentro;   


    BBox boudaring;
 
    
    
    
    // int raio = 100;
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

   boudaring = new BBox(-200, -150, 0, 200, 150, 0);
  // objetosLista.Add(boudaring);






     // if(mouseX != null && mouseY != null){
     //   pontoCentro = new Ponto4D(mouseX, mouseY);
    //  } else{
        pontoCentro = new Ponto4D(0, 0);
   //   } 

      Ponto4D ponto0 = new Ponto4D(0, 0);
      ponto0 = new Ponto4D(0, 0);



       Ponto4D ponto1 = new Ponto4D(140, 150);  



      Ponto4D ponto2 = new Ponto4D(-140, 150);                     



      Ponto4D ponto3 = new Ponto4D(-140, -150);                     


      Ponto4D ponto4 = new Ponto4D(140, -150);             



    /*  circulo_obj = new Circulo(objetoId, null, ponto0 , (double)200);
      circulo_obj.ObjetoCor.CorR = 0;
      circulo_obj.ObjetoCor.CorG = 0;
      circulo_obj.ObjetoCor.CorB = 0;
      circulo_obj.PrimitivaTamanho = 3;

      circulo_obj.PrimitivaTipo = PrimitiveType.Points;
  
      objetosLista.Add(circulo_obj);
      objetoSelecionado = circulo_obj; */


       circulo_obj2 = new Circulo(objetoId, null, pontoCentro, (double)100);
      circulo_obj2.ObjetoCor.CorR = 0;
      circulo_obj2.ObjetoCor.CorG = 0;
      circulo_obj2.ObjetoCor.CorB = 0;
      circulo_obj2.PrimitivaTamanho = 3;
      circulo_obj2.PrimitivaTipo = PrimitiveType.Points;
        objetosLista.Add(circulo_obj2);
      objetoSelecionado = circulo_obj2;


      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new SegReta(objetoId, null, ponto1, ponto2);
      obj_SegReta.ObjetoCor.CorR = 0;
      obj_SegReta.ObjetoCor.CorG = 50;
      obj_SegReta.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;


      
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta2 = new SegReta(objetoId, null, ponto3, ponto4);
      obj_SegReta2.ObjetoCor.CorR = 0;
      obj_SegReta2.ObjetoCor.CorG = 50;
      obj_SegReta2.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta2);
      objetoSelecionado = obj_SegReta2;


       objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta3 = new SegReta(objetoId, null, ponto1, ponto4);
      obj_SegReta3.ObjetoCor.CorR = 0;
      obj_SegReta3.ObjetoCor.CorG = 50;
      obj_SegReta3.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta3);
      objetoSelecionado = obj_SegReta3;



 objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta4 = new SegReta(objetoId, null, ponto3, ponto2);
      obj_SegReta4.ObjetoCor.CorR = 0;
      obj_SegReta4.ObjetoCor.CorG = 50;
      obj_SegReta4.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta4);
      objetoSelecionado = obj_SegReta4;






     

    
      camera.xmin = -600; camera.xmax = 600; camera.ymin = -600; camera.ymax = 600;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");


      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {

      // objetosLista.Clear();                           
            

     // pontoAux = new Ponto4D(ponto.X + ponto0.X, ponto.Y + ponto0.Y);

     // if(mouseX != null && mouseY != null){
     //   pontoCentro = new Ponto4D(mouseX, mouseY);
     // } 
      
      
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
        boudaring.Desenhar();

      for (var i = 0; i < objetosLista.Count; i++) {
        objetosLista[i].Desenhar();
        } 


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
            else if (e.Key == Key.W)
            {        
              
               pontoCentro.Y+= 5;  
              boudaring.Atualizar(pontoCentro);
            }
            else if (e.Key == Key.S)
            {
                pontoCentro.Y-= 5; 
              boudaring.Atualizar(pontoCentro);

            }
            else if (e.Key == Key.A)
            {

                pontoCentro.X-= 5; 
              boudaring.Atualizar(pontoCentro);

              
            }

            else if (e.Key == Key.D)
            {

             pontoCentro.X+= 5; 
            boudaring.Atualizar(pontoCentro);

            }          
            
            else
                Console.WriteLine(" __ Tecla não implementada.");


    }




    //TODO: não está considerando o NDC
   protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      mouseX = e.Position.X;
      mouseY = 600 - e.Position.Y; // Inverti eixo Y

      if (mouseMoverPto && (objetoSelecionado != null))
      {
        objetoSelecionado.PontosUltimo().X = mouseX;
        objetoSelecionado.PontosUltimo().Y = mouseY;
      }
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
