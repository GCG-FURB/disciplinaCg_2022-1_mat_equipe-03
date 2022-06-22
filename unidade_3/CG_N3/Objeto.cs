/**
  Autor: Dalton Solano dos Reis
**/

#define CG_OpenGL
using System;

using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using CG_Biblioteca;

namespace gcgcg
{
  internal abstract class Objeto
  {
    protected char rotulo;
    private Cor objetoCor = new Cor(255, 255, 255, 255);
    public Cor ObjetoCor { get => objetoCor; set => objetoCor = value; }
    private PrimitiveType primitivaTipo = PrimitiveType.LineLoop;
    
// Nova daqui
    private Transformacao4D Matriz = new Transformacao4D(); 

  public Transformacao4D getMatriz() {
    return this.Matriz;
  }

  private static Transformacao4D matrizGlobal = new Transformacao4D();  
  private static Transformacao4D matrizTmpEscala = new Transformacao4D(); // Mudar
  private static Transformacao4D matrizTmpRotacao = new Transformacao4D();
  private static Transformacao4D matrizTmpTranslacao = new Transformacao4D();
  private static Transformacao4D matrizTmpTranslacaoInversa = new Transformacao4D();
    
  private char eixoRotacao = 'z';
    // Até aqui


    public PrimitiveType PrimitivaTipo { get => primitivaTipo; set => primitivaTipo = value; }
    private float primitivaTamanho = 1;
    public float PrimitivaTamanho { get => primitivaTamanho; set => primitivaTamanho = value; }
    private BBox bBox = new BBox();
    public BBox BBox { get => bBox; set => bBox = value; }
    private List<Objeto> objetosLista = new List<Objeto>();
    private Objeto paiRef ;
    public Objeto getPaiRef(){ 
    return this.paiRef;
    }
    public Objeto(char rotulo, Objeto paiRef)
    {
      this.rotulo = rotulo;
    }
    public void removeFilho(Objeto filho) {
      this.objetosLista.Remove(filho);
    }

    public void AtribuirIdentidade() => this.Matriz.AtribuirIdentidade();


    public void abreFecha()
    {
      if (this.primitivaTipo == PrimitiveType.LineLoop){
        this.primitivaTipo = PrimitiveType.LineStrip;
      }
      else{
        this.primitivaTipo = PrimitiveType.LineLoop;
      }
    }
    public Objeto verificaSelecao(Ponto4D mousePto){
      ObjetoGeometria objetoAramado = this  as ObjetoGeometria;
      if(BBox.EstaDentro(mousePto)&&(objetoAramado.ScanLine(mousePto))){  //&&(objetoAramado.Scanline(mousePto))){
      //if(BBox.EstaDentro(mousePto) &&(objetoAramado.Scanline(mousePto))){
          System.Console.WriteLine(" Bbox dentro");
           return this;
      }
        for (var i = 0; i < objetosLista.Count; i++)
      {
        ObjetoGeometria objetoAramadofilho =objetosLista[i] as ObjetoGeometria;
        if(objetosLista[i].verificaSelecao(mousePto)!=null){
          return objetosLista[i].verificaSelecao(mousePto);
        }
      }
      
      System.Console.WriteLine(" Bbox fora");
      return (Objeto) null;
    }
    public void RemoverTudo()
    {
      for (int index = 0; index < this.objetosLista.Count; ++index){
        this.objetosLista[index].RemoverTudo();
      }
      this.objetosLista.Clear();
    }

// Daqui

  public void Rotacao(double angulo)
    {
      this.RotacaoEixo(angulo);
      this.Matriz = Objeto.matrizTmpRotacao.MultiplicarMatriz(this.Matriz);
    }

