﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using FinTrak.Transaction;

namespace FinTrak_WP.View
{
    public partial class TransactionTargetPickerControl : UserControl, INotifyPropertyChanged
    {
        List<ITransactionTarget> _targetsLoaded;

        #region properties

        public string Header {
            get
            {
                return xactPickerHeader.Text;
            }

            set
            {
                xactPickerHeader.Text = value;
                NotifyPropertyChanged("Header");
            }
        }

        public bool IsAsset
        {
            get
            {
                return (bool)xactIsAssetToggle.IsChecked;
            }

            set
            {
                xactIsAssetToggle.IsChecked = value;
                NotifyPropertyChanged("IsAsset");
            }
        }

        public ITransactionTarget SelectedItem
        {
            get
            {
                return _targetsLoaded.ElementAt(xactTargetPicker.SelectedIndex);
            }
        }

        #endregion

        public TransactionTargetPickerControl()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += TransactionTargetPickerControl_Loaded;
        }

        void TransactionTargetPickerControl_Loaded(object sender, RoutedEventArgs e)
        {
            xactIsAssetToggle.Checked += xactIsAssetToggle_Checked;
            xactIsAssetToggle.Unchecked += xactIsAssetToggle_Unchecked;
            RebuildPicker();
        }

        void xactIsAssetToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            RebuildPicker();
        }

        void xactIsAssetToggle_Checked(object sender, RoutedEventArgs e)
        {
            RebuildPicker();
        }

        void RebuildPicker()
        {
            if (IsAsset)
            {
                _targetsLoaded = new List<ITransactionTarget>(MainPage.Assets.ToArray());
            }
            else
            {
                _targetsLoaded = new List<ITransactionTarget>(MainPage.Subjects.ToArray());
            }

            string[] elements = new string[_targetsLoaded.Count];
            int i = 0;
            foreach (var element in _targetsLoaded)
            {
                elements[i] = element.ITLabel;
                i++;
            }

            xactTargetPicker.ItemsSource = elements;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}