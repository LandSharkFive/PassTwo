# Password Manager

This project contains a command line password manger.  The application saves passwords to the vault file.   

## Usage

```
  Usage:  passtwo filename
  The filename is required.
```

## Description

  Password Manager.  The master password is hashed.  The passwords are encrypted by AES. 

## Install and Build

The is a C# Console-Mode Project.  Open with  Visual Studio 2022 and above to compile. 

## Getting Started

When the application starts, enter a master password.  The master password is hashed and saved in the vault file.

When the application restarts, enter the master password.  The master password is verified.  If the master password matches, the program displays:  Get List Add Delete Quit.  Enter the first letter of the command as needed.

To get a list of service names, type "L" at the command prompt.

To see the password for the facebook service, type "G facebook" at the command prompt.  The facebook password is displayed.  The program is case-sensitive.

To delete the facebook password, type "D facebook" at the command prompt.  

To add a new service name, type "A" at the command prompt.  Enter the service name, user id and the password.

To delete the master password, delete the vault file.  To delete all passwords, delete the vault file.

Backup the vault files as needed.  Keep the master passwords safe.

## Examples

See sample.txt

## Recommendations

Keep the source code in a secure location.  The vault file is generally saved in the same directory as the executable.  Keep the executable and the vault files in an encrypted folder.  Only the user should have read and write access to the folder.  Compile in release mode and obfuscate the code.  The vault files can be kept on a encrypted USB drive.  Please keep the master passwords in a safe and secure place.

