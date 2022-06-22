using System;

namespace CG_Biblioteca
{
  /// <summary>
  /// Classe com funções matemáticas.
  /// </summary>
  public abstract class Matematica
  {
    /// <summary>
    /// Função para calcular um ponto sobre o perímetro de um círculo informando um ângulo e raio.
    /// </summary>
    /// <param name="angulo"></param>
    /// <param name="raio"></param>
    /// <returns></returns>
    public static Ponto4D GerarPtosCirculo(double angulo, double raio)
    {
      Ponto4D pto = new Ponto4D();
      pto.X = (raio * Math.Cos(Math.PI * angulo / 180.0));
      pto.Y = (raio * Math.Sin(Math.PI * angulo / 180.0));
      pto.Z = 0;
      return (pto);
    }
    public static bool ScanLine(Ponto4D ptoClique, Ponto4D ptoIni, Ponto4D ptoFim)
    {
      //System.Console.WriteLine("no  scanline mat");
      double aux= (ptoClique.Y -ptoIni.Y)/(ptoFim.Y -ptoIni.Y );//ti= (yi - y1) / (y2 - y1);
      double aux2= ptoIni.X+ ( ptoFim.X-ptoIni.X)*aux; //xi= x1 + (x2 - x1) * ti;
      if(aux>=0.0 && aux<=1.0 ){
        if( aux2 >ptoClique.X){
        return true;
        }
      }
      return false; 
      
    }
    public static double distanciapontos(Ponto4D p1,Ponto4D p2){
        double dx=Math.Pow(p2.X - p1.X, 2.0);
        double dy=Math.Pow(p2.Y - p1.Y, 2.0);
        return  (dx+dy);
    }
    public static String teste(){
      return "teste";
    }
    

    public static double GerarPtosCirculoSimétrico(double raio)
    {
      return (raio * Math.Cos(Math.PI * 45 / 180.0));
    }
    
  }
}