namespace PassTwo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Util util = new Util();

            if (args.Length == 0 ) 
            {
                Usage();
                return;
            }
            if (args.Length > 0) 
            {
                util.SetVaultName(args[0]);
            }

            util.Setup();
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: PassTwo filename");
            Console.WriteLine("Purpose: Password Manager");
        }
    }
}
