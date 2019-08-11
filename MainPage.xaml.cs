using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using uwpContacts.Classes;
using uwpContacts.Dialogs;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uwpContacts {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage: Page {

        List<Contact> contactList;

        public MainPage() {
            InitializeComponent();

            contactList = new List<Contact>();
            ReadDB();
        }

        private async void NewContact_Click(object sender, RoutedEventArgs e) {

            ContentDialog dialog = new UserDialog() {

                FullSizeDesired = true,
                MaxWidth = ActualWidth // Required for Mobile!
            };
            await dialog.ShowAsync();

            ReadDB();
        }

        private void ReadDB() {

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath)) {

                connection.CreateTable<Contact>();
                contactList = connection.Table<Contact>().ToList().OrderBy(c => c.Name).ToList();

                /*
                var list = (from c2 in contactList
                           orderby c2.Name
                           select c2).ToList();
                */

                if (contactList != null) {
                    contactListView.ItemsSource = contactList;
                }
            }
        }

        private async void ContactListView_ItemClick(object sender, ItemClickEventArgs e) {

            var contactImformation = e.ClickedItem as Contact;

            ContentDialog dialog = new ContentDialog() {
                Content = $"You are about to call {contactImformation.Name}",
                PrimaryButtonText = "Call",
                SecondaryButtonText = "Edit Information"
            };

            var res = await dialog.ShowAsync();

            switch (res) {
                case ContentDialogResult.None:
                    break;
                case ContentDialogResult.Primary:
                    await Launcher.LaunchUriAsync(new Uri($"tel:+1{contactImformation.Phone}"));
                    break;
                case ContentDialogResult.Secondary:
                    if (contactImformation != null) {
                        ContentDialog contentDialog = new DetailsDialog(contactImformation) {
                            MaxHeight = ActualHeight,
                            MaxWidth = ActualWidth
                        };
                        var result = await contentDialog.ShowAsync();

                        switch (result) {
                            case ContentDialogResult.None:
                                break;
                            case ContentDialogResult.Primary:
                                ReadDB();
                                break;
                            case ContentDialogResult.Secondary:
                                ReadDB();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
        }
        private void SearchText_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {

            var text = sender as AutoSuggestBox;

            var filteredContact = contactList.Where(c => c.Name.ToLower().Contains(text.Text.ToLower())).ToList();

            /*
             * This is the original way to do it
             * 
            var filteredContact2 = (from c2 in contactList
                                   where c2.Name.ToLower().Contains(text.Text.ToLower())
                                   select c2).ToList();
            */

            contactListView.ItemsSource = filteredContact;
        }
    }
}
