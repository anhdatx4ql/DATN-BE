using Demo.Webapi.Controllers;
using Demo.Webapi.Database;
using Demo.Webapi.Entities;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Test
{
    internal class EmployeeControllerTest
    {
        [Test]
        public void GetEmployeeById_NotExistsEmployee_ReturnNotFound()
        {
            // Arrange
            var employeeId = new Guid("b867b773-5917-4196-b30c-f00c11f28806");
            var expectResult = new NotFoundResult();
            var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
            fakeEmployeeRepository.QueryFirstOrDefault(
                Arg.Any<IDbConnection>(),
                Arg.Any<string>(),
                Arg.Any<object>(),
                Arg.Any<IDbTransaction>(),
                Arg.Any<int?>(),
                Arg.Any<CommandType?>()).Returns((Employee) null);
            var employeeController = new EmployeesController(fakeEmployeeRepository);

            // Act
            var actualResult = (ObjectResult)employeeController.getEmployeeById(employeeId);

            // Assert
            Assert.AreEqual(expectResult.StatusCode, actualResult.StatusCode);
        }

        [Test]
        public void GetEmployeeById_ValidData_ReturnEmployee()
        {
            // Arrange
            var employeeId = new Guid("b867b773-5917-4196-b30c-f00c11f28806");
            var employee = new Employee()
            {
                EmployeeCode = "NV-1111",
                FullName = "Trinh Xuan Dao",
                IdentityNumber = "121232131231"
            };
            var expectResult = new ObjectResult(employee);
            var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
            fakeEmployeeRepository.QueryFirstOrDefault(
                Arg.Any<IDbConnection>(),
                Arg.Any<string>(),
                Arg.Any<object>(),
                Arg.Any<IDbTransaction>(),
                Arg.Any<int?>(),
                Arg.Any<CommandType?>()).Returns(employee);
            var employeeController = new EmployeesController(fakeEmployeeRepository);

            // Act
            var actualResult = (ObjectResult)employeeController.getEmployeeById(employeeId);

            // Assert
            Assert.AreEqual(((Employee)expectResult.Value).EmployeeCode, ((Employee)actualResult.Value).EmployeeCode);
            Assert.AreEqual(((Employee)expectResult.Value).FullName, ((Employee)actualResult.Value).FullName);
            Assert.AreEqual(((Employee)expectResult.Value).IdentityNumber, ((Employee)actualResult.Value).IdentityNumber);
        }

        [Test]
        public void CreatedEmployee_ValidData_ReturnEmployee()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = new Guid("b867b773-5917-4196-b30c-f00c11f28806"),
                EmployeeCode = "NV-1111",
                FullName = "Trinh Xuan Dao",
                IdentityNumber = "121232131231"
            };
            var expectResult = new ObjectResult(employee);
            var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
            fakeEmployeeRepository.QueryFirstOrDefault(
                Arg.Any<IDbConnection>(),
                Arg.Any<string>(),
                Arg.Any<object>(),
                Arg.Any<IDbTransaction>(),
                Arg.Any<int>(),
                Arg.Any<CommandType>()).Returns(employee);
            var employeeController = new EmployeesController(fakeEmployeeRepository);

            // Act
            var actualResult = (ObjectResult)employeeController.CreateEmployee(employee);

            // Assert
            Assert.AreEqual(((Employee)expectResult.Value).EmployeeCode, ((Employee)actualResult.Value).EmployeeCode);
            Assert.AreEqual(((Employee)expectResult.Value).FullName, ((Employee)actualResult.Value).FullName);
            Assert.AreEqual(((Employee)expectResult.Value).IdentityNumber, ((Employee)actualResult.Value).IdentityNumber);
        }
    }
}
