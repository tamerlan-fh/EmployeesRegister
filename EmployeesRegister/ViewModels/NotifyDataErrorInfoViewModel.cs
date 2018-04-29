using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class NotifyDataErrorInfoViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected readonly Dictionary<string, ICollection<string>> errors 
            = new Dictionary<string, ICollection<string>>();

        public bool HasErrors
        {
            get { return errors.Any(); }
        }

        protected void OnErrorsChanged(string property)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
        }

        protected void AddError(string property, string error)
        {
            if (errors.ContainsKey(property))
                errors[property].Add(error);
            else
                errors[property] = new List<string> { error };

            OnErrorsChanged(property);
        }

        protected void RemoveError(string property)
        {
            errors.Remove(property);
            OnErrorsChanged(property);
        }

        public IEnumerable GetErrors(string property)
        {
            if (string.IsNullOrEmpty(property) || !errors.TryGetValue(property, out ICollection<string> value))
                return null;

            return value;
        }

        protected void ValidateNotEmpty(string property, string value)
        {
            RemoveError(property);
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                AddError(property, "поле должно быть запомненным");
        }
    }
}
