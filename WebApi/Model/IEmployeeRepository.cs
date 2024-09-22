namespace WebApi.Model
{
    public interface IEmployeeRepository
    {

        void Add(Employee employee);

            List<Employee> Get();

            Employee Get(int id);

            Employee Delete(int id);

            Employee UpdateAgeName(int id, string name, int age);

    }
}
