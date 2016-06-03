using System;

namespace linq2md {
    class Program {
        public static void Main(string[] args) {
            if (args.Length!=2){
                PrintUseage();
                return;
            }
            try{
                // Parse Config

                // Run
                Run();
                
                Console.WriteLine("Done, Press Any Key to Quit.");
                Console.Read();
            }catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }

        private static void Run(){

        }

        private static void Test(){

        }
        
        private static void PrintUseage() {

        }
    }
}
