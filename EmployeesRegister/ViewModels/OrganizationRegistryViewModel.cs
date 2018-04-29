using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class OrganizationRegistryViewModel : ViewModelBase
    {
        public OrganizationRegistryViewModel()
        {
            RefreshOrganization();
            Repository.Instance.OrganizationChanged += RepositoryOrganizationChanged;
        }

        private void RepositoryOrganizationChanged(object sender, ItemChangedEventArgs<Organization> e)
        {
            if (e.Action == ItemChangedEventArgs<Organization>.ActionType.Add)
            {
                var organization = new OrganizationViewModel(e.Item);
                Organizations.Add(organization);
                SelectedOrganization = organization;
            }
        }

        public ObservableCollection<OrganizationViewModel> Organizations
        {
            get { return organizations; }
            private set { organizations = value; OnPropertyChanged(nameof(Organizations)); }
        }
        private ObservableCollection<OrganizationViewModel> organizations;

        public OrganizationBlankViewModel SelectedOrganization
        {
            get { return selectedOrganization; }
            set { selectedOrganization = value; OnPropertyChanged(nameof(SelectedOrganization)); }
        }
        private OrganizationBlankViewModel selectedOrganization;

        #region refresh organization

        public RelayCommand RefreshOrganizationCommand
        {
            get
            {
                if (refreshOrganizationCommand is null)
                    refreshOrganizationCommand = new RelayCommand(p => RefreshOrganization());
                return refreshOrganizationCommand;
            }
        }
        private RelayCommand refreshOrganizationCommand;

        private void RefreshOrganization()
        {
            var items = Repository.Instance.GetOrganizations();
            if (!items.Success)
            {
                return;
            }

            Organizations = new ObservableCollection<OrganizationViewModel>(items.Value.Select(x => new OrganizationViewModel(x)));
        }

        #endregion refresh organization

        #region create organization

        public RelayCommand CreateOrganizationCommand
        {
            get
            {
                if (createOrganizationCommand is null)
                    createOrganizationCommand = new RelayCommand(p => CreateOrganization());
                return createOrganizationCommand;
            }
        }
        private RelayCommand createOrganizationCommand;

        public void CreateOrganization()
        {
            SelectedOrganization = new OrganizationBlankViewModel();
        }

        #endregion create organization

        #region remove organization

        public RelayCommand RemoveOrganizationCommand
        {
            get
            {
                if (removeOrganizationCommand is null)
                    removeOrganizationCommand = new RelayCommand(RemoveOrganization, CanRemoveOrganization);
                return removeOrganizationCommand;
            }
        }
        private RelayCommand removeOrganizationCommand;

        private void RemoveOrganization(object obj)
        {
            if (!(obj is OrganizationViewModel organization))
                return;

            var result = Repository.Instance.RemoveOrganization(organization.Model);
            if (result.Success)
                Organizations.Remove(organization);
        }

        private bool CanRemoveOrganization(object obj)
        {
            return obj is OrganizationViewModel;
        }

        #endregion remove organization
    }
}
