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
        public List<PRODUCT> products;

        public MenuViewModel()
        {
            LoadedCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => Load(parameter));
            AddToCartCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => AddToCart(p));
        }

        private void Load(MenuUC parameter)
        {
            //products = Data.Ins.DB.PRODUCTs.ToList();

            //parameter.ViewListProducts.ItemsSource = products;
        }

        private void AddToCart(ListViewItem item)
        {
            //var product = item.DataContext as PRODUCT;
            //USER currentUser = new USER();
            //var currentUsers = Data.Ins.DB.USERS.Where(user => user.USERNAME_ == CurrentAccount.Username).ToList();
            //foreach(var user in currentUsers)
            //{
            //    currentUser = user;
            //}
            //CART itemCart = new CART();
            //itemCart.USERNAME_ = currentUser.USERNAME_;
            //itemCart.PRODUCT_ = product.ID_;
            //Data.Ins.DB.CARTs.Add(itemCart);
        }
    }
}