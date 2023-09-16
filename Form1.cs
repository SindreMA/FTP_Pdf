using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string UserId = "#######";
            string FtpServer = "localhost";
            string FileLocation = "d:\\cv.pdf";
            string NewFileLocation = "d:\\" + UserId + ".xml";
            string FTPUser = "#####";
            string FTPUser1 = "#####";
            string FTPPass = "######";
            try
            {
                File.Delete(NewFileLocation);
            }
            catch (Exception) { }
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(FTPUser, FTPPass);
                client.UploadFile("ftp://" + FtpServer + @"/" + UserId + ".pdf", "STOR", FileLocation);
            }
            bool FoundFile = false;
            string url = "ftp://" + FtpServer + "/";
            while (!FoundFile)
            {
                List<string> directories = new List<string>();
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + FtpServer + "/");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(FTPUser, FTPPass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                directories.Add(reader.ReadToEnd());
                reader.Close();
                response.Close();
                Thread.Sleep(1000);
                Thread.Sleep(1000);
                foreach (var item in directories)
                {
                    if (item.Contains(UserId))
                    {
                        FoundFile = true;
                        using (WebClient client = new WebClient())
                        {
                            client.Credentials = new NetworkCredential(FTPUser1, FTPPass);
                            string downloadfilestring = "ftp://" + FtpServer + @"/" + UserId + ".xml";
                            client.DownloadFile(downloadfilestring, NewFileLocation);
                        }
                        Thread.Sleep(1000);
                        FtpWebRequest requestGet = (FtpWebRequest)WebRequest.Create("ftp://" + FtpServer + @"/" + UserId + ".xml");
                        requestGet.Credentials = new NetworkCredential(FTPUser1, FTPPass);
                        requestGet.Method = WebRequestMethods.Ftp.DeleteFile;
                        FtpWebResponse responseGet = (FtpWebResponse)requestGet.GetResponse();
                        responseGet.Close();
                    }
                }
                XmlDocument Doc = new XmlDocument();
                Doc.Load(NewFileLocation);
                XmlNodeList nodeList = Doc.SelectNodes("/data");
                foreach (XmlNode node in nodeList)
                {
                    foreach (XmlNode item in node)
                    {
                        if (item.Name.Contains("Utdanning"))
                        {
                            string Utdanning = item.InnerText;
                        }
                        if (item.Name.Contains("Arbeidserfaring"))
                        {
                            string Arbeidserfaring = item.InnerText;
                        }
                        if (item.Name.Contains("Nøkkelkompetanse"))
                        {
                            string Nøkkelkompetanse = item.InnerText;
                        }
                        if (item.Name.Contains("AnnenErfaring"))
                        {
                            string AnnenErfaring = item.InnerText;
                        }
                        if (item.Name.Contains("Språk"))
                        {
                            string Språk = item.InnerText;
                        }
                    }

                }
            }

        }
    }
}
