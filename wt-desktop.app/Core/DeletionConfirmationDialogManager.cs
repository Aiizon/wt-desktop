using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace wt_desktop.app.Core;

public class DeletionConfirmationDialogManager: INotifyPropertyChanged
{
    private string? _Message;

    public string? Message
    {
        get => _Message;
        set
        {
            _Message = value;
            OnPropertyChanged();
        }
    }

    public Action ConfirmAction { get; set; }
    public Action CancelAction  { get; set; }

    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand  { get; }

    public DeletionConfirmationDialogManager(string message)
    {
        Message = message;

        ConfirmCommand = new RelayCommand(
        () =>
        {
            ConfirmAction?.Invoke();
        }, () => true);

        CancelCommand = new RelayCommand(
        () =>
        {
            CancelAction?.Invoke();
        }, () => true);
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}