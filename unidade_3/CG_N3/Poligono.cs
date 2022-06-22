// Decompiled with JetBrains decompiler
// Type: gcgcg.Poligono
// Assembly: CG_N3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACB211E-4911-4AE3-BD42-BD9E9A575432
// Assembly location: C:\Users\henri\Downloads\win10-x64\win10-x64\CG_N3.dll

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg
{
  internal class Poligono : ObjetoGeometria
  {
    public Poligono(char rotulo, Objeto paiRef)
      : base(rotulo, paiRef)
    {
    }

    protected override void DesenharObjeto()
    {
      GL.Begin(this.PrimitivaTipo);
      foreach (Ponto4D ponto4D in this.pontosLista)
        GL.Vertex2(ponto4D.X, ponto4D.Y);
      GL.End();
    }

    public override string ToString()
    {
      string str = "__ Objeto Poligono: " + this.rotulo.ToString() + "\n";
      for (int index = 0; index < this.pontosLista.Count; ++index)
        str = str + "P" + index.ToString() + "[" + this.pontosLista[index].X.ToString() + "," + this.pontosLista[index].Y.ToString() + "," + this.pontosLista[index].Z.ToString() + "," + this.pontosLista[index].W.ToString() + "]\n";

      /*
      foreach (Objeto filho in this.objetosLista){
        Poligono objetoAramado = filho  as Poligono;

        str = str +"filho n"+indice+"\n";
        for (int index = 0; index < objetoAramado.pontosLista.Count; ++index){
                  str = str + "P" + index.ToString() + "[" + this.pontosLista[index].X.ToString() + "," + this.pontosLista[index].Y.ToString() + "," + this.pontosLista[index].Z.ToString() + "," + this.pontosLista[index].W.ToString() + "]\n";
      
        }
        indice++;

      }*/ 
      //cocertar depois
      
      return str;
    }
  }
}
