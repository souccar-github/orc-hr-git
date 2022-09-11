using Project.Web.Mvc4.Areas.Security.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Xml;

namespace Project.Web.Mvc4.Helpers
{
    public static class NavigationHelper
    {
        public static IList<Project.Web.Mvc4.Models.Tab> Tabs { get; private set; }
        public static Project.Web.Mvc4.Models.Tab SystemTab { get; private set; }
        public static int UsersCount { get; private set; }
        public static bool FireBaseDisabled { get; private set; }
        public static XmlDocument ConfigXmlFile { get; private set; }
        public static IList<string> CountryCodes { get; private set; }
        public static void InitXmlDocument()
        {
            ConfigXmlFile = GetXmlDocumentConfig();
            InitAccountConfig();
            InitNavigationTabs();
            RemoveXmlDocument();
        }
        private static void InitNavigationTabs()
        {
            Tabs = GetNavTabs();
            SystemTab = GetNavTabs("sys").FirstOrDefault();
        }
        private static void InitAccountConfig()
        {
            UsersCount = int.Parse(GetValueByPathAndKeyFromXmlDocumentConfig("/Settings/Users", "count"));
            CountryCodes = GetValuesByPathAndKeyAttributeFromChildernOfXmlDocumentConfig("/Settings/AllowedCountries", "code").ToList();

            var disabled = false;
            if(bool.TryParse(GetValueByPathAndKeyFromXmlDocumentConfig("/Settings/Firebase", "Disabled"), out disabled))
            {
                FireBaseDisabled = disabled;
            }
        }
        private static void FileDecrypt(string inputFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(inputFile, FileMode.OpenOrCreate);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //Application.DoEvents();
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }
        }
        private static XmlDocument GetXmlDocumentConfig()
        {
            string pass = "aksfjhakhewiwuekjahskjashfjkahfaueahkjfhjhf";
            var path = HttpContext.Current.Server.MapPath("~/App_Data/Settings.xml");
            var xmlDocument = new XmlDocument();
            if (System.IO.File.Exists(path + ".aes"))
            {
                FileDecrypt(path, pass);
                if (System.IO.File.Exists(path))
                {
                    xmlDocument.Load(path);
                    return xmlDocument;
                }
            }
            return null;
        }
        private static string GetValueByPathAndKeyFromXmlDocumentConfig(string nodePath, string nodeAttribute)
        {
            var value = "";
            if (ConfigXmlFile != null)
            {
                XmlNode node = ConfigXmlFile.DocumentElement.SelectSingleNode(nodePath);
                value = node?.Attributes[nodeAttribute].InnerText;
                return value;
            }
            return value;
        }
        private static bool RemoveXmlDocument()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/Settings.xml");
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
        private static IList<string> GetValuesByPathAndKeyAttributeFromChildernOfXmlDocumentConfig(string nodePath, string nodeAttribute)
        {
            var values = new List<string>();
            if (ConfigXmlFile != null)
            {
                XmlNode node = ConfigXmlFile.DocumentElement.SelectSingleNode(nodePath);
                if (node != null)
                {
                    foreach (XmlNode element in node.ChildNodes)
                    {
                        values.Add(element.Attributes[nodeAttribute].InnerText);
                    }
                    return values;
                }
            }
            return values;
        }
        private static IList<Project.Web.Mvc4.Models.Tab> GetNavTabs(string type = "primary")
        {
            var tabs = new List<Project.Web.Mvc4.Models.Tab>();
            if (ConfigXmlFile != null)
            {
                XmlNode node = ConfigXmlFile.DocumentElement.SelectSingleNode("/Settings/Tabs");
                foreach (XmlNode element in node.ChildNodes)
                {
                    if (element.Attributes["visible"].InnerText == "1" && ((element.Attributes["key"].InnerText != "System" && type == "primary") ||
                        (element.Attributes["key"].InnerText == "System" && type != "primary")))
                    {
                        var tabModules = new List<Project.Web.Mvc4.Models.TabModule>();
                        foreach (XmlNode moduleElement in element.ChildNodes)
                        {
                            if (moduleElement.Attributes["visible"].InnerText == "1")
                            {
                                tabModules.Add(new Project.Web.Mvc4.Models.TabModule()
                                {
                                    Name = moduleElement.Attributes["key"].InnerText,
                                    Order = int.Parse(moduleElement.Attributes["order"].InnerText)
                                });
                            }
                        }
                        tabs.Add(new Project.Web.Mvc4.Models.Tab()
                        {
                            Name = element.Attributes["key"].InnerText,
                            Order = int.Parse(element.Attributes["order"].InnerText),
                            Modules = tabModules
                        });
                    }

                }
            }
            return tabs;
        }
    }
}