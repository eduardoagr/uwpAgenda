using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace uwpContacts.Controls {
    public sealed partial class ContactControl: UserControl {



        public Contact Contact {
            get { return (Contact)GetValue(ContactProperty); }
            set { SetValue(ContactProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Contact.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContactProperty =
            DependencyProperty.Register("Contact", typeof(Contact), typeof(ContactControl), new PropertyMetadata(null, setText));

        private static void setText(DependencyObject d, DependencyPropertyChangedEventArgs e) {
                       
            var control = d as ContactControl;

            if (control != null) {
                
                control.contactFullnameTextBlock.Text = (e.NewValue as Contact).Name;
                control.contactEmaiTextBlock.Text = (e.NewValue as Contact).Email;
                control.contactPhoneTextBlock.Text = (e.NewValue as Contact).Phone;
            }
        }

        public ContactControl() {
            this.InitializeComponent();
        }
    }
}
