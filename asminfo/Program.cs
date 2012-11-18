using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;

namespace asminfo
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.Error.WriteLine("Expected a single argument: path to assembly file");
                return 1;
            }

            var fileName = args[0];
            if (!File.Exists(fileName))
            {
                Console.Error.WriteLine("File does not exists: {0}", fileName);
                return 2;
            }

            AssemblyName assemblyName;
            try
            {
                assemblyName = AssemblyName.GetAssemblyName(fileName);
            }
            catch (ArgumentException error)
            {
                Console.Error.WriteLine(error.Message);
                return 3;
            }
            catch (FileNotFoundException error)
            {
                Console.Error.WriteLine(error.Message);
                return 2;
            }
            catch (SecurityException error)
            {
                Console.Error.WriteLine(error.Message);
                return 4;
            }
            catch (BadImageFormatException error)
            {
                Console.Error.WriteLine(error.Message);
                return 5;
            }
            catch (FileLoadException error)
            {
                Console.Error.WriteLine(error.Message);
                return 6;
            }

            Console.Out.WriteLine("Name: {0}", assemblyName.Name);
            Console.Out.WriteLine("Culture: {0}", assemblyName.CultureName);
            Console.Out.WriteLine("Version: {0}", assemblyName.Version);
            Console.Out.WriteLine("PublicKeyToken: {0}", ToHexString(assemblyName.GetPublicKeyToken()));

            return 0;
        }

        private static string ToHexString(ICollection<byte> bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Count == 0) return string.Empty;
            var result = new StringBuilder(bytes.Count*2);
            foreach (var b in bytes)
                result.Append(b.ToString("x2"));
            return result.ToString();
        }
    }
}
