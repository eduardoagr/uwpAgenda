using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using uwpContacts.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpContacts.Dialogs {
    public sealed partial class DetailsDialog: ContentDialog {

        Contact contact;

        public DetailsDialog(Contact contact) {
            InitializeComponent();

            this.contact = contact;

            if (!string.IsNullOrEmpty(contact.FirstName)) {
                FirstTextBox.Text = contact.FirstName;
            }
            if (!string.IsNullOrEmpty(contact.LastName)) {
                LastNameTextBox.Text = contact.LastName;
            }
            if (!string.IsNullOrEmpty(contact.Email)) {
                EmailTextBox.Text = contact.Email;
            }
            if (!string.IsNullOrEmpty(contact.Phone)) {
                PhoneNumber.Text = contact.Phone;
            }


        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {

            //Update

            contact.FirstName = FirstTextBox.Text;
            contact.Name = $"{contact.FirstName} {contact.LastName}";
            contact.Email = EmailTextBox.Text;
            contact.Phone = PhoneNumber.Text;

            using (SQLiteConnection con = new SQLiteConnection(App.databasePath)) {

                con.CreateTable<Contact>();
                con.Update(contact);
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {

            Debug.WriteLine(contact);

            //Delete
            using (SQLiteConnection con = new SQLiteConnection(App.databasePath)) {

                con.CreateTable<Contact>();
                con.Delete(contact);
            }
        }

        private void TextManipulationChanging(TextBox sender, TextBoxTextChangingEventArgs args) {
            var senderTag = sender as TextBox;

            switch (senderTag.Tag) {

                case "FirstTextBox":
                    break;
                case "LastNameTextBox":
                    break;
                case "EmailTextBox":
                    break;
                case "PhoneNumber":
                    TextboxTextValidation(sender);
                    break;
                default:
                    break;
            }
        }

        private static void TextboxTextValidation(TextBox sender ) {

            sender.Text = new string(sender.Text.Where(char.IsDigit).ToArray());

        }

    }
}