using AranhaAmazon.Utils;
using Npgsql;
using System.Data;


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

        public static DataTable RellenaDt(string varSql)
        {
            int contador = 0;

            while (contador < 3)
            {
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(cs))
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(varSql, conn);
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("**********\n" + ex.Message + "\n**********");
                    contador++;
                }
            }
            return null;
        }

        public static bool InsertarArticulo(Producto producto)
        {

            EjecutarSQL(@$" INSERT INTO producto(
	                            nombre, precio, url, fecha_lectura, oferta, categoria, id_categoria)
	                            VALUES ('{producto.nombre}', {producto.precio.ToString().Replace(",", ".")}, '{producto.url}', '{producto.fecha_lectura}', {producto.oferta}, '{producto.categoria}', {producto.id_categoria});");

            return true;

        }
        
        internal static bool InsertarCategoriaLeida(int id_categoria, string cat, DateTime fechaInicio, DateTime fechaFin, int estimado, int real)
        {
            EjecutarSQL($@"INSERT INTO categorias_leidas_hoy(
	                        id_categoria, fecha_inicio, fecha_fin, estimado, real)
	                        VALUES ({id_categoria}, '{fechaInicio}', '{fechaFin}', {estimado}, {real});");

            return true;
        }

        public static List<string> CategoriasRevisadasHoy()
        {
            List<string> resp = new List<string>();
            string sql = $@"SELECT c.nombre
                            FROM categoria c
                            WHERE NOT EXISTS (
                                SELECT 1
                                FROM categorias_leidas_hoy clh
                                WHERE clh.id_categoria = c.id
                                AND clh.fecha_inicio::date = CURRENT_DATE
                            )";

            DataTable dtCats = RellenaDt(sql);
            foreach(DataRow fila in dtCats.Rows)
            {
                resp.Add(EsTexto(fila["nombre"], 255));
            }

            return resp;
        }

        internal static int GetIdCategoria(string categoria)
        {
            try
            {
                string sql = $@"SELECT id
                                FROM categoria
                                WHERE nombre = '{categoria}'";
                DataTable dtCats = RellenaDt(sql);
                foreach (DataRow fila in dtCats.Rows)
                {
                    return int.Parse(fila["id"].ToString());
                }
                throw new Exception("No se encontró la categoría especificada.");
            }
            catch(Exception ex)
            {
                throw new Exception("Error GetIdCategoria: " + ex.Message);
            }
            
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
