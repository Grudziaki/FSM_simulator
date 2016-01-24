using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace FSM_Simulator
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        private static Form1 form;
        [STAThread]
        static void Main(string[] args)
        {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                form = new Form1();

                if (args.Count() != 0 && File.Exists(args[0]))
                {
                    form.load_from_xml(args[0]);

                    Application.Run(form);
                }
            }
    }
}
