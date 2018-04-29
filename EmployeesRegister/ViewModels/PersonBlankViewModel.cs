using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;

namespace EmployeesRegister.ViewModels
{
    class PersonBlankViewModel : NotifyDataErrorInfoViewModel
    {
        private readonly PersonBlank blank = new PersonBlank();

        public PersonBlankViewModel()
        {
            ValidateNotEmpty(nameof(Name), Name);
            ValidateNotEmpty(nameof(Phone), Phone);
            ValidateNotEmpty(nameof(Address), Address);
        }

        public virtual string Name
        {
            get { return blank.Name; }
            set
            {
                if (Name == value) return;
                blank.Name = value; OnPropertyChanged(nameof(Name));
                ValidateNotEmpty(nameof(Name), Name);
            }
        }

        public virtual string Phone
        {
            get { return blank.Phone; }
            set
            {
                if (Phone == value) return;
                blank.Phone = value; OnPropertyChanged(nameof(Phone));
                ValidateNotEmpty(nameof(Phone), Phone);
            }
        }

        public virtual string Address
        {
            get { return blank.Address; }
            set
            {
                if (Address == value) return;
                blank.Address = value; OnPropertyChanged(nameof(Address));
                ValidateNotEmpty(nameof(Address), Address);
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                if (saveCommand is null)
                    saveCommand = new RelayCommand(p => Save(), p => CanSave());
                return saveCommand;
            }
        }
        private RelayCommand saveCommand;

        protected virtual void Save()
        {
            Repository.Instance.AddPerson(blank);
        }

        protected virtual bool CanSave()
        {
            return !HasErrors;
        }
    }
}
