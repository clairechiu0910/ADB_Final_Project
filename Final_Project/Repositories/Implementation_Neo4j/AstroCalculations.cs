using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class AstroCalculations
    {
        string pythonPath, astroPath, decPath;
        public AstroCalculations(string pythonPath, string astroPath, string decPath)
        {
            // constructor requires three parameters - the path to python.exe, the paths to the two python files.
            this.pythonPath = pythonPath;
            this.astroPath = astroPath;
            this.decPath = decPath;
        }
        private string RunCmd(string cmd, string args)
        {
            //ProcessStartInfo start = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = string.Format("\"{0}\" {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            string result = "";
            string error = "";
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result += reader.ReadToEnd() + "\n";
                }
                using (StreamReader reader = process.StandardError)
                {
                    error += reader.ReadToEnd() + "\n";
                }
            }
            return result;
        }
        public string[] AstroplanCalculations(string userLong, string userLat, string userAlt, string userElev, string targetLong, string targetLat)
        {
            // requires user long,lat,alt,elev and target long targetlat.
            string args = string.Format("{0} {1} {2} {3} {4} {5}", userLong, userLat, userAlt, userElev, targetLong, targetLat);
            string result = RunCmd(astroPath, args);
            string[] results = result.Split(",");
            // Console.WriteLine(results[0]); // start_time
            // Console.WriteLine(results[1]); // end_time
            return results;
        }
        public string Declinationlimit(string argLong, string argLat, string argAlt, string argElev)
        {
            // requires equipment long lat alt and elev
            string args = string.Format("{0} {1} {2} {3}", argLong, argLat, argAlt, argElev);
            string result = RunCmd(decPath, args);
            string decLimit = result;
            return decLimit;
        }
    }
}
