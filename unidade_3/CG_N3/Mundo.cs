/**
  Autor: Dalton Solano dos Reis
**/

//#define CG_Privado // código do professor.
#define CG_Gizmo  // debugar gráfico.
#define CG_Debug // debugar texto.
#define CG_OpenGL // render OpenGL.
//#define CG_DirectX // render DirectX.

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
    protected List<Objeto> objetosLista = new List<Objeto>();//mostra lista de objeto geometria
    private ObjetoGeometria objetoSelecionado = null;
    private char objetoId = '@';
    private bool bBoxDesenhar = false;
   // private bool novo = false;
    private Poligono poligono;
    int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
    private Ponto4D pontoMouseMove;
    // private Ponto4D mudarponto;
    // private bool mouseMoverPto = false;
    private Retangulo obj_Retangulo;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");
      //Console.WriteLine(Matematica.teste());

      objetoId = Utilitario.charProximo(objetoId);
      obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
      obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Retangulo);
      //objetoSelecionado = obj_Retangulo;


#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      obj_Circulo.PrimitivaTipo = PrimitiveType.Points;
      obj_Circulo.PrimitivaTamanho = 5;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
#if CG_OpenGL
      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
#endif
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
#if CG_OpenGL
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
#endif
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
#if CG_OpenGL
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
#endif
#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();
#if CG_Gizmo
      if (bBoxDesenhar && (objetoSelecionado != null))
        objetoSelecionado.BBox.Desenhar();
