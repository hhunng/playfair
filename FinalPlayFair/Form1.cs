using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace FinalPlayFair
{
    public partial class PlayfairForm : Form
    {
        int ks;
        char[] str = new char[100];
        char[] key = new char[100];
        private Button[] indexes; 
        char[] encryptedStr = new char[100];

        char[,] keyT = new char[5, 5];


        public PlayfairForm()
        {
            InitializeComponent();
        }

        private void PlayfairForm_Load(object sender, EventArgs e)
        {
            indexes = new Button[] {button1th,button2th,button3th,button4th,button5th,
                            button6th,button7th,button8th,button9th,button10th,
                            button11th,button12th,button13th,button14th,button15th,
                            button16th,button17th,button18th,button19th,button20th,
                            button21th,button22th,button23th,button24th,button25th};
          
            char letter = 'A';
            for (int i = 0; i < indexes.Length; i++)
            {
                if (letter == 'J')
                {
                    letter++;
                }
                indexes[i].Text = letter.ToString();
                letter++;
            }
        }

        private void encryptBtn_Click(object sender, EventArgs e)
        {
            key = keyTB.Text.ToCharArray();
            Color paleGreen = Color.FromArgb(152, 251, 152); 

            str = plainTextTB.Text.ToCharArray();
            

            encryptedText.Text = new string(EncryptByPlayfairCipher(str, key));

            int k = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    indexes[k].Text = keyT[i, j].ToString().ToUpper();
                    if (k < ks)
                    {
                        indexes[k].ForeColor = Color.Red;
                        indexes[k].BackColor = paleGreen;
                        indexes[k].Font = new Font(indexes[k].Font, FontStyle.Bold);
                    }
                    k++;
                }
            }

        }
        private void toLowerCase(char[] plain, int ps)
        {
            for (int i = 0; i < ps; i++)
            {
                plain[i] = char.ToLower(plain[i]);
            }
        }

        private int removeSpaces(char[] plain, int ps)
        {
            int count = 0;
            for (int i = 0; i < ps; i++)
            {
                if (plain[i] != ' ')
                {
                    plain[count] = plain[i];
                    count++;
                }
            }
            Array.Resize(ref plain, count + 1);
            plain[count] = '\0';
            return count;
        }
        private void generateKeyTable(char[] key, int ks, char[,] keyT)
        {
            int i, j, k;

            int[] dicty = new int[26];

            for (i = 0; i < ks; i++)
            {
                if (key[i] != 'j')
                    dicty[key[i] - 97] = 2;
            }

            dicty['j' - 97] = 1;

            i = 0;
            j = 0;

            for (k = 0; k < ks; k++)
            {
                if (dicty[key[k] - 97] == 2)
                {
                    dicty[key[k] - 97] -= 1;
                    keyT[i, j] = key[k];
                    j++;
                    if (j == 5)
                    {
                        i++;
                        j = 0;
                    }
                }
            }

            for (k = 0; k < 26; k++)
            {
                if (dicty[k] == 0)
                {
                    keyT[i, j] = (char)(k + 97);
                    j++;
                    if (j == 5)
                    {
                        i++;
                        j = 0;
                    }
                }
            }
        }
        private void search(char[,] keyT, char a, char b, int[] arr)
        {
            int i, j;

            if (a == 'j')
                a = 'i';
            else if (b == 'j')
                b = 'i';

            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (keyT[i, j] == a)
                    {
                        arr[0] = i;
                        arr[1] = j;
                    }
                    else if (keyT[i, j] == b)
                    {
                        arr[2] = i;
                        arr[3] = j;
                    }
                }
            }
        }
        private int Mod5(int a)
        {
            return (a % 5);
        }

        private char[] encrypt(char[] str, char[,] keyT, int ps)
        {
            for (int i = 0; i < ps; i += 2)
            {
                int[] a = new int[4];
                search(keyT, str[i], str[i + 1], a);

                if (a[0] == a[2])
                {
                    str[i] = keyT[a[0], Mod5(a[1] + 1)];
                    str[i + 1] = keyT[a[0], Mod5(a[3] + 1)];
                }
                else if (a[1] == a[3])
                {
                    str[i] = keyT[Mod5(a[0] + 1), a[1]];
                    str[i + 1] = keyT[Mod5(a[2] + 1), a[1]];
                }
                else
                {
                    str[i] = keyT[a[0], a[3]];
                    str[i + 1] = keyT[a[2], a[1]];
                }
            }
            return str;
        }
        private char[] EncryptByPlayfairCipher(char[] str, char[] key)
        {
            int ps;
           
            ks = (char)key.Length;
            ks = removeSpaces(key, ks);
            toLowerCase(key, ks);

            ps = (char)str.Length;
            toLowerCase(str, ps);
            ps = removeSpaces(str, ps);



            if (ps % 2 != 0)
            {
                Array.Resize(ref str, ps + 1);
                str[str.Length - 1] = 'z';
                ps = str.Length;
            }
            for (int i = 0; i < ps; i += 2)
            {
                if (str[i] == str[i + 1])
                {
                    str[i + 1] = 'x';
                }
            }
            generateKeyTable(key, ks, keyT);
            return encrypt(str, keyT, ps);

        }
        private char[] decryptByPlayfairCipher(char[] str, char[] key)
        {
            int ps, ks;

            ks = (char)key.Length;
            ps = (char)str.Length;

            return decrypt(str, keyT, ps);
        }
        private void decryptBtn_Click(object sender, EventArgs e)
        {
            encryptedStr = encryptedText.Text.ToCharArray();

            char[] getDecryptedSequence = decryptByPlayfairCipher(encryptedStr, key);

            for (int i = 0; i < getDecryptedSequence.Length; i+=2) { 
                if (getDecryptedSequence[i+1] == 'x')
                {
                    getDecryptedSequence[i + 1] = getDecryptedSequence[i];
                }
            }
            decryptedText.Text = new string(getDecryptedSequence);
        }
        private char[] decrypt(char[] str, char[,] keyT, int ps)
        {
            int i;
            int[] a = new int[4];
            for (i = 0; i < ps; i += 2)
            {
                search(keyT, str[i], str[i + 1], a);
                if (a[0] == a[2])
                {
                    str[i] = keyT[a[0], Mod5(a[1] - 1)];
                    str[i + 1] = keyT[a[0], Mod5(a[3] - 1)];
                }
                else if (a[1] == a[3])
                {
                    str[i] = keyT[Mod5(a[0] - 1),a[1]];
                    str[i + 1] = keyT[Mod5(a[2] - 1),a[1]];
                }
                else
                {
                    str[i] = keyT[a[0],a[3]];
                    str[i + 1] = keyT[a[2],a[1]];
                }
            }
            return str;
        }

        private void openDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.Title = "Select a Text File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                plainTextTB.Text = fileContent;
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            plainTextTB.Text = "";
            encryptedText.Text = "";
            decryptedText.Text = "";
            keyTB.Text = "";
            char letter = 'A';
            for (int i = 0; i < indexes.Length; i++)
            {
                if (letter == 'J')
                {
                    letter++;
                }
                indexes[i].Text = letter.ToString();
                indexes[i].ForeColor = SystemColors.ControlText;
                indexes[i].BackColor = SystemColors.Control; // Reset the background color to the default
                indexes[i].Font = new Font(indexes[i].Font, FontStyle.Regular);
                letter++;
            }
            Array.Clear(str, 0, str.Length);
            Array.Clear(key, 0, key.Length);
            Array.Clear(encryptedStr, 0, encryptedStr.Length);
            Array.Clear(keyT, 0, keyT.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
