using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailSender
{
    public partial class Form1 : Form
    {
        public List<string> someFiles;

        public Form1()
        {
            InitializeComponent();

          

            passwordTextBox.PasswordChar = '*';
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void textBox2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedText = comboBox1.SelectedText.Replace("\n", "");
            comboBox1.SelectedText = comboBox1.SelectedText.Replace("\r", "");

            someFiles = File.ReadAllLines(textBox2.Text).ToList();
            richTextBox2.AppendText("Mail list loaded into memory !"+ Environment.NewLine);
            comboBox1.SelectedIndex = 0;

            if (comboBox1.SelectedItem == null)
            {
                timer1.Interval = Convert.ToInt16(comboBox1.GetItemText("900")) * 1000;
            }

            timer1.Enabled = true;
            timer1.Interval = Convert.ToInt16(comboBox1.GetItemText("900")) * 1000;
            timer1.Interval = 900000;
          //  timer1.Interval = 600;


            richTextBox2.AppendText("sending interval set to : " + timer1.Interval.ToString()+Environment.NewLine);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("60");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string line = "";
            int counter = -1;
            int flag = 0;
            try {
                // timer1.Enabled = false;
                string Subject = textBox1.Text;
               
                string body;
               
                //var logFile = File.ReadAllLines(textBox2.Text);//eto
                //someFiles = new List<string>(logFile);//tezi dve linii dobavih
                foreach (string  item in someFiles)//initialize the list from file before first use from the upper two lines
                {
                    counter++;
                   
                    // byte[] bytes1 = Encoding.Default.GetBytes(item);
                    //String lin1 = Encoding.UTF8.GetString(bytes1);
                    String lin1 = item;

                    if (!item.Contains("Processed"))
                    {
                        flag = 1;
                        line = lin1;
                        break;
                    }

                }
                if (flag == 1)
                {
                    string[] arrayList = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                //MessageBox.Show(arrayList[1]);

                // Formata na imeto e slednata: ime; mail; grad;adress


                Subject = textBox1.Text.Replace("#", arrayList[0]);
                body = richTextBox1.Text;
                body = body.Replace("#", arrayList[0]);
                body = body.Replace("&", arrayList[2]);
                body = body.Replace("^", arrayList[3]);
              
                    SmtpClient client = new SmtpClient();
                    // client.Port = Convert.ToInt32(textBox1.Text);

                    client.Host = HostTextBox.Text;
                    client.Port = Convert.ToInt32(PortTextBox.Text);
                    client.EnableSsl = false;
                    client.Timeout = Convert.ToInt32(timeOutTextBox.Text);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(fromMailTextBox.Text, passwordTextBox.Text);
                    arrayList[1].Replace("\t","");
                    arrayList[1].Replace("\n", "");
                    arrayList[1].Replace(" ", "");
                    arrayList[1].Replace("\r", "");


                    MailMessage mail = new MailMessage(Convert.ToString(fromMailTextBox.Text), arrayList[1], Subject, body);
                    mail.BodyEncoding = UTF8Encoding.UTF8;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    mail.IsBodyHtml = true;
                    client.Send(mail);

                    someFiles[counter] = line + "; Processed";
                    richTextBox2.AppendText(someFiles[counter] + Environment.NewLine);
                }
                else
                {
                    richTextBox2.AppendText("list finished!!" + Environment.NewLine);
                    StringBuilder sb = new StringBuilder();
                    foreach (var l in someFiles)
                    {
                        sb.AppendLine(l);

                    }
                    File.AppendAllText( "log.txt", sb.ToString());
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                richTextBox2.AppendText(ex.ToString() + Environment.NewLine);
                someFiles[counter] = line + "; Processed->error";

            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            
        }
    }
}
