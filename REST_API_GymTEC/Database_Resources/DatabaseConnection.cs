﻿

using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace REST_API_GymTEC.Database_Resources
{
    /// <summary>
    /// Class to query thr database of the project 
    /// </summary>
    public class DatabaseConnection
    {

        public static string cadenaConexion = ConnectionStringManager.GetConnectionString();

        /// <summary>
        /// Executes a sql query to get all branches from the database 
        /// </summary>
        /// <returns> returns a Datatable with all information of branches needed </returns>
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

        /// <summary>
        /// Executes a sql query to get all the phones associated to a branch 
        /// </summary>
        /// <param name="branch_name"> branch name is the parameter passed to consult the database of the phones associated 
        /// to the branch </param>
        /// <returns> returns a datatable with all the phones associated to a branch </returns>
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

        /// <summary>
        /// Executes a sql query to add new phones to a branch 
        /// </summary>
        /// <param name="branch_phone"> refers to the branch that will have more phones associated to </param>
        /// <param name="phones"> list of phones to add to the database </param>
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

        /// <summary>
        /// Executes a sql query to get all the information needed of a specific branch 
        /// </summary>
        /// <param name="branch_to_get"> refers to an identifier of the branch that the information is needed </param>
        /// <returns> returns a datatable with all the information regarding that specific branch </returns>
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

        /// <summary>
        /// Executes a sql query to add a new branch to the database 
        /// </summary>
        /// <param name="new_branch"> refers to a new branch with attributes to be inserted into the corresponding table of 
        /// branches </param>
        /// <returns> returns a true if the insertion is succesfull, false in case it fails </returns>
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


        /// <summary>
        /// Executes a sql query to update a branch 
        /// </summary>
        /// <param name="updated_branch"> refers to the updated branch</param>
        /// <returns> returns true if the method is executed succesfully or false if not </returns>
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

        /// <summary>
        /// Executes a sql query to delete a branch 
        /// </summary>
        /// <param name="branch_to_delete"> refers to an identifier of the branch that is going to be deleted </param>
        /// <returns> returns true if the execution is succesfull, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to authenticate a worker with its credentials 
        /// </summary>
        /// <param name="credentials"> refers to the credentials that the worker uses to log in </param>
        /// <returns> returns a datatable to get the information of this specific worker </returns>

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

        /// <summary>
        /// Executes a sql query to get all the inventories in a table 
        /// </summary>
        /// <returns> returns a datatable with all the information regarding all the inventories </returns>
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

        /// <summary>
        /// Executes a sql query to get all the information regarding a specific branch 
        /// </summary>
        /// <param name="inventory_Identifier"> refers to an identifier for inventory to get all the information regarding 
        /// this specific inventory </param>
        /// <returns> returns a datatable with all the information regarding this specific inventory </returns>
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

        /// <summary>
        /// Executes a sql query to add a new inventory into a table of the database 
        /// </summary>
        /// <param name="new_inventory"> refers to a new inventory to be added into the database </param>
        /// <returns> returns true if the insertion is succesfull, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to update the information of an inventory 
        /// </summary>
        /// <param name="inventory_to_update"> refers to the inventory that will be 
        /// updated </param>
        /// <returns> returns true if the inventory was succesfully updated, false if not </returns>
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

        
        /// <summary>
        /// Executes a sql query to delete a branch 
        /// </summary>
        /// <param name="inventory_to_delete"> refers to the identifier of branch 
        /// that will be deleted </param>
        /// <returns> returns true if it's succesfully deleted or false if not </returns>
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

        /// <summary>
        /// Executes a sql to associate an inventory to a branch 
        /// </summary>
        /// <param name="associate_Inventory"> refers to a object to associate the branch with an inventory </param>
        /// <returns> returns true if the query is succesful or false if not </returns>

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

        /// <summary>
        /// Executes a sql query to get the inventories that are not associated
        /// </summary>
        /// <returns> returns a datatable with all the required information </returns>
        public static DataTable ExecuteGetNonAssociatedInv()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
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

        /// <summary>
        /// Executes a sql to associate an treatment to a branch 
        /// </summary>
        /// <param name="associate_Treatment"> refers to the object to associate a treatment to a branch </param>
        /// <returns> returns true if the query is succesful, false if not </returns>
        public static bool ExecuteAssociateTreatment(Associate_treatment associate_Treatment)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);

            try
            {
                conn.Open();
                string query = string.Format("INSERT INTO TratamientoXSucursal(Sucursal_nombre, Tratamiento_ID)\r\n" +
                    "VALUES('{0}',{1})",
                    associate_Treatment.sucursal,
                    associate_Treatment.tratamiento_id);

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


        /// <summary>
        /// Executes a sql query to create a new class for the gym 
        /// </summary>
        /// <param name="new_class"> refers to the object with the data to create a new class </param>
        /// <returns> returns true if the query runs succesfully, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to get all the payrolls in the database 
        /// </summary>
        /// <returns> returns a datatable with all the required information </returns>
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

        /// <summary>
        /// Executes a query to get a specific payroll 
        /// </summary>
        /// <param name="cedula"> refers to the identifier to get the payroll for that specific person </param>
        /// <returns> returns a datatable with the respective information </returns>
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

        /// <summary>
        /// Executes a query to get all the employees in a table 
        /// </summary>
        /// <returns> returns a datatable with all the employees </returns>
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

        /// <summary>
        /// Executes a sql query to get information for a specific employee 
        /// </summary>
        /// <param name="cedula"> refers to the identifier to get the information for this specific employee </param>
        /// <returns> returns a datatable with the respective information </returns>
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

        /// <summary>
        /// Executes a sql query to add an employee 
        /// </summary>
        /// <param name="employee"> refers to the object containing the new information to add to a new employee </param>
        /// <returns> returns true if the insertion is succesful, false if not </returns>
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

        /// <summary>
        /// Executes a sql to update an employee 
        /// </summary>
        /// <param name="updated_employee"> refers to the employee object to be updated </param>
        /// <returns> returns true if the query is succesful, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to delete a specific employee from a table 
        /// </summary>
        /// <param name="cedula"> refers to the identifier of the employee that should be deleted </param>
        /// <returns> returns true if the query runs succesfully, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to get all the products from a table 
        /// </summary>
        /// <returns> returns a datatable with the respective information </returns>
        public static DataTable ExecuteGetAllProducts()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {


                string query = String.Format("SELECT Codigo_barras as codigo_barras, Nombre as nombre_producto, Costo as costo\r\nFROM Producto");
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

        /// <summary>
        /// Executes a sql query to get a specific product from a table 
        /// </summary>
        /// <param name="barras"> refers to the identifier of the product that is required </param>
        /// <returns> returns a datatable with the respective information for that product </returns>
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

        /// <summary>
        /// Executes a sql query to add a new product to a table 
        /// </summary>
        /// <param name="product"> refers to the object of product that is inserted in the table of products </param>
        /// <returns> returns true if the insertion is succesful, false if not </returns>
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

        /// <summary>
        /// Executes a sql to get all the positions of an employee 
        /// </summary>
        /// <returns> returns a datatable with the respective information </returns>
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

        /// <summary>
        /// Executes a sql query to update a product 
        /// </summary>
        /// <param name="product"> refers to the product object with the updated information </param>
        /// <returns> returns true if the update query runs succesfully, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to delete a product from a table 
        /// </summary>
        /// <param name="barras"> refers to the identifier of the product that is goingt to be deleted </param>
        /// <returns> returns true if the deletion query runs succesful, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to associate a product to a branch 
        /// </summary>
        /// <param name="product"> refers to the object of product that is linked with a branch </param>
        /// <returns> returns true if the query runs succesful, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to log in a client and get the information from a client 
        /// </summary>
        /// <param name="credentials"> refers to the credentials of the client to log in into the system </param>
        /// <returns> returns a datatable with the requested informaton </returns>
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

        /// <summary>
        /// Executes a sql query to create a new client 
        /// </summary>
        /// <param name="client"> refers to a new client object that will be inserted into the database </param>
        /// <returns> returns true if the add query runs succesful, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to get all the classes from a table 
        /// </summary>
        /// <returns> returns a datatable with all the classes </returns>
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

        /// <summary>
        /// Executes a sql query to filter the classes with two parameters 
        /// </summary>
        /// <param name="filters"> refers to the filters or parameters to get the selected classes </param>
        /// <returns> returns a datatable with the requested information </returns>

        public static DataTable ExecuteFilterClasses(FilterClass filters)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                string query = string.Format("SELECT Servicio.Descripcion as servicio, TratamientoXSucursal.Sucursal_nombre as sucursal, Clase.ID as id, Modo as modo, Clase.Capacidad as capacidad, CAST(Clase.Fecha as varchar) as fecha, Clase.Hora_ing as hora_ing, Clase.Hora_sal as hora_sal, Clase.Encargado as encargado\r\n" +
                    "FROM Servicio \r\n" +
                    "INNER JOIN Clase ON Servicio.ID = Clase.Servicio\r\n" +
                    "INNER JOIN TratamientoXSucursal ON Servicio.ID = TratamientoXSucursal.Tratamiento_ID\r\n" +
                    "WHERE fecha = '{0}' AND Sucursal_nombre = '{1}' AND Clase.Capacidad > 0",
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

        /// <summary>
        /// Executes a sql query to enroll a client into a class 
        /// </summary>
        /// <param name="enroll_class"> refers to an object to enroll a client to a specific class </param>
        /// <returns> returns true if the enroll query runs succesful, false if not </returns>
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

        /// <summary>
        /// Executes a query to get the capacity of a class 
        /// </summary>
        /// <param name="id"> refers to the identifier of the class that the capacity is needed </param>
        /// <returns> reurns an integer that refers to the capacity of people that a class has </returns>

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

        /// <summary>
        /// Executes a sql query to get the services from a table 
        /// </summary>
        /// <returns> returns a datatable with the requested information </returns>
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

        /// <summary>
        /// Executes a sql query to add a new service into the database 
        /// </summary>
        /// <param name="serviceAdd"> refers to the object with the information to add a new service </param>
        /// <returns> returns true if the add query runs succesfully, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to delete a client 
        /// </summary>
        /// <param name="cliente"> refers to the identifier of the client that will be deleted </param>
        /// <returns> returns true if the deletion query runs succesfully, false if not </returns>
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

        /// <summary>
        /// Executes a sql query to copy the information from a branch to another one 
        /// </summary>
        /// <param name="branch"> refers to the branch object to copy  </param>
        /// <returns> returns true if the copy query runs succesfully, false if not </returns>
        public static bool ExecuteCopyBranch(Branch_Copier branch)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try { conn.Open();
                string query = string.Format("DECLARE @newtable TABLE (\r\nNombre VARCHAR(100) NULL)\r\n\r\nINSERT INTO @newtable(Nombre)\r\nVALUES('{1}')\r\n\r\nINSERT INTO Sucursal(Nombre,Fecha_aper,Horario,Cap_max,Provincia,Canton,Distrito,Manager,activeSpa,activeStore)\r\nSELECT b.Nombre as Nombre, Fecha_aper,Horario,Cap_max,Provincia,Canton,Distrito,Manager,activeSpa,activeStore\r\nFROM Sucursal, @newtable b\r\nWHERE Sucursal.Nombre = '{0}'\r\n\r\nINSERT INTO ProductoXSucursal(Sucursal_nombre,Producto_ID)\r\nSELECT b.Nombre, p.Producto_ID\r\nFROM ProductoXSucursal p, @newtable b\r\nWHERE p.Sucursal_nombre = '{0}'\r\n\r\nINSERT INTO TratamientoXSucursal(Sucursal_nombre,Tratamiento_ID)\r\nSELECT b.Nombre, t.Tratamiento_ID\r\nFROM TratamientoXSucursal t, @newtable b\r\nWHERE t.Sucursal_nombre = '{0}'",
                branch.branch_to_copy,
                branch.new_branch);

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

        /// <summary>
        /// Executes a sql query to associate a service to a branch 
        /// </summary>
        /// <param name="service"> refers to the service that will be associated with a branch </param>
        /// <returns> returns true if the associate query runs succesfully, false if not </returns>
        public static bool ExecuteAssociateService(Associate_Service service)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            int servicio = 0;
            try
            {
                if (service.servicio == "Indoor Cycling")
                {
                    servicio = 1;
                }
                if (service.servicio == "Pilates")
                {
                    servicio = 2;
                }
                if (service.servicio == "Yoga")
                {
                    servicio = 3;
                }
                if (service.servicio == "Zumba")
                {
                    servicio = 4;
                }
                if (service.servicio == "Natacion")
                {
                    servicio = 5;
                }
                if (service.servicio == "Crossfit")
                {
                    servicio = 6;
                }
                conn.Open();
                string query = string.Format("INSERT INTO ServicioXSucursal(Sucursal_nombre,Servicio_ID)VALUES('{0}',{1})", service.sucursal, servicio);
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


        /// <summary>
        /// Executes a sql query to get all the treatments in a table 
        /// </summary>
        /// <returns> returns a datatable with the requested information </returns>
        public static DataTable ExecuteGetAllTreatments()
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {
                string query = String.Format("SELECT ID, Descripcion \r\n" +
                    "FROM Tratamiento ");
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


    }
}
