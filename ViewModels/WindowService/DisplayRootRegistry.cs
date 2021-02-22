using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModels.WindowService
{
    public static class DisplayRootRegistry
    {
        static Dictionary<Type, Type> vmToWindowMapping = new Dictionary<Type, Type>();

        /// <summary>
        /// Регистрирует окно.
        /// </summary>
        /// <typeparam name="VM"> ViewModel. </typeparam>
        /// <typeparam name="Win"> Окно. </typeparam>
        public static void RegisterWindowType<VM, Win>() where Win : Window, new() where VM : class
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is already registered");
            vmToWindowMapping[vmType] = typeof(Win);
        }

        /// <summary>
        /// Удаляет регистрацию окна.
        /// </summary>
        /// <typeparam name="VM"> ViewModel. </typeparam>
        /// <typeparam name="Win"> Окно. </typeparam>
        public static void UnregisterWindowType<VM>()
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (!vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is not registered");
            vmToWindowMapping.Remove(vmType);
        }

        /// <summary>
        /// Создает экземпляр окна с помощью ViewModel.
        /// </summary>
        /// <param name="vm"> ViewModel. </param>
        /// <returns> Окно. </returns>
        public static Window CreateWindowInstanceWithVM(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            Type windowType = null;

            var vmType = vm.GetType();
            while (vmType != null && !vmToWindowMapping.TryGetValue(vmType, out windowType))
                vmType = vmType.BaseType;

            if (windowType == null)
                throw new ArgumentException(
                    $"No registered window type for argument type {vm.GetType().FullName}");

            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = vm;
            return window;
        }

        static Dictionary<object, Window> openWindows = new Dictionary<object, Window>();

        /// <summary>
        /// Создает окно.
        /// </summary>
        /// <param name="vm"> ViewModel. </param>
        public static void ShowPresentation(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (openWindows.ContainsKey(vm))
                throw new InvalidOperationException("UI for this VM is already displayed");
            var window = CreateWindowInstanceWithVM(vm);
            window.Show();
            openWindows[vm] = window;
        }

        /// <summary>
        /// Закрывает окно.
        /// </summary>
        /// <param name="vm"> ViewModel. </param>
        public static void ClosePresentation(object vm)
        {
            Window window;
            if (!openWindows.TryGetValue(vm, out window))
                throw new InvalidOperationException("UI for this VM is not displayed");
            window.Close();
            openWindows.Remove(vm);
        }

        /// <summary>
        /// Создает диалоговое окно.
        /// </summary>
        /// <param name="vm"> ViewModel. </param>
        public static void ShowModalPresentation(object vm)
        {
            var window = CreateWindowInstanceWithVM(vm);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            openWindows[vm] = window;
            window.ShowDialog();
        }
    }
}
