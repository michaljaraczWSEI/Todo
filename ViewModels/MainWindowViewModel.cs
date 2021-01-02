﻿using System;
using System.Reactive.Linq;
using ReactiveUI;
using Todo.Models;
using Todo.Services;

namespace Todo.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    ViewModelBase content;
    public TodoListViewModel List { get; }
    public MainWindowViewModel(Database db)
    {
      Content = List = new TodoListViewModel(db.GetItems());
    }
    public ViewModelBase Content
    {
      get => content;
      private set => this.RaiseAndSetIfChanged(ref content, value);
    }
    public void AddItem()
    {
      AddItemViewModel vm = new AddItemViewModel();

      Observable
        .Merge(
            vm.Ok,
            vm.Cancel.Select(_ => (TodoItem)null)
        )
        .Take(1)
        .Subscribe(model => 
        {
            if (model != null)
            {
                List.Items.Add(model);
            }

            Content = List;
        });
      
      Content = vm;
    }
  }
}
