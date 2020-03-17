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
        // Array that stores temporarily the paths of audio files that will be saveded to configuration file
        string[] audiosPath = new string[11];
        // Array that contains the paths of previously loaded audio files in the configuration file
        // this is used every time that a config file is readed/ComboBox changes


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initial lecture of "PROGRAMAS.txt"-> the file contains a list of the current existing programs in Radio Quántica-EPN
            lecturaArchivos("PROGRAMAS.txt");
        }

        /// <summary>
        /// Read some .txt file
        /// Set the initial configuration is the entry paramater is "PROGRAMAS.txt"
        /// Set the label names of each button of the interface if a config file is readed
        /// </summary>
        /// <param name="archivoL">The path of a file</param>
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
                // If the file doesn´t exist all label will show ""
                Console.WriteLine("Error: File not Found: "+ex.Message);
                cleanLabels();             
            }
            catch(IndexOutOfRangeException ex)
            {
                // If the file is empty all label will show ""
                cleanLabels();
                Console.WriteLine("Archivo de configuración vacio: " + ex.Message);
            }
        }

        /// <summary>
        /// Clean all the labels in the groupbox called "grpSonidos"
        /// </summary>
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

        /// <summary>
        /// Cut paths and search a substring between two strings or characters
        /// in this case is used to show the name of a file in a path
        /// the name of a path is between chFrom and chTo
        /// </summary>
        /// <param name="lines">Array with the contents of a .txt file</param>
        /// <param name="chFrom">char or sequence from which the "lines" parameter is cutted</param>
        /// <param name="chTo">char or sequence to which the "lines" parameter is cutted</param>
        /// <returns>String array with the sequence required</returns>
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
                //if the las index os chTo it means that the line is empty
                if(lines[i].LastIndexOf(chTo)!=-1)
                {
                    pFrom = lines[i].LastIndexOf(chFrom) + chFrom.Length;
                    pTo = lines[i].LastIndexOf(chTo);
                    result[i] = lines[i].Substring(pFrom, pTo - pFrom);
                }
                else
                {
                    // If the line is empty it remains that way to be shown,
                    // is made this way to avoid an exception
                    result[i] = "";
                }
                // Console control
                Console.WriteLine(result[i]);
                i++;
            }
            return result;
        }

        /// <summary>
        /// The button that saves Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    for(int i=0; i<=audiosPath.Length; i++)
                    {
                        //The text added are the paths of the .wav files  
                        Byte[] audioAgregado = new UTF8Encoding(true).GetBytes(audiosPath[i] + "\n");
                        fs.Write(audioAgregado, 0, audioAgregado.Length);
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

        /// <summary>
        /// Button that opens the "PROGRAMAS.txt" file that contains the list of current emiting programs in Radio Quantica-EPN
        /// to create/delete a program just write/delete the name of the list
        /// the program file to store the audio config of each program will be created or deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearPrograma_Click(object sender, EventArgs e)
        {
            //HARDCODE
            // The notepad whit the Programs is opened
            Process.Start("notepad.exe", @"C:\Users\JONA\Desktop\RADIO QUANTICA\PROGRAMAS.txt").WaitForExit();
            // The ComboBox is cleaned, to display the real existing programs
            cbxProgramas.Items.Clear();
            // Programs are shown in the GUI
            lecturaArchivos("PROGRAMAS.txt");
            // The existence or not of a file is examinated
            verificarExistentes();
        }

        /// <summary>
        /// Verifies the existence or not of a file
        /// </summary>
        private void verificarExistentes()
        {
            //Files that Exists in the folder
            //HARDCODE
            string[] filePaths = Directory.GetFiles(@"C:\Users\JONA\Desktop\RADIO QUANTICA\", "*.txt", SearchOption.TopDirectoryOnly);
            // Names are cutted to verify the ones that are in the Folder - JUST THE NAMES - to be compared with the ones on the PROGRAMAS.txt 
            filePaths = recortarPaths(filePaths, "\\", ".txt");
            // Console control
            //Console.WriteLine("***DOCUMENTOS EXISTENTES");
            Array.Sort(filePaths);
            //foreach (string iter in filePaths)
            //{
            //    Console.WriteLine(iter);
            //}
            //**************************************************
            //List of existing programs in the PROGRAMAS.txt
            List <String> existentes = new List<String>();
            //Console.WriteLine("***Programas existentes en el archivo de PROGRAMAS");
            foreach (string iter in cbxProgramas.Items)
            {
                existentes.Add(iter);
            }
            existentes.Sort();
            //foreach (string iter in existentes)
            //{
            //    Console.WriteLine(iter);
            //}
            //Console.WriteLine("***Los que no existen");
            //***********************************
            //HARDCODE
            //CREAR SI NO EXISTE
            //Console.WriteLine("***Para ver cual toca crear");
            // If a file doesn´t exist, then is created
            foreach (string iter in existentes)
            {
                if (!File.Exists(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt"))
                {
                    //Console.WriteLine("no existe, crear: "); Console.WriteLine(iter);
                    File.Create(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt").Dispose();
                }
            }
            //If a program does not Exist in PROGRAMAS.txt, is deleted
            foreach (string iter in filePaths.Except(existentes))
            {
                // Everything that isnt in the PROGRAMAS file, except for, obviously PROGRMAS.txt
                if (!iter.Equals("PROGRAMAS"))
                {
                    try
                    {
                        //Console.WriteLine("Borrar: " + iter);
                        //METER EL PATH Y BORRAR EL ARCHIVO ITER
                        //HARDCODE
                        File.Delete(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt");
                    }
                    catch(IOException ex)
                    {
                        // Usually error when the process is being ocupated by something else
                        // UPDATE: Bug fixed with File.Create().Dispose();
                        Console.WriteLine("Error: " +ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Open folders and filter for .wav files
        /// used for every button and seth the path of the .wav sound of each button of the physical interface
        /// </summary>
        /// <param name="label">the label that will receive the name</param>
        /// <param name="index">Index in the audiospath[index] array </param>
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
                    audiosPath[index]= filePath;
                    //Console control
                    Console.WriteLine(filePath);
                }
            }
        }

        /// <summary>
        /// ComboBox with the PROGRAMAS.txt content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProgramas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set de GUI labels for each button according to the selected option in the ComboBox
            lecturaArchivos(cbxProgramas.SelectedItem.ToString() + ".txt");
            //HARDCODE
            // Reads the contents of a .txt file according to the selected option
            audiosPath = System.IO.File.ReadAllLines(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + cbxProgramas.SelectedItem.ToString() + ".txt");
            // Console Control, to see the paths that are being loaded/saved
            //foreach(string iter in audiosPath)
            //{
            //    Console.WriteLine(iter);
            //}

            //The following section is to block the save function of the default config
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
