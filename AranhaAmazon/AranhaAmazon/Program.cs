﻿
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using AranhaAmazon.Utils;
using AranhaAmazon.sql;

partial class Program
{
    static void Main(string[] args)
    {
        //######################################################################################

        //++++CREAR UN HISTORIAL DE ERRORES EN LOCAL++++

        //######################################################################################

        string erroresPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "erroresAmazon");

        if (!Directory.Exists(erroresPath))
        {
            Console.WriteLine("Creando directorio errores...");
            Directory.CreateDirectory(erroresPath);
            Console.WriteLine("Directorio creado.");
        }

        Thread.Sleep(500);

        //######################################################################################

        //++++++++++++++++++++++++++++++++++++++++++++++

        //######################################################################################

        DateTime fechaInicio = DateTime.Now;

        //Opciones para cuando iniciemos Chrome. Tamaño, idioma, desactivar características blink controladas por automatización...
        IWebDriver GetDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--lang=es");
            //options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            IWebDriver driver = new ChromeDriver(Directory.GetCurrentDirectory(), options);
            return driver;
        }
        //Creamos en driver con las configuraciones de antes. Maximizamos pantalla.
        //MUY IMPORTANTE añadir tiempos de espera que sea más difícil que localicen que es un bot. No queremos que nos baneen la IP :(
        var driver = GetDriver();
        Thread.Sleep(500);
        driver.Manage().Window.Maximize();
        Thread.Sleep(1000);
        driver.Navigate().GoToUrl("https://www.amazon.es/");
        Thread.Sleep(1000);
        driver.Navigate().Refresh();
        Thread.Sleep(2000);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        Thread.Sleep(800);

        //Rechazamos las cookies cuando nos salga el tremendo texto
        var rejectCookiesButton = driver.FindElement(By.Id("sp-cc-rejectall-link"));
        Thread.Sleep(500);
        rejectCookiesButton.Click();

        //Seleccionamos de la BD las categorías que no han sido checkeadas a día de hoy
        List<string> categorias = Postgresql.CategoriasRevisadasHoy();

        //Buscamos artículos por categoría
        foreach (var cat in categorias)
        {
            Console.WriteLine($"\n\n---- {cat.ToUpper()} ----");

            DateTime fechaInicioCategoria = DateTime.Now;

            //Buscamos un atributo del front de amazon que nos pueda indicar con exactitud el elemento que buscamos, en este caso la barra de búsqueda y la lupa de navegación.
            var textBox = driver.FindElement(By.Id("twotabsearchtextbox"));
            var submitButton = driver.FindElement(By.Id("nav-search-submit-button"));
            //Escribimos el texto en el textbox (barra de búsqueda) y navegamos.
            Thread.Sleep(1000);
            textBox.Clear();
            textBox.SendKeys(cat);
            Thread.Sleep(500);
            submitButton.Click();
            Thread.Sleep(2000);

            int pag = 1;
            bool salir = false;
            int real = 0;
            do
            {
                //Seleccionamos todos los poductos que no estén patrocinados.
                List<IWebElement> products = new List<IWebElement>(driver.FindElements(By.CssSelector(".puis-card-container.s-card-container:not(:has(.puis-sponsored-label-text))")));
                real += products.Count();
                Console.WriteLine(products.Count() + " ARTÍCULOS ENCONTRADOS (esperados 48 si no es la última página)");
                Thread.Sleep(750);

                //Añadimos los datos que quiera guardar de cada producto.
                List<Producto> listProducts = new List<Producto>();//Lista por si quiero hacer cositas después.
                try
                {
                    //Cambiamos el tiempo que sigue buscando un elemento que no ve de primeras para aumentar la velocidad de lectura de productos.
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.2);
                    foreach (IWebElement producto in products)
                    {

                        Producto p = new Producto();
                        try
                        {
                            //Leemos un producto y le pasamos la categoría
                            p = LecturaProducto(producto, cat);
                            listProducts.Add(p);

                            //Insertamos en postgresql
                            Postgresql.InsertarArticulo(p);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }


                    }
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    try
                    {
                        //Si existe este elemento es que no hay más páginas.
                        driver.FindElement(By.CssSelector(".s-pagination-item.s-pagination-next.s-pagination-disabled"));
                        var fechaFin = DateTime.Now;

                        Console.WriteLine(fechaInicio);
                        Console.WriteLine(fechaFin);

                        //AÑADIMOS LA CATEGORÍA A LA LISTA DE YA CHECKEADAS HOY
                        int id_categoria = Postgresql.GetIdCategoria(cat);
                        string strEstimado = Postgresql.EsTexto(driver.FindElement(By.CssSelector(".sg-col-14-of-20 > .sg-col-inner > .a-section.a-spacing-small.a-spacing-top-small")).Text, 255);
                        string[] s1 = strEstimado.Split(' ');
                        string[] s2 = s1[0].Split('-');
                        int estimado = int.Parse(s2[1]);
                        bool insertado = Postgresql.InsertarCategoriaLeida(id_categoria, cat, fechaInicioCategoria, fechaFin, estimado, real);
                        Console.WriteLine($"No hay más páginas. Añadiendo categoría '{cat}' a checkeadas hoy - {insertado}");

                        Console.WriteLine("\n***********************  TERMINAMOS LECTURA.");
                        break;
                    }
                    catch (Exception)
                    {
                        //Seguimos navegando
                    }
                    Console.WriteLine($"\n-- SIGUIENTE PÁGINA ({++pag}) --\n");
                    IWebElement sig = driver.FindElement(By.CssSelector(".s-pagination-item.s-pagination-next.s-pagination-button.s-pagination-separator"));
                    sig.Click();
                    Thread.Sleep(4000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ha habido un error: " + ex);
                }

            } while (!salir);


        }

        Console.WriteLine("\n***********  Se cierra araña.");
        driver.Close();
        driver.Quit();
    }




    //<<<MÉTODOS>>>
    private static Producto LecturaProducto(IWebElement producto, string categoria)
    {
        Producto p = new Producto();
        Console.Write("Obteniendo nombre -- ");
        p.nombre = Postgresql.EsTexto(producto.FindElement(By.CssSelector(".a-size-base-plus.a-color-base.a-text-normal")).Text, 500);
        Console.WriteLine("Done.");
        Console.Write("Obteniendo precio -- ");
        try
        {
            p.precio = Postgresql.EsNumero(double.Parse(producto.FindElement(By.CssSelector(".a-price-whole")).Text + "," + producto.FindElements(By.CssSelector(".a-price-fraction"))[0].Text));
        }
        catch
        {
            p.precio = -1;
        }

        Console.WriteLine("Done.");
        Console.Write("Obteniendo  URL -- ");
        p.url = Postgresql.EsTexto(producto.FindElement(By.CssSelector(".a-link-normal.s-underline-text.s-underline-link-text.s-link-style.a-text-normal")).GetAttribute("href"), 10000);
        Console.WriteLine("Done.");
        Console.Write("Obteniendo fecha_lectura (no debería de tardar nada xd) -- ");
        p.fecha_lectura = DateTime.Now.ToString();
        Console.WriteLine("Done.");
        Console.Write("Obteniendo oferta -- ");
        p.oferta = producto.FindElements(By.CssSelector(".a-badge-label-inner.a-text-ellipsis .a-badge-text")).Count > 0;
        Console.WriteLine("Done.");
        p.categoria = categoria;
        p.id_categoria = Postgresql.GetIdCategoria(categoria);
        return p;
    }
}
