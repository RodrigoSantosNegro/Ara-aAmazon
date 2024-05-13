using AranhaAmazon.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranhaAmazon.sql
{
    internal class Postgresql
    {
        private static string cs = "Server=localhost; Port=5432; User Id=postgres; Password=1234; Database=postgres";


        public static bool EjecutarSQL(string varSql)
        {


            int contador = 0;

            while (contador < 3)
            {
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(cs))
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(varSql, conn);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("**********\n" + ex.Message + "\n**********");
                    contador++;
                }
            }
            Console.WriteLine("Fallo al ejecutar SQL\n-----------------------");
            return false;

        }

        public static bool InsertarArticulo(Producto producto)
        {

            EjecutarSQL(@$" INSERT INTO public.producto(
	                            nombre, precio, url, fecha_lectura, oferta, categoria)
	                            VALUES ('{producto.nombre}', {producto.precio.ToString().Replace(",", ".")}, '{producto.url}', '{producto.fecha_lectura}', {producto.oferta}, '{producto.categoria}');");

            return true;

        }

        public static string EsTexto(Object texto, int caracteres)
        {

            if (texto == DBNull.Value || texto == null)
            {
                return "";
            }
            else
            {
                string resultado = texto.ToString();
                if (resultado != "")
                {
                    if (resultado.Length > caracteres)
                    {
                        resultado = resultado.Substring(0, caracteres);
                    }
                    resultado = resultado.Replace("'", "´");
                    resultado = resultado.Replace("<", "");
                    resultado = resultado.Replace(">", "");
                }
                return resultado;
            }
        }

        public static double EsNumero(Object numero)
        {
            double resultado;
            if (numero != DBNull.Value && numero != null)
            {
                if (double.TryParse(numero.ToString(), out double result))
                {
                    numero = EsTexto(numero, 200).Replace(".", ",");
                    resultado = Convert.ToDouble(numero);
                }
                else
                {
                    resultado = 0;
                }
            }
            else
            {
                resultado = 0;
            }
            return resultado;
        }
    }
}
