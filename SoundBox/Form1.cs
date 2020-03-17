using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace SoundBox
{
    public partial class Form1 : Form
    {
        //Creation of the object serial port to read the data from Arduino
        SerialPort serialPort1 = new SerialPort();

        // Array that stores temporarily the paths of audio files that will be saveded to configuration file
        string[] audiosPath = new string[11];
        // Array that contains the paths of previously loaded audio files in the configuration file
        // this is used every time that a config file is readed/ComboBox changes
        // Delegate to read lines of COM port
        public delegate void LineReceivedEvent(string line);
        //Object media player to reproduce audios
        MediaPlayer m_mediaPlayer = new MediaPlayer();
        //Path of the Resource folder in the running instance
        string FileName = string.Format("{0}Resources\\RQ_Config", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")));

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initial lecture of "PROGRAMAS.txt"-> the file contains a list of the current existing programs in Radio Quántica-EPN
            lecturaArchivos("PROGRAMAS.txt");
        }

        public Form1()
        {

            InitializeComponent();
            // Ports found in PC
            string[] ports = SerialPort.GetPortNames();
            // Console Control
            //Console.WriteLine("Puertos hallados:");
            // Adding the ports found to comboBox
            foreach (string port in ports)
            {
                Console.WriteLine(port);
                serialPort1.PortName = port;
                cbxPuertoCOM.Items.Add(port);
            }
            cbxPuertoCOM.SelectedIndex = 0;
            //COM port parameters
            serialPort1.BaudRate = 9600;
            serialPort1.DtrEnable = true;
            serialPort1.Open();
            serialPort1.DataReceived += serialPort1_DataReceived;

        }

        #region Serial Communication
        /// <summary>
        /// Lines received in de Serial COM port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string line = serialPort1.ReadLine();
            this.BeginInvoke(new LineReceivedEvent(LineReceived), line);
        }


        /// <summary>
        /// Lines Received, used to distinguish the button that has been pressed an set de volume
        /// </summary>
        /// <param name="line"></param>
        public void LineReceived(string line)
        {
            // System Volume 
            try
            {
                // Console Control
                //Console.WriteLine(Double.Parse(line) / 255.0);
                // Conversion used to calibrate de Volume of de track reproduced
                m_mediaPlayer.Volume = Double.Parse(line) / 255.0;
                // Volumen is Shown in the GUI
                lblVolumen.Text = "- - - VOLUMEN: "+ String.Format("{0:00}", m_mediaPlayer.Volume * 100) + " % - - -";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Detects which button has been pressed
            #region if else to detect pressed button
            if (line.Contains("@1"))
            {
                reproducirAudio(0);
            }
            else if (line.Contains("@2"))
            {
                reproducirAudio(1);
            }
            else if (line.Contains("@3"))
            {
                reproducirAudio(2);
            }
            else if (line.Contains("@4"))
            {
                m_mediaPlayer.Stop();
            }
            else if (line.Contains("@5"))
            {
                reproducirAudio(4);
            }
            else if (line.Contains("@6"))
            {
                reproducirAudio(5);
            }
            else if (line.Contains("@7"))
            {
                reproducirAudio(6);
            }
            else if (line.Contains("@8"))
            {
                reproducirAudio(7);
            }
            else if (line.Contains("@9"))
            {
                reproducirAudio(8);
            }
            else if (line.Contains("@A"))
            {
                reproducirAudio(9);
            }
            else if (line.Contains("@B"))
            {
                reproducirAudio(10);
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// Reproduce de .wav File
        /// </summary>
        /// <param name="index">index of audiosPath Array, that contains the path of the .wav file to be played</param>
        private void reproducirAudio(int index)
        {
            try
            {
                m_mediaPlayer.Open(new Uri(Path.GetFullPath(audiosPath[index])));
                m_mediaPlayer.Play();
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " +ex.Message);
            }

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
                //HARDCODE
                // Lines of the program to be readed
                //FileName+"\\" + 
                //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
                //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" + archivoL);
                string[] lines = System.IO.File.ReadAllLines(FileName + "\\" + archivoL);
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
                        lbl_4.Text = "STOP";//result[3];
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
                string[] lines = { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", };
                // WriteAllLines creates a file, writes a collection of strings to the file, and then closes the file.
                System.IO.File.WriteAllLines(FileName+"\\"+cbxProgramas.SelectedItem+".txt", lines);
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
        /// Verifies the existence or not of a file
        /// </summary>
        private void verificarExistentes()
        {
            //Files that Exists in the folder
            //HARDCODE
            //FileName+"\\" + 
            //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
            //string[] filePaths = Directory.GetFiles(@"C:\Users\JONA\Desktop\RADIO QUANTICA\", "*.txt", SearchOption.TopDirectoryOnly);
            string[] filePaths = Directory.GetFiles(FileName + "\\", "*.txt", SearchOption.TopDirectoryOnly);
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
                //HARDCODE
                //FileName+"\\" + 
                //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
                //if (!File.Exists(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt"))
                if (!File.Exists(FileName + "\\" + iter + ".txt"))
                {
                    //Console.WriteLine("no existe, crear: "); Console.WriteLine(iter);
                    //FileName+"\\" + 
                    //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
                    //File.Create(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt").Dispose();
                    File.Create(FileName + "\\" + iter + ".txt").Dispose();
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
                        //FileName+"\\" + 
                        //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
                        //File.Delete(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + iter + ".txt");
                        File.Delete(FileName + "\\" + iter + ".txt");
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
                    if (audiosPath.Length == 0)
                    {
                        string[] audiosPath = new string[11];
                        for (int i = 0; i < 11; i++)
                        {
                            audiosPath[i] = " ";
                        }
                    }
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
            //FileName+"\\" + 
            //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
            //audiosPath = System.IO.File.ReadAllLines(@"C:\Users\JONA\Desktop\RADIO QUANTICA\" + cbxProgramas.SelectedItem.ToString() + ".txt");
            audiosPath = System.IO.File.ReadAllLines(FileName + "\\" + cbxProgramas.SelectedItem.ToString() + ".txt");
            //case for empty or recently created files
            
            
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
        /// <summary>
        /// The button that saves Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarConfig_Click(object sender, EventArgs e)
        {
            //HARDCODE
            Console.WriteLine(cbxProgramas.SelectedItem.ToString());
            //FileName+"\\" + 
            //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
            //string fileName = @"C:\Users\JONA\Desktop\RADIO QUANTICA\" + cbxProgramas.SelectedItem.ToString() + ".txt";
            string fileName = FileName + "\\" + cbxProgramas.SelectedItem.ToString() + ".txt";
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
                    for (int i = 0; i <= audiosPath.Length; i++)
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
            //FileName+"\\" + 
            //@"C:\Users\JONA\Desktop\RADIO QUANTICA\\" +
            //Process.Start("notepad.exe", @"C:\Users\JONA\Desktop\RADIO QUANTICA\PROGRAMAS.txt").WaitForExit();
            Process.Start("notepad.exe", FileName + "\\PROGRAMAS.txt").WaitForExit();
            // The ComboBox is cleaned, to display the real existing programs
            cbxProgramas.Items.Clear();
            // Programs are shown in the GUI
            lecturaArchivos("PROGRAMAS.txt");
            // The existence or not of a file is examinated
            verificarExistentes();
        }

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
            m_mediaPlayer.Stop();
            //abrirArchivos(lbl_4, 3);
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
