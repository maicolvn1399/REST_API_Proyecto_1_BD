

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

            } catch (Exception ex)
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

            } catch (Exception ex)
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

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            } finally
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
            } catch (Exception ex)
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


            } catch (Exception ex)
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



            } catch (Exception ex)
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
                    "where Nombre = '{0}'",branch_to_delete.nombre_sucursal);

                Console.WriteLine(query);
                //Ejecucion de query 
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                //Retorna true si se ejecuta correctamente
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

            } catch (Exception ex)
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
            } finally
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

            } catch (Exception e)
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


            } catch (Exception e)
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

            } catch (Exception e)
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
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            } finally { conn.Close(); }

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

            }catch(Exception e){
                Console.WriteLine(e.Message);
                return false;
            }
            finally { conn.Close(); }
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
            catch(Exception e)
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

            }catch(Exception e)
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

                foreach(DataRow row in table.Rows)
                {
                    capacity = Convert.ToInt32(row["Capacidad"]);
                }
                return capacity;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);   
                return 0;
            }
            finally
            {
                conn.Close();
            }
            
        }
        



    }
}
