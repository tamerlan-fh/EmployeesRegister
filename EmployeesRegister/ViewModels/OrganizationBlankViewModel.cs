using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;

namespace EmployeesRegister.ViewModels
{
    class OrganizationBlankViewModel : NotifyDataErrorInfoViewModel
    {
        private readonly OrganizationBlank blank = new OrganizationBlank();

        public OrganizationBlankViewModel()
        {
            ValidateNotEmpty(nameof(Caption), Caption);
        }

        public virtual string Caption
        {
            get { return blank.Caption; }
            set
            {
                if (Caption == value) return;
                blank.Caption = value; OnPropertyChanged(nameof(Caption));
                ValidateNotEmpty(nameof(Caption), Caption);
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
            Repository.Instance.AddOrganization(blank);
        }

        protected virtual bool CanSave()
        {
            return !HasErrors;
        }
    }
}
