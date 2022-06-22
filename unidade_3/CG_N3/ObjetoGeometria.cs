/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Debug
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg
{
  internal abstract class ObjetoGeometria : Objeto
  {
    protected List<Ponto4D> pontosLista = new List<Ponto4D>();

    public ObjetoGeometria(char rotulo, Objeto paiRef) 	: base(rotulo, paiRef)
{
}


public List<Ponto4D> getPontosLista(){
  return this.pontosLista;
}
  /*public Objeto verificaSelecao(Ponto4D mousePto){
      
      if(1==2){
        return this;
      }
     
      return null;
    }
    public bool scanline(){
      for (var i = 0; i < pontosLista.Count; i++)
      {

      }
      return false;
    }*/

 protected List<ObjetoGeometria>objetosLista;
    /*
    public void Desenhar()
    {
#if CG_OpenGL
      GL.PushMatrix();                                    // N3-Exe12: grafo de cena
      GL.MultMatrix(matriz.ObterDados());
      GL.Color3(objetoCor.CorR, objetoCor.CorG, objetoCor.CorB);
      GL.LineWidth(primitivaTamanho);
      GL.PointSize(primitivaTamanho);
#endif
      DesenharGeometria();
      for (var i = 0; i < objetosLista.Count; i++)//desenhar dos filhos
      {
        objetosLista[i].Desenhar();
      }
      GL.PopMatrix();                                     // N3-Exe12: grafo de cena
    }
    //*/

      public bool ScanLine(Ponto4D ptoClique)
    {
      int num = 0;
      bool boleano=true;
      for (int index = 0; index < this.pontosLista.Count ; index++)
      {
        if (Matematica.ScanLine(ptoClique, this.pontosLista[index], this.pontosLista[(index + 1)%pontosLista.Count])){
          num++;
        }
      }

      if(num % 2 == 0){
        boleano=false;
      }
      
      return boleano;
    }
     public Ponto4D qualPtoPerto(Ponto4D mousePto)
    {

      Ponto4D retorno;
      
      retorno=pontosLista[0];
      
      double aux = Matematica.distanciapontos(mousePto, this.pontosLista[0]);
      for (int i = 1; i < this.pontosLista.Count; ++i)
      {
        double aux2 =Matematica.distanciapontos(mousePto, this.pontosLista[i]);
        if (aux > aux2)
        {
          aux = aux2;
          retorno=pontosLista[i];
        }
      }
      return retorno;
      /*
      if (!remover)
        return this.pontosLista[index1];
      this.pontosLista.RemoveAt(index1);
      return (Ponto4D) null;
      */
      
    }
    public Ponto4D removerPtoPerto(Ponto4D mousePto){
      Ponto4D p=qualPtoPerto(mousePto);
      pontosLista.Remove(p);
      return p;
    }
    public void PontosAtualiza()
    {
      this.BBox.Atribuir(this.pontosLista[0]);
      for (int index = 1; index < this.pontosLista.Count; ++index)
        this.BBox.Atualizar(this.pontosLista[index]);
      this.BBox.ProcessarCentro();
    }
  
    protected override void DesenharGeometria()
    {
      DesenharObjeto();//sera que isso?
    }
    protected abstract void DesenharObjeto();
    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
      if (pontosLista.Count.Equals(1))
        base.BBox.Atribuir(pto);
      else
        base.BBox.Atualizar(pto);
      base.BBox.ProcessarCentro();
    }
    //rever
    public void PontosAtualizaFinal()
    {
      this.BBox.Atribuir(this.pontosLista[0]);
      for (int index = 1; index < this.pontosLista.Count; ++index)
        this.BBox.Atualizar(this.pontosLista[index]);
      this.BBox.ProcessarCentro();
    }


    public void PontosRemoverUltimo()
    {
      pontosLista.RemoveAt(pontosLista.Count - 1);
    }

    protected void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    public Ponto4D PontosUltimo()
    {
      return pontosLista[pontosLista.Count - 1];
    }

    public void PontosAlterar(Ponto4D pto, int posicao)
    {
      pontosLista[posicao] = pto;
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }
#endif

  }
}