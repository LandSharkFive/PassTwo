# Password Manager

This project contains a command line password manger.  The application gets and saves passwords to a password vault file.  The passwords are printed to the screen.  Use Copy and Paste (Ctrl-C and Shift-Insert) on the command line as needed.  

## Usage

```
  Password filename
  The filename is required.
```

## Description

  Password Manager.  The master password is hashed.  The account passwords are encrypted by AES. 

## Install and Build

The is a C# Console-Mode Project.  Open with  Visual Studio 2022 and above to compile. 

## Getting Started

When the application starts, enter a master password.  The master password is hashed and saved in the password vault file.

When the application restarts, enter the master password.  The master password is verified.  If the master password matches, the program displays:  Get List Add Delete Quit.  Enter the first letter of the command as needed.

To get a list of service names, type "L" at the command prompt.

To see the password for the facebook service, type "G facebook" at the command prompt.  The facebook password is displayed.  The program is case-sensitive.

To delete the facebook password, type "D facebook" at the command prompt.  

To add a new service name, type "A" at the command prompt.  Enter the service name, user id and the password.

To delete the master password, delete the password vault file.  To delete all account passwords, delete the password vault file.

Backup the password vault files as needed.  Keep the master passwords safe.

## Examples

See sample.txt

## Recommendations

Keep the source code in a secure location.  The password vault file is saved in the same directory as the executable.  Keep the executable and the password vault file in an encrypted folder.  Only the user should have read and write permissions to the folder.  Obfuscate the code and remove debug information during compilation.  Do not store the source code with the executable.  The files can be kept on a secure USB drive.
Keep the master password safe.  If you lose the master password, you will lose the other passwords too.  The master passwords should be written down in a safe and secure place.

