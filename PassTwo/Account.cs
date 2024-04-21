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

        public Account(string service, string name, string value)
        {
            this.Service = service;
            this.User = name;
            this.Value = value;
        }

        public void Print()
        {
            Console.WriteLine(Service);
            Console.WriteLine(User);
            Console.WriteLine(Value);
            Console.WriteLine();
        }

        public string GetString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Service);
            sb.Append('|');
            sb.Append(User);
            sb.Append('|');
            sb.Append(Value);
            sb.Append('|');
            return sb.ToString();
        }

    }
}
