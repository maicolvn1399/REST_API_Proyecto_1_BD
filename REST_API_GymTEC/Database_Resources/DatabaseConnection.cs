

using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace REST_API_GymTEC.Database_Resources
{
    public class DatabaseConnection
    {

        public static string cadenaConexion = ConnectionStringManager.GetConnectionString();

        public static DataTable ExecuteGetAllBranches()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Nombre FROM Sucursal", conn);
                cmd.CommandType = System.Data.CommandType.Text;

                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable ExecuteGetPhonesXBranch(string branch_name)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                string query = String.Format("SELECT Telefono \r\n" +
                    "FROM TelefonoXSucursal\r\n" +
                    "WHERE Sucursal_nombre = '{0}'", branch_name);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void ExecuteAddPhonesBranch(string branch_phone, List<string> phones)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int i;
            try
            {
                conn.Open();
                foreach (var phone in phones)
                {
                    string query = String.Format("INSERT INTO TelefonoXSucursal(Sucursal_nombre, Telefono)\r\n" +
                    "VALUES('{0}','{1}')", branch_phone, phone);
                    Console.WriteLine(query);

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                conn.Close();
            }

        }

        public static DataTable ExecuteGetBranch(Branch_Identifier branch_to_get)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {

                string query = String.Format("SELECT Nombre, CAST(Fecha_aper as varchar) as Fecha_aper, Horario, Cap_max, Provincia, Canton, Distrito, Manager, activeSpa, activeStore\r\nFROM Sucursal\r\nWHERE Nombre = '{0}'",
                    branch_to_get.nombre_sucursal);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DatabaseConnection");
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteAddBranch(Branch new_branch)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                string query = String.Format("INSERT INTO Sucursal(Nombre, Fecha_aper, Horario, Cap_max, Provincia, Canton, Distrito, Manager, activeSpa, activeStore)" +
                    "\r\nVALUES('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}',{8},{9})",

                    new_branch.nombre_sucursal,
                    new_branch.fecha_apertura,
                    new_branch.horario,
                    new_branch.cap_max,
                    new_branch.provincia,
                    new_branch.canton,
                    new_branch.distrito,
                    new_branch.manager,
                    Convert.ToInt32(new_branch.active_spa),
                    Convert.ToInt32(new_branch.active_store));

                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }


        public static bool ExecuteUpdateBranch(Branch updated_branch)
        {

            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                //Conversion de string a datetime 
                DateTime dateTime = Convert.ToDateTime(updated_branch.fecha_apertura);
                DateOnly dateOnly = DateOnly.FromDateTime(dateTime);
                string dbDate = dateOnly.ToString("yyyy-MM-dd");
                Console.WriteLine(dbDate);
                DateOnly dateOnly1 = DateOnly.ParseExact(dbDate, "yyyy-MM-dd");
                Console.WriteLine(dateOnly1);
                DateTime testDateTime = dateOnly1.ToDateTime(TimeOnly.Parse("12:00 AM"));
                Console.WriteLine(testDateTime);
                Console.WriteLine(dateTime.ToString());
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("UPDATE Sucursal\r\n" +
                    "SET Nombre = '{0}'," +
                    "Fecha_aper = '{1}', " +
                    "Horario = '{2}', " +
                    "Cap_max = {3}, " +
                    "Provincia = '{4}', " +
                    "Canton = '{5}'," +
                    "\r\nDistrito = '{6}', " +
                    "Manager = '{7}', " +
                    "activeSpa = {8}, " +
                    "activeStore = {9}\r\n" +
                    "WHERE Nombre = '{0}'",
                    updated_branch.nombre_sucursal,
                    testDateTime,
                    updated_branch.horario,
                    updated_branch.cap_max,
                    updated_branch.provincia,
                    updated_branch.canton,
                    updated_branch.distrito,
                    updated_branch.manager,
                    Convert.ToInt32(updated_branch.active_spa),
                    Convert.ToInt32(updated_branch.active_store));

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteDeleteBranch(Branch_Identifier branch_to_delete)
        {

            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {

                conn.Open();
                string query = String.Format("DELETE from TelefonoXSucursal\r\n" +
                    "where Sucursal_nombre = '{0}'\r\n\r\n" +
                    "DELETE from ProductoXSucursal\r\n" +
                    "where Sucursal_nombre = '{0}'\r\n\r\n" +
                    "DELETE from TratamientoXSucursal\r\n" +
                    "where Sucursal_nombre = '{0}'\r\n\r\n" +
                    "DELETE from ServicioXSucursal\r\n" +
                    "where Sucursal_nombre = '{0}'\r\n\r\n" +
                    "UPDATE Empleado\r\n" +
                    "SET Sucursal = NULL\r\n" +
                    "where Sucursal = '{0}'\r\n\r\n" +
                    "UPDATE Inventario\r\n" +
                    "SET Sucursal = NULL\r\n" +
                    "where Sucursal = '{0}'\r\n\r\n" +
                    "DELETE from Sucursal\r\n" +
                    "where Nombre = '{0}'", branch_to_delete.nombre_sucursal);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }




        }

        public static DataTable ExecuteLoginWorker(Credentials_Worker credentials)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            string encrypted_password = Encryption.encrypt_password(credentials.password);
            try
            {


                string query = String.Format("SELECT Cedula,Nombre,Apellido1,Apellido2,Provincia,Canton,Distrito,Salario,Correo,Password\r\n" +
                    "FROM Empleado\r\n" +
                    "WHERE Cedula = '{0}' AND Password = '{1}'", credentials.cedula, encrypted_password);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }


        public static DataTable ExecuteGetAllInventories()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                string query = string.Format("SELECT Num_serie as num_serie, Descripcion as description, Marca as marca\r\n" +
                    "FROM Inventario INNER JOIN Tipo_Equipo ON Inventario.Tipo_Equipo = Tipo_Equipo.ID");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            { conn.Close(); }
        }

        public static DataTable ExecuteGetInventory(Inventory_Identifier inventory_Identifier)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {

                string query = string.Format("SELECT Num_serie as num_serie, Marca as marca, Costo as costo, is_Used as used, Descripcion as description, Sucursal as sucursal\r\n" +
                    "FROM Inventario INNER JOIN Tipo_Equipo ON Inventario.Tipo_Equipo = Tipo_Equipo.ID\r\n" +
                    "WHERE Num_serie = {0}", inventory_Identifier.num_serie);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);


                return table;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteAddInventory(Inventory new_inventory)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int equipment_id = 0;
            try
            {
                if (new_inventory.tipo_equipo == "Cinta de correr")
                {
                    equipment_id = 1;
                }
                else if (new_inventory.tipo_equipo == "Bicicleta estacionaria")
                {
                    equipment_id = 2;
                }
                else if (new_inventory.tipo_equipo == "Multigimnasio")
                {
                    equipment_id = 3;
                }
                else if (new_inventory.tipo_equipo == "Remo")
                {
                    equipment_id = 4;
                }
                else
                {
                    equipment_id = 5;
                }

                conn.Open();
                string query = String.Format("INSERT INTO Inventario(Num_serie,Marca,Costo, is_Used, Tipo_Equipo, Sucursal)\r\n" +
                    "VALUES({0},'{1}',{2},{3},{4},NULL)",
                    new_inventory.num_serie,
                    new_inventory.marca,
                    new_inventory.costo,
                    Convert.ToInt32(new_inventory.is_used),
                    equipment_id);


                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }


        }

        public static bool ExecuteUpdateInventory(Inventory inventory_to_update)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int equipment_id = 0;
            try
            {
                if (inventory_to_update.tipo_equipo == "Cinta de correr")
                {
                    equipment_id = 1;
                }
                else if (inventory_to_update.tipo_equipo == "Bicicleta estacionaria")
                {
                    equipment_id = 2;
                }
                else if (inventory_to_update.tipo_equipo == "Multigimnasio")
                {
                    equipment_id = 3;
                }
                else if (inventory_to_update.tipo_equipo == "Remo")
                {
                    equipment_id = 4;
                }
                else
                {
                    equipment_id = 5;
                }

                conn.Open();
                string query = string.Format("UPDATE Inventario\r\n" +
                    "SET Num_serie = {0}, Marca = '{1}', Costo = {2}, is_Used = {3}, Tipo_Equipo = {4}, Sucursal = NULL\r\n" +
                    "WHERE Num_serie = {0}",
                    inventory_to_update.num_serie,
                    inventory_to_update.marca,
                    inventory_to_update.costo,
                    Convert.ToInt32(inventory_to_update.is_used),
                    equipment_id);

                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteDeleteInventory(Inventory_Identifier inventory_to_delete)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {

                conn.Open();
                string query = string.Format("DELETE FROM Inventario\r\n" +
                    "WHERE Num_serie = {0}", inventory_to_delete.num_serie);
                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteAssociateInventory(Associate_Inventory associate_Inventory)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                string query = string.Format("UPDATE Inventario\r\nSET Sucursal = '{0}'\r\n" +
                    "WHERE num_serie = {1}", associate_Inventory.sucursal, associate_Inventory.num_serie);
                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }


        public static DataTable ExecuteGetNonAssociatedInv()
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                string query = string.Format("SELECT Inventario.Num_serie as num_serie, Tipo_Equipo.Descripcion as tipo_equipo\r\n" +
                    "FROM Inventario\r\n" +
                    "INNER JOIN Tipo_Equipo ON Tipo_Equipo.ID = Inventario.Tipo_Equipo\r\n" +
                    "WHERE Sucursal IS NULL");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);


                return table;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally { conn.Close(); }

        }


        public static bool ExecuteAssociateTreatment(Associate_treatment associate_Treatment)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int treatment_type;

            try
            {

                conn.Open();
                string query = string.Format("UPDATE Inventario\r\n" +
                    "SET Sucursal = '{0}'\r\n" +
                    "WHERE Num_serie = {1}",
                    associate_Treatment.sucursal,
                    associate_Treatment.num_serie);

                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool ExecuteCreateClass(Class new_class)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int service_id = 0;
            try
            {
                if (new_class.servicio == "Indoor Cycling")
                {
                    service_id = 1;

                }
                else if (new_class.servicio == "Pilates")
                {
                    service_id = 2;

                }
                else if (new_class.servicio == "Yoga")
                {
                    service_id = 3;
                }
                else if (new_class.servicio == "Zumba")
                {
                    service_id = 4;
                }
                else if (new_class.servicio == "Natacion")
                {
                    service_id = 5;
                }

                DateTime dateTime = Convert.ToDateTime(new_class.fecha);
                DateOnly dateOnly = DateOnly.FromDateTime(dateTime);
                string dbDate = dateOnly.ToString("yyyy-MM-dd");
                Console.WriteLine(dbDate);
                DateOnly dateOnly1 = DateOnly.ParseExact(dbDate, "yyyy-MM-dd");
                Console.WriteLine(dateOnly1);
                DateTime testDateTime = dateOnly1.ToDateTime(TimeOnly.Parse("12:00 AM"));
                Console.WriteLine(testDateTime);

                conn.Open();
                string query = string.Format("INSERT INTO Clase(Servicio,Modo,Capacidad,Fecha,Hora_ing,Hora_sal,Encargado)\r\n" +
                    "VALUES({0},'{1}',{2},'{3}','{4}','{5}','{6}')",
                    service_id,
                    new_class.modo,
                    Convert.ToInt32(new_class.capacidad),
                    testDateTime,
                    new_class.hora_ingreso,
                    new_class.hora_salida,
                    new_class.encargado);

                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                return (i > 0) ? true : false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally { conn.Close(); }
        }
        public static DataTable ExecuteGetAllPayrolls()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT\r\n\te.Cedula as empleado_cedula,\r\n\tp.Descripcion as planilla_tipo,\r\n\tCASE\r\n\t\tWHEN p.Descripcion = 'Mensual' THEN e.Salario\r\n\t\tWHEN p.Descripcion = 'Por clase' THEN COUNT(c.ID) * e.Salario\r\n\t\tWHEN p.Descripcion = 'Por hora' THEN SUM(DATEDIFF(HOUR, c.Hora_ing, c.Hora_sal)) * e.Salario\r\n\t\tEND AS salario\r\n\tFROM \r\n\tEmpleado e \r\n\tINNER JOIN Planilla p ON e.Planilla = p.ID\r\n\tLEFT JOIN Clase c ON e.Cedula = c.Encargado\r\n\r\n\tWHERE \r\n\t\te.Planilla IN (1,2,3)\r\n\r\n\tGROUP BY \r\n\t\te.Cedula,\r\n\t\te.Nombre,\r\n\t\te.Planilla,\r\n\t\tp.Descripcion,\r\n\t\te.Salario;");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteGetPayroll(Employee_Identifier cedula)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT\r\n\te.Cedula as empleado_cedula,\r\n\tp.Descripcion as planilla_tipo,\r\n\tCASE\r\n\t\tWHEN p.Descripcion = 'Mensual' THEN e.Salario\r\n\t\tWHEN p.Descripcion = 'Por clase' THEN COUNT(c.ID) * e.Salario\r\n\t\tWHEN p.Descripcion = 'Por hora' THEN SUM(DATEDIFF(HOUR, c.Hora_ing, c.Hora_sal)) * e.Salario\r\n\t\tEND AS salario\r\n\tFROM \r\n\tEmpleado e \r\n\tINNER JOIN Planilla p ON e.Planilla = p.ID\r\n\tLEFT JOIN Clase c ON e.Cedula = c.Encargado\r\n\r\n\tWHERE \r\n\t\te.Cedula = '{0}'\r\n\r\n\tGROUP BY \r\n\t\te.Cedula,\r\n\t\te.Nombre,\r\n\t\te.Planilla,\r\n\t\tp.Descripcion,\r\n\t\te.Salario;", cedula.cedula_empleado);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteGetAllEmployees()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Cedula as cedula, Nombre as nombre, Apellido1 as apellido_1, Apellido2 as apellido_2\r\nFROM Empleado");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteGetEmployee(Employee_Identifier cedula)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Cedula as cedula_empleado, Nombre as nombre, Apellido1 as apellido_1, Apellido2 as apellido_2,\r\n\t\tProvincia as provincia, Canton as canton, Distrito as distrito, Salario as salario, Correo as correo,\r\n\t\tPassword as password, Sucursal as nombre_sucursal, p.Descripcion as puesto_descripcion, pl.Descripcion as planilla_descripcion\r\nFROM Empleado INNER JOIN Puesto p ON Empleado.Puesto = p.ID\r\n\t\t\t  INNER JOIN Planilla pl ON Empleado.Planilla = pl.ID\r\nWHERE Empleado.Cedula = '{0}'", cedula.cedula_empleado.ToString());
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteAddEmployee(Employee_Extended employee)
        {
            int planilla = 0;
            int puesto = 0;


            if (employee.planilla_descripcion == "Mensual")
            {
                planilla = 1;
            }
            if (employee.planilla_descripcion == "Por hora")
            {
                planilla = 2;
            }
            if (employee.planilla_descripcion == "Por clase")
            {
                planilla = 3;
            }
            if (employee.puesto_descripcion == "Administrador")
            {
                puesto = 1;
            }
            if (employee.puesto_descripcion == "Instructor")
            {
                puesto = 2;
            }
            if (employee.puesto_descripcion == "Dependiente Spa")
            {
                puesto = 3;
            }
            if (employee.puesto_descripcion == "Dependiente Tienda")
            {
                puesto = 4;
            }


            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("INSERT INTO Empleado (Cedula, Nombre, Apellido1, Apellido2, Provincia, Canton, Distrito, Salario, Correo, Password, Sucursal, Puesto, Planilla)\r\nVALUES(\r\n'{0}',\r\n'{1}',\r\n'{2}',\r\n'{3}',\r\n'{4}',\r\n'{5}',\r\n'{6}',\r\n{7},\r\n'{8}',\r\n'{9}',\r\n'{10}',\r\n{11},\r\n{12});",
                        employee.cedula_empleado,
                        employee.nombre,
                        employee.apellido_1,
                        employee.apellido_2,
                        employee.provincia,
                        employee.canton,
                        employee.distrito,
                        employee.salario,
                        employee.correo,
                        Encryption.encrypt_password(employee.password),
                        employee.nombre_sucursal,
                        puesto,
                        planilla);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteUpdateEmployee(Employee_Extended updated_employee)
        {

            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                int planilla = 0;
                int puesto = 0;


                if (updated_employee.planilla_descripcion == "Mensual")
                {
                    planilla = 1;
                }
                if (updated_employee.planilla_descripcion == "Por hora")
                {
                    planilla = 2;
                }
                if (updated_employee.planilla_descripcion == "Por clase")
                {
                    planilla = 3;
                }
                if (updated_employee.puesto_descripcion == "Administrador")
                {
                    puesto = 1;
                }
                if (updated_employee.puesto_descripcion == "Instructor")
                {
                    puesto = 2;
                }
                if (updated_employee.puesto_descripcion == "Dependiente Spa")
                {
                    puesto = 3;
                }
                if (updated_employee.puesto_descripcion == "Dependiente Tienda")
                {
                    puesto = 4;
                }
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("UPDATE Empleado\r\n" + "SET Cedula = '{0}'," + "Nombre = '{1}', " + "Apellido1 = '{2}', " + "Apellido2 = '{3}', " + "Provincia = '{4}', " + "Canton = '{5}'," + "\r\nDistrito = '{6}', " + "Salario = {7}, " + "Correo = '{8}', " + "Password = '{9}'\r\n" +
                    ", Sucursal = '{10}'," + "Puesto = {11}," + "Planilla = {12}\r\n" +
                    "WHERE Cedula = '{0}'",
                    updated_employee.cedula_empleado,
                    updated_employee.nombre,
                    updated_employee.apellido_1,
                    updated_employee.apellido_2,
                    updated_employee.provincia,
                    updated_employee.canton,
                    updated_employee.distrito,
                    updated_employee.salario,
                    updated_employee.correo,
                    Encryption.encrypt_password(updated_employee.password),
                    updated_employee.nombre_sucursal,
                    puesto,
                    planilla);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteDeleteEmployee(Employee_Identifier cedula)
        {

            SqlConnection conn = new SqlConnection(cadenaConexion);

            try
            {

                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("UPDATE Clase Set Encargado = null\r\nwhere Encargado = '{0}'\r\n\r\nUPDATE Sucursal \r\nSet Manager = null\r\nwhere Manager = '{0}'\r\n\r\nDELETE FROM Empleado\r\nwhere Cedula = '{0}'", cedula.cedula_empleado);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteGetAllProducts()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Nombre as nombre_producto, Costo as costo\r\nFROM Producto");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteGetProduct(Product_Identifier barras)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Codigo_barras as codigo_barras, Nombre as nombre_producto, Costo as costo, Descripcion as descripcion\r\nFROM Producto\r\nWHERE Codigo_barras = '{0}'", barras.codigo_barras.ToString());
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteAddProduct(Product product)
        {
            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("INSERT INTO Producto (Codigo_barras, Nombre, Costo, Descripcion)\r\nVALUES\r\n('{0}','{1}',{2},'{3}');",
                        product.codigo_barras,
                        product.nombre_producto,
                        product.costo,
                        product.descripcion);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteGetPositions()

        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Descripcion as tipo_puesto \r\nFROM Puesto");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteUpdateProduct(Product product)
        {

            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("UPDATE Producto\r\nSET \r\nCodigo_barras = '{0}',\r\nNombre = '{1}',\r\nCosto = {2},\r\nDescripcion = '{3}'\r\nWHERE \r\nCodigo_barras = '{0}';",
                    product.codigo_barras,
                    product.nombre_producto,
                    product.costo,
                    product.descripcion);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteDeleteProduct(Product_Identifier barras)
        {

            SqlConnection conn = new SqlConnection(cadenaConexion);

            try
            {

                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("DELETE FROM ProductoXSucursal\r\nWHERE Producto_ID = '{0}'\r\n\r\nDELETE FROM Producto\r\nWHERE Codigo_barras = '{0}';", barras.codigo_barras);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static bool ExecuteAssignProduct(Associate_product product)
        {

            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("INSERT INTO ProductoXSucursal(Sucursal_nombre, Producto_ID)\r\nVALUES(\r\n'{0}','{1}');",
                    product.sucursal,
                    product.product);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable ExecuteLoginClient(Credentials_Client credentials)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            string encrypted_password = Encryption.encrypt_password(credentials.password);
            try
            {


                string query = String.Format("SELECT Cedula as cedula_cliente, Nombre as nombre, Apellido1 as apellido_1, Apellido2 as apellido_2, Direccion as direccion, \r\nCorreo as email, Password as password, (Peso/POWER(Altura, 2))*10000 as IMC, DATEDIFF(YEAR,Fecha_nac,GETDATE()) as edad\r\nFROM Cliente\r\nWHERE Correo = '{0}'\r\nAND Password = '{1}'", credentials.email, encrypted_password);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }





        }
        public static bool ExecuteCreateClient(Client_Register client)
        {
            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            string encrypted_password = Encryption.encrypt_password(client.password);
            try
            {
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("INSERT INTO Cliente(Cedula,Nombre,Apellido1,Apellido2,Direccion,Correo,Password,Altura,Peso,Fecha_nac)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},'{9}')",
                        client.cedula_cliente,
                        client.nombre,
                        client.apellido_1,
                        client.apellido_2,
                        client.direccion,
                        client.email,
                        encrypted_password,
                        client.altura,
                        client.peso,
                        client.fecha_nac);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }

        public static DataTable ExecuteGetAllClasses()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                string query = string.Format("SELECT Servicio.Descripcion as servicio, Clase.ID as id, Clase.Modo as modo, Clase.Capacidad as capacidad, Clase.Fecha as fecha, Clase.Hora_ing as hora_ing, Clase.Hora_sal as hora_sal, Clase.Encargado as encargado\r\n" +
                    "FROM Servicio \r\n" +
                    "INNER JOIN Clase ON Servicio.ID = Clase.Servicio");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable ExecuteFilterClasses(FilterClass filters)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                string query = string.Format("SELECT Servicio.Descripcion as servicio, TratamientoXSucursal.Sucursal_nombre as sucursal, Clase.ID as id, Modo as modo, Clase.Capacidad as capacidad, Clase.Fecha as fecha, Clase.Hora_ing as hora_ing, Clase.Hora_sal as hora_sal, Clase.Encargado as encargado\r\n" +
                    "FROM Servicio \r\n" +
                    "INNER JOIN Clase ON Servicio.ID = Clase.Servicio\r\n" +
                    "INNER JOIN TratamientoXSucursal ON Servicio.ID = TratamientoXSucursal.Tratamiento_ID\r\n" +
                    "WHERE fecha = '{0}' AND Sucursal_nombre = '{1}' ",
                    filters.fecha,
                    filters.sucursal);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool ExecuteEnrollClass(Enroll_Class enroll_class)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int capacity = ExecuteGetCapacity(enroll_class.clase_id);

            if (capacity > 0)
            {
                try
                {
                    conn.Open();
                    string query = string.Format("INSERT INTO ClientesXClase(Cliente_cedula, Clase_ID)\r\n" +
                        "VALUES('{0}',{1})\r\n" +
                        "UPDATE Clase\r\n" +
                        "SET Capacidad = Capacidad-1\r\n" +
                        "WHERE ID = {1};",
                        enroll_class.cedula_cliente,
                        enroll_class.clase_id);

                    Console.WriteLine("Capacity: " + capacity);
                    Console.WriteLine(query);


                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    int i = cmd.ExecuteNonQuery();
                    return (i > 0) ? true : false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }

            }
            else
            {
                Console.WriteLine("No capacity");
                return false;

            }

        }

        public static int ExecuteGetCapacity(int id)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int capacity = 0;
            try
            {
                conn.Open();
                string query = string.Format("SELECT Capacidad \r\n" +
                    "FROM Clase\r\n" +
                    "WHERE ID = {0}", id);
                Console.WriteLine(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    capacity = Convert.ToInt32(row["Capacidad"]);
                }
                return capacity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        public static DataTable ExecuteGetServices()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Sucursal_nombre as nombre_sucursal, Servicio.Descripcion as servicio\r\nFROM Servicio INNER JOIN ServicioXSucursal on Servicio.ID = ServicioXSucursal.Servicio_ID");
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteAddService(ServiceAdd serviceAdd)
        {

            //Se genera la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("INSERT INTO Servicio(Descripcion)" +
                    "\r\nVALUES('{0}')",
                    serviceAdd.servicio);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }



            
        }
        public static bool ExecuteDeleteClient(Cedula_Cliente cliente)
        {

            SqlConnection conn = new SqlConnection(cadenaConexion);

            try
            {

                conn.Open();
                //Se genera el query de SQL
                string query = String.Format("UPDATE ClientesXClase\r\nSET Cliente_cedula = NULL\r\nWHERE Cliente_cedula = '{0}'\r\n\r\nDELETE FROM Cliente\r\nWHERE Cedula = '{0}'", cliente.cedula_cliente);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
                return (i > 0) ? true : false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}
