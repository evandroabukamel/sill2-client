/**
 * SILL2 - Server (Sistema Individual LimitLogin v2.0)
 * 
 * Author: Evandro Abu Kamel
 * Company: Pontifícia Universidade Católica de Minas Gerais
 * Copyright: Evandro Abu Kamel © 2011
 * Description: This file contains an abstract class and a method that is responsable to read the configuration file
 *				to access the database. The method parse the lines of the file and creates a MySqlConnection, returning it. 
 **/

using System;
using System.IO;
using Ini;
using MySql.Data.MySqlClient;
using System.Data;

namespace sill2_client
{
	/**
	 * This class is responsable to read the configuration file, named "dbconnect.cfg", that must be
	 * in the same directory of the executable file "sill2-client".
	 */
	public abstract class DBConnection
	{
		private static string fileServer, dbServer, database, port, user, password;
		public static MySqlConnection connection = null;
		public static int lastErrorCode = 0;

		/**
		 * This method reads to configuration file and save its values.
		 */
		public static bool config(sill2_client_form form)
		{
			IniFile localConfigFile;
			// Try to read the configuration file for database connection
			try
			{
				localConfigFile = new IniFile(@"" + Directory.GetCurrentDirectory() + @"\sill2-client-config.ini");
				dbServer = localConfigFile.IniReadValue("Config", "DbServer");
				database = localConfigFile.IniReadValue("Config", "Database");
				port = localConfigFile.IniReadValue("Config", "Port");
				user = localConfigFile.IniReadValue("Config", "User");
				password = localConfigFile.IniReadValue("Config", "Password");
				// Sets the connection string
				/*using (connection = new MySqlConnection("server=" + dbServer + "; port=" + port + "; database=" + database + "; uid=" + user + "; pwd=" + password + ""))
				{
					connection.Close();
					connection.Dispose();
				}*/
                return true;
            }
			catch (FileNotFoundException ex)
			{
				if (lastErrorCode != 3)
				{
					form.SetTrayIconError("sill2-client: 3 - The configuration file could not be found.\n" + ex.Message);
					lastErrorCode = 3;
				}
				Verification.connectionLost = true;
				return false;
			}
			catch (DirectoryNotFoundException ex)
			{
				if (lastErrorCode != 4)
				{
					form.SetTrayIconError("sill2-client: 4 - The specified path is invalid.\n" + ex.Message);
					lastErrorCode = 4;
				}
				Verification.connectionLost = true;
				return false;
			}
			catch (IOException ex)
			{
				if (lastErrorCode != 5)
				{
					form.SetTrayIconError("sill2-client: 5 - The path includes an incorrect or invalid syntax for file name, directory name, or volume label.\n" + ex.Message);
					lastErrorCode = 5;
				}
				Verification.connectionLost = true;
				return false;
			}
		}

		/**
		 * This method reads and parses the configuration file and creates the MySqlConnection, returning it in the end.
		 */
		public static MySqlConnection create(sill2_client_form form)
		{
			/*try
			{
				serverConfigFile = new IniFile(@"" + fileServer + @"\sill2-client-config.ini");

				// Sets the connection variables
				// Compare DbServer
				if (String.Compare(
					localConfigFile.IniReadValue("Config", "DbServer"),
					serverConfigFile.IniReadValue("Config", "DbServer"),
					false) == 0)
				{
					dbServer = localConfigFile.IniReadValue("Config", "DbServer");
				}
				else
				{
					dbServer = serverConfigFile.IniReadValue("Config", "DbServer");
					localConfigFile.IniWriteValue("Config", "DbServer", dbServer);
				}

				// Compare Database
				if (String.Compare(
					localConfigFile.IniReadValue("Config", "Database"),
					serverConfigFile.IniReadValue("Config", "Database"),
					false) == 0)
				{
					database = localConfigFile.IniReadValue("Config", "Database");
				}
				else
				{
					database = serverConfigFile.IniReadValue("Config", "Database");
					localConfigFile.IniWriteValue("Config", "Database", database);
				}

				// Compare Port
				if (String.Compare(
					localConfigFile.IniReadValue("Config", "Port"),
					serverConfigFile.IniReadValue("Config", "Port"),
					false) == 0)
				{
					port = localConfigFile.IniReadValue("Config", "Port");
				}
				else
				{
					port = serverConfigFile.IniReadValue("Config", "Port");
					localConfigFile.IniWriteValue("Config", "Port", port);
				}

				// Compare User
				if (String.Compare(
					localConfigFile.IniReadValue("Config", "User"),
					serverConfigFile.IniReadValue("Config", "User"),
					false) == 0)
				{
					user = localConfigFile.IniReadValue("Config", "User");
				}
				else
				{
					user = serverConfigFile.IniReadValue("Config", "User");
					localConfigFile.IniWriteValue("Config", "User", user);
				}

				// Compare Password
				if (String.Compare(
					localConfigFile.IniReadValue("Config", "Password"),
					serverConfigFile.IniReadValue("Config", "Password"),
					false) == 0)
				{
					password = localConfigFile.IniReadValue("Config", "Password");
				}
				else
				{
					password = serverConfigFile.IniReadValue("Config", "Password");
					localConfigFile.IniWriteValue("Config", "Password", password);
				}
			}
			catch (Exception ex)
			{
				dbServer = localConfigFile.IniReadValue("Config", "DbServer");
				database = localConfigFile.IniReadValue("Config", "Database");
				port = localConfigFile.IniReadValue("Config", "Port");
				user = localConfigFile.IniReadValue("Config", "User");
				password = localConfigFile.IniReadValue("Config", "Password");
				// MessageBox.Show("sill2-client: Could not be possible to write the INI file.\n" + ex.Message);
			}*/

			// Create the MySQL connection
			try
			{
				// If connection is null or closed or broken, close and dispose it to create another
				if (connection == null || connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
				{
					if (connection != null)
					{
						connection.Close();
					}
					connection = new MySqlConnection("server=" + dbServer + "; port=" + port + "; database=" + database + "; uid=" + user + "; pwd=" + password + "");
					connection.Close();
					return connection;
				}
				else // If the connection is already open, return it
				{
					connection.Close();
					return connection;
				}
			}
			catch (MySqlException ex)
			{
				if (lastErrorCode != 1)
				{
					form.SetTrayIconError("sill2-client: 1 - The MySQL connection could not be created.\n" + ex.Message);
					lastErrorCode = 1;
				}
				Verification.connectionLost = true;
			}
			catch (Exception ex)
			{
				if (lastErrorCode != 1)
				{
					form.SetTrayIconError("sill2-client: 2 - The MySQL connection could not be created.\n" + ex.Message);
					lastErrorCode = 2;
				}
				Verification.connectionLost = true;
			}
			return connection;
		}
	}
}