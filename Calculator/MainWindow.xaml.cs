using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		// Initial size of the MainWindow and its elements
		Size windowInitialSize = new Size(300.0, 400.0);
		double tbDisplayInitialFontSize = 36.0;
		double tbHistoryInitialFontSize = 12.0;
		double buttonsInitialFontSize = 20.0;
		Thickness elementsMargin = new Thickness(1.0);

		// Math operations that the app is ablo to do
		enum Operation { Addition, Subtraction, Multiplication, Division }

		// Internal data to support calculation process
		bool numberInputFinished;
		double? operand1, operand2;  // operands of the current math expression
		Operation? operation;  // current operation


		public MainWindow()
		{
			InitializeComponent();

			// Set initial window size
			// This also calls mainWindow_SizeChanged that sets font size of all elements on the window
			this.Height = windowInitialSize.Height;
			this.Width = windowInitialSize.Width;  

			// Set margin for all elements
			tbDisplay.Margin = elementsMargin;
			tbHistory.Margin = elementsMargin;
			foreach (var button in mainGrid.Children.OfType<Button>())
			{
				button.Margin = elementsMargin;
			}

			numberInputFinished = true;
			Reset();
		}

		// Resets previously entered and stored math expressions
		private void Reset()
		{
			operand1 = null;
			operand2 = null;
			operation = null;
		}


		// Ajusts font size of all elements to the size of the form (namely its height) pro rata to the initial values
		private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.HeightChanged)
			{
				tbDisplay.FontSize = Math.Round(e.NewSize.Height / windowInitialSize.Height * tbDisplayInitialFontSize);
				tbHistory.FontSize = Math.Round(e.NewSize.Height / windowInitialSize.Height * tbHistoryInitialFontSize);
				foreach (var button in mainGrid.Children.OfType<Button>())
				{
					button.FontSize = Math.Round(e.NewSize.Height / windowInitialSize.Height * buttonsInitialFontSize);
				}
			}
		}

		// Responds to clicking any of the numbers button (from 0 to 9)
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

		// Insert decimal point
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

		// Clears the currently entered number 
		private void btnCE_Click(object sender, RoutedEventArgs e)
		{
			tbDisplay.Text = "0";
			numberInputFinished = true;
		}

		// Clears both displays and all previously entered data, returning the calculator to its initial state
		private void btnC_Click(object sender, RoutedEventArgs e)
		{
			tbDisplay.Text = "0";
			tbHistory.Text = "";
			numberInputFinished = true;
			Reset();
		}

		// Addition(+): remembers the first operand and waits for the second one
		private void btnAddition_Click(object sender, RoutedEventArgs e)
		{
			btnEqual_Click(sender, e); // when clicked right after the previous math expression was entered, calculates it and uses the result as a first operand for addition 
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

		// Subtraction (-) is twofold:
		// it either adds a negative sign to the number going to be entered,
		// or perform math operation (as other operators buttons do) remembering the first operand and waiting for the second one
		private void btnSubtraction_Click(object sender, RoutedEventArgs e)
		{
			// add a negative sign to the number going to be entered
			if (numberInputFinished && !tbHistory.Text.EndsWith(" = "))
			{
				tbDisplay.Text = "-";
				numberInputFinished = false;
				return;
			}

			btnEqual_Click(sender, e); // when clicked right after the previous math expression was entered, calculates it and uses the result as a first operand for subtraction

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

		// Multiplication(*): remembers the first operand and waits for the second one
		private void btnMultiplication_Click(object sender, RoutedEventArgs e)
		{
			btnEqual_Click(sender, e); // when clicked right after the previous math expression was entered, calculates it and uses the result as a first operand for multiplication
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

		// Division(/): remembers the first operand and waits for the second one
		private void btnDivision_Click(object sender, RoutedEventArgs e)
		{
			btnEqual_Click(sender, e); // when clicked right after the previous math expression was entered, calculates it and uses the result as a first operand for division
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

		// Calculates the math expression entered
		private void btnEqual_Click(object sender, RoutedEventArgs e)
		{
			// 1,2) check if first operand and the operation were entered
			// 3) if numberInputFinished == true, the user didn't even started to enter the second operand
			// if it doesn't meet the abovementioned criteria, '=' button does nothing
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
