using System.Security.Cryptography;
using System.Text;

namespace PassTwo
{
    internal class Util
    {
        private string SaltOne = "jJ3LukLw";
        private string SaltTwo = "E5W3yRvz";
        private byte[] SaltThree = new byte[32];
        private byte[] SaltFour = new byte[16] { 210, 67, 162, 175, 239, 47, 218, 77, 252, 234, 50, 127, 135, 12, 114, 224 };
        private string FileNameOne = String.Empty;
        private List<Account> Accounts = new List<Account>();

        public void SetFileName(string name)
        {
            FileNameOne = name;
        }

        private byte[] HashThree(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var salted = String.Concat(salt, password);
                byte[] bytes = Encoding.UTF8.GetBytes(salted);
                return sha256.ComputeHash(bytes);
            }
        }

        private byte[] HashFour(string password)
        {
            return HashThree(password, SaltOne).Reverse().ToArray();
        }

        private byte[] HashFive(string password)
        {
            return HashThree(password, SaltTwo).Reverse().ToArray();
        }

        private void SaveTwo(byte[] value)
        {
            byte[] block = new byte[512];
            RandomNumberGenerator.Create().GetBytes(block);
            Array.Clear(block, 0, 8);
            Array.Copy(value, 0, block, 8, 32);
            File.WriteAllBytes(FileNameOne, block);
        }

        internal void Setup()
        {
            RandomNumberGenerator.Create().GetBytes(SaltThree);
            Console.WriteLine("Password?");
            string pass = Console.ReadLine();
            if (String.IsNullOrEmpty(pass))
            {
                return;
            }
            if (pass.Length < 10)
            {
                Console.WriteLine("Too short");
                return;
            }
            if (pass.Length > 100)
            {
                Console.WriteLine("Too long");
                return;
            }
            byte[] hash = HashFour(Repeat(pass, 3));

            if (File.Exists(FileNameOne))
            {
                byte[] mac = GetMac(FileNameOne);
                if (mac.SequenceEqual(hash))
                {
                    Console.Clear();
                    GetSaltThree(pass);
                    Job();
                }
            }
            else
            {
                SaveTwo(hash);
            }
        }

        private byte[] GetMac(string fileName)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    stream.Position = 8;
                    using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        return br.ReadBytes(32);
                    }
                }
            }
            catch { }
            return new byte[1];
        }

        private void Job()
        {
            while (true)
            {
                Console.WriteLine("Get List Add Delete Quit");
                Console.WriteLine("Command?");
                string command = Console.ReadLine().Trim();
                if (String.IsNullOrEmpty(command))
                {
                    break;
                }

                string op = GetFirstLetter(command).ToUpper();
                string[] arg = command.Split();

                if (op == "Q")
                {
                    break;
                }

                switch (op)
                {
                    case "A":
                        Add();
                        break;
                    case "D":
                        if (arg.Length > 1)
                        { 
                            DeleteName(arg[1]);
                        }
                        break;
                    case "G":
                        if (arg.Length > 1)
                        {
                            GetName(arg[1]);
                        }
                        break;
                    case "L":
                        ShowList();
                        break;
                }
            }
        }

        private string GetFirstLetter(string a)
        {
            if (a.Length > 0)
            {
                return a.Substring(0, 1);
            }

            return String.Empty;
        }

        private void GetSaltThree(string text)
        {
            byte[] array = HashFive(Repeat(text, 3));
            Array.Copy(array, SaltThree, 32);
        }

        private void Add()
        {
            var a = new Account();
            Console.Write("Service? ");
            a.Service = Console.ReadLine();
            Console.Write("User? ");
            a.User = Console.ReadLine();
            Console.Write("Password? ");
            a.Value = Console.ReadLine();
            Accounts.Add(a);
            WriteFile(FileNameOne);
        }

        private void ShowList()
        {
            ReadFile(FileNameOne);
            List<string> list = new List<string>();
            foreach (var act in Accounts)
            {
                list.Add(act.Service);
            }
            list.Sort();
            foreach (var svc in list)
            {
                Console.WriteLine(svc);
            }
            Console.WriteLine();
        }

        private void GetName(string name)
        {
            ReadFile(FileNameOne);
            foreach (var act in Accounts)
            {
                if (act.Service.StartsWith(name))
                {
                    act.Print();
                    break;
                }
            }
            Console.WriteLine();
        }

        private void DeleteName(string name)
        {
            ReadFile(FileNameOne);
            foreach (var act in Accounts)
            {
                if (act.Service.StartsWith(name) && name.Length > 3)
                {
                    Console.WriteLine("Are you sure?");
                    string result = Console.ReadLine();
                    if (IsYes(result))
                    {
                        Accounts.Remove(act);
                        WriteFile(FileNameOne);
                    }
                    break;
                }
            }
            Console.WriteLine();
        }

        private bool IsYes(string a)
        {
            a = a.Trim().ToUpper();
            if (GetFirstLetter(a) == "Y")
            {
                return true;
            }
            return false;
        }

        private void WriteFile(string fileName)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Open))
                {
                    using (var bw = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        byte[] bytes = EncryptSix();
                        bw.Write(Accounts.Count);
                        bw.Write(bytes.Length);
                        bw.Seek(40, SeekOrigin.Begin);
                        bw.Write(bytes);
                    }
                }
            } 
            catch { }
        }

        private byte[] EncryptSix()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Accounts.Count; ++i)
            {
                sb.AppendLine(Accounts[i].GetString());
            }
            return EncryptTwo(sb.ToString());
        }

        private string DecryptSix(byte[] a)
        {
            return DecryptTwo(a);
        }

        private void ReadFile(string fileName)
        {
            try
            {
                Accounts.Clear();
                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        int length = br.ReadInt32();
                        int numBytes = br.ReadInt32();
                        br.ReadBytes(32);
                        byte[] bytes = br.ReadBytes(numBytes);
                        string pt = DecryptSix(bytes);
                        pt = pt.Replace("\r", "");
                        string[] lines = pt.Split('\n');
                        foreach (var item in lines)
                        {
                            string[] parts = item.Split('|');
                            if (parts.Length == 4)
                            {
                                Accounts.Add(new Account(parts[0], parts[1], parts[2]));
                            }
                        }
                    }
                }
            } 
            catch { }
        }

        private string Repeat(string text, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(text);
            }

            return sb.ToString();
        }

        private byte[] EncryptTwo(string text)
        {
            return EncryptFive(SaltThree, text);
        }

        private string DecryptTwo(byte[] bytes)
        {
            return DecryptFive(SaltThree, bytes);
        }

        private byte[] EncryptFive(byte[] key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            Array.Copy(SaltFour, iv, 16);
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return array;
        }

        private string DecryptFive(byte[] key, byte[] cipherText)
        {
            byte[] iv = new byte[16];

            Array.Copy(SaltFour, iv, 16);
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
