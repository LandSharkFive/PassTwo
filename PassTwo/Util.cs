using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PassTwo
{
    internal class Util
    {
        private byte[] SaltThree = new byte[32];
        private byte[] SaltFour = new byte[16] { 210, 67, 162, 175, 239, 47, 218, 77, 252, 234, 50, 127, 135, 12, 114, 224 };
        private string VaultName = String.Empty;
        private List<Account> Accounts = new List<Account>();

        public void SetVaultName(string name)
        {
            VaultName = name;
        }

        private byte[] HashFour(string text)
        {
            try
            {
                byte[] array = Encoding.UTF8.GetBytes(Repeat(text, 3));
                return new Rfc2898DeriveBytes(array, SaltFour, 1000, HashAlgorithmName.SHA256).GetBytes(32);
            }
            catch { }
            return new byte[0];
        }

        private void SaveTwo(byte[] value)
        {
            byte[] block = new byte[512];
            RandomNumberGenerator.Create().GetBytes(block);
            Array.Clear(block, 0, 8);
            Array.Copy(value, 0, block, 8, 32);
            File.WriteAllBytes(VaultName, block);
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
            byte[] hash = HashFour(pass);
            if (hash.Length < 32)
            {
                return;
            }

            if (File.Exists(VaultName))
            {
                byte[] mac = GetMac(VaultName);
                if (mac.Length < 32)
                {
                    return;
                }
                if (mac.SequenceEqual(hash))
                {
                    Console.Clear();
                    Array.Copy(hash, SaltThree, 32);
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
            return new byte[0];
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

        private void Add()
        {
            var a = new Account();
            Console.Write("Service? ");
            string service = Console.ReadLine();
            if (String.IsNullOrEmpty(service) || service.Length < 3)
            {
                Console.WriteLine("Too short");
                return;
            }
            a.Service = service;
            Console.Write("User? ");
            a.User = Console.ReadLine();
            Console.Write("Password? ");
            a.Value = Console.ReadLine();
            Accounts.Add(a);
            Accounts = Accounts.OrderBy(x => x.Service).ToList();
            WriteFile(VaultName);
        }

        private void ShowList()
        {
            ReadFile(VaultName);
            List<string> list = new List<string>();
            foreach (var act in Accounts)
            {
                Console.WriteLine(act.Service);
            }
            Console.WriteLine();
        }

        private void GetName(string name)
        {
            ReadFile(VaultName);
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
            ReadFile(VaultName);
            foreach (var act in Accounts)
            {
                if (act.Service.StartsWith(name) && name.Length > 0)
                {
                    Console.WriteLine("Are you sure?");
                    string result = Console.ReadLine();
                    if (IsYes(result))
                    {
                        Accounts.Remove(act);
                        WriteFile(VaultName);
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
            return EncryptTwo(JsonSerializer.Serialize(Accounts));
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
                        var list = JsonSerializer.Deserialize<List<Account>>(pt);
                        foreach (var item in list)
                        {
                            Accounts.Add(item);
                        }
                    }
                }
            }
            catch { }
        }

        private string Repeat(string text, int count)
        {
            return new StringBuilder().Insert(0, text, count).ToString();
        }

        private byte[] EncryptTwo(string text)
        {
            return EncryptFive(SaltThree, SaltFour, text);
        }

        private string DecryptTwo(byte[] bytes)
        {
            return DecryptFive(SaltThree, SaltFour, bytes);
        }

        private byte[] EncryptFive(byte[] key, byte[] iv, string plainText)
        {
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

        private string DecryptFive(byte[] key, byte[] iv, byte[] cipherText)
        {
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
