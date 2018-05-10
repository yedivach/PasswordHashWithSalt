using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static string saltPath = "";
        static string Salt = "";

        public Form1()
        {
            InitializeComponent();

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string SHA256HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }
        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(Salt == "")
            {
                System.Windows.Forms.MessageBox.Show("You have to choose Salt!");
            }
            else
            {

                Stopwatch stopWatch = new Stopwatch();  // Calculate time:
                 stopWatch.Start();  // Calculate time:

                string ans = CreateToken(webSiteAddres.Text, Password.Text + Salt );
                Console.WriteLine("Salt: "+Salt +"\n password Text: " + Password.Text + "\n ");
                HashedPassword.Text = (ans);

                stopWatch.Stop();  // Calculate time:
                TimeOfCalc.Text = stopWatch.ElapsedMilliseconds.ToString();// display the time
            }

        }

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return string.Join("", BitConverter.ToString(hashmessage).Split('-'));

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(Generate_Salt.Checked)
            {
                Console.Write("line 107");
                // This line calls the folder diag
                System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                // This is what will execute if the user selects a folder and hits OK (File if you change to FileBrowserDialog)
                Console.Write("line 111");
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Console.Write("line 114");
                    saltPath =dlg.SelectedPath+ "\\Salt.txt";
                    Console.Write("line 116");
                    string randomString = RandomString(20);
                    System.Windows.Forms.MessageBox.Show(saltPath);
                    // WriteAllText creates a file, writes the specified string to the file,
                    // and then closes the file.    You do NOT need to call Flush() or Close().
                    Console.Write("line 121");
                    System.IO.File.WriteAllText(@saltPath, randomString);
                    Console.Write("line 123");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("You have to choose a path for the Salt!");
                }
            }
            else if(Choose_Salt.Checked)
            {


                Stream myStream = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                StreamReader reader = new StreamReader(myStream);
                                 Salt = reader.ReadToEnd();
                                System.Windows.Forms.MessageBox.Show("You Salt is: \n" + Salt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }


            }
        }
    }
}
