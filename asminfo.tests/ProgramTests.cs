using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace asminfo.tests
{
    [TestClass]
    public class ProgramTests
    {
        private StringWriter _stderr;
        private StringWriter _stdout;
        private StringBuilder Error { get; set; }
        private StringBuilder Out { get; set; }

        [TestInitialize]
        public void RedirectStandardErrorAndOutput()
        {
            _stderr = new StringWriter(Error = new StringBuilder());
            _stdout = new StringWriter(Out = new StringBuilder());

            Console.SetError(_stderr);
            Console.SetOut(_stdout);
        }

        [TestCleanup]
        public void ClearOutput()
        {
            _stderr.Dispose();
            _stdout.Dispose();
            _stderr = null;
            _stdout = null;
            Error = null;
            Out = null;
        }

        [TestMethod]
        public void Main_NoArgument_ExpectedSingleArgumentExitCode1()
        {
            var actualExitCode = Program.Main(new string[0]);

            Assert.AreEqual(1, actualExitCode, "#1 Exit code");
            Assert.AreEqual("Expected a single argument: path to assembly file\r\n", Error.ToString(), "#2 Error output");
            Assert.AreEqual("", Out.ToString(), "#3 output");
        }

        [TestMethod]
        public void Main_NullArgument_ExpectedSingleArgumentExitCode1()
        {
            var actualExitCode = Program.Main(null);

            Assert.AreEqual(1, actualExitCode, "#1 exit code");
            Assert.AreEqual("Expected a single argument: path to assembly file" + Environment.NewLine, Error.ToString(), "#2 error output");
            Assert.AreEqual("", Out.ToString(), "#3 output");
        }

        [TestMethod]
        public void Main_InvalidPath_FileDoesNotExistExitCode2()
        {
            var actualExitCode = Program.Main(new [] { @"C:\foo\bar\to\random\file\that\doesnt\exists.dll" });

            Assert.AreEqual(2, actualExitCode, "#1 exit code");
            Assert.AreEqual(@"File does not exists: C:\foo\bar\to\random\file\that\doesnt\exists.dll" + Environment.NewLine, Error.ToString(), "#2 error output");
            Assert.AreEqual("", Out.ToString(), "#3 output");
        }

        [TestMethod]
        public void Main_PathIsNotAssembly_PrintErrorAndExitCode5()
        {
            var actualExitCode = Program.Main(new[] { @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe.config" });

            Assert.AreEqual(5, actualExitCode, "#1 exit code");
            Assert.IsTrue(Error.Length > 0, "#2 error output");
            Assert.IsTrue(Out.Length == 0, "#3 output");
        }

        [TestMethod]
        public void Main_PathValid_AssemblyInfoPrintedExitCode0()
        {
            var actualExitCode = Program.Main(new[] { @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" });

            Assert.AreEqual(0, actualExitCode, "#1 exit code");
            Assert.IsTrue(Error.Length == 0, "#2 error output");

            var expectedOutput = new []
            {
                "Name: MSBuild",
                "Culture: ",
                "Version: 4.0.0.0",
                "PublicKeyToken: b03f5f7f11d50a3a"
            };

            var expectedOutputString = string.Join(Environment.NewLine, expectedOutput) + Environment.NewLine;

            Assert.AreEqual(expectedOutputString, Out.ToString(), "#3 output");
        }
    }
}
