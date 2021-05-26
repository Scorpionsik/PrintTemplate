using System;
using System.Threading;
using System.Windows;

namespace Test_DragDrop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TimeSpan Locality;

        public static string AppTitle = "CoreWPF";

        /// Хранит именованный мьютекс, чтобы сохранить владение им до конца пробега программы
        private static Mutex InstanceCheckMutex;

        /// <summary>
        /// Проверяем, запущено ли приложение
        /// </summary>
        /// <returns>Возвращает true, если приложение не запущено, иначе false</returns>
        private static bool InstanceCheck()
        {
            bool isNew;
            App.InstanceCheckMutex = new Mutex(true, App.AppTitle, out isNew);
            return isNew;
        } //---метод InstanceCheck

        public App()
        {
            if (!App.InstanceCheck())
            {
                App.SetMessageBox("Программа уже запущена!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(0);
            }

            Locality = DateTimeOffset.Now.Offset;
        }

        public static MessageBoxResult SetMessageBox(string text, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.Information, string subtitle = "")
        {
            return MessageBox.Show(text, AppTitle + (subtitle != null && subtitle.Length > 0 ? ": " + subtitle : ""), buttons, image);
        }
    }
}
