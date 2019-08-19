﻿using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.VisualTree;
using ReactiveUI;

namespace ControlCatalog.ViewModels
{
    public class MenuPageViewModel : ReactiveObject
    {
        public Control View { get; set; }
        public MenuPageViewModel()
        {
            OpenCommand = ReactiveCommand.CreateFromTask(Open);
            SaveCommand = ReactiveCommand.Create(Save, Observable.Return(false));
            OpenRecentCommand = ReactiveCommand.Create<string>(OpenRecent);

            NewFileCommand = ReactiveCommand.Create(() =>
            {
                Message = "New File Menu Item Clicked";
            });
            
            NativeMenuItems = new List<NativeMenuItem>
            {
                new NativeMenuItem()
                {
                    Header = "_File",
                    Items = new List<NativeMenuItem>
                    {
                        new NativeMenuItem() { Header = "_Open...", Command = OpenCommand },
                        new NativeMenuItem { Header = "Save", Command = SaveCommand },
                        new NativeMenuItem { Header = "-" },
                        new NativeMenuItem
                        {
                            Header = "Recent",
                            Items = new List<NativeMenuItem>
                            {
                                new NativeMenuItem
                                {
                                    Header = "File1.txt",
                                    Command = OpenRecentCommand,
                                    CommandParameter = @"c:\foo\File1.txt"
                                },
                                new NativeMenuItem
                                {
                                    Header = "File2.txt",
                                    Command = OpenRecentCommand,
                                    CommandParameter = @"c:\foo\File2.txt"
                                },
                            }
                        },
                    }
                },
                new NativeMenuItem
                {
                    Header = "_Edit",
                    Items = new List<NativeMenuItem>
                    {
                        new NativeMenuItem { Header = "_Copy" },
                        new NativeMenuItem { Header = "_Paste" },
                    }
                }
            };

            MenuItems = new[]
            {
                new MenuItemViewModel
                {
                    Header = "_File",
                    Items = new[]
                    {
                        new MenuItemViewModel { Header = "_Open...", Command = OpenCommand },
                        new MenuItemViewModel { Header = "Save", Command = SaveCommand },
                        new MenuItemViewModel { Header = "-" },
                        new MenuItemViewModel
                        {
                            Header = "Recent",
                            Items = new[]
                            {
                                new MenuItemViewModel
                                {
                                    Header = "File1.txt",
                                    Command = OpenRecentCommand,
                                    CommandParameter = @"c:\foo\File1.txt"
                                },
                                new MenuItemViewModel
                                {
                                    Header = "File2.txt",
                                    Command = OpenRecentCommand,
                                    CommandParameter = @"c:\foo\File2.txt"
                                },
                            }
                        },
                    }
                },
                new MenuItemViewModel
                {
                    Header = "_Edit",
                    Items = new[]
                    {
                        new MenuItemViewModel { Header = "_Copy" },
                        new MenuItemViewModel { Header = "_Paste" },
                    }
                }
            };
        }
        
        public  IReadOnlyList<NativeMenuItem> NativeMenuItems { get; set; }

        public IReadOnlyList<MenuItemViewModel> MenuItems { get; set; }
        public ReactiveCommand<Unit, Unit> OpenCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<string, Unit> OpenRecentCommand { get; }
        
        public  ReactiveCommand<Unit, Unit> NewFileCommand { get; }

        public async Task Open()
        {
            var window = View?.GetVisualRoot() as Window;
            if (window == null)
                return;
            var dialog = new OpenFileDialog();
            var result = await dialog.ShowAsync(window);

            if (result != null)
            {
                foreach (var path in result)
                {
                    System.Diagnostics.Debug.WriteLine($"Opened: {path}");
                }
            }
        }

        public void Save()
        {
            System.Diagnostics.Debug.WriteLine("Save");
        }

        public void OpenRecent(string path)
        {
            System.Diagnostics.Debug.WriteLine($"Open recent: {path}");
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }
    }
}
