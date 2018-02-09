using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading;

namespace GuiMailHangLoat
{
    public partial class Form1 : Form
    {
        Attachment attach = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    FileInfo file = new FileInfo(txtFileAttach.Text);
                    attach = new Attachment(txtFileAttach.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                string email;
                StreamReader reader = new StreamReader(txtTo.Text);
                while ((email = reader.ReadLine()) != null)
                {
                    SendMail(txtFrom.Text, email, txtSubject.Text, txtContent.Text, attach);
                }
                reader.Close();
            });
            thread.Start();
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ( dialog.ShowDialog() == DialogResult.OK)
            {
                txtFileAttach.Text = dialog.FileName;
            }
        }
        public void SendMail(string from, string to, string subject, string content, Attachment fileattach = null)
        {
            // tạo 1 message chứa các thông tin và nội dung
            MailMessage message = new MailMessage(from,to,subject,content);
            if ( fileattach != null)
            {
                message.Attachments.Add(fileattach);
            }
            // tạo smtpclient dùng để gửi mail
            SmtpClient client = new SmtpClient("smtp.gmail.com",587);
            client.EnableSsl = true;
            // đăng nhập tài khoản
            client.Credentials = new NetworkCredential(txtUserName.Text?.ToString(), txtPassword.Text?.ToString());
            client.Send(message);
        }

        private void btnAttachMail_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtTo.Text = dialog.FileName;
            }
        }
    }
}
