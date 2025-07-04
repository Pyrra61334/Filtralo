using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;



namespace Filtralo
{
    public partial class Form2 : Form
    {
        string version = "1.4";
        const float SegundosPorNumero = 5f;
        int intervalo_ms = 30;
        int ultimohecho = -1;
        bool Salir = false;
        public CancellationTokenSource tokenCancel;
        public DialogResult MBresult;
        public Task<bool> MBCerrado;
        public DateTime NowTimeDef;
        public DateTime NowTimei;
        public DateTime NowTimef;
        //Console.WriteLine($"\nTotal de numeros: {n} \t" + "Tiempo Estimado: " + Leer_Archivo.ConvertToMinutes(Convert.ToInt32(6.22 * n + 13)));

        public Form2()
        {
            InitializeComponent();
            var NowTimeh = DateTime.Now;
            if (NowTimeh.Month == 7 && NowTimeh.Day == 01)//suprise
            {
                EnviarLog();
                //Application.Run(new Filtralo.Form1($@"{Form2.Raiz}\Listas\No Abrir.mp4"));
            }

            try { User.EliminarPrimerEspacioVacioFiltro(""); } catch { }
            lab_Estado2.Text= "En espera";
            int Error;
            if ((Error = User.LeerInfotxt(""))!= 0)
            {
                
                switch (Error)
                {
                    case 1:
                        MessageBox.Show("Verifica que el archivo ese cerrado antes de ingresarlo al programa");
                        break;

                    case 2:
                        MessageBox.Show("Verifica que el archivo \"Lista de numeros\" exista en la carpeta de Listas del programa");
                        break;
                    case 3:
                        lab_Estado2.Text = "No hay ningun numero para filtrar";
                        break;

                }
            }
            for(int i = 0; i<5; i++)
            {
                //dgv_Usuarios.Rows.Add();
                //dgv_Usuarios.Rows[i].Cells[0].Value = $"{i + 1}";
                User.Filtros.Add(false);
                Tokens.Add(tokenCancel);
            }

            string directoryPath = Path.Combine(Form2.Raiz, "Listas");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static int Buscados;
        public static string Raiz = Application.StartupPath;

        public static List<CancellationTokenSource> Tokens = new List<CancellationTokenSource>();


        private async void btn_Reset_Click(int n)
        {
            
            if (Program.Usuarios.Count > 0)//Verifica si se ingresaron numeros para buscar, de otra forma, alerta al usuario
            {
                if (Checkbtn_ResetnBackColor(n+1) != Color.Yellow)
                {

                    if (NowTimei == NowTimeDef)//Hora a la que se abre el primer filtro
                    {
                        NowTimei = DateTime.Now;
                        Log.NowTimei = DateTime.Now;
                    }

                    btn_ResetnBackColor(n + 1, 3);
                    if (User.Filtros[n] == true)//Si ya habia un filtro abierto
                    {
                        Tokens[n].Cancel();
                        await Task.Run(() =>//Este nuevo filtro no empieza hasta que el filtro anterior se marque como falso
                        {
                            while (User.Filtros[n] == true) ;
                        });
                        btn_ResetnBackColor(n + 1, 3);
                    }

                    Tokens[n] = new CancellationTokenSource();
                    User.Filtros[n] = true;
                    var token = Tokens[n].Token;

                    await Task.Run(() =>
                    {
                        labEstado2("Buscando");
                        int posicion = 0;
                        int reintenta = 0;
                        try
                        {
                            HoraFinalEstimada();
                            IWebDriver driver = new FirefoxDriver();
                            driver.Manage().Window.Minimize();
                            posicion = 0;
                            double num = 0;
                            btn_ResetnBackColor(n + 1, 0);

                            while (ultimohecho + 1 < User.NumeroDeUsuarios)
                            {
                                try
                                {
                                    btn_ResetnBackColor(n + 1, 0);
                                    if (reintenta == 0)//primer intento
                                    {
                                        labEstado2($"Buscando {User.NumeroDeUsuarios - ultimohecho} restantes");
                                        ultimohecho++;
                                        posicion = ultimohecho;
                                        Log.CantidadNumeros = ultimohecho;
                                        Program.Usuarios[posicion].Compañia = "P";
                                        Program.Usuarios[posicion].Localidad = "P";
                                        num = Program.Usuarios[ultimohecho].Celular;
                                        CrearFilaDgv_Usuarios();

                                        dgv_Usuarios.Rows[posicion].Cells[0].Value = $"{posicion + 1}";
                                        dgv_Usuarios.Rows[posicion].Cells[1].Value = num;
                                        dgv_Usuarios.Rows[posicion].Cells[2].Value = "Buscando";
                                        dgv_Usuarios.Rows[posicion].Cells[3].Value = "Buscando";
                                    }
                                    else// x reintento
                                    {

                                        labEstado2($"Buscando {User.NumeroDeUsuarios - ultimohecho+1} restantes");
                                        if (reintenta == 4) //Numero maximo de reintentos superado
                                        {
                                            reintenta = 0;
                                            ultimohecho++;
                                            posicion = ultimohecho;
                                            Program.Usuarios[posicion].Compañia = "P";
                                            Program.Usuarios[posicion].Localidad = "P";
                                            num = Program.Usuarios[ultimohecho].Celular;
                                            CrearFilaDgv_Usuarios();

                                            dgv_Usuarios.Rows[posicion].Cells[0].Value = $"{posicion + 1}";
                                            dgv_Usuarios.Rows[posicion].Cells[1].Value = num;
                                            dgv_Usuarios.Rows[posicion].Cells[2].Value = "Buscando";
                                            dgv_Usuarios.Rows[posicion].Cells[3].Value = "Buscando";
                                        }
                                        else dgv_Usuarios.Rows[posicion].Cells[2].Value = $"Reintento: {reintenta}";

                                    }

                                    btn_ResetnBackColor(n + 1, 0);
                                    try
                                    {
                                        driver.Url = "https://sns.ift.org.mx:8081/sns-frontend/consulta-numeracion/numeracion-geografica.xhtml";
                                    }
                                    catch//Si no se puede ir a la url, o el navegdador o la consola fallaron y ambos se reinician
                                    {
                                        btn_ResetnBackColor(n + 1, 0);
                                        driver.Quit();
                                        driver = new FirefoxDriver();
                                        driver.Manage().Window.Minimize();
                                        driver.Url = "https://sns.ift.org.mx:8081/sns-frontend/consulta-numeracion/numeracion-geografica.xhtml";
                                    }

                                    if (num > 1000000000 && num < 10000000000)
                                    {
                                        driver.FindElement(By.XPath("//*[@class='ui-inputfield ui-inputtext ui-widget ui-state-default ui-corner-all']")).SendKeys(Convert.ToString(num));
                                        try
                                        {
                                            DarClickSearch(driver, intervalo_ms, 0);
                                            try//Comprueba si estan disponibles la compañia y la localidad
                                            {
                                                if(RegistrarCL(driver, intervalo_ms, posicion, 0)==true) reintenta = 0;
                                                else//Si fallo en encontrar compañia y localidad
                                                {
                                                    try//Comprueba si salio el aviso de "Numero no existe"
                                                    {
                                                        string a = driver.FindElement(By.XPath("/html/body/div[2]/div/div[4]/div/form/div[1]/div/div[1]/div/div[1]/div/ul/li/span[1]")).Text.ToString();
                                                        Program.Usuarios[posicion].Compañia = "NE";
                                                        Program.Usuarios[posicion].Localidad = "NE";
                                                        reintenta = 0;
                                                    }
                                                    catch//Fallo la busqueda, volver a intentar
                                                    {
                                                        btn_ResetnBackColor(n + 1, 1);
                                                        reintenta++;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                reintenta++;
                                            }
                                        }
                                        catch
                                        {
                                            btn_ResetnBackColor(n + 1, 1);
                                            reintenta++;
                                        }
                                    }
                                    else//No es un numero de 10 digitos por lo que sin buscar, se sabe que no existe
                                    {
                                        Program.Usuarios[posicion].Compañia = "NE";
                                        Program.Usuarios[posicion].Localidad = "NE";
                                        reintenta = 0;

                                    }
                                    dgv_Usuarios.Rows[posicion].Cells[0].Value = $"{posicion + 1}";
                                    dgv_Usuarios.Rows[posicion].Cells[1].Value = num;
                                    dgv_Usuarios.Rows[posicion].Cells[2].Value = Program.Usuarios[posicion].Compañia;
                                    dgv_Usuarios.Rows[posicion].Cells[3].Value = Program.Usuarios[posicion].Localidad;
                                }
                                catch
                                {
                                    dgv_Usuarios.Rows[posicion].Cells[0].Value = $"{posicion + 1}";
                                    dgv_Usuarios.Rows[posicion].Cells[1].Value = Program.Usuarios[ultimohecho].Celular;
                                    dgv_Usuarios.Rows[posicion].Cells[2].Value = Program.Usuarios[posicion].Compañia;
                                    dgv_Usuarios.Rows[posicion].Cells[3].Value = Program.Usuarios[posicion].Localidad;
                                    reintenta++;
                                    btn_ResetnBackColor(n + 1, 1);
                                    Thread.Sleep(500);
                                }
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }
                            }

                            try
                            {
                                driver.Quit();
                            }
                            catch
                            {

                            }
                            btn_ResetnBackColor(n + 1, 2);
                            User.Filtros[n] = false;


                            if (Salir == true && User.Filtros[0] == false && User.Filtros[1] == false && User.Filtros[2] == false && User.Filtros[3] == false && User.Filtros[4] == false)
                            {
                                
                                Log.CantidadNumeros = User.NumeroDeUsuarios; //Guatdando info en log

                                if (token.IsCancellationRequested)//Se apreto el boton de guardar y salir(y Se guardo en GuardarYSalir()) ó Se le dio click a salir sin guardar//
                                {
                                    MessageBox.Show("Gracias por confiar en Erick Industries (◉⩊◉) bye bye");
                                    Application.Exit();
                                }
                                if (posicion + 1 >= User.NumeroDeUsuarios)//Se completo la lista de numeros a buscar//
                                {
                                    User.Filtros[n] = true;
                                    labEstado2("Guardando");
                                    User.GuardarInfo(User.NumeroDeUsuarios);
                                    labEstado2("Lista completa guardada (◉⩊◉), cerrando ventanas");
                                    MessageBox.Show("Gracias por confiar en Erick Industries (◉⩊◉) Bye Bye");
                                    Application.Exit();
                                }
                            }
                        }
                        catch
                        {
                        }
                    }, token);
                }
            }
            else
            {
                await Task.Run(() =>
                { 
                    int MiliIntervalo = 333;
                    labEstado2("NO HAY NINGUN NUMERO PARA FILTRAR");
                    btn_ResetnBackColor(n + 1, 1);
                    Thread.Sleep(MiliIntervalo);
                    labEstado2("No hay ningun numero para filtrar");
                    btn_ResetnBackColor(n + 1, 2);
                    Thread.Sleep(MiliIntervalo);
                    labEstado2("NO HAY NINGUN NUMERO PARA FILTRAR");
                    btn_ResetnBackColor(n + 1, 1);
                    Thread.Sleep(MiliIntervalo);
                    labEstado2("No hay ningun numero para filtrar");
                    btn_ResetnBackColor(n + 1, 2);
                    Thread.Sleep(MiliIntervalo);
                    labEstado2("NO HAY NINGUN NUMERO PARA FILTRAR");
                    btn_ResetnBackColor(n + 1, 1);
                    Thread.Sleep(MiliIntervalo);
                    labEstado2("No hay ningun numero para filtrar");
                    btn_ResetnBackColor(n + 1, 2);
                });
            }
        }
        private void btn_Reset1_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(0);
        }
        private void btn_Reset2_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(1);
        }
        private void btn_Reset3_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(2);
        }
        private void btn_Reset4_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(3);
        }
        private void btn_Reset5_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(4);
        }

        private async void Form2_DragDrop(object sender, DragEventArgs e)
        {
            await Task.Run(() =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                    foreach (string file in files)
                    {

                        if (User.EliminarPrimerEspacioVacioFiltro(file) == true)
                        {

                            int Error;
                            if ((Error = User.LeerInfotxt(file)) == 0)
                            {
                                labEstado2($"{User.NumeroDeUsuarios} numeros en espera");
                                User.SeArrastroArchivo = true;
                            }
                            else
                            {
                                switch (Error)
                                {
                                    case 1:
                                        MessageBox.Show("Verifica que el archivo ese cerrado antes de ingresarlo al programa");
                                        break;

                                    case 2:
                                        MessageBox.Show("Verifica que el archivo \"Lista de numeros\" exista en la carpeta de Listas del programa");
                                        break;
                                    case 3:
                                        labEstado2($"No se leyo ningun numero extra, aun asi hay {User.NumeroDeUsuarios} numeros en espera");
                                        break;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Verifica que el archivo ese cerrado antes de ingresarlo al programa");
                        }

                        int SegundosEstimados = Convert.ToInt32((SegundosPorNumero* User.NumeroDeUsuarios) / 5 + 13);
                        lab_HFE.Invoke((MethodInvoker)(() => lab_HFE.Text = $"{SegundosEstimados/3600}h {(SegundosEstimados%3600)/60}m (5 Filtros activos)"));
                    }
                }
            });
        }
        private void Form2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private async void btn_SalirSinGuardar_Click(object sender, EventArgs e)
        {
            Log.CantidadFiltros = NumeroFiltrosActivos();//Guardando info en log
            Salir = true;
            lab_Estado2.Text = "Cerrando...";
            await Task.Run(() =>
            {
                EnviarLog();
                if (User.Filtros[0] == true)
                {
                    Tokens[0].Cancel();
                }
                if (User.Filtros[1] == true)
                {
                    Tokens[1].Cancel();
                }
                if (User.Filtros[2] == true)
                {
                    Tokens[2].Cancel();
                }
                if (User.Filtros[3] == true)
                {
                    Tokens[3].Cancel();
                }
                if (User.Filtros[4] == true)
                {
                    Tokens[4].Cancel();
                }
                if (User.Filtros[0] == false && User.Filtros[1] == false && User.Filtros[2] == false && User.Filtros[3] == false && User.Filtros[4] == false)
                {
                    Application.Exit();                
                }
            });
            Thread.Sleep(10000);
        }
        private async void btn_SaveAndQui_Click(object sender, EventArgs e)
        {
            Log.CantidadFiltros = NumeroFiltrosActivos();//Guardando info en log
            Salir = true;
            await Task.Run(() =>
            {
                EnviarLog();
                if (User.Filtros[0] == true)
                {
                    Tokens[0].Cancel();
                }
                if (User.Filtros[1] == true)
                {
                    Tokens[1].Cancel();
                }
                if (User.Filtros[2] == true)
                {
                    Tokens[2].Cancel();
                }
                if (User.Filtros[3] == true)
                {
                    Tokens[3].Cancel();
                }
                if (User.Filtros[4] == true)
                {
                    Tokens[4].Cancel();
                }

                labEstado2("Guardando");
                Buscados = User.GuardarInfo(0);
                labEstado2("Guardado (◉⩊◉), cerrando ventanas");

                //if(Buscados>-1 && Buscados < 5) { Buscados = 5; }
                if (Buscados == -1)
                {
                    MBresult = MessageBox.Show("Algo salio mal al guardar los datos");
                }
                else
                {
                    if (Buscados == -2)
                    {
                        MBresult = MessageBox.Show("Algo salio mal al actualizar las listas");
                    }
                    else
                    {
                        MBresult = DialogResult.OK;
                    }
                }
                
                
            });

            await Task.Run(() =>
            {
                switch (MBresult)
                {
                    case DialogResult.OK:
                    case DialogResult.Yes:
                        MBCerrado = Definir(true);
                        break;

                    case DialogResult.No:
                    case DialogResult.Abort:
                        MBCerrado = Definir(false);
                        break;

                    default:
                        MBCerrado = Definir(true);
                        break;
                }
            });

            await MBCerrado;
        }
        void labEstado2(string texto)
        {
            lab_Estado2.Invoke((MethodInvoker)(() => lab_Estado2.Text = texto));//Detiene momentaneamente el hilo que la llama y actualiza el label//
        }
        async void btn_ResetnBackColor(int boton, int color)//0 = Verde, 1 = Rojo, 2 = Light Gray, 3 = Amarillo//
        {
            await Task.Run(() =>
            {
                switch (boton)
                {
                    case 1:
                        switch (color)
                        {
                            case 0:

                                btn_Reset1.BackColor = Color.Green;
                                break;

                            case 1:
                                btn_Reset1.BackColor = Color.Red;
                                break;

                            case 2:
                                btn_Reset1.BackColor = Color.LightGray;
                                break;

                            case 3:
                                btn_Reset1.BackColor = Color.Yellow;
                                break;
                        }
                        break;

                    case 2:
                        switch (color)
                        {
                            case 0:

                                btn_Reset2.BackColor = Color.Green;
                                break;

                            case 1:
                                btn_Reset2.BackColor = Color.Red;
                                break;

                            case 2:
                                btn_Reset2.BackColor = Color.LightGray;
                                break;

                            case 3:
                                btn_Reset2.BackColor = Color.Yellow;
                                break;
                        }
                        break;

                    case 3:
                        switch (color)
                        {
                            case 0:

                                btn_Reset3.BackColor = Color.Green;
                                break;

                            case 1:
                                btn_Reset3.BackColor = Color.Red;
                                break;

                            case 2:
                                btn_Reset3.BackColor = Color.LightGray;
                                break;

                            case 3:
                                btn_Reset3.BackColor = Color.Yellow;
                                break;
                        }
                        break;

                    case 4:
                        switch (color)
                        {
                            case 0:

                                btn_Reset4.BackColor = Color.Green;
                                break;

                            case 1:
                                btn_Reset4.BackColor = Color.Red;
                                break;

                            case 2:
                                btn_Reset4.BackColor = Color.LightGray;
                                break;

                            case 3:
                                btn_Reset4.BackColor = Color.Yellow;
                                break;
                        }
                        break;

                    case 5:
                        switch (color)
                        {
                            case 0:

                                btn_Reset5.BackColor = Color.Green;
                                break;

                            case 1:
                                btn_Reset5.BackColor = Color.Red;
                                break;

                            case 2:
                                btn_Reset5.BackColor = Color.LightGray;
                                break;

                            case 3:
                                btn_Reset5.BackColor = Color.Yellow;
                                break;
                        }
                        break;
                }
            });

            
        }
        Color Checkbtn_ResetnBackColor(int boton)
        {
            switch (boton)
            {
                case 1:
                    return btn_Reset1.BackColor;

                case 2:
                    return btn_Reset2.BackColor;

                case 3:
                    return btn_Reset3.BackColor;

                case 4:
                    return btn_Reset4.BackColor;

                case 5:
                    return btn_Reset5.BackColor;
            }
            return btn_Reset5.BackColor;
        }
        void CrearFilaDgv_Usuarios()
        {
            dgv_Usuarios.Invoke((MethodInvoker)(() => dgv_Usuarios.Rows.Add()));//Detiene momentaneamente el hilo que la llama y actualiza el label//
        }
        void DarClickSearch(IWebDriver driver, int intervalo, int contador)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                IWebElement search = wait.Until(driver =>
                    driver.FindElement(By.XPath("//*[@id='FORM_myform:BTN_publicSearch']")).Enabled
                        ? driver.FindElement(By.XPath("//*[@id='FORM_myform:BTN_publicSearch']"))
                        : null);
                search.Click();
            }
            catch
            {
                contador++;
                Thread.Sleep(intervalo);
                if (contador < 500/intervalo_ms) DarClickSearch(driver, intervalo, contador);
            }

        }
        bool RegistrarCL(IWebDriver driver, int intervalo, int posicion, int contador)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                _ = wait.Until(drv => drv.FindElement(By.XPath("//*[@id='FORM_myform:TBL_numberInfoTable_content']/div[7]/div[2]")));
                Program.Usuarios[posicion].Compañia = driver.FindElement(By.XPath("/ html / body / div[2] / div / div[4] / div / form / div[3] / div / div / div[2] / div[7] / div[2]")).Text.ToString();
                Program.Usuarios[posicion].Localidad = driver.FindElement(By.XPath("//*[@id='FORM_myform:TBL_numberInfoTable_content']/div[3]/div[2]")).Text.ToString();
                Program.Usuarios[posicion].Buscado = true;
                return true;

            }
            catch
            {
                contador++;
                while (contador < 500/intervalo_ms)
                {
                    try
                    {
                        Thread.Sleep(intervalo);
                        Program.Usuarios[posicion].Compañia = driver.FindElement(By.XPath("/ html / body / div[2] / div / div[4] / div / form / div[3] / div / div / div[2] / div[7] / div[2]")).Text.ToString();
                        Program.Usuarios[posicion].Localidad = driver.FindElement(By.XPath("//*[@id='FORM_myform:TBL_numberInfoTable_content']/div[3]/div[2]")).Text.ToString();
                        Program.Usuarios[posicion].Buscado = true;
                        return true;
                    }
                    catch
                    {
                        contador++;
                    }
                }
                return false;
            }
           
        }
        private async void HoraFinalEstimada()
        {
            await Task.Run(() =>
            {
                double TPPB = 7;
                Log.Registro = 0;//Pa saber cuando borrar datos
                Log.RondasLog = 0;// Pa'nobrar archivo

                string line = "";
                int fila = 1;
                try
                {
                    using (StreamReader file = new StreamReader($@"{Form2.Raiz}\Listas\LogControl.txt"))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            //Console.WriteLine(line);
                            try
                            {
                                switch (fila)
                                {
                                    case 1:
                                        Log.Registro = System.Convert.ToInt32(line);
                                        break;

                                    case 2:
                                        Log.RondasLog = System.Convert.ToInt32(line);
                                        break;

                                    case 3:
                                        TPPB = System.Convert.ToDouble(line);
                                        break;
                                    default:

                                        break;
                                }
                            }
                            catch
                            {
                            }
                            fila++;
                        }
                    }//Leyendo Archivo de Control//
                }
                catch
                {
                    using (System.IO.StreamWriter file =
                           new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
                    {
                        file.WriteLine($"{Log.Registro}");
                    }
                    using (System.IO.StreamWriter file =
                           new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
                    {
                        file.WriteLine($"{Log.RondasLog}");
                    }
                    using (System.IO.StreamWriter file =
                           new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
                    {
                        file.WriteLine($"{TPPB}");
                    }
                }
                int NumFiltros = User.Filtros.Count();
                int FiltrosActivos = 0;
                for(int i = 0; i<NumFiltros; i++)
                {
                    if (User.Filtros[i] == true) FiltrosActivos++;
                }
                double TiempoEstimadoSeg = Convert.ToDouble((TPPB*(User.NumeroDeUsuarios-ultimohecho+1)/FiltrosActivos)+13);

                var NowTime = DateTime.Now;
                var FinalTime = NowTime.AddSeconds(TiempoEstimadoSeg);

                //13 seg iniciar navegador, 3 segundos busqueda por numero

                lab_TE.Invoke((MethodInvoker)(() => lab_TE.Text = "Hora final estimada:"));
                lab_HFE.Invoke((MethodInvoker)(() => lab_HFE.Text = FinalTime.ToString()));
            });
        }
        async Task<bool> Definir (bool b)
        {
            if (b == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int NumeroFiltrosActivos()
        {
            int Num = 0;
            for (int c = 0; c < 5; c++)
            {
                if (User.Filtros[c]) Num++;
            }
            return Num;
        }
        void CrearControlLogDefault()
        {
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("Registro:");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine($"{Log.Registro}");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("RondasLog:");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine($"{Log.RondasLog}");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("Timempo por busqueda (s):");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine(7);
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("Puerto para envio correos:");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("587");
            }
        }
        void CrearControlLog(int Registro, int RondasLog, double TPPB, int Puerto)
        {
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("Registro:");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine(Registro);
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("RondasLog:");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine(RondasLog);
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("Timempo por busqueda (s):");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine(TPPB);
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine("Puerto para envio correos:");
            }
            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\LogControl.txt", true))
            {
                file.WriteLine(Puerto);
            }
        }

        private async void EnviarLog()
        {
            
            await Task.Run(() =>
            {
                try
                {
                    Log.Registro = 0;//Pa saber cuando borrar datos
                    Log.RondasLog = 0;// Pa'nobrar archivo
                    Log.TPPB = (((DateTime.Now - Log.NowTimei).TotalSeconds-10) * Log.CantidadFiltros) / ultimohecho;
                    Log.Puerto = 587;

                    string line = "";
                    int fila = 1;

                    try
                    {
                        using (StreamReader file = new StreamReader($@"{Form2.Raiz}\Listas\LogControl.txt"))
                        {
                            while ((line = file.ReadLine()) != null)
                            {
                                try
                                {
                                    switch (fila)
                                    {
                                        case 2:
                                            Log.Registro = System.Convert.ToInt32(line);
                                            break;

                                        case 4:
                                            Log.RondasLog = System.Convert.ToInt32(line);
                                            break;
                                        case 8:
                                            Log.Puerto = System.Convert.ToInt32(line);
                                            break;

                                        default:

                                             break;
                                    }
                                }
                                catch
                                {
                                }
                                fila++;
                            }
                        }//Leyendo Archivo de Control//
                    }
                    catch
                    {
                        CrearControlLogDefault();
                    }//Crear un Control Log default

                    
                    if (Log.Registro < 50)
                    {

                        if (Log.Registro++ == 0)//Si es primer Log.Registro del excel
                        {
                            using (System.IO.StreamWriter file =
                                new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\Log({ (50 * Log.RondasLog) + 1}~{ 50 * (Log.RondasLog + 1)}).csv", true))
                            {
                                file.WriteLine($"Registro, Version, Nombre PC, Numero de consultas, Cantidad de filtros, Tiempo promedio por busqueda, Inicio, Final");
                            }
                        }

                        if (DateTime.Now.Month == 7 && DateTime.Now.Day == 01)//suprise
                        {
                            //Ya lo vio 7u7 
                            //Envio Log
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("enviologs@outlook.com");
                            mail.To.Add("recibologs@outlook.com");
                            //mail.To.Add("recibirlogs@outlook.com");
                            mail.Subject = $"Filtralo Happy Birthday Video Visto 7u7";
                            mail.Body = $"We got'er";
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.outlook.com";
                            smtp.Port = Log.Puerto;
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("enviologs@outlook.com", "Uwu12345");
                            smtp.Send(mail);
                        }
                        else//Si no es cumple de mom
                        {
                            //Regisro, Version, Cantidad de numeros buscados, Cantidad de filtros, Tiempo Busqueda por usuario, Fecha y Hora inicial de busqueda 

                            Log.NowTimef = DateTime.Now;
                            //Escribir Log
                            
                            
                            System.Security.Principal.WindowsIdentity user = System.Security.Principal.WindowsIdentity.GetCurrent();
                            string usuario = user.Name;

                            using (System.IO.StreamWriter file =
                                new System.IO.StreamWriter($@"{Form2.Raiz}\Listas\Log({ (50 * Log.RondasLog) + 1}~{ 50 * (Log.RondasLog + 1)}).csv", true))
                            {
                                file.WriteLine($"{Log.Registro}, {version}, {usuario}, {Log.CantidadNumeros}, {Log.CantidadFiltros}, {Log.TPPB}, {Log.NowTimei}, {Log.NowTimef}");
                            }

                            //Actualizar Control.txt
                            File.Delete($@"{Form2.Raiz}\Listas\LogControl.txt");
                            CrearControlLog(Log.Registro, Log.RondasLog, Log.TPPB, Log.Puerto);
                            

                            //Envio Log
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("enviologs@outlook.com");
                            mail.To.Add("recibologs@outlook.com");
                            //mail.To.Add("recibirlogs@outlook.com");
                            mail.Subject = $"Filtralo {Log.NowTimef}";
                            mail.Body = $"Filtralo v{version}  {Log.NowTimef}";
                            Attachment at = new Attachment($@"{Form2.Raiz}\Listas\Log({ (50 * Log.RondasLog) + 1}~{ 50 * (Log.RondasLog + 1)}).csv");
                            mail.Attachments.Add(at);
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.outlook.com";
                            smtp.Port = Log.Puerto;
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("enviologs@outlook.com", "Uwu12345");
                            smtp.Send(mail);
                        }
                    }
                    else//Si hay mas de 50 registros en un excel
                    {
                        File.Delete($@"{Form2.Raiz}\Listas\Log({ (50 * Log.RondasLog) + 1}~{ 50 * (Log.RondasLog + 1)}).csv");
                        Log.RondasLog++;
                        Log.Registro = 0;

                        //Actualizar Control.txt
                        File.Delete($@"{Form2.Raiz}\Listas\LogControl.txt");
                        CrearControlLog(Log.Registro, Log.RondasLog, Log.TPPB, Log.Puerto);
                        EnviarLog();
                    }

                }
                catch
                {

                }
            });
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
