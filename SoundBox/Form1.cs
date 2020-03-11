using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundBox
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lecturaProgramas();
            cargarBotones();
        }

        private void cargarBotones()
        {
            throw new NotImplementedException();
        }

        private string[] lecturaProgramas()
        {
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\JONA\Desktop\PROGRAMAS.txt");

            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                cbxProgramas.Items.Add(line);
                Console.WriteLine("\t" + line);
            }
            cbxProgramas.SelectedIndex = 0;
            return lines;
        }
    }
}
