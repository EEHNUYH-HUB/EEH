using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EEH.WPF
{
    public class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper),
          new FrameworkPropertyMetadata(OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper));
        public static readonly DependencyProperty EditProperty = DependencyProperty.RegisterAttached("Edit", typeof(bool), typeof(PasswordHelper));

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }
        public static bool GetEdit(DependencyObject dp)
        {
            return (bool)dp.GetValue(EditProperty);
        }

        public static void SetEdit(DependencyObject dp, bool value)
        {
            dp.SetValue(EditProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;


            if (!GetEdit(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }


            bool isAttach = GetAttach(passwordBox);
            if (!isAttach)
            {
                passwordBox.PasswordChanged += PasswordChanged;
                SetAttach(passwordBox, true);
            }

        }



        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetEdit(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetEdit(passwordBox, false);
        }
    }
}
