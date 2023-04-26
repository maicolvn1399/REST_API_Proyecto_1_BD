

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



    }
}
