using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using LSharp;

namespace LSharpSilverlightRepl
{
    public partial class Page : UserControl
    {
        private SilverlightWriter Out;
        private SilverlightWriter Error;
        private Runtime Runtime;
        private List<string> History;
        private int HistoryPointer;

        public Page()
        {
            InitializeComponent();

            Out = new SilverlightWriter(Write);
            Error = new SilverlightWriter(Write);
            Runtime = new Runtime(null, Out, Error);
            History = new List<string>();
            HistoryPointer = 0;

            this.Loaded += Page_Loaded;
            ConsoleTextBox.LostFocus += ConsoleTextBox_LostFocus;

            Runtime.SilverlighReplInit();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.Plugin.Focus();
            ConsoleTextBox.Focus();
        }

        private void ConsoleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ConsoleTextBox.Focus();
        }

        private void Write(string value)
        {
            ConsoleTextBlock.Text += value;
            ConsoleScrollViewer.ScrollToVerticalOffset(Double.MaxValue);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                ConsoleTextBox.Text = "";
            if (e.Key == Key.Up && HistoryPointer > 0)
            {
                HistoryPointer--;
                if (History.Count > 0)
                {
                    ConsoleTextBox.Text = History[HistoryPointer];
                    ConsoleTextBox.Select(ConsoleTextBox.Text.Length, 0);
                }
                else
                {
                    ConsoleTextBox.Text = "";
                }
            }
            if (e.Key == Key.Down && HistoryPointer < History.Count)
            {
                HistoryPointer++;
                if (HistoryPointer < History.Count)
                {
                    ConsoleTextBox.Text = History[HistoryPointer];
                    ConsoleTextBox.Select(ConsoleTextBox.Text.Length, 0);
                }
                else
                {
                    ConsoleTextBox.Text = "";
                }
            }
            if (e.Key == Key.Enter)
            {
                Write("> " + ConsoleTextBox.Text + "\n");
                Runtime.SilverlighRepl(ConsoleTextBox.Text);
                if (History.Count == 0 || History[History.Count - 1] != ConsoleTextBox.Text)
                    History.Add(ConsoleTextBox.Text);
                HistoryPointer = History.Count;
                ConsoleTextBox.Text = "";
            }
        }
    }
}
