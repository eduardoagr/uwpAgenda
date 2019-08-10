using SQLite;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using uwpContacts.Classes;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpContacts.Dialogs {

    public sealed partial class UserDialog: ContentDialog {
        public UserDialog() {
            InitializeComponent();
        }

        private void TextManipulationChanging(TextBox sender, TextBoxTextChangingEventArgs args) {
            var senderTag = sender as TextBox;

            switch (senderTag.Tag) {

                case "FirstTextBox":
                    TextboxTextValidation(sender, 1);
                    break;
                case "LastNameTextBox":
                    TextboxTextValidation(sender, 1);
                    break;
                case "EmailTextBox":
                    break;
                case "PhoneNumber":
                    TextboxTextValidation(sender, 0);
                    break;
                default:
                    break;
            }
        }

        private static void TextboxTextValidation(TextBox sender, int x) {
            if (x == 1) {
                sender.Text = new string(sender.Text.Where(char.IsLetter).ToArray());
            }
            else {
                sender.Text = new string(sender.Text.Where(char.IsDigit).ToArray());
            }
        }

        private void ContentDialog_SaveClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            Contact contact = new Contact();

            var validEmail = new EmailAddressAttribute().IsValid(EmailTextBox.Text);
            ValidateAndFormat(contact, validEmail);

            using (SQLiteConnection con = new SQLiteConnection(App.databasePath)) {

                if (!string.IsNullOrEmpty(contact.Name)
                    || !string.IsNullOrEmpty(contact.Email)
                    || !string.IsNullOrEmpty(contact.Phone)) {

                    con.CreateTable<Contact>();
                    con.Insert(contact);
                }
            }
        }

        private void ValidateAndFormat(Contact contact, bool validEmail) {

            if (!string.IsNullOrEmpty(FirstTextBox.Text) || !string.IsNullOrEmpty(LastNameTextBox.Text)) {
                contact.FirstName = ToUpperCase(FirstTextBox.Text);
                contact.LastName = ToUpperCase(LastNameTextBox.Text);

                contact.Name = $"{contact.FirstName} {contact.LastName}";
            }
            if (validEmail) {
                contact.Email = EmailTextBox.Text;
            }
            if (!string.IsNullOrEmpty(PhoneNumber.Text)) {
                contact.Phone = string.Format("{0:(###) ###-####}", Convert.ToInt64(PhoneNumber.Text));
            }
        }
        private string ToUpperCase(string word) {
            if (!string.IsNullOrEmpty(word)) {
                char[] letters = word.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                return new string(letters);
            }
            return null;
        }
    }
}
