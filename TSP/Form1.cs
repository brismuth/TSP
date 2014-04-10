using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TSP
{
    public partial class Form1 : Form
    {
        private ProblemAndSolver CityData;
        private int problemCounter;
        public Form1()
        {
            InitializeComponent();
            CityData = new ProblemAndSolver();
            GenerateProblem();
        }

        /// <summary>
        /// overloaded to call the redraw method for CityData. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SetClip(new Rectangle(0,0,this.Width, this.Height - this.toolStrip1.Height-35));
            CityData.Draw(e.Graphics);
        }

        private void GenerateProblem()
        {
            if (!Regex.IsMatch(this.tbProblemSize.Text.Trim(), "^[0-9]+$"))
            {
                MessageBox.Show("Problem size must be a positive integer.");

            }
            else if (!Regex.IsMatch(this.txtSeed.Text.Trim(), "^[0-9]*$"))
            {
                MessageBox.Show("Problem seed must be a nonnegative integer (or blank).");
            }
            else
            {
                int problemSize = int.Parse(this.tbProblemSize.Text);
                int seed;
                if (this.txtSeed.Text.Trim() == "")
                {
                    CityData = new ProblemAndSolver(ProblemAndSolver.DEFAULT_SEED,problemSize);
                    lblProblem.Text = "--";
                }
                else if ((seed = int.Parse(this.txtSeed.Text)) != CityData.Seed || problemSize != CityData.Size)
                {
                    CityData = new ProblemAndSolver(seed, problemSize);
                    problemCounter = 1;
                    lblProblem.Text = problemCounter.ToString();
                }
                else
                {
                    CityData.GenerateProblem(problemSize);
                    problemCounter++;
                    lblProblem.Text = problemCounter.ToString();
                }
                this.Invalidate();
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }


        private void btnRun_Click(object sender, EventArgs e)
        {
            CityData.solveProblem();
        }

        private void bNewProblem_Click(object sender, EventArgs e)
        {
            GenerateProblem();
        }


    }
}