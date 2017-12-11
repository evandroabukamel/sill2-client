/**
 * SILL2 - Client (Sistema Individual LimitLogin v2.0)
 * 
 * Author: Evandro Abu Kamel
 * Company: Pontifícia Universidade Católica de Minas Gerais
 * Copyright: Evandro Abu Kamel © 2011
 * Description: This file contains the main code of the client version of SILL2.
 *				It is responsable to get the user name, computer name and domain and send these information to a database.
 *				The objective is to prevent the user to make logon in more than one computer at the same time.
 **/

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace sill2_client
{
	public static class Verification
	{
		// MySQL connection attributes
		public static MySqlConnection dbConn;
		public static MySqlDataAdapter dbAdapter;
		public static sill2_client_form form;

		// User domain/login and computer name
		public static string userLogin = "", domainName = "", computerName = "";

		// Last error code
		public static int lastErrorCode = 0;

		// Connection lost sometime
		public static bool connectionLost = false;
		
		public static void Start(sill2_client_form f)
		{
			// Get a form instance
			form = f;

			// Verify if another sill2.exe is running
			if (IsProcessOpen("sill2-client") > 1)
			{
				Environment.Exit(0);
			}
			
			// Execute the initial config of the MySQL Coonection
			if (DBConnection.config(form))
			{
				// Send date/time NOW() to database infinitely
				using (dbConn = DBConnection.create(form))
				{
					// Executes the initial procedures when user logs on
					Update(true);

					while (true)
					{
						try
						{
							while (dbConn.State != ConnectionState.Open)
							{
								// Delay for 10 seconds...
								System.Threading.Thread.Sleep(10000);

								// Checks if connection is closed
								if (dbConn.State == ConnectionState.Closed)
								{
									dbConn.Open();
								}
								else if (dbConn.State == ConnectionState.Broken)
								{
									dbConn.Close();
									dbConn.Open();
								}
							}

							// Verifies in database if the user is already logged
							string sqlVerifiesLog = "SELECT `user`, `computer` FROM `sill2`.`logged` WHERE `user`='" + userLogin + "' AND `computer`='" + computerName + "';";
							using (MySqlCommand verifiesLog = new MySqlCommand(sqlVerifiesLog, dbConn))
							{
								using (MySqlDataReader loggedResult = verifiesLog.ExecuteReader())
								{
									// If user is already logged...
									if (loggedResult.HasRows)
									{
										// Close DataReader
										loggedResult.Close();

										// Update the logged current time
										string logUpQuery = "UPDATE `sill2`.`logged` SET `lognow`=NOW() WHERE `user`='" + userLogin + "' AND `computer`='" + computerName + "';";
										using (MySqlCommand logUpComm = new MySqlCommand(logUpQuery, dbConn))
										{
											// Executes the update query									
											if (logUpComm.ExecuteNonQuery() > 0)
											{
												connectionLost = false;

												// Executes the initial procedures when user logs on
												Update(false);
											}
											else
											{
												// Do all the tests if the user is logged on another computers
												TestUserLogged(false);
											}
										}
									}
									else
									{
										// Close DataReader
										loggedResult.Close();

										// Executes the initial procedures when user logs on
										Update(false);
									}
								}

								// Close the database connection
								dbConn.Close();
							}

							// Delay for 3 seconds...
							System.Threading.Thread.Sleep(3000);
						}
						catch (Exception ex)
						{
							connectionLost = true;
							//MessageBox.Show("sill2-client: Error while trying to update the actual time.\n" + ex.Message);
							if (lastErrorCode != 6)
							{
								form.SetTrayIconError("sill2-client: 6 - Erro enquanto tentava atualizar a hora atual.\n" + ex.Message);
								lastErrorCode = 6;
								// saveException(ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
				}
			}
			else
			{
				connectionLost = true;
				Environment.Exit(0);
			}
		}

		/* Executes the initial procudures when user logs on. */
		private static void Update(bool firstLoop)
		{
			/* Send logon time to database */
			// Open MySQL connection
			try
			{
				// Checks if connection is closed
				if (dbConn.State == ConnectionState.Closed)
				{
					dbConn.Open();
				}
				else if (dbConn.State == ConnectionState.Broken)
				{
					dbConn.Close();
					dbConn.Open();
				}
			}
			catch (Exception ex)
			{
				connectionLost = true;
				if (lastErrorCode != 8)
				{
					form.SetTrayIconError("sill2-client: 8 - Não pôde ser estabelecida conexão com o banco de dados.\n" + ex.Message);
					lastErrorCode = 8;
					// saveException(ex.Message + "\n" + ex.StackTrace);
				}
			}

			// Verifies if the connection is open
			if (dbConn.State == ConnectionState.Open)
			{
				// Do all the tests if the user is logged on another computers
				TestUserLogged(firstLoop);

				// Insert the logon in the database
				try
				{
					string sqlSameCompLog = "SELECT `user`, `computer` FROM `sill2`.`logged` WHERE `user`='" + userLogin + "' AND `computer`='" + computerName + "';";
					using (MySqlCommand sameCompLog = new MySqlCommand(sqlSameCompLog, dbConn))
					using (MySqlDataReader sameCompLogResult = sameCompLog.ExecuteReader())
					{
						string logOnQuery = "";
						// If user is still logged on the same computer as before
						if (sameCompLogResult.HasRows)
						{
							logOnQuery = "UPDATE `sill2`.`logged` SET `logon`=NOW(), `lognow`=NOW() WHERE `user`='" + userLogin + "' AND `computer`='" + computerName + "';";
						}
						else if (firstLoop || connectionLost) // Connection lost
						{
							logOnQuery = "INSERT INTO `sill2`.`logged`(`computer`, `user`, `logon`, `lognow`) VALUES('" + computerName + "','" + userLogin + "', NOW(), NOW());";
						}
						else // User removed from Logged list
						{
							logoff(true);
						}

						// Close DataReader
						sameCompLogResult.Close();

						// Executes the logon query
						using (MySqlCommand logOnComm = new MySqlCommand(logOnQuery, dbConn))
						{
							if (logOnComm.ExecuteNonQuery() > 0)
							{
								connectionLost = false;
							}
						}
					}
				}
				catch (Exception ex)
				{
					connectionLost = true;
					if (lastErrorCode != 10)
					{
						form.SetTrayIconError("sill2-client: 10 - Erro enquanto tentava executar a query de LogOn.\n" + ex.Message);
						lastErrorCode = 10;
						// saveException(ex.Message + "\n" + ex.StackTrace);
					}
				}
            }	
		}

		/* Test if user is allowed to and is logged on another computers. */
		public static void TestUserLogged(bool firstLoop)
		{
			try
			{
				// Verifies if it is free for unlimited sessions and if is allowed for EVERYONE to logon multiple times
				string sqlVerifiesFree = "SELECT `login` FROM `sill2`.`freeusers` " +
					"WHERE `login`='" + userLogin + "' OR " +
					"UPPER(`login`) IN('*', '%', 'TODOS', 'ALL', 'EVERYONE', 'ALL USERS');";
				using (MySqlCommand verifiesFree = new MySqlCommand(sqlVerifiesFree, dbConn))
				using (MySqlDataReader unlimitedsResult = verifiesFree.ExecuteReader())
				{
					// If user cannot log on unlimited sessions...
					if (!unlimitedsResult.HasRows)
					{
						// Close DataReader
						unlimitedsResult.Close();

						if (!form.TimerLockEnabled)
						{
							// Enable userlock timer
							form.timerLockEnable();
						}

						// Then verifies in database if the user is already logged on another computer
						string sqlVerifiesLog = "SELECT `user`, `computer` FROM `sill2`.`logged` " +
						"WHERE `user`='" + userLogin + "' AND `computer`<>'" + computerName + "';";
						using (MySqlCommand verifiesLog = new MySqlCommand(sqlVerifiesLog, dbConn))
						using (MySqlDataReader loggedResult = verifiesLog.ExecuteReader())
						{
							// If user is already logged on another computer...
							if (loggedResult.HasRows)
							{
								List<string> loggedComputerList = new List<string>();
								while (loggedResult.Read())
								{
									loggedComputerList.Add(loggedResult.GetString("computer"));
								}

								form.SetTrayIconWarning("ATENÇÃO, \nSeu usuário " + userLogin +
									" ainda está ativo nos seguintes computadores: " + string.Join(", ", loggedComputerList) + ".\n" +
									"Será feito LOG OFF deste computador aqui em 3 minutos, a menos que você faça LOG OFF " +
									"do seu usuário nesses outros computadores.");

								if (!form.TimerLogoffEnabled)
								{
									// Enable logoff timer
									form.timerLogoffEnable();
								}
							}
							else
							{
								if (form.TimerLogoffEnabled)
								{
									// Enable logoff timer
									form.timerLogoffDisable();

									if (!firstLoop)
									{
										form.SetTrayIconText("Tudo certo, agora você só está usando um computador.");
									}
								}
							}
						
							// Close DataReader
							loggedResult.Close();
						}
					}
					else
					{
						// Close DataReader
						unlimitedsResult.Close();

						if (form.TimerLogoffEnabled)
						{
							// Enable logoff timer
							form.timerLogoffDisable();

							if (!firstLoop)
							{
								form.SetTrayIconText("Tudo certo, agora você só está usando um computador.");
							}
						}

						if (form.TimerLockEnabled)
						{
							// Enable userlock timer
							form.timerLockDisable();
						}
					}
				}
			}
			catch (Exception ex)
			{
				connectionLost = true;
				if (lastErrorCode != 9)
				{
					form.SetTrayIconError("sill2-client: 9 - Erro enquanto tentava executar as queries de verificação.\n" + ex.Message);
					lastErrorCode = 9;
					// saveException(ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		/**
		 * Make the user to logoff.
		 */
		public static void logoff(bool showMessage = true)
		{
			if (showMessage)
			{
				/*form.SetTrayIconWarning("ATENÇÃO, \nEste usuário, " + userLogin + ", " +
										"será deslogado em 1 minuto.\nSalve seus arquivos e utilize o seu próprio usuário.");*/
				form.SetTrayIconWarning("ATENÇÃO, \nEste usuário, " + userLogin + ", " +
										"solicitou que seja feito logoff.\nFavor utilizar o seu próprio usuário.");

				// Delay for 60 seconds...
				System.Threading.Thread.Sleep(60000);
			}

			mysqlDispose();
			//ExitWindowsEx(0, 0x00000020); // Logoff, Terminal Services
		}

		/**
		 * Dispose all MySQL connections
		 */
		public static void mysqlDispose()
		{
			if (dbConn != null)
			{
				dbConn.Close();
				dbConn.Dispose();
				MySqlConnection.ClearAllPools();
			}
		}

		/**
		 * Save the exceptions messages on the 'exceptions' table.
		 */
		/*public static void // saveException(string errorMsg)
		{
			MySqlCommand exceptionComm;
			string exceptionQuery = "INSERT INTO `sill2`.`exception`(`computer`, `datahora`, `errormsg`) VALUES('" + computerName + "', NOW(), '" + errorMsg + "');";

			// Defines connection string
			dbConn = DBConnection.create();

			// Open MySQL connection
			try
			{
				dbConn.Open();
			}
			catch (Exception ex)
			{
				MessageBox.Show("sill2-client: Connection could not be established.\n" + ex.Message);
			}

			exceptionComm = new MySqlCommand(exceptionQuery, dbConn);
			exceptionComm.ExecuteNonQuery();

			dbConn.Close();
		}*/

		public static int IsProcessOpen(string name)
		{
			int count = 0;
			// Here we're going to get a list of all running processes on the computer
			foreach (Process clsProcess in Process.GetProcesses())
			{
				/**
				 *	Now we're going to see if any of the running processes
				 *	match the currently running processes. Be sure to not
				 *	add the .exe to the name you provide, i.e: NOTEPAD,
				 *	not NOTEPAD.EXE or false is always returned even if
				 *	notepad is running.
				 *	Remember, if you have the process running more than once,
				 *	say IE open 4 times the loop thr way it is now will close all 4,
				 *	if you want it to just close the first one it finds
				 *	then add a return; after the Kill
				 */
				if (clsProcess.ProcessName.ToLower().Equals(name))
				{
					// if the process is found to be running then we return a true
					count++;
				}
			}
			//otherwise we return a false
			return count;
		}
		
		[DllImport("user32.dll")]
		private static extern int ExitWindowsEx(int uFlags, int dwReason);
	}
}