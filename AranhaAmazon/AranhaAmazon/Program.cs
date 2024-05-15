
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

        string panhales = "pañales dodot talla 4";
        string tecladosGM = "teclado gaming mecanico";
        string[] categorias = { panhales, tecladosGM };

        //AÑADIR TABLA CATEGORÍAS FEITAS A DÍA DE HOXE PARA NON REPETIR ARAÑA.

        foreach (var cat in categorias)
        {
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
            do
            {
                //Seleccionamos todos los poductos que no estén patrocinados.
                List<IWebElement> products = new List<IWebElement>(driver.FindElements(By.CssSelector(".puis-card-container.s-card-container:not(:has(.puis-sponsored-label-text))")));
                Console.WriteLine(products.Count() + " ARTÍCULOS (debería de haber 48 si no es la última página)");
                Thread.Sleep(5000);

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
                            if(cat == panhales)
                            {
                                p = lecturaProductoPanhal(producto);
                            }
                            else if(cat == tecladosGM)
                            {
                                p = lecturaProductoTecladoMecanico(producto);
                            }
                            else
                            {
                                Console.WriteLine("No existe la categoría");
                            }
                            //Insertamos en postgresql
                            //bool insertado = Postgresql.InsertarArticulo(p);
                            Console.WriteLine("\n");
                            Console.WriteLine("Insertado");
                            Console.WriteLine("\n=====================================================\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }

                        listProducts.Add(p);

                    }
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    try
                    {
                        //Si existe este elemento es que no hay más páginas.
                        driver.FindElement(By.CssSelector(".s-pagination-item.s-pagination-next.s-pagination-disabled"));
                        var fechaFin = DateTime.Now;

                        Console.WriteLine(fechaInicio);
                        Console.WriteLine(fechaFin);

                        Console.WriteLine("\n***********************  TERMINAMOS PAÑALES.");
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





        //var textBox2 = driver.FindElement(By.Id("twotabsearchtextbox"));
        //var submitButton2 = driver.FindElement(By.Id("nav-search-submit-button"));
        ////Suponemos que el id de la barra de búsqueda y de la lupita son las mismas que las del inicio. Si no funcionara pues a buscar la ruta en el inspector de código.
        //Thread.Sleep(3000);
        //textBox2.SendKeys("teclado gaming mecanico");
        //Thread.Sleep(500);
        //submitButton2.Click();
        //Thread.Sleep(2000);
        ////Ahora vamos a buscar teclados gaming mecánicos (pasamos de buscar unos 170 artículos a tener una araña que mira 2k productos)
        //do
        //{
        //    //Seleccionamos todos los poductos que no estén patrocinados Y TENGAN EL BOTON DE AÑADIR A LA CESTA.
        //    List<IWebElement> products = new List<IWebElement>(driver.FindElements(By.CssSelector(".puis-card-container.s-card-container:not(:has(.puis-sponsored-label-text)):has(.a-button-inner > .a-icon.a-icon-cart)")));
        //    Console.WriteLine(products.Count() + " TECLADOS");
        //    Console.WriteLine("---------------------------");
        //    Thread.Sleep(1000);

        //    //Añadimos los datos que quiera guardar de cada producto.
        //    List<Producto> listProducts = new List<Producto>();//Lista por si quiero hacer cositas después.
        //    try
        //    {
        //        int pag = 1;
        //        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.2);
        //        foreach (IWebElement producto in products)
        //        {

        //            Producto p = new Producto();
        //            try
        //            {
        //                p = lecturaProductoTecladoMecanico(producto);
        //                //Insertamos en postgresql
        //                bool insertado = Postgresql.InsertarArticulo(p);
        //                Console.WriteLine("\n=====================================================\n");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error: " + ex.Message);
        //            }

        //            listProducts.Add(p);

        //        }
        //        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        //        try
        //        {
        //            //Si existe este elemento es que no hay más páginas.
        //            driver.FindElement(By.CssSelector(".s-pagination-item.s-pagination-next.s-pagination-disabled"));
        //            salir = true;
        //            var fechaFin = DateTime.Now;

        //            Console.WriteLine("--------------");
        //            Console.WriteLine(fechaInicio);
        //            Console.WriteLine(fechaFin);
        //            Console.WriteLine("--------------");

        //            break;
        //        }
        //        catch (Exception)
        //        {
        //            //Seguimos navegando
        //        }
        //        pag = pag++;
        //        Console.WriteLine($"\n-- SIGUIENTE PÁGINA ({pag}) --\n");
        //        IWebElement sig = driver.FindElement(By.CssSelector(".s-pagination-item.s-pagination-next.s-pagination-button.s-pagination-separator"));
        //        sig.Click();
        //        Thread.Sleep(4000);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Ha habido un error: " + ex);
        //    }

        //} while (!salir);



        Console.WriteLine("\n***********  Se cierra araña.");
        driver.Close();
        driver.Quit();
    }




    private static Producto lecturaProductoTecladoMecanico(IWebElement producto)
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
        p.categoria = "Teclado gaming mecanico";

        return p;
    }

    private static Producto lecturaProductoPanhal(IWebElement producto)
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
        //string v = "";
        //p.valoracion = -1;
        //try
        //{
        //    v = producto.FindElement(By.CssSelector("div.s-desktop-width-max.s-desktop-content.s-opposite-dir.s-wide-grid-style.sg-row > div.sg-col-20-of-24.s-matching-dir.sg-col-16-of-20.sg-col.sg-col-8-of-12.sg-col-12-of-16 > div > span.rush-component.s-latency-cf-section > div.s-main-slot.s-result-list.s-search-results.sg-row > div:nth-child(11) > div > div > span > div > div > div.a-section.a-spacing-small.puis-padding-left-small.puis-padding-right-small > div:nth-child(2) > div.a-row.a-size-small > span:nth-child(1) > span > a > i.a-icon.a-icon-star-small.a-star-small-5.aok-align-bottom > span")).Text;
        //    p.valoracion = float.Parse(v.Split(' ')[0]);
        //}
        //catch (Exception)
        //{

        //}        

        //IWebElement datosPrecio = producto.FindElement(By.CssSelector("div[data-cy='title-recipe']"));
        Console.Write("Obteniendo oferta -- ");
        p.oferta = producto.FindElements(By.CssSelector(".a-badge-label-inner.a-text-ellipsis .a-badge-text")).Count > 0;
        Console.WriteLine("Done.");
        p.categoria = "Pañales dodot talla 4";
        //try
        //{
        //    p.pvpr = float.Parse(producto.FindElement(By.CssSelector("div:nth-child(1) > a > div > span.a-price.a-text-price > span.a-offscreen")).Text);
        //}
        //catch (Exception)
        //{
        //    Console.WriteLine("No existe pvpr para este producto");
        //}
        return p;
    }
}