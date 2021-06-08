using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintTemplate.Views
{
    /// <summary>
    /// Логика взаимодействия для DraggableSquare.xaml
    /// </summary>
    public partial class DraggableSquare : Grid
    {
        
        public DraggableSquare()
        {
            InitializeComponent();
            // устанавливаем Binding RequestMove из VM на свойство RequestMoveCommand:
            SetBinding(RequestMoveCommandProperty, new Binding("RequestMove"));
        }

        // стандартное DependencyProperty
        #region dp ICommand RequestMoveCommand
        public ICommand RequestMoveCommand
        {
            get { return (ICommand)GetValue(RequestMoveCommandProperty); }
            set { SetValue(RequestMoveCommandProperty, value); }
        }

        public static readonly DependencyProperty RequestMoveCommandProperty =
            DependencyProperty.Register("RequestMoveCommand", typeof(ICommand),
                                        typeof(DraggableSquare));
        #endregion

        Vector relativeMousePos; // смещение мыши от левого верхнего угла квадрата
        Canvas container;        // канвас-контейнер

        // по нажатию на левую клавишу начинаем следить за мышью
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            container = FindParent<Canvas>();
            relativeMousePos = e.GetPosition(this) - new Point();
            MouseMove += OnDragMove;
            LostMouseCapture += OnLostCapture;
            Mouse.Capture(this);
        }

        // клавиша отпущена - завершаем процесс
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }

        // потеряли фокус (например, юзер переключился в другое окно) - завершаем тоже
        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void FinishDrag(object sender, MouseEventArgs e)
        {
            MouseMove -= OnDragMove;
            LostMouseCapture -= OnLostCapture;
            UpdatePosition(e);
        }

        // требуем у VM обновить позицию через команду
        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(container);
            // не забываем проверку на null
            RequestMoveCommand?.Execute(point - relativeMousePos);
        }

        // это вспомогательная функция, ей место в общей библиотеке
        private T FindParent<T>() where T : FrameworkElement
        {
            FrameworkElement current = this;
            T t;
            do
            {
                t = current as T;
                current = (FrameworkElement)VisualTreeHelper.GetParent(current);
            }
            while (t == null && current != null);
            return t;
        }
    }
}
