
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using AranhaAmazon.Utils;

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
        var driver = GetDriver();
        Thread.Sleep(500);
        driver.Manage().Window.Maximize();
        Thread.Sleep(1000);
        driver.Navigate().GoToUrl("https://www.amazon.es/");
        Thread.Sleep(1000);
        driver.Navigate().Refresh();
        Thread.Sleep(2000);
        //MUY IMPORTANTE añadir tiempos de espera que sea más difícil que localicen que es un bot. No queremos que nos baneen la IP :(
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        Thread.Sleep(5000);

        //Rechazamos las cookies cuando nos salga el tremendo texto
        var rejectCookiesButton = driver.FindElement(By.Id("sp-cc-rejectall-link"));
        Thread.Sleep(500);
        rejectCookiesButton.Click();
        //Buscamos un atributo del front de amazon que nos pueda indicar con exactitud el elemento que buscamos, en este caso la barra de búsqueda y la lupa de navegación.
        var textBox = driver.FindElement(By.Id("twotabsearchtextbox"));
        var submitButton = driver.FindElement(By.Id("nav-search-submit-button"));
        //Escribimos el texto en el textbox (barra de búsqueda) y navegamos.
        Thread.Sleep(1000);
        textBox.SendKeys("pañales dodot talla 4");
        Thread.Sleep(500);
        submitButton.Click();
        Thread.Sleep(2000);



        //Seleccionamos todos los poductos que no estén patrocinados.
        List<IWebElement> products = new List<IWebElement>(driver.FindElements(By.CssSelector(".puis-card-container.s-card-container:not(:has(.puis-sponsored-label-text))")));
        Console.WriteLine(products.Count() + " PAÑALES");
        Thread.Sleep(5000);

        //Añadimos los datos que quiera guardar de cada producto.
        List<Producto> listProducts = new List<Producto>();
        foreach (IWebElement producto in products)
        {

            Producto p = new Producto();
            try
            {
                p = lecturaProductoPanhal(producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            listProducts.Add(p);
            Console.WriteLine(p);

        }





        driver.Quit();
    }

    private static Producto lecturaProductoPanhal(IWebElement producto)
    {
        Producto p = new Producto();
        p.nombre = producto.FindElement(By.CssSelector(".a-size-base-plus.a-color-base.a-text-normal")).Text;
        p.precio = float.Parse(producto.FindElement(By.CssSelector(".a-price-whole")).Text + "," + producto.FindElements(By.CssSelector(".a-price-fraction"))[0].Text);
        p.url = producto.FindElement(By.CssSelector(".a-link-normal.s-underline-text.s-underline-link-text.s-link-style.a-text-normal")).GetAttribute("href");
        p.fecha_lectura = DateTime.Now.ToString();
        string v = "";
        p.valoracion = -1;
        try
        {
            v = producto.FindElement(By.CssSelector(".a-badge > .a-badge-label > .a-icon.a-icon-star-small > .a-badge-text")).Text;
            p.valoracion = float.Parse(v.Split(' ')[0]);
        }
        catch (Exception)
        {

        }        
        
        IWebElement datosPrecio = producto.FindElement(By.CssSelector("div[data-cy='title-recipe']"));
        p.oferta = producto.FindElements(By.CssSelector(".a-badge-label-inner.a-text-ellipsis .a-badge-text")).Count > 0;
        p.categoria = "pañales dodot talla 4";
        p.precio_antes_oferta = float.Parse(producto.FindElement(By.CssSelector("")).Text);
        
        return p;
    }
}