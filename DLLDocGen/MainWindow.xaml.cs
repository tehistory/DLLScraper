﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLLDocGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBoxIn.Text != null && textBoxOut.Text != null)
                {
                    Generator myGen = new Generator(textBoxIn.Text.ToString(), textBoxOut.Text.ToString(), true);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Attempt failed");
            }
            
        }
    }
}
