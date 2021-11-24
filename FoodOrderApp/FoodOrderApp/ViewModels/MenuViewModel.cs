﻿using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        //public ICommand ItemClickCommand { get; set; }
        public List<PRODUCT> products;

        public MenuViewModel()
        {
            AddToCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => AddToCart(parameter));
            LoadedCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => Load(parameter));
            //AddToCartCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => AddToCart(p));
            //ItemClickCommand = new RelayCommand<ListViewItem>((parameter) => parameter == null ? false : true, (parameter) => ItemClick(parameter));
        }

        private void Load(MenuUC parameter)
        {
            products = Data.Ins.DB.PRODUCTs.ToList();

            parameter.ViewListProducts.ItemsSource = products;
        }

        private void AddToCart(ListViewItem parameter)
        {
            try
            {
                var item = parameter.DataContext as PRODUCT;
                int cartsCount = Data.Ins.DB.CARTs.Where(x => x.USERNAME_ == CurrentAccount.Username && x.PRODUCT_ == item.ID_).Count();
                int idCarts = Data.Ins.DB.CARTs.Count();
                if (cartsCount == 0)
                {
                    string tmpID = CurrentAccount.Username +"_"+ item.ID_;
                    Data.Ins.DB.CARTs.Add(new CART() { ID_ = tmpID, PRODUCT_ = item.ID_, USERNAME_ = CurrentAccount.Username, AMOUNT_ = 1 });
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Đã thêm " + item.NAME_.ToString() + " vào giỏ hàng thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                    CustomMessageBox.Show("Món ăn " + item.NAME_.ToString() + " đã có sẵn trong giỏ hàng", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //private void ItemClick(ListViewItem parameter)
        //{
        //    ProductDetail pd = new ProductDetail();
        //    pd.ShowDialog();
        //}
    }
}