#endif
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
        Console.WriteLine("--- Objetos / Pontos: ");
        for (var i = 0; i < objetosLista.Count; i++)
        {
          Console.WriteLine(objetosLista[i]);
        }
      }
      else if (e.Key == Key.O)
        bBoxDesenhar = !bBoxDesenhar;
      else if (e.Key == Key.R)
        {
        if(objetoSelecionado!=null){
          this.objetoSelecionado.ObjetoCor.CorR = (byte)255;
          this.objetoSelecionado.ObjetoCor.CorG = (byte) 0;
          this.objetoSelecionado.ObjetoCor.CorB = (byte) 0;
        }
        }
        else if (e.Key == Key.G)
        {
         if(objetoSelecionado!=null){ 
          this.objetoSelecionado.ObjetoCor.CorR = (byte) 0;
          this.objetoSelecionado.ObjetoCor.CorG = (byte)255;
          this.objetoSelecionado.ObjetoCor.CorB = (byte) 0;
         }
        }
        else if (e.Key == Key.B)
        {
          if(objetoSelecionado!=null){
          this.objetoSelecionado.ObjetoCor.CorR = (byte) 0;
          this.objetoSelecionado.ObjetoCor.CorG = (byte) 0;
          this.objetoSelecionado.ObjetoCor.CorB = (byte)255;
          }
        }
        else if (e.Key == Key.Space)
      {
        
        if (this.poligono == null)
        {
          
          this.objetoId = Utilitario.charProximo(this.objetoId);
          if (this.objetoSelecionado == null)
          {//novo
            this.poligono = new Poligono(this.objetoId, (Objeto) null);
            this.objetosLista.Add((Objeto) this.poligono);
          }
          else
          {//filho
            this.poligono = new Poligono(this.objetoId, (Objeto) this.objetoSelecionado);
            this.objetoSelecionado.FilhoAdicionar((Objeto) this.poligono);
            
          }
          this.poligono.PontosAdicionar(new Ponto4D((double) this.mouseX, (double) this.mouseY));
          this.poligono.PontosAdicionar(new Ponto4D((double) this.mouseX, (double) this.mouseY));
        }
        else{//novo ponto
          this.poligono.PontosAdicionar(new Ponto4D((double) this.mouseX, (double) this.mouseY));
          
          }
      }
            else if (e.Key == Key.Enter)
      {
        if (this.objetoSelecionado != null){
        this.objetoSelecionado.PontosAtualizaFinal();
        }
        if (this.poligono != null )
        {
        
          this.poligono.PontosRemoverUltimo();
          //this.objetoSelecionado = (ObjetoGeometria) this.poligono;
          //objetosLista.Add(poligono);//rever lugar
          this.poligono = (Poligono) null;
        }
        this.pontoMouseMove = (Ponto4D) null;

      }   else if (e.Key == Key.A){
        Console.WriteLine("antes do grafo de cena");
        percoreGrafoCena();
      }else if (e.Key == Key.S){
        if(objetoSelecionado!=null){
          objetoSelecionado.abreFecha();
        }
      }        else if (e.Key == Key.C){
        if(objetoSelecionado!=null){
          this.RemovePoli();
        }
      }    
      else if (e.Key == Key.V){
      if(objetoSelecionado!=null){
    pontoMouseMove = this.objetoSelecionado.qualPtoPerto(new Ponto4D((double) this.mouseX, (double) this.mouseY));
      }
    } else if (e.Key == Key.D &&objetoSelecionado!=null)
        {
          
          if(objetoSelecionado.getPontosLista().Count>2){
          pontoMouseMove = this.objetoSelecionado.removerPtoPerto(new Ponto4D((double) this.mouseX, (double) this.mouseY));
          if (pontoMouseMove != null){
            this.objetoSelecionado.PontosAtualiza();
          }
          }else {
            RemovePoli();
          }
          
          
        }

        else if (e.Key == Key.M && objetoSelecionado != null)
          Console.WriteLine(this.objetoSelecionado.getMatriz());


        else if (e.Key == Key.Number1 && objetoSelecionado != null)
        {                    
         this.objetoSelecionado.Rotacao(5.0);         
        }

        else if (e.Key == Key.Number2 && objetoSelecionado != null)
        {                    
         this.objetoSelecionado.Rotacao(-5.0);         
        }

        else if (e.Key == Key.Number3 && objetoSelecionado != null) {
          this.objetoSelecionado.RotacaoZBBox(5.0);
          }
        else if (e.Key == Key.Number4 && objetoSelecionado != null) {
          this.objetoSelecionado.RotacaoZBBox(-5.0);
          }

          else if (e.Key == Key.I && objetoSelecionado != null) {
         this.objetoSelecionado.AtribuirIdentidade();
          }

          else if (e.Key == Key.Home && objetoSelecionado != null){
         this.objetoSelecionado.tamanho(0.5, 0.5, 0.5);
          }
        else if (e.Key == Key.End && objetoSelecionado != null) {
        this.objetoSelecionado.tamanho(2.0, 2.0, 2.0);
          }

        else if (e.Key == Key.PageUp && objetoSelecionado != null) {
          this.objetoSelecionado.escala(0.5, 0.5, 0.5);
          }
        else if (e.Key == Key.PageDown && objetoSelecionado != null) {
          this.objetoSelecionado.escala(2.0, 2.0, 2.0);
          }

        else if (e.Key == Key.Left && objetoSelecionado != null) {
          this.objetoSelecionado.movimentar(-5.0, 0.0, 0.0);
        }
        else if (e.Key == Key.Right && objetoSelecionado != null) {
          this.objetoSelecionado.movimentar(5.0, 0.0, 0.0);
          }
        else if (e.Key == Key.Up && objetoSelecionado != null) {
          this.objetoSelecionado.movimentar(0.0, 5.0, 0.0);
          }
        else if (e.Key == Key.Down && objetoSelecionado != null) {
          this.objetoSelecionado.movimentar(0.0, -5.0, 0.0);
          }
        
      else
        Console.WriteLine(" __ Tecla não implementada.");
    }

    

    //TODO: não está considerando o NDC

    
   
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      this.mouseX = e.Position.X;
      //this.mouseY = 600 - e.Position.Y;
      this.mouseY = 600 -e.Position.Y;
      if (this.poligono != null)
      {
        this.poligono.PontosUltimo().X = (double) this.mouseX;
        this.poligono.PontosUltimo().Y = (double) this.mouseY;
      }
      else if (this.pontoMouseMove != null)
      {
        this.pontoMouseMove.X = (double) this.mouseX;
        this.pontoMouseMove.Y = (double) this.mouseY;
        if(objetoSelecionado!=null){
        this.objetoSelecionado.PontosAtualizaFinal();
      }
      }
    }
        private void RemovePoli()
    {
      objetoSelecionado.RemoverTudo();
      if (this.objetoSelecionado.getPaiRef() == null){
        objetosLista.Remove((Objeto) this.objetoSelecionado);
      }
      else{
        objetoSelecionado.getPaiRef().removeFilho((Objeto) this.objetoSelecionado);
      }
      this.objetoSelecionado = (ObjetoGeometria) null;
    }
    public void percoreGrafoCena(){
      Objeto retorno = (Objeto) null;
       //Console.WriteLine("no grafo de cena");
      for(var i=0; i<objetosLista.Count;i++){
        Ponto4D p= new Ponto4D(this.mouseX,this.mouseY,0);
        Console.WriteLine("pX = "+p.X);
        Console.WriteLine("pY = "+p.Y);
        ///Ponto4D teste= new Ponto4D(100,100,0);
        //retorno= this.objetosLista[i].verificaSelecao(pontoMouseMove);//mudar lista para lista de objeto geometria
        retorno= this.objetosLista[i].verificaSelecao(p);
        if (retorno != null)
        {
          this.objetoSelecionado = retorno as ObjetoGeometria;
          //poligono=retorno as Poligono;//para dizer que ele é o pai 
          break;
        }
      }
      if (retorno == null){
        this.objetoSelecionado = (ObjetoGeometria) null;
      }
      /*
      chama
      
      */
    }
   
#if CG_Gizmo
    private void Sru3D()
    {
#if CG_OpenGL
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
#endif
    }
#endif    
  }
  class Program
  {
    static void Main(string[] args)
    {
      ToolkitOptions.Default.EnableHighResolution = false;
      Mundo window = Mundo.GetInstance(600, 600);
      window.Title = "CG_N2";
      window.Run(1.0 / 60.0);
    }
  }
}
