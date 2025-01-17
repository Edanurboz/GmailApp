﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GmailApp
{
    public partial class Form1 : Form
    {
        string[] Scopes = { GmailService.Scope.GmailSend };
        string ApplicationName = "GmailApp";
       
        public Form1()
        {
            InitializeComponent();
        }

        string Base64UrlEncode(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(data).Replace(".", "-").Replace("/", "_").Replace("=", "");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            UserCredential credential;

            using (FileStream stream = new FileStream(Application.StartupPath+@"/credentials.json",FileMode.Open,FileAccess.Read))
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                path = Path.Combine(path, ".credentials/gmail-dotnet-quickstart.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,Scopes,"user", CancellationToken.None, new FileDataStore(path, true)).Result;
            }
            string message = $"To: {TBReciever.Text}\r\nSubject: {TBSubject.Text}\r\nContent-Type: text/html;charset=utf-8\r\n\r\n<h1>{TBMessage.Text}</h1>";
            //call your gmail service
            var service = new GmailService(new BaseClientService.Initializer() { HttpClientInitializer = credential, ApplicationName = ApplicationName });
            var msg = new Google.Apis.Gmail.v1.Data.Message();
            msg.Raw = Base64UrlEncode(message.ToString());
            service.Users.Messages.Send(msg, "me").Execute();
            MessageBox.Show("Your email has been successfully sent.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
