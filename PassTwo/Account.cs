using System.Text;

namespace PassTwo
{
    internal class Account
    {
        public string Service { get; set; }
        public string User { get; set; }
        public string Value { get; set; }

        public Account() 
        { 
            this.Service = String.Empty;
            this.User = String.Empty;
            this.Value = String.Empty;
        }

        public void Print()
        {
            Console.WriteLine(Service);
            Console.WriteLine(User);
            Console.WriteLine(Value);
            Console.WriteLine();
        }

    }
}
