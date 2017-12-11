# SILL2 Client

This the host module of a program that limits a user to logon on more than 1 computer in a network. When it detects that the user 
is logged on another computer it logs off immediately.

It inserts a row in a database table at logon and update it every 3 seconds to signal that the user is logged.

The SILL2 Server module reads this table and show all logged on users and computers.
