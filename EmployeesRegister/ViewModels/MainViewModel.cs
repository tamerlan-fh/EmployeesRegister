namespace EmployeesRegister.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public OrganizationRegistryViewModel OrganizationRegistry { get; } 
            = new OrganizationRegistryViewModel();

        public PersonRegistryViewModel PersonRegistry { get; } 
            = new PersonRegistryViewModel();
    }
}
