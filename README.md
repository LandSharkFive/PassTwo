# PassTwo: Lightweight CLI Password Manager

**PassTwo** is a secure, command-line utility for managing passwords. It uses **AES encryption** for stored credentials and hashes your master password to ensure your data remains private even if the vault file is accessed.

---

## Quick Start

1.  **Launch:** Run the executable with a vault name:  
    `passtwo myvault.dat`
2.  **Initialize:** On first run, enter a strong Master Password. This creates your vault.
3.  **Login:** On subsequent runs, enter that same password to unlock your data.
4.  **Manage:** Use single-letter commands (L, G, A, D, Q) to interact with your vault.

---

## Usage & Examples

Once authenticated, the program displays: `Get List Add Delete Quit`. Enter the first letter of the command:

| Command | Action | Example |
| :--- | :--- | :--- |
| **L** | **List** all saved service names. | `L` |
| **G** | **Get** a password for a service. | `G facebook` |
| **A** | **Add** a new service entry. | `A` (then follow prompts) |
| **D** | **Delete** a specific entry. | `D facebook` |
| **Q** | **Quit** the application. | `Q` |

> [!IMPORTANT]
> **Case Sensitivity:** Service names are case-sensitive. Searching for Facebook will not find facebook.

---

## Security & Storage

* **Encryption:** Credentials are encrypted via **AES**. The master password is never stored in plain text; it is hashed for verification.
* **The Vault:** All data resides in the filename you specified. To "factory reset" or start over, simply delete the vault file.
* **Recommendations:** * Run the app from an encrypted folder or USB drive.
    * Compile in Release mode and use obfuscation for the executable.
    * Back up your vault file frequently.

---

## Build Instructions

This is a **C# Console Project**.
1. Open the project in **Visual Studio 2022** or newer.
2. Restore NuGet packages if prompted.
3. Build the solution (Ctrl+Shift+B).

