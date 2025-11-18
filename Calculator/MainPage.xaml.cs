using System;
using System.Globalization; 
using Microsoft.Maui.Controls;

namespace Calculator;

public partial class MainPage : ContentPage
{
    private readonly CultureInfo culture = CultureInfo.InvariantCulture; 

    private string currentInput = "0";      
    private decimal storedValue = 0;        
    private string currentOperator = "";    
    private bool isNewEntry = true;         

    public MainPage()
    {
        InitializeComponent();
        ResultDisplay.Text = "0";
    }

    private void OnNumberPressed(object sender, EventArgs e)
    {
        var button = sender as Button;
        string number = button.Text;

        if (isNewEntry)
        {
            currentInput = "";
            isNewEntry = false;
        }

        if (number == "." && currentInput.Contains("."))
            return;

        currentInput += number;
        ResultDisplay.Text = currentInput;
    }

    private void OnOperatorPressed(object sender, EventArgs e)
    {
        var button = sender as Button;
        string op = button.Text.Trim();

        switch (op)
        {
            case "C":
                currentInput = "0";
                storedValue = 0;
                currentOperator = "";
                ResultDisplay.Text = "0";
                OperationDisplay.Text = "";
                isNewEntry = true;
                break;

            case "CE":
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                if (currentInput == "")
                    currentInput = "0";
                ResultDisplay.Text = currentInput;
                break;

            case "+/-":
                if (currentInput.StartsWith("-"))
                    currentInput = currentInput.Substring(1);
                else if (currentInput != "0")
                    currentInput = "-" + currentInput;
                ResultDisplay.Text = currentInput;
                break;

            case ".":
                if (!currentInput.Contains("."))
                    currentInput += ".";
                ResultDisplay.Text = currentInput;
                break;

            case "√":
                decimal sqrtVal = (decimal)Math.Sqrt((double)decimal.Parse(currentInput, culture));
                OperationDisplay.Text = $"√({currentInput})";
                currentInput = sqrtVal.ToString(culture); 
                ResultDisplay.Text = currentInput;
                isNewEntry = true;
                break;

            case "x²":
                decimal sqVal = (decimal)Math.Pow((double)decimal.Parse(currentInput, culture), 2);
                OperationDisplay.Text = $"({currentInput})²";
                currentInput = sqVal.ToString(culture); 
                ResultDisplay.Text = currentInput;
                isNewEntry = true;
                break;

            case "%":
            case "+":
            case "-":
            case "*":
            case "/":
                ProcessOperator(op);
                break;

            case "=":
                CalculateResult();
                currentOperator = "";
                isNewEntry = true;
                break;
        }
    }

    private void ProcessOperator(string op)
    {
        if (!string.IsNullOrEmpty(currentOperator) && !isNewEntry)
        {
            CalculateResult();
        }
        else if (string.IsNullOrEmpty(currentOperator))
        {
            storedValue = decimal.Parse(currentInput, culture);
        }
        
        currentOperator = op;
        OperationDisplay.Text = $"{storedValue.ToString(culture)} {currentOperator}";
        isNewEntry = true;
    }

    private void CalculateResult()
    {
        if (string.IsNullOrEmpty(currentOperator))
            return;
            
        decimal currentValue = decimal.Parse(currentInput, culture);
        decimal result = storedValue;

        string opTextBeforeCalculation = $"{storedValue.ToString(culture)} {currentOperator} {currentValue.ToString(culture)}";

        switch (currentOperator)
        {
            case "+":
                result = storedValue + currentValue;
                break;
            case "-":
                result = storedValue - currentValue;
                break;
            case "*":
                result = storedValue * currentValue;
                break;
            case "/":
                if (currentValue != 0)
                    result = storedValue / currentValue;
                else
                {
                    ResultDisplay.Text = "Error: Division by zero";
                    OperationDisplay.Text = "";
                    currentInput = "0";
                    storedValue = 0;
                    currentOperator = "";
                    isNewEntry = true;
                    return;
                }
                break;
            case "%":
                if (currentValue != 0)
                {
                    long intStored = (long)Math.Round(storedValue);
                    long intCurrent = (long)Math.Round(currentValue);
                    result = intStored % intCurrent;
                }
                else
                {
                    ResultDisplay.Text = "Error: Division by zero";
                    OperationDisplay.Text = "";
                    currentInput = "0";
                    storedValue = 0;
                    currentOperator = "";
                    isNewEntry = true;
                    return;
                }
                break;
        }

        string resultToDisplay;

        if (result == Math.Round(result))
        {
            resultToDisplay = ((long)result).ToString(culture);
        }
        else
        {
            resultToDisplay = result.ToString(culture);
        }

        OperationDisplay.Text = $"{opTextBeforeCalculation} ="; 
        
        ResultDisplay.Text = resultToDisplay;
        currentInput = result.ToString(culture);
        storedValue = result;
        isNewEntry = true;
    }
}