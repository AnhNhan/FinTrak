using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using FinTrak.Asset;

namespace FinTrak_WP.View
{
    public partial class AssetEditPage : PhoneApplicationPage
    {
        private AssetModel asset;
        private bool _didNotExist = true;

        public AssetEditPage()
        {
            InitializeComponent();
            InitializePage();
        }

        private void InitializePage()
        {
            List<string> typeStrings = new List<string>
            {
                AssetModel.GetTypeNameForAssetTypeId(AssetType.Cash),
                AssetModel.GetTypeNameForAssetTypeId(AssetType.CashSavings),
                AssetModel.GetTypeNameForAssetTypeId(AssetType.Account),
                AssetModel.GetTypeNameForAssetTypeId(AssetType.AccountSavings),
                AssetModel.GetTypeNameForAssetTypeId(AssetType.CreditCard),
                AssetModel.GetTypeNameForAssetTypeId(AssetType.DebitCard),
                AssetModel.GetTypeNameForAssetTypeId(AssetType.Prepaid),
            };
            typePicker.ItemsSource = typeStrings;

            this.Loaded += AssetEditPage_Loaded;
        }

        void AssetEditPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationBar.Buttons.Add(Resources["cancelButton"]);
            this.Loaded -= AssetEditPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("assetId"))
            {
                uint assetId = uint.Parse(NavigationContext.QueryString["assetId"]);
                asset = MainPage.Assets.Where(_asset => _asset.Id == assetId).First();
                _didNotExist = false;

                this.Loaded += OnNavigation_Loaded;
            }
        }

        private void OnNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationBar.Buttons.Add(Resources["deleteButton"]);
            assignAsset(asset);

            this.Loaded -= OnNavigation_Loaded;
        }

        private void assignAsset(AssetModel _asset)
        {
            assetLabel.Text = _asset.ITLabel;
            typePicker.SelectedIndex = (int)Math.Log((int)_asset.TypeId, 2) + 1;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (_didNotExist)
            {
                asset = new AssetModel();
            }

            AssetType _typeId;
            string name;

            switch (typePicker.SelectedIndex)
            {
                default:
                case 0:
                    _typeId = AssetType.Cash;
                    break;
                case 1:
                    _typeId = AssetType.CashSavings;
                    break;
                case 2:
                    _typeId = AssetType.Account;
                    break;
                case 3:
                    _typeId = AssetType.AccountSavings;
                    break;
                case 4:
                    _typeId = AssetType.CreditCard;
                    break;
                case 5:
                    _typeId = AssetType.DebitCard;
                    break;
                case 6:
                    _typeId = AssetType.Prepaid;
                    break;
            }

            name = assetLabel.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("You have to provide a label for the asset", "No label", MessageBoxButton.OK);
                return;
            }

            asset.Title = name;
            asset.TypeId = _typeId;

            if (_didNotExist)
            {
                MainPage.Assets.Add(asset);
            }

            NavigationService.GoBack();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Delete is not supported yet!");
        }
    }
}