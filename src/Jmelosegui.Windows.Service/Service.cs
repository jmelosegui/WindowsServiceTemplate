using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Jmelosegui.Windows.Service.Native;

namespace Jmelosegui.Windows.Service
{
    public class Service : ServiceBase
    {
        #region Fields
        private readonly ILogger logger;
        private static System.Timers.Timer timer;
        #endregion

        //TODO: Setup more suitable names here
        private const string serviceName = "WindowsService";
        private const string serviceDisplayName = "Windows Service Template";
        private const string serviceDescription = "Windows Service Description";
        
        #region Contructors
        public Service() : this(new NullLogger())
        {

        }

        public Service(ILogger logger)
        {
            this.logger = logger;
        }
        #endregion

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;

            try
            {
                //Do jour job here.
                logger.Info(DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                if (timer != null)
                {
                    timer.Enabled = true;
                }
            }
        }

        #region Methods
        protected override void OnStart(string[] args)
        {
            logger.Info("Starting the Service");

            int interval;
            if (!int.TryParse(ConfigurationManager.AppSettings["Interval"], out interval))
            {
                interval = 1; //1 second
            }

            timer = new System.Timers.Timer(interval * 1000); // time in seconds
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            logger.Info("The service will be Stopped");

            timer.Enabled = false;
            timer.Elapsed -= OnTimedEvent;
            timer.Dispose();
            timer = null;

            base.OnStop();

            logger.Info("The service has been Stopped");
        }

        static void Usage()
        {
            Console.WriteLine("\r\nUsage: WindowsService [-I | -U  | -D | -<help | h>]");
            Console.WriteLine("\tI - To install the service");
            Console.WriteLine("\tU - To uninstall the service");
            Console.WriteLine("\tD - Goes into debug mode (running as a console application).");
            Console.WriteLine("\thelp - Shows the help.");
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var arg = args[0];
                arg = arg.ToUpper();

                if (arg == "/I" || arg == "-I")
                {
                    try
                    {
                        Install();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\r\nThe service was successfully installed.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not install the service. {0}{1}{2}{3}",
                            Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace);
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }
                else if (arg == "/U" || arg == "-U")
                {
                    try
                    {
                        Uninstall();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\r\nThe service was successfully uninstalled.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not uninstall the service. {0}{1}{2}{3}",
                            Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
#if DEBUG
                else if (arg == "/D" || arg == "-D")
                {
                    var service = new Service();
                    service.OnStart(null);

                    do
                    {
                        Thread.Sleep(1000);
                    } while (true);
                }
#endif
                else
                {
                    Usage();
                }
            }
            else
            {
                var servicesToRun = new ServiceBase[] { new Service() };
                Run(servicesToRun);
            }
        }

 
        private static void Install()
        {
            string servicePath = AppDomain.CurrentDomain.BaseDirectory + typeof(Service).Assembly.ManifestModule.Name;

            AdvApi32.CreateService(servicePath, serviceName, serviceDisplayName, serviceDescription, null, null);
        }

        private static void Uninstall()
        {
            AdvApi32.DeleteService(serviceName);
        }

        #endregion
    }
}
