using WebApi.Domain.DTOs;

namespace WebApi.Domain.Model
{
    public interface IEmployeeRepository
    {

        void Add(Employee employee);

        List<EmployeeDTO> Get(int pageNumber, int pageQuantity);

        List<Employee> Get();

        Employee Get(int id);

        Employee Delete(int id);

        Employee UpdateAgeName(int id, string name, int age);

    }
}
