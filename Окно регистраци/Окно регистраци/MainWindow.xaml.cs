using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.IO;

namespace Окно_регистраци
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TextBoxLogin = (TextBox)sender;
            TextBoxLogin.Text = string.Empty;

            
            
        }

        private void TextBoxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TextBoxPassword = (TextBox)sender;
            TextBoxPassword.Text = string.Empty;
        }

        private void TextBoxRepeatPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TextBoxRepeatPassword = (TextBox)sender;
            TextBoxRepeatPassword.Text = string.Empty;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            //string Value = TextBoxLogin.Text;

            //if (Value.Length < 10)
            //{
            //    MessageBox.Show("Длина логина должна превышать 10 символов");
            //}
            //if (char.IsDigit(Value[0]))
            //{
            //    // Первый символ является цифрой
            //    MessageBox.Show("Первый символ является цифрой.");
            //}

            string login = TextBoxLogin.Text;
            string password = TextBoxPassword.Text;
            string confirmPassword = TextBoxRepeatPassword.Text;
            string email = TextBoxEmail.Text;
            string language = LanguageComboBox.SelectedItem as string;
            bool isSubscribed = NewsCheckBox.IsChecked ?? false;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }

            if (login.Length < 10 || char.IsDigit(login[0]))
            {
                MessageBox.Show("Invalid login. Login must be at least 10 characters long and cannot start with a digit.");
                return;
            }

            if (password == login)
            {
                MessageBox.Show("Password must be different from the login.");
                return;
            }

            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Invalid password. Password can only contain letters and digits.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (isSubscribed && string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please provide an email address for newsletter subscription.");
                return;
            }

            // Запись данных в текстовый файл
            string filePath = "C:\\Users\\Houme-PC\\Source\\Repos\\registration_data.txt";
            string registrationData = $"Login: {login}\nPassword: {password}\nEmail: {email}\nLanguage: {language}\nSubscription: {(isSubscribed ? "Yes" : "No")}";

            try
            {
                File.WriteAllText(filePath, registrationData);
                MessageBox.Show("Registration successful. Data has been saved to a file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the registration data:\n{ex.Message}");
            }
        }
    }
}
