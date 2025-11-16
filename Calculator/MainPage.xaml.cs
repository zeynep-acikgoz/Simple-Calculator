namespace Calculator;

public partial class MainPage : ContentPage
{
    private double firstNumber = 0;
    private double secondNumber = 0;
    private string currentOperator = ""; // + - * / =
    private bool isFirstNumberAfterOperator = true;
    private string currentInput = "";
    public MainPage()
    {
        InitializeComponent();
    }


    private void OnNumberPressed(object? sender, EventArgs e)
    {
        Button pressedButton = sender as Button;

        if (pressedButton != null)
        {
            if (isFirstNumberAfterOperator)
            {
                currentInput = pressedButton.Text;
                isFirstNumberAfterOperator = false;
            }
            else
            {
                currentInput = currentInput + pressedButton.Text;
            }
            OperationDisplay.Text = "";

            // Eğer işlem bitmişse (currentOperator boşsa), yeni işlem başlat
            if (currentOperator != "")
            {
                 ResultDisplay.Text = $"{firstNumber} {currentOperator} {currentInput}";
            }
            
            else
            {
                ResultDisplay.Text = currentInput;
            }
            
        
        }
    }

    
    private void OnOperatorPressed(object? sender, EventArgs e)
    {
        Button pressedButton = sender as Button;
        
        
        // *** YENİ: C butonu - Her şeyi temizle ***
        if (pressedButton.Text == "C")
        {
            firstNumber = 0;
            secondNumber = 0;
            currentOperator = "";
            currentInput = "";
            isFirstNumberAfterOperator = true;
            OperationDisplay.Text = "";
            ResultDisplay.Text = "0";
            return;
        }
        
        // *** YENİ: CE butonu - Son haneleri sil (backspace) ***
        if (pressedButton.Text == "CE")
        {
            if (currentInput.Length > 0)
            {
                // Son karakteri sil
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                
                // Eğer hiç karakter kalmadıysa "0" göster
                if (currentInput.Length == 0)
                {
                    if (currentOperator != "")
                    {
                        ResultDisplay.Text = $"{firstNumber} {currentOperator}";
                    }
                    else
                    {
                        ResultDisplay.Text = "0";
                    }
                }
                else
                {
                    // Hala karakter varsa güncelle
                    if (currentOperator != "")
                    {
                        ResultDisplay.Text = $"{firstNumber} {currentOperator} {currentInput}";
                    }
                    else
                    {
                        ResultDisplay.Text = currentInput;
                    }
                }
            }
            return;
        }
        
        // Özel operatörler için erken kontrol
        
        if (pressedButton.Text == "√")
        {
            double number = Double.Parse(currentInput != "" ? currentInput : ResultDisplay.Text);
            double result = Math.Sqrt(number);

            OperationDisplay.Text = $"√({number})";
            ResultDisplay.Text = result.ToString();
            currentOperator = "";
            isFirstNumberAfterOperator = true;
            currentInput = result.ToString();
            return;
        }
        
        if (pressedButton.Text == "x²")
        {
            double number = Double.Parse(currentInput != "" ? currentInput : ResultDisplay.Text);
            double result = number * number;

            OperationDisplay.Text = $"{number}²";
            ResultDisplay.Text = result.ToString();
            currentOperator = "";
            isFirstNumberAfterOperator = true;
            currentInput = result.ToString();
            return;
        }

        if (currentOperator == "" && !isFirstNumberAfterOperator)
        {
            currentOperator = pressedButton.Text;
            firstNumber = Double.Parse(currentInput);
            isFirstNumberAfterOperator = true;
            currentInput = "";
            
            // Üst satırı temizle, alt satırda yeni işlemi göster
            OperationDisplay.Text = "";
            ResultDisplay.Text = $"{firstNumber} {currentOperator}";
            return;
        }

        // *** YENİ: Operatör zaten seçiliyken başka operatöre basılırsa ***
        if (isFirstNumberAfterOperator && currentOperator != "")
        {
            currentOperator = pressedButton.Text;
            
            // Üst satırı temizle, alt satırda güncelle
            OperationDisplay.Text = "";
            ResultDisplay.Text = $"{firstNumber} {currentOperator}";
            return;
        }
        isFirstNumberAfterOperator = true;
        
        
        ////////
        
        
        
        isFirstNumberAfterOperator = true;
        
        // *** YENİ: İlk operatör seçiliyor ***
        if (currentOperator == "")
        {
            currentOperator = pressedButton.Text;
            firstNumber = Double.Parse(currentInput);
            currentInput = "";
            
            // Üst satırı temizle, alt satırda işlemi göster
            OperationDisplay.Text = "";
            ResultDisplay.Text = $"{firstNumber} {currentOperator}";
        }
        else
        {
            // *** YENİ: İşlem hesaplanıyor ***
            secondNumber = Double.Parse(currentInput);
            double result = 0;
            
            switch (currentOperator)
            {
                case "+" :   result = firstNumber + secondNumber; break;
                case "-" :   result = firstNumber - secondNumber; break;
                case "*" :   result = firstNumber * secondNumber; break;
                case "/" :   result = firstNumber / secondNumber; break;
                case "%" :   result = firstNumber % secondNumber; break;
                
                
            }

           
            if (pressedButton.Text == "=")
            {
                // İşlemi ÜSTE taşı, sonucu ALTA yaz
                OperationDisplay.Text = $"{firstNumber} {currentOperator} {secondNumber}";
                ResultDisplay.Text = result.ToString();
                
                currentOperator = "";
                firstNumber = result;
                currentInput = result.ToString();
            }
            else
            {
                // *** YENİ: Başka bir operatör basıldı, işleme devam ***
                firstNumber = result;
                currentOperator = pressedButton.Text;
                currentInput = "";
                
                // Üst satırı temizle, alt satırda yeni işlemi göster
                OperationDisplay.Text = "";
                ResultDisplay.Text = $"{result} {currentOperator}";
            }
        }
    }
    

}
