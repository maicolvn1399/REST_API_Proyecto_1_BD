

using REST_API_GymTEC.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;

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
                SqlCommand cmd = new SqlCommand("SELECT Nombre FROM Sucursal",conn);
                cmd.CommandType = System.Data.CommandType.Text;
                
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
                return table;

            }catch (Exception ex)
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

            }catch(Exception ex)
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
                foreach(var phone in phones)
                {
                    string query = String.Format("INSERT INTO TelefonoXSucursal(Sucursal_nombre, Telefono)\r\n" +
                    "VALUES('{0}','{1}')", branch_phone,phone);
                    Console.WriteLine(query);

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();

                }
                   
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }finally 
            {
                conn.Close(); 
            }

        }

        public static DataTable ExecuteGetBranch(Branch_Identifier branch_to_get)
        {
            SqlConnection conn = new SqlConnection(cadenaConexion);
            try
            {

                string query = String.Format("SELECT Nombre, Fecha_aper, Horario, Cap_max, Provincia, Canton, Distrito, Manager, activeSpa, activeStore\r\nFROM Sucursal\r\nWHERE Nombre = '{0}'", 
                    branch_to_get.nombre_sucursal);
                Console.WriteLine(query);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
   
                return table;
            }catch(Exception ex)
            {
                Console.WriteLine("DatabaseConnection");
                Console.WriteLine (ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public static bool ExecuteAddBranch(Branch new_branch)
        {
            SqlConnection conn = new SqlConnection (cadenaConexion);
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


            }catch(Exception ex)
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



            }catch (Exception ex)
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


            return true;

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

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

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


    }
}
