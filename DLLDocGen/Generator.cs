using System;
using System.Windows;
using System.Reflection;
using System.IO;

namespace DLLDocGen
{
    internal class Generator
    {
        private string inFilePath { get; set; }
        private string outFilePath { get; set; }
        private string fileName { get; set; }
        private string printString;

        public Generator(string dllFilePath, string outputFilePath, bool startScrape)
        {
            inFilePath = dllFilePath;
            outFilePath = outputFilePath;
            if (startScrape)
            {
                scrapeDLL();
            }
        }

        public void scrapeDLL()
        {
            Assembly thisAssembly = null;
            try
            {
                try
                {
                    //open assembly with LoadFile()
                    thisAssembly = Assembly.LoadFrom(inFilePath);
                    fileName = thisAssembly.FullName;
                    printString = "Assembly Name: " + fileName + "\n";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                //get assembly attributes
                pullAttributes(thisAssembly.GetCustomAttributes(true));
                //read modules
                Module[] thisModules = AssemblyExtensions.GetModules(thisAssembly);
                //for each module get name, field, and method info
                pullModuleInfo(thisModules);
            }
            catch (Exception)
            {
                MessageBox.Show("string writing failed");
            }
            try
            {
                //print all to txt file
                printToFile();
            }
            catch (Exception)
            {
                MessageBox.Show("printing failed");
            }
        }

        private void pullModuleInfo(Module[] moduleInfo)
        {
            foreach (Module m in moduleInfo) 
            {
                //add name and header
                printString = printString + "\nModule: " + m.Name + "\n";
                //get Attributes
                pullAttributes(m.GetCustomAttributes(true));
                //get Fields
                pullFieldInfo(m.GetFields());
                //get Methods
                pullMethodInfo(m.GetMethods());
            }
        }

        private void pullFieldInfo(FieldInfo[] fieldInfo)
        {
            printString = printString + "\nFields: \n";
            //for each field get info
            if (fieldInfo != null)
            {
                foreach (FieldInfo f in fieldInfo)
                {
                    printString = printString + f.Name + " ~ " + f.Attributes + "\n";
                }
            }
        }

        private void pullMethodInfo(MethodInfo[] methodInfo)
        {
            // for each method get info and arguments
            //get method parameters from methodInfo()
            foreach(MethodInfo m in methodInfo)
            {
                //get method name
                printString = printString + "\nMethod: " + m.Name + "\n";
                //get method scope
                if (m.IsPrivate)
                {
                    printString = printString + "private ";
                }
                else if(m.IsPublic)
                {
                    printString = printString + "public ";
                }
                if (m.IsFinal)
                {
                    printString = printString + "final ";
                }
                if (m.IsStatic)
                {
                    printString = printString + "static ";
                }
                if (m.IsVirtual)
                {
                    printString = printString + "virtual ";
                }
                printString = printString + m.ReturnType + "\n";
                //get attributes for the method
                pullAttributes(m.GetCustomAttributes(true));
                //get method parameters
                pullParameters(m.GetParameters());
            }
        }

        private void pullAttributes(Object[] attributeInfo)
        {
            //print attribute using object.ToString()
            printString = printString + "\nCustom Attributes: \n";
            foreach(Object o in attributeInfo)
            {
                printString = printString + o.ToString() + "\n";
            }
        }

        private void pullParameters(ParameterInfo[] paramInfo)
        {
            printString = printString + "\nParameters: \n";
            //print basic parameter information
            foreach(ParameterInfo p in paramInfo)
            {
                printString = printString + p.ParameterType + " " + p.Name + " ";
                if (p.HasDefaultValue)
                {
                    printString = printString + "Default Value: " + p.DefaultValue + "\n";
                }
                else
                {
                    printString = printString + "\n";
                }
            }
        }

        private void printToFile()
        {
            StreamWriter Sw = new StreamWriter(outFilePath + "\\AutoGenDoc " + fileName + ".txt");
            Sw.Write(printString);
            Sw.Flush();
            Sw.Close();
        }
    }
}
