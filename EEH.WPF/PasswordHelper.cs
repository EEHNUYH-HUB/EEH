using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;



namespace EEH.WPF
{
    public  class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty;
        public static readonly DependencyProperty AttachProperty;
        public static readonly DependencyProperty IsUpdatingProperty;
        public static readonly DependencyProperty CheckPasswordProperty;


        //public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(EzBoundPasswordBox),
        //  new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        //public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(EzBoundPasswordBox),
        //    new PropertyMetadata(false, Attach));

        //private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
        //   typeof(EzBoundPasswordBox));

        static PasswordHelper()
        {

        }
    }
}
