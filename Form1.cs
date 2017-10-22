using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;

namespace Sorter
{
    public partial class Form1 : Form
    {
        public int FileCounter = 0;
        public int CurDir = 0;


        //Firstly define 4 arrays of .txt files that will be processed
        FileInfo[] firstArray;
        FileInfo[] secondArray;
        FileInfo[] thirdArray;
        FileInfo[] forthArray;
      
        public Form1()
        {
            InitializeComponent();
        }

      



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {


           
         //Other tasks

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
            progressBar1.Value = e.ProgressPercentage;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            AssignTasks();
         
            button1.Enabled = false;
            button2.Enabled = false;

        }


        void AssignTasks()
        {
            #region SORT
                      
           label1.Text = "(1/5) Sorting, removing duplicates and place mean value for duplicates...";
          
            //Creating Output directory
            DirectoryInfo Dir = new DirectoryInfo(SourcePath.Text);
            Directory.CreateDirectory(SourcePath.Text + @"\Sorted");
            int TotalFiles = Dir.GetFiles().Length;
            if (TotalFiles % 5 != 0)
            error.Visible = true;
                
            //Splitting all files into 4 part to assign to Workers by Array 'Take' and 'Skip'
            FileInfo[] FI = Dir.GetFiles();

            FileInfo[] temp1 = FI.Take(FI.Length / 2).ToArray();
            FileInfo[] temp2 = FI.Skip(FI.Length / 2).ToArray();

            firstArray = temp1.Take(temp1.Length / 2).ToArray();
            secondArray = temp1.Skip(temp1.Length / 2).ToArray();
            thirdArray = temp2.Take(temp2.Length / 2).ToArray();
            forthArray = temp2.Skip(temp2.Length / 2).ToArray();

            //Running Workers
            SortWorker1.RunWorkerAsync(firstArray);
            SortWorker2.RunWorkerAsync(secondArray);
            SortWorker3.RunWorkerAsync(thirdArray);
            SortWorker4.RunWorkerAsync(forthArray);
                   
            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != null)
            {
                SourcePath.Text = folderBrowserDialog1.SelectedPath;

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }

       

        private void button3_Click(object sender, EventArgs e)
        {

            System.IO.StreamReader TotalFile = new System.IO.StreamReader(SourcePath.Text + @"\Result\Total.txt");
            List<string> Freq = new List<string>();
            List<double> Pow = new List<double>();

            string line = "";
            while ((line = TotalFile.ReadLine()) != null)
            {

                Freq.Add(line.Split('\t')[0]);
                Pow.Add(double.Parse(line.Split('\t')[1]));

            }
            TotalFile.Close();


            Plot frm = new Plot();
            frm.Freq = Freq;
            frm.Pow = Pow;
            frm.ShowDialog();
        }

        private void SortWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int Counter = 0;
            int TotalFiles = (e.Argument as FileInfo[]).Length;
            foreach (FileInfo fi in e.Argument as FileInfo[])
            {
                Counter++;
                if (fi.Extension == ".txt")
                {
                    //Some Heavy tasks
                }
                SortWorker1.ReportProgress((Counter * 100) / TotalFiles);
            }
        }

        private void SortWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
           
          
            foreach (FileInfo fi in e.Argument as FileInfo[])
            {
             
                if (fi.Extension == ".txt")
                {
                    string[] scores = File.ReadAllLines(fi.FullName);

                    var orderedScores = Duplicate(scores.ToList()).OrderBy(x => float.Parse(x.Split(' ')[0]));

                    StreamWriter stw = new StreamWriter(SourcePath.Text + @"\Sorted\" + fi.Name);

                    foreach (var score in orderedScores)
                    {
                        stw.WriteLine(score);
                    }

                    stw.Flush();
                    stw.Close();
                }

              

            }
        }
               
        private void SortWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
           
            foreach (FileInfo fi in e.Argument as FileInfo[])
            {
              
                if (fi.Extension == ".txt")
                {
                    //Some Heavy tasks
                }

              
            }
        }

        private void SortWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
           
            foreach (FileInfo fi in e.Argument as FileInfo[])
            {
               
                if (fi.Extension == ".txt")
                {
                    //Some Heavy tasks
                }

               

            }
        }

        private void SortWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        // After all workers finish their job, running another task using one worker
        private void SortWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!SortWorker1.IsBusy &&  !SortWorker2.IsBusy && !SortWorker3.IsBusy && !SortWorker4.IsBusy && !backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(SourcePath.Text);
            }
        }

  

    }
}


