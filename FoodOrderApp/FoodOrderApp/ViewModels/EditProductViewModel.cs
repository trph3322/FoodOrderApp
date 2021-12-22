﻿using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls.Admin;
using FoodOrderApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.IO;

namespace FoodOrderApp.ViewModels
{
    internal class EditProductViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand OpenImageCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand UpdateButtonCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        private string Image;

        public string IMAGE_
        { get => Image; set { Image = value; OnPropertyChanged("Image"); } }

        //private string Name;

        //public string NAME_
        //{ get => Name; set { Name = value; OnPropertyChanged("Name"); } }

        //private string Price;

        //public string PRICE_
        //{ get => Price; set { Price = value; OnPropertyChanged("Price"); } }

        //private string Description;

        //public string DESCRIPTION_
        //{ get => Description; set { Description = value; OnPropertyChanged("Description"); } }

        //private string Discount;

        //public string DISCOUNT_
        //{ get => Discount; set { Discount = value; OnPropertyChanged("Discount"); } }

        private PRODUCT Current_Product;

        public List<PRODUCT> pRODUCTs;
        public EditProductViewModel()
        {
            LoadedCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Loaded(parameter));
            AddProductCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Add(parameter));
            UpdateProductCommand = new RelayCommand<System.Windows.Controls.ListViewItem>((parameter) => true, (parameter) => Update(parameter));
            DeleteProductCommand = new RelayCommand<System.Windows.Controls.ListViewItem>((parameter) => true, (parameter) => Delete(parameter));
            SelectImageCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => SelectImage(parameter));
            UpdateButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => UpdateProduct(parameter));
            AddButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => AddProduct(parameter));
            CloseButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => CloseButton(parameter));
        }
        public void Loaded(EditProductUC editProductUC)
        {
            pRODUCTs = Data.Ins.DB.PRODUCTs.ToList();
            editProductUC.ListView.ItemsSource = pRODUCTs;
        }
        public void Add(EditProductUC editProductUC)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.updatebtn.Visibility = Visibility.Collapsed;
            addProductWindow.ShowDialog();
        }
        public void Update(System.Windows.Controls.ListViewItem listViewItem)
        {
            //Get current product

            PRODUCT pRODUCT = listViewItem.DataContext as PRODUCT;
            Current_Product = pRODUCT;

            //Open AddProductWindow with current product data

            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.addbtn.Visibility = Visibility.Collapsed;
            addProductWindow.txtName.Text = pRODUCT.NAME_;
            addProductWindow.txtPrice.Text = pRODUCT.PRICE_.ToString();
            addProductWindow.txtDiscount.Text = (pRODUCT.DISCOUNT_ * 100).ToString();
            addProductWindow.txtDescription.Text = pRODUCT.DESCRIPTION_;

            //Load current product image
            if (!string.IsNullOrEmpty(pRODUCT.IMAGE_))
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(pRODUCT.IMAGE_, UriKind.Absolute);
                bitmap.EndInit();
                addProductWindow.Image.ImageSource = bitmap;
                addProductWindow.ShowDialog();
            }
        }
        public void Delete(System.Windows.Controls.ListViewItem listViewItem)
        {
            PRODUCT pRODUCT = listViewItem.DataContext as PRODUCT;
            Data.Ins.DB.PRODUCTs.Remove(pRODUCT);
            Data.Ins.DB.SaveChanges();
        }
        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files | *.jpg; *.png | All files | *.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                //Create connection to Storage

                string containerName = "container";
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodorderapp1;AccountKey=i1GnOJc+VJJpoRe3l44AeH3uBiq3n+ZbFELlNJMyiZuyovi7RlmYA5bTNoUWGFvS6tUTURPGRfgRvkXlsiDm/Q==;EndpointSuffix=core.windows.net";
                BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

                //Update Image


                //Get name of Image

                string[] filename = Path.GetFileName(openFileDialog.FileName).Split('.');

                //Delete old Image

                if (!string.IsNullOrEmpty(Current_Product.IMAGE_))
                {
                    BlobClient blobClient = new BlobClient(connectionString, containerName, Current_Product.ID_);
                    blobClient.Delete();
                }
                //Start upload

                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)))
                {
                    //Upload new Image

                    containerClient.UploadBlob(Current_Product.ID_ + "." + filename[1], stream);
                    CustomMessageBox.Show("Thay đổi ảnh thành công", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }

                //Update new Image link

                PRODUCT product = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
                product.IMAGE_ = "https://foodorderapp1.blob.core.windows.net/container" + CurrentAccount.Username + "." + filename[1];

                //Save database change

                Data.Ins.DB.SaveChanges();
                
                IMAGE_ = openFileDialog.FileName;
            }
        }
        public void SelectImage(AddProductWindow addProductWindow)
        {
            UploadImage();
            System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(IMAGE_, UriKind.Absolute);
            bitmap.EndInit();
            addProductWindow.Image.ImageSource = bitmap;
        }
        public void UpdateProduct(AddProductWindow addProductWindow)
        {
            PRODUCT pRODUCT = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
            pRODUCT.NAME_ = addProductWindow.txtName.Text;
            pRODUCT.PRICE_ = Convert.ToInt32(addProductWindow.txtPrice.Text);
            pRODUCT.DESCRIPTION_ = addProductWindow.txtDescription.Text;
            pRODUCT.DISCOUNT_ = Convert.ToDecimal(addProductWindow.txtDiscount.Text)/100;
            pRODUCT.IMAGE_ = IMAGE_;
            Data.Ins.DB.SaveChanges();
            addProductWindow.Close();
        }
        public void AddProduct(AddProductWindow addProductWindow)
        {
            PRODUCT newProduct = new PRODUCT();
            Current_Product = newProduct;
            newProduct.DESCRIPTION_ = addProductWindow.txtDescription.Text;
            newProduct.DISCOUNT_ = Convert.ToInt32(addProductWindow.txtDiscount.Text);
            newProduct.NAME_ = addProductWindow.txtName.Text;
            newProduct.PRICE_ = Convert.ToInt32(addProductWindow.txtPrice.Text);
            List<PRODUCT> a = Data.Ins.DB.PRODUCTs.ToList();
            newProduct.ID_ = (Convert.ToInt32(a[a.Count-1].ID_) + 1).ToString();
            newProduct.IMAGE_ = IMAGE_;
            Data.Ins.DB.PRODUCTs.Add(newProduct);
            IMAGE_ = "";
            Data.Ins.DB.SaveChanges();
            addProductWindow.Close();
        }
        
        public void CloseButton(AddProductWindow addProductWindow)
        {
            IMAGE_ = "";
            addProductWindow.Close();
        }
    }
}
