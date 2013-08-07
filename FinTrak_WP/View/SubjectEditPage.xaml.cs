using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using FinTrak.Subject;

namespace FinTrak_WP.View
{
    public partial class SubjectEditPage : PhoneApplicationPage
    {
        bool _didNotExist = true;
        SubjectModel subject;

        public SubjectEditPage()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (_didNotExist)
            {
                subject = new SubjectModel();
            }

            string name = sName.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("You have to provide a name for the subject (a person or company), e.g. \"John Smith\" or \"Best Buy\"", "No name provided", MessageBoxButton.OK);
                return;
            }

            string label = sLabel.Text.Trim();
            if (label.Length == 0)
            {
                MessageBox.Show("You have to provide a label for the subject, e.g. \"Friend\", \"Restaurant\" or \"Store\"", "No label provided", MessageBoxButton.OK);
                return;
            }

            subject.Name = name;
            subject.Label = label;
            subject.Phone = sPhoneNr.Text;
            subject.Email = sEmail.Text;

            if (_didNotExist)
            {
                MainPage.Subjects.Add(subject);
            }

            NavigationService.GoBack();
        }
    }
}