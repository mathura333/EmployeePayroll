﻿namespace EmployeePayroll
{
    using System;
    using System.Data.SqlClient;
    using System.Reflection.Metadata.Ecma335;

    public class EmployeeRepo
    {
        /// <summary>The connection string</summary>
        public static string connectionString = @"Data Source=DESKTOP-8UMNEFU\MSSQLSERVER01;Initial Catalog=payroll_service;Integrated Security=True";
        //SqlConnection connection = new SqlConnection(connectionString);
        /// <summary>Checks the connection.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public bool CheckConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    CustomPrint.PrintInRed("Connection is opened");
                    CustomPrint.PrintInRed("Connection good");
                    connection.Close();
                    CustomPrint.PrintInRed("Connection is closed");
                    return true;
                }
            }
            catch (Exception e)
            {
                CustomPrint.PrintInMagenta(e.Message);
                return false;
            }
        }
        /// <summary>Gets all employee.</summary>
        public void GetAllEmployee()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SelectAllRowsFromEmployeePayroll";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        CustomPrint.PrintInRed($"All employees data :");
                        CustomPrint.PrintDashLine();
                        Console.WriteLine(CustomPrint.PrintRow("Emp ID", "Emp Name", "Company ID", "Company Name", "Dept ID", "Dept Name", "Gender", "Phone No", "Address", "Start Date", "Basic Pay", "Deductions", "Taxable Pay", "Tax", "Net Pay"));
                        CustomPrint.PrintDashLine();
                        while (dr.Read())
                        {
                            employeeModel.employeeID = dr.GetInt32(0);
                            employeeModel.employeeName = dr.GetString(1);
                            employeeModel.companyId = dr.GetInt32(2);
                            employeeModel.companyName = dr.GetString(3);
                            employeeModel.departmentId = dr.GetInt32(4);
                            employeeModel.departmentName = dr.GetString(5);
                            employeeModel.gender = Convert.ToChar(dr.GetString(6));
                            employeeModel.phoneNumber = dr.GetString(7);
                            employeeModel.address = dr.GetString(8);
                            employeeModel.startDate = dr.GetDateTime(9);
                            employeeModel.basicPay = dr.GetDecimal(10);
                            employeeModel.deductions = dr.GetDecimal(11);
                            employeeModel.taxablePay = dr.GetDecimal(12);
                            employeeModel.tax = dr.GetDecimal(13);
                            employeeModel.netPay = dr.GetDecimal(14);
                            Console.WriteLine(employeeModel);
                        }
                        CustomPrint.PrintDashLine();
                        Console.WriteLine();
                    }
                    else
                    {
                        CustomPrint.PrintInMagenta("No data found");
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                CustomPrint.PrintInMagenta(e.Message);
            }
        }
        /// <summary>Updates the employee salary.</summary>
        /// <param name="model">The empModel</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public bool UpdateEmployeeSalary(EmployeeModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("UpdateSalaryByName", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeName", model.employeeName);
                    command.Parameters.AddWithValue("@BasicPay", model.basicPay);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    CustomPrint.PrintInRed($"{result} rows affected");
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                CustomPrint.PrintInMagenta(e.Message);
                return false;
            }
        }
        /// <summary>Gets employee by name.</summary>
        /// <param name="empName">Name of the emp.</param>
        /// <returns>true if emp model is returned.false if no data or connection failed</returns>
        public bool GetEmpByName(string empName)
        {
            try
            {
                EmployeeModel model = new EmployeeModel();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("GetEmpByName", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", empName);
                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        CustomPrint.PrintInRed($"Data for employee with name : {empName}");
                        CustomPrint.PrintDashLine();
                        Console.WriteLine(CustomPrint.PrintRow("Emp ID", "Emp Name", "Company ID", "Company Name", "Dept ID", "Dept Name", "Gender", "Phone No", "Address", "Start Date", "Basic Pay", "Deductions", "Taxable Pay", "Tax", "Net Pay"));
                        CustomPrint.PrintDashLine();
                        while (dr.Read())
                        {
                            model.employeeID = dr.GetInt32(0);
                            model.employeeName = dr.GetString(1);
                            model.companyId = dr.GetInt32(2);
                            model.companyName = dr.GetString(3);
                            model.departmentId = dr.GetInt32(4);
                            model.departmentName = dr.GetString(5);
                            model.gender = Convert.ToChar(dr.GetString(6));
                            model.phoneNumber = dr.GetString(7);
                            model.address = dr.GetString(8);
                            model.startDate = dr.GetDateTime(9);
                            model.basicPay = dr.GetDecimal(10);
                            model.deductions = dr.GetDecimal(11);
                            model.taxablePay = dr.GetDecimal(12);
                            model.tax = dr.GetDecimal(13);
                            model.netPay = dr.GetDecimal(14);
                            Console.WriteLine(model);
                        }
                        CustomPrint.PrintDashLine();
                        Console.WriteLine();
                        return true;
                    }
                    CustomPrint.PrintInMagenta("No data found");
                    connection.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                CustomPrint.PrintInMagenta(e.Message);
                return false;
            }
        }
        /// <summary>Gets the emp in given date range.</summary>
        /// <param name="initialDate">The initial date.</param>
        /// <param name="lastDate">The last date.</param>
        /// <returns>true if emp model is returned.false if no data or connection failed</returns>
        public bool GetEmpInDateRange(DateTime initialDate, DateTime lastDate)
        {
            try
            {
                EmployeeModel model = new EmployeeModel();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("GetEmpInDateRange", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@initialDate", initialDate);
                    command.Parameters.AddWithValue("@lastDate", lastDate);
                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        CustomPrint.PrintInRed($"Data for employees who started within {initialDate.ToShortDateString()} and {lastDate.ToShortDateString()} : ");
                        CustomPrint.PrintDashLine();
                        Console.WriteLine(CustomPrint.PrintRow("Emp ID", "Emp Name", "Company ID", "Company Name", "Dept ID", "Dept Name", "Gender", "Phone No", "Address", "Start Date", "Basic Pay", "Deductions", "Taxable Pay", "Tax", "Net Pay"));
                        CustomPrint.PrintDashLine();
                        while (dr.Read())
                        {
                            model.employeeID = dr.GetInt32(0);
                            model.employeeName = dr.GetString(1);
                            model.companyId = dr.GetInt32(2);
                            model.companyName = dr.GetString(3);
                            model.departmentId = dr.GetInt32(4);
                            model.departmentName = dr.GetString(5);
                            model.gender = Convert.ToChar(dr.GetString(6));
                            model.phoneNumber = dr.GetString(7);
                            model.address = dr.GetString(8);
                            model.startDate = dr.GetDateTime(9);
                            model.basicPay = dr.GetDecimal(10);
                            model.deductions = dr.GetDecimal(11);
                            model.taxablePay = dr.GetDecimal(12);
                            model.tax = dr.GetDecimal(13);
                            model.netPay = dr.GetDecimal(14);
                            Console.WriteLine(model);
                        }
                        CustomPrint.PrintDashLine();
                        Console.WriteLine();
                        return true;
                    }
                    CustomPrint.PrintInMagenta("No data found");
                    connection.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                CustomPrint.PrintInMagenta(e.Message);
                return false;
            }
        }
    }
}