    public void RotacaoEixo(double angulo) // Ele vai sempre ir pro case 'z'
    {
      switch (this.eixoRotacao)
      {
        case 'x':
        Console.WriteLine("Entrou no x");
          Objeto.matrizTmpRotacao.AtribuirRotacaoX(Transformacao4D.DEG_TO_RAD * angulo);
          break;
        case 'y':
        Console.WriteLine("Entrou no y");
          Objeto.matrizTmpRotacao.AtribuirRotacaoY(Transformacao4D.DEG_TO_RAD * angulo);
          break;
        case 'z':
          Objeto.matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
          break;
      }
    }


    public void RotacaoZBBox(double angulo)
    {
      matrizGlobal.AtribuirIdentidade();
      Ponto4D pontoPivo = bBox.obterCentro;

      matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
      matrizGlobal = matrizTmpTranslacao.MultiplicarMatriz(matrizGlobal);

      RotacaoEixo(angulo);
      matrizGlobal = matrizTmpRotacao.MultiplicarMatriz(matrizGlobal);

      matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
      matrizGlobal = matrizTmpTranslacaoInversa.MultiplicarMatriz(matrizGlobal);

      Matriz = Matriz.MultiplicarMatriz(matrizGlobal);
    }



  public void tamanho(double x, double y, double z)
    {
      Objeto.matrizGlobal.AtribuirIdentidade();

      Ponto4D centro = this.bBox.obterCentro;
      Objeto.matrizTmpTranslacao.AtribuirTranslacao(-centro.X, -centro.Y, -centro.Z); // É assim?
      Objeto.matrizGlobal = Objeto.matrizTmpTranslacao.MultiplicarMatriz(Objeto.matrizGlobal);

      Objeto.matrizTmpEscala.AtribuirEscala(x, y, z);
      Objeto.matrizGlobal = Objeto.matrizTmpEscala.MultiplicarMatriz(Objeto.matrizGlobal);
      Objeto.matrizTmpTranslacaoInversa.AtribuirTranslacao(centro.X, centro.Y, centro.Z);

      Objeto.matrizGlobal = Objeto.matrizTmpTranslacaoInversa.MultiplicarMatriz(Objeto.matrizGlobal);
      this.Matriz = this.Matriz.MultiplicarMatriz(Objeto.matrizGlobal);
    }

    public void escala(double x, double y, double z)
    {
      Transformacao4D transformacao4D = new Transformacao4D();
      transformacao4D.AtribuirEscala(x, y, z);
      this.Matriz = transformacao4D.MultiplicarMatriz(this.Matriz);
    }

    public void movimentar(double x, double y, double z)
    {
      Transformacao4D transformacao4D = new Transformacao4D();
      transformacao4D.AtribuirTranslacao(x, y, z);
      this.Matriz = transformacao4D.MultiplicarMatriz(this.Matriz);
    }

// Até aqui




public void Desenhar()
    {
#if CG_OpenGL
      GL.PushMatrix();                                    // N3-Exe12: grafo de cena
      GL.MultMatrix(Matriz.ObterDados());
      GL.Color3(objetoCor.CorR, objetoCor.CorG, objetoCor.CorB);
      GL.LineWidth(primitivaTamanho);
      GL.PointSize(primitivaTamanho);
#endif
      DesenharGeometria();
      for (var i = 0; i < objetosLista.Count; i++)
      {
        objetosLista[i].Desenhar();
      }
      GL.PopMatrix();                                     // N3-Exe12: grafo de cena
    }


 /*   public void Desenhar()
    {
#if CG_OpenGL
      GL.Color3(objetoCor.CorR, objetoCor.CorG, objetoCor.CorB);
      GL.LineWidth(primitivaTamanho);
      GL.PointSize(primitivaTamanho);
#endif
      DesenharGeometria();
      for (var i = 0; i < objetosLista.Count; i++)
      {
        objetosLista[i].Desenhar();
      }
    } */

    protected abstract void DesenharGeometria();
    public void FilhoAdicionar(Objeto filho)
    {
      this.objetosLista.Add(filho);
    }

  }
}