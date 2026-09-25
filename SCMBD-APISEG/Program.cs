using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace SCMBD_APISEG
{
    class Program
    {
        [STAThread]  // ✅ MODO STA — REQUIRED para Clipboard
        static int Main(string[] args)
        {
            bool hayEntrada = args.Length > 0 || !Console.IsInputRedirected == false;

            if (hayEntrada)
            {
                return ProcesarPeticion(args);
            }

            Console.WriteLine("======================================");
            Console.WriteLine("     SCMBD - APISEG  -  MENÚ");
            Console.WriteLine("======================================");
            Console.WriteLine("");
            Console.WriteLine("  1 = Generar cadena cifrada (pantalla)");
            Console.WriteLine("  2 = Procesar JSON (modo servicio)");
            Console.WriteLine("");
            Console.Write("Elige una opción [1-2]: ");

            var tecla = Console.ReadKey(true);
            Console.WriteLine();
            Console.WriteLine("");

            if (tecla.Key == ConsoleKey.D1)
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new FrmGeneradorClaves());
                return 0;
            }
            else if (tecla.Key == ConsoleKey.D2)
            {
                Console.WriteLine("Modo servicio: envía datos por entrada estándar");
                Console.WriteLine("Ejemplo: Get-Content prueba.json | .\\SCMBD-APISEG.exe");
                Console.WriteLine("");
                Console.Write("Presiona ENTER para continuar...");
                Console.ReadLine();
                return ProcesarPeticion(args);
            }

            Console.WriteLine("Opción no válida");
            return 1;
        }

        static int ProcesarPeticion(string[] args)
        {
            try
            {
                string jsonEntrada = args.Length > 0 && !string.IsNullOrWhiteSpace(args[0])
                    ? args[0]
                    : Console.In.ReadToEnd();

                if (string.IsNullOrWhiteSpace(jsonEntrada))
                {
                    DevolverError("No se recibió contenido JSON");
                    return 1;
                }

                var solicitudes = JsonConvert.DeserializeObject<List<Solicitud>>(jsonEntrada);
                if (solicitudes == null || solicitudes.Count == 0)
                {
                    DevolverError("Arreglo de solicitudes vacío");
                    return 1;
                }

                var respuesta = new XElement("RespuestaGlobal");
                foreach (var sol in solicitudes)
                    respuesta.Add(Procesador.Procesar(sol));

                Console.WriteLine(new XDocument(respuesta).ToString());
                return 0;
            }
            catch (Exception ex)
            {
                DevolverError($"{ex.GetType().Name}: {ex.Message}");
                return 1;
            }
        }

        static void DevolverError(string mensaje)
        {
            Console.WriteLine(new XDocument(
                new XElement("RespuestaGlobal",
                    new XElement("Resultados",
                        new XElement("Ejecucion",
                            new XElement("PnEstatus", 1),
                            new XElement("PsMensaje", mensaje)
                        )
                    )
                )
            ).ToString());
        }
    }
}
