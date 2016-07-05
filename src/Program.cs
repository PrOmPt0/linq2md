using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace linq2md {
    class Program {
        public static void Main(string[] args) {
            if (args.Length!=1){
                PrintUseage();
                return;
            }
            //try{
                // Parse Config
                var md = Path.Combine(Environment.CurrentDirectory, args[0]);

                // Run
                Run(md);
                
                Console.WriteLine("Done, Press Any Key to Quit.");
                Console.Read();
            //}catch(Exception e){
            //    PrintUseage();
            //    Console.WriteLine(e.Message);
            //}
        }

        private static void Run(string md){
            
            var path = Path.GetDirectoryName(md);
            var filaName = Path.GetFileNameWithoutExtension(md);
            var output = Path.Combine(path, filaName+".html");
            var html = md.Parse().Emit();
            File.WriteAllText(output,html);

            var libs = GetLibs();
            var assembly=Assembly.GetExecutingAssembly();
            foreach (var lib in libs){
                var destFile=Path.Combine(path, Path.GetFileName(lib.Item1));
                var stream=assembly.GetManifestResourceStream(lib.Item2);
                using (var sr = new StreamReader(stream)){
                    if (File.Exists(destFile)) {
                        File.Delete(destFile);
                    }
                    var outputText = sr.ReadToEnd();
                    File.WriteAllText(destFile,outputText);
                }
            }

            Console.WriteLine("output:{0}",output);
        }

        private static void Test(){

        }
        
        private static void PrintUseage() {

        }

        private static IEnumerable<Tuple<string,string>> GetLibs() {
            var executingAssembly=Assembly.GetExecutingAssembly();
            var aname=executingAssembly.GetName();
            string folderName=aname.Name+".Resources.";

            var resources=executingAssembly.GetManifestResourceNames();
            foreach (var r in resources) {
                if (r.StartsWith(folderName)) {
                    yield return Tuple.Create(r.Replace(folderName, ""), r);
                }
            }
        }
    }
}
