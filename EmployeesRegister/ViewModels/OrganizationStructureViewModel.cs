using EmployeesRegister.DataAccess;
using EmployeesRegister.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeesRegister.ViewModels
{
    class OrganizationStructureViewModel : ViewModelBase
    {
        public OrganizationStructureViewModel()
        {
            RefreshOrganization();
            Repository.Instance.OrganizationChanged += RepositoryOrganizationChanged;
        }

        #region organizations

        private void RepositoryOrganizationChanged(object sender, ItemChangedEventArgs<Organization> e)
        {
            if (e.Action == ItemChangedEventArgs<Organization>.ActionType.Add)
            {
                var organization = new OrganizationViewModel(e.Item);
                Organizations.Add(organization);
            }

            if (e.Action == ItemChangedEventArgs<Organization>.ActionType.Remove)
                foreach (var organization in Organizations.Where(x => x.Model.Id == e.Item.Id).ToArray())
                    Organizations.Remove(organization);
        }

        public ObservableCollection<OrganizationViewModel> Organizations
        {
            get { return organizations; }
            private set { organizations = value; OnPropertyChanged(nameof(Organizations)); }
        }
        private ObservableCollection<OrganizationViewModel> organizations;

        private void RefreshOrganization()
        {
            var items = Repository.Instance.GetOrganizations();
            if (!items.Success)
            {
                return;
            }

            Organizations = new ObservableCollection<OrganizationViewModel>(items.Value.Select(x => new OrganizationViewModel(x)));
        }

        #endregion organizations
    }
}
