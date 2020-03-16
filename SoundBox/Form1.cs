using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundBox
{
    public partial class Form1 : Form
    {
        string[] efectosAudio = new string[11];
        string[] audiosPath = new string[11];



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lecturaArchivos("PROGRAMAS.txt");
        }

        private void lecturaArchivos(string archivoL)
        {
            // Read each line of the file into a string array. 
            // Each element of the array is one line of the file.
            try
            {
                // Lines of the program to be readed
                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + archivoL);

                // First execution of the program; all contents in "PROGRAMAS.txt" will appear in ComboBox as options to select sounds
                if (archivoL.Equals("PROGRAMAS.txt"))
                {
                    foreach (string line in lines)
                    {
                        // Use a tab to indent each line of the file.
                        // Lines comented with "#" will be discarded
                        if (!line.Contains("#"))
                        {
                            //Contents of "PROGRAMAS.txt" will be shown on comboBox
                            cbxProgramas.Items.Add(line);
                            //Console control
                            Console.WriteLine("\t" + line);
                        }
                    }
                    //Default option in ComboBox = "Predeterminado"
                    cbxProgramas.SelectedIndex = 0;
                }
                else //This condition is only valid for the files that contain .wav Files Paths
                {
                    string[] result = new string[11];
                    //This method will create substring of the path
                    // Saving all the string between "\" caracter and ".wav" string
                    result = recortarPaths(lines,"\\",".wav");
                    // Labels showing the name of the .wav file assigned t each button of the interface
                        lbl_1.Text = result[0];
                        lbl_2.Text = result[1];
                        lbl_3.Text = result[2];
                        lbl_4.Text = result[3];
                        lbl_5.Text = result[4];
                        lbl_6.Text = result[5];
                        lbl_7.Text = result[6];
                        lbl_8.Text = result[7];
                        lbl_9.Text = result[8];
                        lbl_10.Text = result[9];
                        lbl_11.Text = result[10];                  
                }
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Error: File not Found: "+ex.Message);
                cleanLabels();
                // If the file doesn´t exist all label will show ""
               
            }
            catch(IndexOutOfRangeException ex)
            {
                // If the file is empty all label will show ""
                cleanLabels();
                Console.WriteLine("Archivo de configuración vacio: " + ex.Message);
            }
        }

        private void cleanLabels()
        {
            foreach (Control ctr in grpSonidos.Controls)
            {
                //Cleaning of labels
                if (ctr is Label)
                {
                    ctr.Text = "";
                }
            }
        }

        private string[] recortarPaths(string[] lines, string chFrom, string chTo)
        {
            // The contents of a file which stores the path to .wav sounds
            string[] result = new string[lines.Length];
            // Variables used to save the name of the file, discarding all the path.
            int i = 0;
            int pTo = 0;
            int pFrom = 0;
            // Saving all the string between "chFrom" caracter and "chTo" string
            while (i < lines.Length )//&& lines[i].LastIndexOf(chTo) != -1)
            {

                if(lines[i].LastIndexOf(chTo)!=-1)
                {
                    pFrom = lines[i].LastIndexOf(chFrom) + chFrom.Length;
                    pTo = lines[i].LastIndexOf(chTo);
                    result[i] = lines[i].Substring(pFrom, pTo - pFrom);
                }
                else
                {
                    result[i] = "";
                }
               
                // Console control
                Console.WriteLine(result[i]);
                i++;
            }

            return result;
        }

        private void btnGuardarConfig_Click(object sender, EventArgs e)
        {
            
            //HARDCODE
            Console.WriteLine(cbxProgramas.SelectedItem.ToString());
            string fileName = @"C:\Users\JONA\Desktop\RADIO QUANTICA\"+cbxProgramas.SelectedItem.ToString()+".txt";
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    // The content of the file will be the .wav paths assigned to each button
                    for(int i=0; i<=efectosAudio.Length; i++)
                    {
                        Byte[] audioAgregado = new UTF8Encoding(true).GetBytes(efectosAudio[i] + "\n");
                        fs.Write(audioAgregado, 0, audioAgregado.Length);
                    }
                }
                // Open the stream and read it back.    
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            cbxProgramas.Items.Clear();
            lecturaArchivos("PROGRAMAS.txt");
        }

        private void btnCrearPrograma_Click(object sender, EventArgs e)
        {
            //HARDCODE
            Process.Start("notepad.exe", @"C:\Users\JONA\Desktop\RADIO QUANTICA\PROGRAMAS.txt").WaitForExit();

            cbxProgramas.Items.Clear();
            lecturaArchivos("PROGRAMAS.txt");

            verificarExistentes();
        }

        private void verificarExistentes()
        {
            //Files that Exists
            //HARDCODE
            string[] filePaths = Directory.GetFiles(@"C:\Users\JONA\Desktop\RADIO QUANTICA\", "*.txt", SearchOption.TopDirectoryOnly);
            
            filePaths = recortarPaths(filePaths, "\\", ".txt");
            Console.WriteLine("DOCUMENTOS EXISTENTES");
            Array.Sort(filePaths);
            //foreach (string iter in filePaths)
            //{
            //    Console.WriteLine(iter);
            //}
            //**************************************************
            List<String> existentes = new List<String>();
            Console.WriteLine("Programas existentes en el archivo de PROGRAMAS");

            foreach (string iter in cbxProgramas.Items)
            {
                existentes.Add(iter);
            }
            existentes.Sort();

            //foreach (string iter in existentes)
            //{
            //    Console.WriteLine(iter);
            //}
            Console.WriteLine("***Los que no existen");

            
            //***********************************
            //HARDCODE
            //CREAR SI NO EXISTE
            Console.WriteLine("*****PAra ver cual toca crear");
            foreach (string iter in existentes)
            {
                if (!File.Exists(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt"))
                {
                    Console.WriteLine("no existe, crear: "); Console.WriteLine(iter);
                    File.Create(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt").Dispose();
                    //File.Delete(fileName);
                }
            }
            /////
            foreach (string iter in filePaths.Except(existentes))
            {
                if (!iter.Equals("PROGRAMAS"))
                {
                    try
                    {
                        Console.WriteLine("Borrar: " + iter);
                        //meTER EL PATH Y BORRAR EL ARCHIVO ITER
                        File.Delete(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt");
                    }
                    catch(IOException ex)
                    {
                        Console.WriteLine("Error: " +ex.Message);
                    }
                    
                }

            }
        }

        private void abrirArchivos(Label label,int index)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            //Filter all the files in a folder just showing the.wav files
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "wav files (*.wav)|*.wav";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                // When the file explorer is closed the name of the file selected is saved in "efectodAudio" array
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    //Shows the file name in the button label
                    label.Text = openFileDialog.SafeFileName;
                    //Adds th path to string to be saved
                    efectosAudio[index]= filePath;
                    //Console control
                    Console.WriteLine(filePath);
                }
            }
        }

        private void cbxProgramas_SelectedIndexChanged(object sender, EventArgs e)
        {
            lecturaArchivos(cbxProgramas.SelectedItem.ToString() + ".txt");
            audiosPath = System.IO.File.ReadAllLines(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + cbxProgramas.SelectedItem.ToString() + ".txt");

            foreach(string iter in audiosPath)
            {
                Console.WriteLine(iter);
            }

            if (cbxProgramas.SelectedItem.Equals("Predeterminado"))
            {
                btnGuardarConfig.Enabled = false;
            }
            else
            {
                btnGuardarConfig.Enabled = true;
            }
        }

        #region Button Events
        private void btn_1_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_1, 0);
        }

        private void btn_2_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_2, 1);
        }

        private void btn_3_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_3, 2);
        }

        private void btn_4_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_4, 3);
        }

        private void btn_5_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_5, 4);
        }

        private void btn_6_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_6, 5);
        }

        private void btn_7_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_7, 6);
        }

        private void btn_8_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_8, 7);
        }

        private void btn_9_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_9, 8);
        }

        private void btn_10_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_10, 9);
        }

        private void btn_11_Click(object sender, EventArgs e)
        {
            abrirArchivos(lbl_11, 10);
        }


        #endregion
    }
}
