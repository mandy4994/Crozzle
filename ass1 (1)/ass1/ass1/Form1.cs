using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace ass1
{
    public partial class Form1 : Form
    {
        int width, height, wordCount = 0;
        string level = "";
        const int MINIMUM_WORDS = 10;
        const int MAXIMUM_WORDS = 1000;
        const int MINIMUM_ROWS = 4;
        const int MAXIMUM_ROWS = 400;
        const int MINIMUM_COLS = 4;
        const int MAXIMUM_COLS = 400;
        List<string> WordList = new List<string>();       
        List<WordCoordinates> WordCoordinatesList = new List<WordCoordinates>();
        public Form1()
        {

            InitializeComponent();
        }
        /// <summary>
        /// Converts string to number
        /// </summary>
        /// <param name="s">string value</param>
        /// <returns>integer number</returns>
        public int StringToNumber(string s)
        {
            int retval = 0;
            retval = char.ToUpper(s[0]) - 64;
            return retval;
        }

        /// <summary>
        /// Checks for word in datagrid (both row and column) and returns score (10 score) accordingly
        /// </summary>
        /// <param name="datagridview"></param>
        /// <param name="list">Wordlist</param>
        /// <returns>integer score</returns>
        int FindWordScore(DataGridView datagridview, List<string> list )
        {
            int scoreRow = 0;
            int scoreCol = 0;
            int score = 0;
            foreach (DataGridViewRow dr in datagridview.Rows) // Find words in rows first
            {
                int count = 0;
                string word = "";
                for (int i = 0; i < dr.Cells.Count; i++)
                {
                    
                    if(dr.Cells[i].Value != null)
                    {
                        string dcString = dr.Cells[i].Value.ToString();
                        if (dcString != " ")
                        {
                            count++;
                            word += dcString;
                            if (count > 1) // min length of name is 2
                            {
                                foreach (string s in list)
                                {
                                    if (s.Equals(word))
                                    {
                                        if (i == dr.Cells.Count - 1)
                                        {
                                            scoreRow += 10;
                                            break;
                                        }
                                        else
                                        {
                                            if (dr.Cells[i + 1].Value.ToString() == " ")
                                            {
                                                scoreRow += 10;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            count = 0;
                            word = "";
                        }
                    }
                }
            
            
            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++) // find words in columns
            {
                int count = 0;
                string word = "";
                for(int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                
                    

                
                        if (dataGridView1[i,j].Value != null)
                        {
                            string dcString = dataGridView1[i,j].Value.ToString();
                            if (dcString != " ")
                            {
                                count++;
                                word += dcString;
                                if (count > 1) // min length of name is 2
                                {
                                    foreach (string s in list)
                                    {
                                        if (s.Equals(word))
                                        {
                                            if (j == dataGridView1.Rows.Count - 1)
                                            {
                                                scoreCol += 10;
                                                break;
                                            }
                                            else
                                            {
                                                if (dataGridView1[i, j + 1].Value.ToString() == " ")
                                                {
                                                    scoreCol += 10;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                count = 0;
                                word = "";
                            }
                        }
                    }
                


            }
            score = scoreRow + scoreCol;
            return score;
        }

        /// <summary>
        /// Returns score from Intersecting alphabet
        /// </summary>
        /// <param name="datagridview"></param>
        /// <returns>score as integer</returns>
        int ScoreIntersection(DataGridView datagridview)
        {
            int score = 0;
            for (int i = 0; i < datagridview.Rows.Count; i++) //Iterating through rows
            {

                    //if (datagridview.Rows[i].Cells[i].Value != null && datagridview.Rows[i].Cells[i].Value.ToString() != " ")
                    //{
                    //    if ((datagridview.Rows[i - 1].Cells[i].Value != null && datagridview.Rows[i - 1].Cells[i].Value.ToString() != " ") ||
                    //        (datagridview.Rows[i + 1].Cells[i].Value != null && datagridview.Rows[i + 1].Cells[i].Value.ToString() != " ") ||
                    //        (datagridview.Rows[i].Cells[i + 1].Value != null && datagridview.Rows[i].Cells[i + 1].Value.ToString() != " ") ||
                    //        (datagridview.Rows[i].Cells[i - 1].Value != null && datagridview.Rows[i].Cells[i - 1].Value.ToString() != " "))
                    //    {
                    //        int letterScore = GetScoreByLetter(datagridview.Rows[i].Cells[i].Value.ToString());
                    //        score += letterScore;
                            
                    //    }

                    //}
                for (int j = 0; j < datagridview.Columns.Count; j++) //Iterating through each column
                {
                    if (datagridview[j, i].Value.ToString() != " ")
                    {
                        //Top left corner
                        if (i == 0 && j == 0)
                        {
                            if ((datagridview[j + 1, i].Value.ToString() != " ") && (datagridview[j, i + 1].Value.ToString() != " "))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }
                        // Bottom right corner
                        else if (i == datagridview.Rows.Count - 1 && j == datagridview.Columns.Count - 1)
                        {
                            if ((datagridview[j - 1, i].Value.ToString() != " ") && (datagridview[j, i - 1].Value.ToString() != " "))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }
                        // Top right corner
                        else if (i == 0 && j == datagridview.Columns.Count - 1)
                        {
                            if ((datagridview[j - 1, i].Value.ToString() != " ") && (datagridview[j, i + 1].Value.ToString() != " "))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }

                                // Bottom left corner
                        else if (i == datagridview.Rows.Count - 1 && j == 0)
                        {
                            if ((datagridview[j + 1, i].Value.ToString() != " ") && (datagridview[j, i - 1].Value.ToString() != " "))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }
                        //Right Edge
                        else if (j == datagridview.Columns.Count - 1 && i != 0 && i != datagridview.Rows.Count - 1)
                        {
                            if ((datagridview[j - 1, i].Value.ToString() != " ") && ((datagridview[j, i - 1].Value.ToString() != " ") || (datagridview[j, i + 1].Value.ToString() != " ")))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }
                        //Bottom edge
                        else if (i == datagridview.Rows.Count - 1 && j != datagridview.Columns.Count - 1 && j != 0)
                        {
                            if ((datagridview[j, i - 1].Value.ToString() != " ") && ((datagridview[j - 1, i].Value.ToString() != " ") || (datagridview[j + 1, i].Value.ToString() != " ")))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }
                        // Top Edge
                        else if (i == 0 && j != 0 && j != datagridview.Columns.Count - 1)
                        {
                            if ((datagridview[j, i + 1].Value.ToString() != " ") && ((datagridview[j - 1, i].Value.ToString() != " ") || (datagridview[j + 1, i].Value.ToString() != " ")))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }
                        }
                        // Left Edge
                        else if (j == 0 && i != 0 && i != datagridview.Rows.Count - 1)
                        {
                            if ((datagridview[j + 1, i].Value.ToString() != " ") && ((datagridview[j, i - 1].Value.ToString() != " ") || (datagridview[j, i + 1].Value.ToString() != " ")))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }

                        }
                        //Middle
                        else
                        {
                            if (((datagridview[j + 1, i].Value.ToString() != " ") || (datagridview[j - 1, i].Value.ToString() != " ")) && ((datagridview[j, i - 1].Value.ToString() != " ") || (datagridview[j, i + 1].Value.ToString() != " ")))
                            {
                                score += GetScoreByLetter(datagridview[j, i].Value.ToString());
                            }


                        }
                    }
                }
                                   
            }


                return score;
        }

        /// <summary>
        /// Method which returns score according to the alphabet
        /// </summary>
        /// <param name="p">string as alphabet passed to get score</param>
        /// <returns>integer score</returns>
        private int GetScoreByLetter(string p)
        {
            int score = 0;
            switch(p.ToUpper())
            {
                case "A":
                case "E":
                case "I":
                case "O":
                case "U": 
                    score = 1;
                    break;

                case "B":
                case "C":
                case "D":
                case "F":
                case "G":
                    score = 2;
                    break;

                case "H":
                case "J":
                case "K":
                case "L":
                case "M":
                    score = 4;
                    break;

                case "N":
                case "P":
                case "Q":
                case "R":
                    score = 8;
                    break;

                case "S":
                case "T":
                case "V":
                    score = 16;
                    break;

                case "W":
                case "X":
                case "Y":
                    score = 32;
                    break;

                case "Z":
                    score = 64;
                    break;

            }
            return score;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    LoadCrozzle();
        //}

        /// <summary>
        /// Method which loads crozzle in a datagrid
        /// </summary>
        private void LoadCrozzle()
        { 
            OpenFileDialog dialog = new OpenFileDialog();   
            dialog.Filter = "txt files (*.txt)|*.txt";
            DialogResult result = dialog.ShowDialog();
            Char[] separator = { '\r' };
            string filename = "";
            String[] rows = null;
            if (result == DialogResult.OK)
            {
                filename = dialog.FileName;
                StreamReader sr = new StreamReader(filename);
                //while (sr.EndOfStream == false)
                //{
                //    line = sr.ReadLine().Split(separator);

                //}
                //char[] delimiter = new char[] { '\r' };
                string AllData = sr.ReadToEnd();
                rows = AllData.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                
            }

            if (ValidateCrozzle(rows, filename) == false)   //-------------------Validates crozzle file------------------------------
            {
                lblError.Text = "Error in Crozzle File";
                return;
            }
            string tablename = "Crozzle";
            DataSet dataset = new DataSet();
            dataset.Tables.Add(tablename);

            for (int i = 0; i < height; i++)
            {
                dataset.Tables[tablename].Columns.Add(); 
                
            }


                foreach (string r in rows)
                {
                    string[] items = new string[r.Length];
                    for (int i = 0; i < r.Length; i++)
                    {
                        items[i] = r[i].ToString();
                    }
                    dataset.Tables[tablename].Rows.Add(items);
                
                }

            
            this.dataGridView1.DataSource = dataset.Tables[0].DefaultView;
            
            foreach (DataGridViewColumn c in dataGridView1.Columns)
                c.Width = dataGridView1.Width / dataGridView1.Columns.Count;
            foreach (DataGridViewRow c in dataGridView1.Rows)
                c.Height = dataGridView1.Height / dataGridView1.Rows.Count;

            int score = 0;
            if (level.ToUpper() == "EASY")
            {
                score = CalculateScoreEasy(dataGridView1);
                
            }
            else if (level.ToUpper() == "MEDIUM")
            {
                score = CalculateScoreMedium(dataGridView1);
            }
            else if (level.ToUpper() == "HARD")
            {
                score =  CalculateScoreMedium(dataGridView1) + FindWordScore(dataGridView1, WordList);
            }

            else if (level.ToUpper() == "EXTREME")
            {
                score = FindWordScore(dataGridView1, WordList) + ScoreIntersection(dataGridView1);
            }
            //for (int i = 0; i < width; i++)
            //{
            //    dataGridView1.Columns[i].Width = 20;
            //}
            lblScoreNo.Text = score.ToString();
        }

        /// <summary>
        /// Method ehich validates crozzle file
        /// </summary>
        /// <param name="rows">String array of rows in crozzle</param>
        /// <param name="filename"></param>
        /// <returns>bool value ; false if invalid</returns>
        private bool ValidateCrozzle(string[] rows, string filename)
        {
            int errorCount = 0;
            StreamWriter logfile = new StreamWriter("log.txt", true);
            //check for correct number of rows and columns in crozzle
            if (rows.Length != width || rows[0].Length != height)
            {
                errorCount++;
                logfile.WriteLine(DateTime.Now + " - Number of Rows or Columns don't match with their respective values in WordList file");
                //logfile.Close();
                //return false;
            }

            //foreach(string r in rows)
            //{
                for (int i = 0; i < rows.Length; i++ )
                {
                    if (!Regex.IsMatch(rows[i], "^[A-Za-z \t]+$"))
                    {
                        Console.WriteLine(rows[i].ToString());
                        errorCount++;
                        logfile.WriteLine(DateTime.Now + " - Invalid Character in Crozzle");
                        //logfile.Close();
                        //return false;
                    }
                }
            //}
            logfile.Close();
            if (errorCount > 0)
                return false;
            else
            return true;
        }

        /// <summary>
        /// Method to load wordlist file
        /// </summary>
        private void LoadWordList()
        {
            WordList.Clear();
            textBox1.Text = "";
            lblError.Text = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "csv files (*.csv)|*.csv";
            DialogResult result = dialog.ShowDialog();           
            string filename = "";
            
            if (result == DialogResult.OK)
            {
                filename = dialog.FileName;
                ValidateWordList(filename);     //======================================Validating Wordlist===========================================
                

            }
            
        
        
        }

        /// <summary>
        /// Function to Validate WordList
        /// </summary>
        /// <param name="filename"></param>
        private void ValidateWordList(string filename)
        {
            int errorCount = 0;
            String[] line = null;
            int n;
            Char[] separator = { ',' };
            StreamReader sr = new StreamReader(filename);
            StreamWriter logfile = new StreamWriter("log.txt", true);
            
            while (sr.EndOfStream == false)
            {
                line = sr.ReadLine().Split(separator);

            }
            //========= Validating Header ========

            // Checking for empty fields
            for (int i = 0; i < 4; i++)
            {
                if (line[i].Length == 0)
                {
                    errorCount++;
                    logfile.WriteLine(DateTime.Now + " - Field "+ (i+1) +" is empty in header");
                }            
            }

            // Checking Number of Words Field in integer and in range
            if (Int32.TryParse(line[0], out n))
            {
                if (n < MINIMUM_WORDS || n > MAXIMUM_WORDS)
                {
                    errorCount++;
                    logfile.WriteLine(DateTime.Now + " - Number of Words are out of range");
                }
                
            }
            else
            {
                errorCount++;
                logfile.WriteLine(DateTime.Now + " - Number of Words value is not an integer");
            }

            // Checking Row and Column fields are integers and in range

            if (Int32.TryParse(line[1], out n))
            {
                if (n < MINIMUM_ROWS || n > MAXIMUM_ROWS)
                {
                    errorCount++;
                    logfile.WriteLine(DateTime.Now + " - Number of Rows are out of range");
                }
                
            }
            else
            {
                errorCount++;
                logfile.WriteLine(DateTime.Now + " - Number of Rows value is not an integer");
            }

            if (Int32.TryParse(line[2], out n))
            {
                if (n < MINIMUM_COLS || n > MAXIMUM_COLS)
                {
                    errorCount++;
                    logfile.WriteLine(DateTime.Now + " - Number of Columns are out of range");
                }
                
            }
            else
            {
                errorCount++;
                logfile.WriteLine(DateTime.Now + " - Number of Columns value is not an integer");
            }


            //Validating difficulty level value

            if (!(line[3].ToString().ToUpper() == "EASY" || line[3].ToString().ToUpper() == "MEDIUM" || line[3].ToString().ToUpper() == "HARD" || line[3].ToString().ToUpper() == "EXTREME"))
            {
                errorCount++;
                logfile.WriteLine(DateTime.Now + " - Difficulty value is not valid");
            }

            if (errorCount > 0)
            {
                logfile.Close();
                lblError.Text = "Error in Wordlist";
                return;
            }
            else
            {
                wordCount = int.Parse(line[0]);
                width = int.Parse(line[1]);
                height = int.Parse(line[2]);
                level = line[3];

                //Checking if number of words are correct
                if (wordCount != line.Count() - 4)
                {
                    errorCount++;
                    logfile.WriteLine(DateTime.Now + " - Number of words dont match the field value");
                }

                //Check if words match regular expression
                for (int i = 4; i < line.Count(); i++)
                {
                    int counter = 0;
                    if (Regex.IsMatch(line[i], "^[a-zA-Z]+$"))
                    {
                        // Check if words are getting repeated
                        if (WordList.Contains(line[i]))
                        {
                            errorCount++;
                            logfile.WriteLine(DateTime.Now + " - Word " + line[i] + " is repeated");
                            //logfile.Close();
                            //textBox1.Clear();
                            lblError.Text = "Error in WordList";
                            //return;
                        }
                        else
                        {
                            WordList.Add(line[i]);
                            counter++;
                            textBox1.Text += line[i] + " \r\n";
                        }
                    }
                    else
                    {
                        textBox1.Clear();
                        errorCount++;
                        logfile.WriteLine(DateTime.Now + " - "+ line[i].ToString()+" is not a valid word");
                        return;
                    }
                }
            }
            logfile.Close();
        }

        //public enum AlphaNumeric : int { A = 1, B = 2, C=3, D=4, E=5, F=6, G=7, H=8, I=9, J=10, K=11, L=12,
        //M = 13, N = 14, O = 15, P = 16, Q = 17, R = 18, S = 19, T= 20, U = 21, V = 22, W = 23, X = 24,
        //Y = 25, Z = 26};

        //public enum AlphaNumeric { A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z}


        /// <summary>
        /// Function for calculating score for easy level
        /// </summary>
        /// <param name="datagridview"></param>
        /// <returns>score as integer</returns>
        int CalculateScoreEasy(DataGridView datagridview)
        {
            int score = 0;

            // Easy (Valid) Crozzle Score Calculation
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    if (dc.Value != null)
                    {
                        if (dc.Value.ToString() != " ")
                            score += 1;
                    }
                }
            }
            
            


            return score;
        
        }
        /// <summary>
        /// Function for calculating score for medium difficulty
        /// </summary>
        /// <param name="datagridview"></param>
        /// <returns>score as integer</returns>
        int CalculateScoreMedium(DataGridView datagridview)
        {
            int score = 0;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    if (dc.Value != null)
                    {
                        string dcString = dc.Value.ToString();
                        if (dcString != " ")
                        {
                            score += StringToNumber(dcString);
                        }

                    }
                }
            }

            return score;
        
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    LoadWordList();
        //}


        private void loadWordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblScoreNo.Text = "";
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            LoadWordList();
            loadCrozzleToolStripMenuItem.Enabled = true;
            loadWordListToolStripMenuItem.Enabled = false;
        }

        private void loadCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadWordListToolStripMenuItem.Enabled = true;
            LoadCrozzle();
            loadCrozzleToolStripMenuItem.Enabled = false;

        }

        /// <summary>
        /// Finds and Checks every word in crozzle row wise and column wise if its present in wordlist
        /// </summary>
        /// <returns> bool value ; false if not found</returns>
        private bool ValidateWordFind()
        {
            string errors = "";
            List<string> CrozzleWordsFound = new List<string>();
            List<string> WordListWords = WordList;
            bool found = true;
            StreamWriter logfile = new StreamWriter("log.txt", true);
            for(int j = 0; j < dataGridView1.Rows.Count; j++)       //Iterating each row first
            {
                int count = 0;
                string word = "";
                for (int i = 0; i < dataGridView1.Columns.Count; i++)       //Checking each cell of the row
                {

                    if (dataGridView1[i,j].Value != null)
                    {
                        string dcString = dataGridView1[i,j].Value.ToString();
                        if (dcString != " ")
                        {
                            count++;
                            word += dcString;                           // word stores the value of DataCell if its not empty
                            if (count > 1) // min length of name is 2
                            {                                

                                        if ((i == dataGridView1.Columns.Count - 1) || (dataGridView1[i + 1,j].Value.ToString() == " "))
                                        {
                                            if ((WordListWords.Contains(word)))
                                            {
                                                WordCoordinatesList.Add(new WordCoordinates(word, j, i, true));
                                                if (CrozzleWordsFound.Contains(word))
                                                {
                                                    errors += word+" repeated. \n";
                                                    found = false;
                                                }
                                                else
                                                {
                                                    CrozzleWordsFound.Add(word);
                                                    //WordListWords.Remove(word);
                                                }
                                                //return false;
                                            }
                                            else
                                            {
                                                errors += word + " found in crozzle but not in wordlist. \n";
                                                found = false;
                                            }
                                        }
                                    }
                                
                            }
                        
                        else
                        {
                            count = 0;
                            word = "";
                        }
                    }
                }


            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++)       //Looping through Each column
            {
                int count = 0;
                string word = "";
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {




                    if (dataGridView1[i, j].Value != null)
                    {
                        string dcString = dataGridView1[i, j].Value.ToString();
                        if (dcString != " ")
                        {
                            count++;
                            word += dcString;
                            if (count > 1) // min length of name is 2
                            {
                                if ((j == dataGridView1.Rows.Count - 1) || (dataGridView1[i, j + 1].Value.ToString() == " "))
                                        {
                                            if ((WordListWords.Contains(word)))
                                            {
                                                WordCoordinatesList.Add(new WordCoordinates(word, j, i, false));
                                                if (CrozzleWordsFound.Contains(word))
                                                {
                                                    errors += word + " repeated. \n";
                                                    found = false;
                                                }
                                                else
                                                {
                                                    CrozzleWordsFound.Add(word);
                                                    //WordListWords.Remove(word);
                                                }
                                                //return false;
                                            }
                                            else
                                            {
                                                errors += word + " found in crozzle but not in wordlist. \n";
                                                found = false;
                                            }
                                        }                                                                  
                            }
                        }
                        else
                        {
                            count = 0;
                            word = "";
                        }
                    }
                }

                

            }
            logfile.WriteLine(errors);
            logfile.Close();
            return found;
        }

        private void validateCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Constraint 1 and 2
                if(ValidateWordFind() == false)
                    lblError.Text = "Word not found in wordlist";

                //ValidateE4E5();

                if (level.ToUpper() == "MEDIUM" || level.ToUpper() == "HARD")
                {
                    if (ValidateIntersections() != "") lblError.Text = "Error Check Log File";
                }

                //if (ValidateC4C5() != "") lblError.Text = "Error Check Log File";
            
        }

        
        /// <summary>
        /// Constraint for checking horizontal word intersecting number of vertical words and vice versa
        /// </summary>
        /// <returns>Error as string ; empty string if its valid</returns>
        private string ValidateIntersections()
        {
            StreamWriter logFile = new StreamWriter("log.txt", true);
            int count = 0;
            string error = "";
            foreach (WordCoordinates w in WordCoordinatesList)
            {
                count = 0;
                if (w.horizontal == true)
                {
                    
                    for (int i = (w.y - (w.word.Length-1)); i <= (w.y); i++)
                    {
                        // if word is in Middle
                        if (w.x != 0 && w.x != dataGridView1.Rows.Count - 1)
                        {
                            if (dataGridView1[i, (w.x - 1)].Value.ToString() != " " || dataGridView1[i, (w.x + 1)].Value.ToString() != " ")
                            {
                                count++;                                

                            }
                        }

                        //if word is on Top
                        else if (w.x == 0)
                        {
                            if (dataGridView1[i, (w.x + 1)].Value.ToString() != " ")
                            {
                                count++;                               

                            }
                        }
                        
                            // if word is at Bottom
                        else if (w.x == dataGridView1.Rows.Count - 1)
                        {
                            //if (w.x != 0 && w.x != dataGridView1.Rows.Count - 1)
                            //{
                                if (dataGridView1[i, (w.x - 1)].Value.ToString() != " ")
                                {
                                    count++;

                                }
                            //}
                        
                        }
                    }

                    if (level.ToUpper()=="MEDIUM")
                    {
                        if (count > 2)
                        {
                            error += "\n " + w.word + " intersects more than 2 words";
                            //break;
                        }
                        else if (count == 0)
                        {
                            error += "\n " + w.word + " intersects 0 words";
                            //break;
                        }
                    }
                    else if (level.ToUpper() == "HARD")
                    {
                        if (count == 0)
                        {
                            error += "\n " + w.word + " intersects 0 words";
                            //break;
                        }
                    }


                }
                else            //Check for Vertical words
                {
                    for (int i = (w.x - (w.word.Length - 1)); i <= (w.x); i++)
                    {
                        //Middle
                        if (w.y != 0 && w.y != dataGridView1.Columns.Count - 1)
                        {
                            if (dataGridView1[(w.y - 1), i].Value.ToString() != " " || dataGridView1[(w.y + 1), i].Value.ToString() != " ")
                            {
                                count++;

                            }
                        }

                        //Top
                        else if (w.y == 0)
                        {
                            if (dataGridView1[(w.y+1),i].Value.ToString() != " ")
                            {
                                count++;

                            }
                        }

                        else if (w.y == dataGridView1.Columns.Count - 1)
                        {
                            //if (w.y != 0 && w.x != dataGridView1.Rows.Count - 1)
                            //{
                                if (dataGridView1[(w.y-1),i].Value.ToString() != " ")
                                {
                                    count++;

                                }
                            //}

                        }
                    }

                    if (level.ToUpper() == "MEDIUM")
                    {
                        if (count > 2)
                        {
                            error += "\n " + w.word + " intersects more than 2 words";
                            //break;
                        }
                        else if (count == 0)
                        {
                            error += "\n " + w.word + " intersects 0 words";
                            //break;
                        }
                    }
                    else if (level.ToUpper() == "HARD")
                    {
                        if (count == 0)
                        {
                            error += "\n " + w.word + " intersects 0 words";
                            //break;
                        }
                    }

                }
            }
            logFile.WriteLine(error);
            logFile.Close();
            return error;
        }


    }
}
