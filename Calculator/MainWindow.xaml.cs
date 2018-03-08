using System;
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

namespace Calculator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Size windowInitialSize = new Size(300, 400);
		int tbDisplayFontSize = 36;
		int tbHistoryFontSize = 12;
		int buttonsFontSize = 20;

		enum Operation { Addition, Subtraction, Multiplication, Division }

		bool numberInputFinished;
		double? operand1, operand2;
		Operation? operation;

		public MainWindow()
		{
			InitializeComponent();

			this.Height = windowInitialSize.Height;
			this.Width = windowInitialSize.Width;
			tbDisplay.FontSize = tbDisplayFontSize;
			tbHistory.FontSize = tbHistoryFontSize;
			foreach (var element in mainGrid.Children)
			{
				if (element is Button)
					(element as Button).FontSize = buttonsFontSize;
			}

			numberInputFinished = true;
			Reset();
		}

		private void Reset()
		{
			operand1 = null;
			operand2 = null;
			operation = null;
		}

		private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.HeightChanged)
			{
				tbDisplay.FontSize = Math.Round(e.NewSize.Height / windowInitialSize.Height * tbDisplayFontSize);
				tbHistory.FontSize = Math.Round(e.NewSize.Height / windowInitialSize.Height * tbHistoryFontSize);
				foreach (var element in mainGrid.Children)
				{
					if (element is Button)
					{
						(element as Button).FontSize = Math.Round(e.NewSize.Height / windowInitialSize.Height * buttonsFontSize);
					}
				}
			}
		}

		private void btnNumber_Click(object sender, RoutedEventArgs e)
		{
			string numberEntered = (sender as Button).Content.ToString();
			if (tbDisplay.Text == "0" || numberInputFinished)
			{
				tbDisplay.Text = numberEntered;
			}
			else
			{
				tbDisplay.AppendText(numberEntered);
			}
			numberInputFinished = false;
		}

		private void btnPoint_Click(object sender, RoutedEventArgs e)
		{
			if (numberInputFinished)
			{
				tbDisplay.Text = "0.";
			}
			else if (tbDisplay.Text == "-")
			{
				tbDisplay.Text = "-0.";
			}
			else if (!tbDisplay.Text.Contains("."))
			{
				tbDisplay.AppendText(".");
			}
			numberInputFinished = false;
		}

		private void btnBackspace_Click(object sender, RoutedEventArgs e)
		{
			if (tbDisplay.Text.Length > 1)
			{
				tbDisplay.Text = tbDisplay.Text.Remove(tbDisplay.Text.Length - 1, 1);
				numberInputFinished = false;
			}
			else if (tbDisplay.Text.Length == 1)
			{
				tbDisplay.Text = "0";
			}
		}

		private void btnCE_Click(object sender, RoutedEventArgs e)
		{
			tbDisplay.Text = "0";
			numberInputFinished = true;
		}

		private void btnC_Click(object sender, RoutedEventArgs e)
		{
			tbDisplay.Text = "0";
			tbHistory.Text = "";
			numberInputFinished = true;
			Reset();
		}

		private void btnAddition_Click(object sender, RoutedEventArgs e)
		{
			btnEqual_Click(sender, e);
			try
			{
				operand1 = double.Parse(tbDisplay.Text);
				operation = Operation.Addition;
				tbHistory.Text = tbDisplay.Text + " + ";
			}
			catch (Exception ex)
			{
				tbDisplay.Text = "Error";
				Reset();
			}
			numberInputFinished = true;
		}

		private void btnSubtraction_Click(object sender, RoutedEventArgs e)
		{
			if (numberInputFinished && !tbHistory.Text.EndsWith(" = "))
			{
				tbDisplay.Text = "-";
				numberInputFinished = false;
				return;
			}
			btnEqual_Click(sender, e);
			try
			{
				operand1 = double.Parse(tbDisplay.Text);
				operation = Operation.Subtraction;
				tbHistory.Text = tbDisplay.Text + " - ";
			}
			catch (Exception ex)
			{
				tbDisplay.Text = "Error";
				Reset();
			}
			numberInputFinished = true;
		}

		private void btnMultiplication_Click(object sender, RoutedEventArgs e)
		{
			btnEqual_Click(sender, e);
			try
			{
				operand1 = double.Parse(tbDisplay.Text);
				operation = Operation.Multiplication;
				tbHistory.Text = tbDisplay.Text + " * ";
			}
			catch (Exception ex)
			{
				tbDisplay.Text = "Error";
				Reset();
			}
			numberInputFinished = true;
		}

		private void btnDivision_Click(object sender, RoutedEventArgs e)
		{
			btnEqual_Click(sender, e);
			try
			{
				operand1 = double.Parse(tbDisplay.Text);
				operation = Operation.Division;
				tbHistory.Text = tbDisplay.Text + " / ";
			}
			catch (Exception ex)
			{
				tbDisplay.Text = "Error";
				Reset();
			}
			numberInputFinished = true;
		}

		private void btnEqual_Click(object sender, RoutedEventArgs e)
		{
			if (operand1 != null && operation != null && numberInputFinished == false)
			{
				try
				{
					operand2 = double.Parse(tbDisplay.Text);
					tbHistory.AppendText(tbDisplay.Text + " = ");
					switch (operation)
					{
						case Operation.Addition:
							tbDisplay.Text = (operand1 + operand2).ToString();
							break;
						case Operation.Subtraction:
							tbDisplay.Text = (operand1 - operand2).ToString();
							break;
						case Operation.Multiplication:
							tbDisplay.Text = (operand1 * operand2).ToString();
							break;
						case Operation.Division:
							tbDisplay.Text = (operand1 / operand2).ToString();
							break;
					}
					Reset();
				}
				catch (Exception ex)
				{
					tbDisplay.Text = "Error";
					Reset();
				}
				numberInputFinished = true;
			}
		}

	}
}
